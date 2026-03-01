using Application.DTOs.Categories;
using Application.DTOs.Reports;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FintorDbContext _context;
        public TransactionRepository(FintorDbContext context)
        {
            _context = context;
        }

        public async void CreateTransactionAsync(Transaction movement)
        {
            _context.Transactions.Add(movement);
            return;
        }

        public async Task<List<Transaction>> GetAccountTransactionsAsync(Guid accountId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(m => m.AccountId == accountId)
                .Include(m => m.Category)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<OverviewResponseDTO>> GetOverview(Guid userId)
        {
            List<OverviewRowSql> rows = await _context.Database
                .SqlQueryRaw<OverviewRowSql>("EXEC dbo.usp_GetOverview {0}", userId)
                .ToListAsync();

            return rows.Select(r => new OverviewResponseDTO(
                    r.TotalBalance,
                    r.TotalIncome,
                    r.TotalExpense,
                    JsonSerializer.Deserialize<List<CategorySummaryDto>>(r.CategorySpendingJson) ?? new(),
                    JsonSerializer.Deserialize<List<CategorySummaryDto>>(r.CategoryEarningJson) ?? new()
                )
            {
                DaysAgo = r.DaysAgo
            })
                .ToList();
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(Guid userId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.Account.UserId == userId)
                .Include(t => t.Category)
                .Include(t => t.Account)
                    .ThenInclude(a => a.Currency)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Transaction?> GetTransactionAsync(Guid transactionId, Guid userId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Include(t => t.Category)
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.Account.UserId == userId);
        }

        public async Task<Transaction> GetTrackedTransactionAsync(Guid transactionId, Guid userId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.Account.UserId == userId);
        }

        public void RemoveTransaction(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
        }
    }
}
