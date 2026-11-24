using System.Reflection;
using Application.DTOs.Transactions;
using Application.DTOs.RecurringTransactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.UseCases.Accounts;
using Application.UseCases.Transactions;
using Application.UseCases.RecurringTransactions;
using Application.UseCases.PendingApprovalTransactions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

// Run with: dotnet run --project Tests/AccountAuthorization -m:1
var owner = Guid.NewGuid();
var stranger = Guid.NewGuid();
var account = new Account(owner, Guid.NewGuid(), "Owner account", "wallet");
var mapper = new MapperConfiguration(c =>
{
    c.CreateMap<Transaction, TransactionDTO>();
    c.CreateMap<RecurringTransaction, RecurringTransactionDTO>();
}, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance).CreateMapper();
int passed = 0;
foreach (var operation in new[] { "create", "create recurring", "read", "read recurring", "reschedule" })
foreach (var allowed in new[] { false, true })
{
    var caller = allowed ? owner : stranger;
    int saves = 0, accesses = 0;
    var accounts = Stub.For<IAccountRepository>((name, args) =>
    {
        if (name != "GetAccountByIdAsync") throw new Exception("Unexpected account call");
        if ((Guid)args[0]! != account.Id || (Guid)args[1]! != owner)
            return Task.FromException<Account>(new NotFoundException("Account"));
        return Task.FromResult(account);
    });
    var unit = Stub.For<IUnitOfWork>((name, _) =>
    {
        if (name != "SaveChangesAsync") throw new Exception("Unexpected unit of work call");
        saves++;
        return Task.CompletedTask;
    });
    Transaction? created = null;
    var transactions = Stub.For<ITransactionRepository>((name, args) =>
    {
        accesses++;
        if (name == "CreateTransaction") { created = (Transaction)args[0]!; return null; }
        if (name == "GetTransactionAsync") return Task.FromResult(created!);
        if (name == "GetAccountTransactionsAsync" && (Guid)args[1]! == owner)
            return Task.FromResult(new List<Transaction>());
        throw new Exception("Unexpected transaction call or user scope");
    });
    var recurring = Stub.For<IRecurringTransactionRepository>((name, args) =>
    {
        accesses++;
        if (name == "AddAsync") return null;
        if (name == "GetAccountRecurringTransactionsAsync" && (Guid)args[1]! == owner)
            return Task.FromResult(new List<RecurringTransaction>());
        throw new Exception("Unexpected recurring call or user scope");
    });
    var due = new DateOnly(2026, 1, 1);
    var pending = new PendingApprovalTransaction(account.Id, Guid.NewGuid(), Guid.NewGuid(), 10m, "Test", TransactionType.Income, due);
    typeof(PendingApprovalTransaction).GetProperty("Account")!.SetValue(pending, account);
    var pendingRepo = Stub.For<IPendingApprovalTransactionRepository>((name, _) =>
        name == "GetByIdToUpdateAsync" ? Task.FromResult<PendingApprovalTransaction?>(pending) : throw new Exception("Unexpected pending call"));
    try
    {
        switch (operation)
        {
            case "create":
                await new CreateTransaction(accounts, transactions, mapper, unit, Stub.For<IGoalRepository>((_, _) => throw new Exception("No goal lookup expected"))).ExecuteAsync(new CreateTransactionDTO
                { AccountId = account.Id, CategoryId = Guid.NewGuid(), Amount = 10m, Description = "Test" }, caller);
                break;
            case "create recurring":
                await new CreateRecurringTransaction(accounts, recurring, mapper, unit).ExecuteAsync(new CreateRecurringTransactionDTO
                { AccountId = account.Id, CategoryId = Guid.NewGuid(), Name = "Test", Description = "Test", Amount = 10m, StartDate = due, EndDate = due.AddYears(1) }, caller);
                break;
            case "read":
                await new GetAccountTransactions(accounts, transactions, mapper).ExecuteAsync(account.Id, caller);
                break;
            case "read recurring":
                await new GetAccountRecurringTransactions(accounts, recurring, mapper).ExecuteAsync(account.Id, caller);
                break;
            case "reschedule":
                await new ReschedulePendingApprovalTransaction(pendingRepo, mapper, unit).ExecuteAsync(pending.Id, caller, due.AddDays(1));
                break;
        }
        if (!allowed) throw new Exception("Foreign account was accepted");
        bool writes = operation.StartsWith("create") || operation == "reschedule";
        if (saves != (writes ? 1 : 0)) throw new Exception("Wrong save count");
        if (operation == "reschedule" && pending.DueDate != due.AddDays(1)) throw new Exception("Date was not changed");
    }
    catch (DomainException ex) when (!allowed && ex is NotFoundException or ForbiddenException)
    {
        if (saves != 0 || accesses != 0 || pending.DueDate != due)
            throw new Exception("Unauthorized operation read or changed data");
    }
    Console.WriteLine($"PASS {operation}: {(allowed ? "owner allowed" : "foreign account blocked before access/save")}");
    passed++;
}
Console.WriteLine($"{passed} authorization checks passed.");

