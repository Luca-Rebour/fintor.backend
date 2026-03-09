using Application.DTOs.Accounts;
using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FintorDbContext _context;
        public AccountRepository(FintorDbContext context)
        {
            _context = context;
        }
        public void CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
        }

        public async Task DeleteAccountAsync(Guid accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                throw new NotFoundException("Acount");
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account> GetAccountByIdAsync(Guid accountId, Guid userId)
        {
            Account? account = await _context.Accounts
                .AsNoTracking()
                .Include(a => a.Currency)
                .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);

            if (account == null)
            {
                throw new NotFoundException("Account");
            }

            return account;
        }

        public async Task<Account> GetAccountByIdToUpdateAsync(Guid accountId, Guid userId)
        {
            Account? account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);

            if (account == null)
            {
                throw new NotFoundException("Account");
            }

            return account;
        }

        public async Task<AccountDetailDTO> GetAccountDetailAsync(Guid accountId, Guid userId)
        {
            DateTime now = DateTime.UtcNow;
            DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime startOfNextMonth = startOfMonth.AddMonths(1);

            AccountDetailDTO? accountDetail = await _context.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId && a.Id == accountId)
                .Select(a => new AccountDetailDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    CurrencyCode = a.Currency.Code,

                    TotalBalance = a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Income)
                        .Sum(t => (decimal?)t.Amount) ?? 0m
                        - 
                        a.Transactions
                            .Where(t => t.TransactionType == TransactionType.Expense)
                            .Sum(t => (decimal?)t.Amount) ?? 0m
                        ,

                    AllocatedToGoalsBalance = a.Goals
                        .SelectMany(g => g.Transactions)
                        .Where(t => t.TransactionType == TransactionType.Income)
                        .Sum(t => (decimal?)t.Amount) ?? 0m 
                        -
                         a.Goals
                        .SelectMany(g => g.Transactions)
                        .Where(t => t.TransactionType == TransactionType.Expense)
                        .Sum(t => (decimal?)t.Amount) ?? 0m
                        ,

                    MonthlyIncome = a.Transactions
                        .Where(t => t.Date >= startOfMonth &&
                                    t.Date < startOfNextMonth &&
                                    t.TransactionType == TransactionType.Income)
                        .Sum(t => (decimal?)t.Amount) ?? 0m,

                    MonthlyExpense = a.Transactions
                        .Where(t => t.Date >= startOfMonth &&
                                    t.Date < startOfNextMonth &&
                                    t.TransactionType == TransactionType.Expense)
                        .Sum(t => (decimal?)t.Amount) ?? 0m,

                    AvailableBalance = a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Income)
                        .Where(t => t.GoalId == null)
                        .Sum(t => (decimal?)t.Amount) ?? 0m
                        -
                        a.Transactions
                            .Where(t => t.TransactionType == TransactionType.Expense)
                            .Sum(t => (decimal?)t.Amount) ?? 0m
                            ,

                    Transactions = a.Transactions
                        .OrderByDescending(t => t.Date)
                        .Select(t => new TransactionDTO
                        {
                            Id = t.Id,
                            Amount = t.Amount,
                            Date = t.Date,
                            Description = t.Description ?? string.Empty,
                            Icon = t.Category.Icon,
                            CategoryName = t.Category.Name,
                            TransactionType = t.TransactionType,
                            IsRecurringTransaction = t.PendingApprovalTransactionId != null,
                            CurrencyCode = a.Currency.Code,
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (accountDetail == null)
            {
                throw new NotFoundException("Account");
            }

            return accountDetail;
        }

        public async Task<List<GetAccountDTO>> GetAccountSummariesAsync(Guid userId)
        {
            var summaries = await _context.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    Id = a.Id,
                    Name = a.Name,
                    Icon = a.Icon,
                    TotalBalance =
                        (a.Transactions
                            .Where(t => t.TransactionType == TransactionType.Income)
                            .Sum(t => (decimal?)t.Amount) ?? 0m)
                        -
                        (a.Transactions
                            .Where(t => t.TransactionType == TransactionType.Expense)
                            .Sum(t => (decimal?)t.Amount) ?? 0m),
                    AvailableBalance =
                        (a.Transactions
                            .Where(t => t.TransactionType == TransactionType.Income && t.GoalId == null)
                            .Sum(t => (decimal?)t.Amount) ?? 0m)
                        -
                        (a.Transactions
                            .Where(t => t.TransactionType == TransactionType.Expense)
                            .Sum(t => (decimal?)t.Amount) ?? 0m),
                    CurrencyId = a.Currency.Id,
                    CurrencyCode = a.Currency.Code
                })
                .ToListAsync();

            return summaries
                .Select(s => new GetAccountDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Balance = s.TotalBalance,
                    TotalBalance = s.TotalBalance,
                    AvailableBalance = s.AvailableBalance,
                    Currency = new GetAccountsCurrencyResponseDTO
                    {
                        Id = s.CurrencyId,
                        Code = s.CurrencyCode
                    },
                    Icon = s.Icon
                })
                .ToList();
        }

        public async Task<List<Account>> GetAllAccountsAsync(Guid userId)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .Include(a => a.Currency)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactions(Guid accountId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.AccountId == accountId)
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public void UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
        }
    }
}
