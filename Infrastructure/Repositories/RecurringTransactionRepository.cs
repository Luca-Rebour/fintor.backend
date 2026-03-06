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
        public async Task<List<RecurringTransaction>> GetAsync(Guid userId)
        {
            return await _context.RecurringTransactions
                .AsNoTracking()
                .Include(r => r.Category)
                .Include(r => r.Account)
                    .ThenInclude(a => a.Currency)
                .Where(r => r.Account.UserId == userId)
				.ToListAsync();
        }


        public async Task<RecurringTransaction?> GetByIdAsync(Guid id)
        {
            return await _context.RecurringTransactions
                .AsNoTracking()
				.FirstOrDefaultAsync(r => r.Id == id);
        }

        public void AddAsync(RecurringTransaction recurringTransaction)
        {
            _context.RecurringTransactions.AddAsync(recurringTransaction);
        }

        public void DeleteAsync(RecurringTransaction recurringTransaction)
        {
                _context.RecurringTransactions.Remove(recurringTransaction);
        }

        public async Task<List<RecurringTransaction>> GetAccountRecurringTransactionsAsync(Guid accountId)
        {
            return await _context.RecurringTransactions
                .AsNoTracking()
                .Where(m => m.AccountId == accountId)
                .ToListAsync();
        }

        public void Update(RecurringTransaction recurringTransaction)
        {
            _context.RecurringTransactions.Update(recurringTransaction);
		}

        public async Task<List<RecurringTransaction>> GetRecurringTransactionsDueUpTo(DateOnly date)
        {
            List<RecurringTransaction> recurrents = await _context.RecurringTransactions
                .Where(r => r.NextDueDate <= date && r.NextDueDate <= r.EndDate)
                .ToListAsync();
            return recurrents;

		}

        public async Task<RecurringTransaction?> GetBydIdToUpdateAsync(Guid RecurringTransactionId)
        {
            return await _context.RecurringTransactions
                .Include(r => r.Account)
				.FirstOrDefaultAsync(r => r.Id == RecurringTransactionId);
		}

		public void Delete(RecurringTransaction recurringTransaction)
		{
			_context.RecurringTransactions.Remove(recurringTransaction);
		}
	}
}