foreach (var type in new[] { TransactionType.Income, TransactionType.Expense })
foreach (var scenario in new[] { "valid", "none", "foreign", "other account", "missing" })
{
    Guid? goalId = scenario == "none" ? null : Guid.NewGuid();
    int saved = 0, writes = 0, lookups = 0;
    Transaction? recorded = null;
    var accounts = Stub.For<IAccountRepository>((_, _) => Task.FromResult(account));
    var goals = Stub.For<IGoalRepository>((name, args) =>
    {
        lookups++;
        if (name != "BelongsToAccountAsync" || (Guid)args[0]! != goalId || (Guid)args[1]! != account.Id || (Guid)args[2]! != owner)
            throw new Exception("Incorrect goal ownership check");
        return Task.FromResult(scenario == "valid");
    });
    var transactions = Stub.For<ITransactionRepository>((name, args) =>
    {
        if (name == "CreateTransaction") { writes++; recorded = (Transaction)args[0]!; return null; }
        if (name == "GetTransactionAsync") return Task.FromResult(recorded!);
        throw new Exception("Unexpected transaction call");
    });
    var unit = Stub.For<IUnitOfWork>((_, _) => { saved++; return Task.CompletedTask; });
    bool allowed = scenario is "valid" or "none";
    try
    {
        var result = await new CreateTransaction(accounts, transactions, mapper, unit, goals).ExecuteAsync(
            new CreateTransactionDTO { AccountId = account.Id, GoalId = goalId, CategoryId = Guid.NewGuid(), Amount = 25m, Description = "Goal test", TransactionType = type }, owner);
        if (!allowed || saved != 1 || writes != 1 || recorded!.GoalId != goalId || result.GoalId != goalId)
            throw new Exception("Goal was not preserved through save and response");
    }
    catch (NotFoundException) when (!allowed)
    {
        if (saved != 0 || writes != 0) throw new Exception("Invalid goal persisted a transaction");
    }
    if (lookups != (goalId.HasValue ? 1 : 0)) throw new Exception("Unexpected goal lookup count");
    Console.WriteLine($"PASS goal {type}: {scenario}");
}

Console.WriteLine("KAN-12: recurring edits");
foreach (var scenario in new[] { "valid", "foreign", "missing", "invalid amount", "invalid dates", "foreign account", "foreign category" })
{
    var currency = new Currency("USD");
    var editAccount = new Account(owner, currency.Id, "Savings", "wallet");
    typeof(Account).GetProperty("Currency")!.SetValue(editAccount, currency);
    var category = new Category(owner, "Salary", "DollarSign", "#123456");
    var start = new DateOnly(2026, 1, 1);
    var recurring = new RecurringTransaction(category.Id, editAccount.Id, "Old", 10m, "Old", TransactionType.Income, Frequency.Monthly, start, start.AddYears(1));
    typeof(RecurringTransaction).GetProperty("Account")!.SetValue(recurring, editAccount);
    recurring.ConsumeNextDueDate();
    var previousNext = recurring.NextDueDate;
    int saves = 0, updates = 0;
    var repo = Stub.For<IRecurringTransactionRepository>((name, _) => name switch
    {
        "GetBydIdToUpdateAsync" => Task.FromResult<RecurringTransaction?>(scenario == "missing" ? null : recurring),
        "Update" => CountUpdate(),
        _ => throw new Exception("Unexpected recurring operation")
    });
    object? CountUpdate() { updates++; return null; }
    var accounts = Stub.For<IAccountRepository>((_, _) => scenario == "foreign account"
        ? Task.FromException<Account>(new NotFoundException("Account")) : Task.FromResult(editAccount));
    var categories = Stub.For<ICategoryRepository>((_, _) => Task.FromResult<Category?>(scenario == "foreign category" ? null : category));
    var unit = Stub.For<IUnitOfWork>((_, _) => { saves++; return Task.CompletedTask; });
    var dto = new CreateRecurringTransactionDTO
    {
        AccountId = editAccount.Id, CategoryId = category.Id, Name = "Updated", Amount = scenario == "invalid amount" ? -1 : 25m,
        Description = "Updated", transactionType = TransactionType.Expense, Frequency = Frequency.Monthly,
        StartDate = start, EndDate = scenario == "invalid dates" ? start.AddDays(-1) : start.AddYears(2)
    };
    try
    {
        var result = await new UpdateRecurringTransaction(repo, accounts, categories, unit).ExecuteAsync(recurring.Id, dto, scenario == "foreign" ? stranger : owner);
        if (scenario != "valid" || saves != 1 || updates != 1 || result.Amount != 25m || result.CurrencyCode != "USD" || result.CategoryId != category.Id)
            throw new Exception("Invalid edit result");
        if (recurring.NextDueDate != previousNext || recurring.PendingApprovalTransactions.Count != 0)
            throw new Exception("Edit rewound the schedule or changed generated occurrences");
    }
    catch (DomainException) when (scenario != "valid")
    {
        if (saves != 0 || updates != 0 || recurring.Name != "Old") throw new Exception("Rejected edit mutated data");
    }
    Console.WriteLine($"PASS edit: {scenario}");
}
var schedule = new RecurringTransaction(Guid.NewGuid(), account.Id, "Schedule", 10m, "Test", TransactionType.Income, Frequency.Monthly, new DateOnly(2026, 1, 1), new DateOnly(2027, 1, 1));
schedule.ConsumeNextDueDate();
schedule.Edit(schedule.CategoryId, schedule.AccountId, "Schedule", 10m, "Test", TransactionType.Income, Frequency.Weekly, new DateOnly(2026, 1, 1), new DateOnly(2027, 1, 1));
if (schedule.NextDueDate < new DateOnly(2026, 2, 1)) throw new Exception("Schedule change regenerated past occurrences");
Console.WriteLine("PASS changed frequency does not rewind the generation cursor");

