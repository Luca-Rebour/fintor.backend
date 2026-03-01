using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPendingApprovalTransactionRepository
    {
        void CreatePendingApprovalTransaction(PendingApprovalTransaction recurringTransaction);
        Task<List<PendingApprovalTransaction>> GetAsync(Guid userId);
        Task<PendingApprovalTransaction?> GetByIdAsync(Guid id);
        Task<PendingApprovalTransaction?> GetByIdToUpdateAsync(Guid id);
        void Update(PendingApprovalTransaction recurringTransaction);
        void DeleteAsync(PendingApprovalTransaction pendingAprovalTransaction);
        Task<List<PendingApprovalTransaction>> GetAccountPendingApprovalTransactionsAsync(Guid accountId);
    }
}
