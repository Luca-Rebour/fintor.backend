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
        Task<List<RecurringTransaction>> GetAllAsync();
        Task<RecurringTransaction?> GetByIdAsync(Guid id);
        Task AddAsync(RecurringTransaction recurringTransaction);
        Task UpdateAsync(RecurringTransaction recurringTransaction);
        Task DeleteAsync(Guid id);
        Task<List<RecurringTransaction>> GetAccountRecurringTransactionsAsync (Guid accountId);
    }
}
