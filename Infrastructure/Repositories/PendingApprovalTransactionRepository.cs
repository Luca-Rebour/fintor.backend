using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PendingApprovalTransactionRepository : IPendingApprovalTransactionRepository
    {
        private readonly FintorDbContext _context;
        public PendingApprovalTransactionRepository(FintorDbContext context)
        {
            _context = context;
        }
        public void CreatePendingApprovalTransaction(PendingApprovalTransaction recurringTransaction)
        {
            _context.PendingAprovalTransactions.Add(recurringTransaction);
        }

        public void DeleteAsync(PendingApprovalTransaction pendingAprovalTransaction)
        {
            _context.PendingAprovalTransactions.Remove(pendingAprovalTransaction);
        }

        public async Task<List<PendingApprovalTransaction>> GetAccountPendingApprovalTransactionsAsync(Guid accountId)
        {
            List<PendingApprovalTransaction> pendingAprovalTransactions = await _context.PendingAprovalTransactions
                .AsNoTracking()
                .Where(p => p.AccountId == accountId)
                .ToListAsync();
            return pendingAprovalTransactions;
        }

        public async Task<List<PendingApprovalTransaction>> GetAsync(Guid userId)
        {
            List<PendingApprovalTransaction> pendingAprovalTransactions = await _context.PendingAprovalTransactions
                    .AsNoTracking()
                    .Where(p => p.Account.UserId == userId)
                    .ToListAsync();
            return pendingAprovalTransactions;
        }

        public async Task<PendingApprovalTransaction?> GetByIdAsync(Guid id)
        {
            return await _context.PendingAprovalTransactions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PendingApprovalTransaction?> GetByIdToUpdateAsync(Guid id)
        {
            return await _context.PendingAprovalTransactions
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(PendingApprovalTransaction recurringTransaction)
        {
            _context.PendingAprovalTransactions.Update(recurringTransaction);
        }
    }
}
