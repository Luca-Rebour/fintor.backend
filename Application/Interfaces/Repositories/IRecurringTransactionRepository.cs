using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRecurringTransactionRepository
    {
        Task<RecurringTransaction> CreateRecurringTransactionAsync(RecurringTransaction recurringTransaction);
        Task<RecurringTransaction?> GetByIdAsync(Guid id);
        void AddAsync(RecurringTransaction recurringTransaction);
        void Update(RecurringTransaction recurringTransaction);
        void DeleteAsync(RecurringTransaction recurringTransaction);
        Task<List<RecurringTransaction>> GetAccountRecurringTransactionsAsync (Guid accountId);
        Task<List<RecurringTransaction>> GetAsync(Guid userId);
        Task<RecurringTransaction?> GetBydIdToUpdateAsync(Guid RecurringTransactionId);
        Task<List<RecurringTransaction>> GetRecurringTransactionsDueUpTo(DateOnly date);
        void Delete(RecurringTransaction recurringTransaction);
    }
}
