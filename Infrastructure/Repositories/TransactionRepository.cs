using Application.DTOs.Categories;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : IMovementRepository
    {
        private readonly FintorDbContext _context;
        public TransactionRepository(FintorDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateMovementAsync(Transaction movement)
        {
            _context.Transactions.Add(movement);
            await _context.SaveChangesAsync();
            return movement;
        }

        public async Task<List<Transaction>> GetAccountMovementsAsync(Guid accountId)
        {
            return await _context.Transactions
                .Where(m => m.AccountId == accountId)
                .Include(m => m.Category)
                .ToListAsync();
        }

        public async Task<List<CategorySummaryDto>> GetCategorySpending(Guid userId, int filter)
        {
            var sql = @"
        SELECT 
            t.CategoryId,
            c.Name as CategoryName,
            SUM(t.Amount) as Total
        FROM Transactions t
        INNER JOIN Accounts a ON t.AccountId = a.Id
        INNER JOIN Categories c ON t.CategoryId = c.Id
        WHERE t.MovementType = 1 
        AND t.Date >= DATEADD(DAY, -{1}, GETUTCDATE())
        AND a.UserId = {0}
        GROUP BY t.CategoryId, c.Name
        ORDER BY SUM(t.Amount) DESC";

            return await _context.Database
                .SqlQueryRaw<CategorySummaryDto>(sql, userId, filter)
                .ToListAsync();
        }

        public async Task<List<CategorySummaryDto>> GetCategoryEarning(Guid userId, int filter)
        {
            var sql = @"
        SELECT 
            t.CategoryId,
            c.Name as CategoryName,
            SUM(t.Amount) as Total
        FROM Transactions t
        INNER JOIN Accounts a ON t.AccountId = a.Id
        INNER JOIN Categories c ON t.CategoryId = c.Id
        WHERE t.MovementType = 0 
          AND a.UserId = {0}
          AND t.Date >= DATEADD(DAY, -{1}, GETUTCDATE())
        GROUP BY t.CategoryId, c.Name
        ORDER BY SUM(t.Amount) DESC";

            return await _context.Database
                .SqlQueryRaw<CategorySummaryDto>(sql, userId, filter)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalExpense(Guid userId, int filter)
        {
            var fromDate = DateTime.UtcNow.AddDays(-filter);

            return await _context.Transactions
                    .Where(t => t.MovementType == TransactionType.Expense
                             && t.Account.UserId == userId
                             && t.Date >= fromDate)
                    .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalIncome(Guid userId, int filter)
        {
            var fromDate = DateTime.UtcNow.AddDays(-filter);

            return await _context.Transactions
                .Where(t => t.MovementType == TransactionType.Income
                         && t.Account.UserId == userId
                         && t.Date >= fromDate)
                .SumAsync(t => t.Amount);
        }
    }
}
