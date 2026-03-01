using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RecurringTransactionRepository : IRecurringTransactionRepository
    {
        private readonly FintorDbContext _context;
        public RecurringTransactionRepository(FintorDbContext context)
        {
            _context = context;
        }

        public async Task<RecurringTransaction> CreateRecurringTransactionAsync(RecurringTransaction recurringTransaction)
        {
            _context.RecurringTransactions.Add(recurringTransaction);
            await _context.SaveChangesAsync();
            return recurringTransaction;
        }
        public async Task<List<RecurringTransaction>> GetAllAsync()
        {
            return await _context.RecurringTransactions.ToListAsync();
        }


        public async Task<RecurringTransaction?> GetByIdAsync(Guid id)
        {
            return await _context.RecurringTransactions
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(RecurringTransaction recurringTransaction)
        {
            await _context.RecurringTransactions.AddAsync(recurringTransaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecurringTransaction recurringTransaction)
        {
            _context.RecurringTransactions.Update(recurringTransaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.RecurringTransactions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<RecurringTransaction>> GetAccountRecurringTransactionsAsync(Guid accountId)
        {
            return await _context.RecurringTransactions
                .Where(m => m.AccountId == accountId)
                .ToListAsync();
        }
    }
}
