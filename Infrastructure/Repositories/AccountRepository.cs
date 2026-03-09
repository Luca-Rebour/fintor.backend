using Application.DTOs.Accounts;
using Application.DTOs.Categories;
using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<AccountDTO>> GetAllAccountsAsync(Guid userId)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .Select(a => new AccountDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    CurrencyCode = a.Currency.Code,
                    Icon = a.Icon,
                    TotalBalance =
                        (a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Income)
                        .Sum(t => (decimal?)t.Amount) ?? 0m)
                        -
                        (a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Expense)
                        .Sum(t => (decimal?)t.Amount) ?? 0m)
                        ,
                    AvailableBalance =
                        (a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Income)
                        .Sum(t => (decimal?)t.Amount) ?? 0m)
                        -
                        (a.Goals.SelectMany(g => g.Transactions)
                            .Sum(t => (decimal?)t.Amount) ?? 0m)
                        -
                        (a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Expense)
                        .Sum(t => (decimal?)t.Amount) ?? 0m)
                })

                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<AccountDetailDTO?> GetAccountDetailAsync(Guid userId, Guid accountId)
        {
            DateTime now = DateTime.UtcNow;
            int year = now.Year;
            int month = now.Month;

            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId && a.Id == accountId)
                .Select(a => new AccountDetailDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    CurrencyCode = a.Currency.Code,

                    TotalBalance = a.Transactions
                        .Sum(t => (decimal?)t.Amount) ?? 0m,

                    AllocatedToGoalsBalance = a.Goals
                        .SelectMany(g => g.Transactions)
                        .Sum(t => (decimal?)t.Amount) ?? 0m,

                    MonthlyIncome = a.Transactions
                        .Where(t => t.Date.Year == year &&
                                    t.Date.Month == month &&
                                    t.Amount > 0 &&
                                    t.TransactionType == TransactionType.Income)
                        .Sum(t => (decimal?)t.Amount) ?? 0m,

                    MonthlyExpense = a.Transactions
                        .Where(t => t.Date.Year == year &&
                                    t.Date.Month == month &&
                                    t.Amount > 0&&
                                    t.TransactionType == TransactionType.Expense)
                        .Sum(t => (decimal?)t.Amount) ?? 0m,

                    AvailableBalance =
                        (a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Income)
                        .Sum(t => (decimal?)t.Amount) ?? 0m)
                        -
                        (a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Expense)
                        .Sum(t => (decimal?)t.Amount) ?? 0m)
                        -
                        (a.Goals.SelectMany(g => g.Transactions)
                            .Sum(t => (decimal?)t.Amount) ?? 0m),

                    Transactions = a.Transactions
                        .OrderByDescending(t => t.Date)
                        .Select(t => new TransactionDTO
                        {
                            Id = t.Id,
                            Amount = t.Amount,
                            Date = t.Date,
                            Description = t.Description,
                            Icon = t.Category.Icon,
                            CategoryName = t.Category.Name,
                            TransactionType = t.TransactionType,
                            IsRecurringTransaction = t.PendingApprovalTransaction != null,
                            CurrencyCode = a.Currency.Code,
                            AccountName = a.Name,
                            ExchangeRate = t.ExchangeRate
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetAvailableBalance(Guid accountId, Guid userId)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId && a.Id == accountId)
                .Select(a =>
                    (a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Income)
                        .Sum(t => (decimal?)t.Amount) ?? 0m)
                    -
                    (a.Transactions
                        .Where(t => t.TransactionType == TransactionType.Expense)
                        .Sum(t => (decimal?)t.Amount) ?? 0m))
                .FirstOrDefaultAsync();
        }
    }
}