Console.WriteLine("KAN-13: pending mapping");
var pendingConfig = new MapperConfiguration(c => c.AddProfile<Application.MappingProfiles.PendingApprovalTransactionProfile>(), Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance);
pendingConfig.AssertConfigurationIsValid();
var pendingCurrency = new Currency("UYU");
typeof(Account).GetProperty("Currency")!.SetValue(account, pendingCurrency);
var pendingCategory = new Category(owner, "Food", "ShoppingCart", "#abcdef");
var pendingSource = new PendingApprovalTransaction(account.Id, Guid.NewGuid(), pendingCategory.Id, 123m, "Pending test", TransactionType.Expense, new DateOnly(2026, 12, 1));
typeof(PendingApprovalTransaction).GetProperty("Account")!.SetValue(pendingSource, account);
typeof(PendingApprovalTransaction).GetProperty("Category")!.SetValue(pendingSource, pendingCategory);
var mappedPending = pendingConfig.CreateMapper().Map<Application.DTOs.PendingApprovalTransactions.PendingApprovalTransactionDTO>(pendingSource);
if (mappedPending.CurrencyCode != "UYU" || mappedPending.CategoryName != "Food" || mappedPending.Icon != "ShoppingCart" || mappedPending.Amount != 123m || mappedPending.AccountName != account.Name || mappedPending.DueDate != pendingSource.DueDate || mappedPending.Status != pendingSource.Status)
    throw new Exception("Pending mapping lost API fields");
Console.WriteLine("PASS pending profile configuration and actual DTO mapping");

Console.WriteLine("KAN-16: AutoMapper recursion regression");
var recursionConfig = new MapperConfiguration(c => c.CreateMap<RecursiveSource, RecursiveDestination>(),
    Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance);
var root = new RecursiveSource();
var tail = root;
for (int i = 0; i < 200; i++) { tail.Child = new RecursiveSource(); tail = tail.Child; }
var destination = recursionConfig.CreateMapper().Map<RecursiveDestination>(root);
int depth = 0;
for (var node = destination; node != null; node = node.Child) depth++;
if (depth <= 0 || depth > 64) throw new Exception("AutoMapper did not enforce its default recursive depth limit");
Console.WriteLine($"PASS recursive mapping stopped at depth {depth}");

public class RecursiveSource { public RecursiveSource? Child { get; set; } }
public class RecursiveDestination { public RecursiveDestination? Child { get; set; } }

public class Stub : DispatchProxy
{
    public Func<string, object?[], object?> Handler = null!;
    public static T For<T>(Func<string, object?[], object?> handler) where T : class
    {
        var proxy = Create<T, Stub>();
        ((Stub)(object)proxy).Handler = handler;
        return proxy;
    }
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Handler(method!.Name, args ?? []);
}
