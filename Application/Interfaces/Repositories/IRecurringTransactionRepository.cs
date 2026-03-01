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
        Task<RecurringTransaction> CreateRecurringMovementAsync(RecurringTransaction recurringMovement);
        Task<List<RecurringTransaction>> GetAllAsync();
        Task<RecurringTransaction?> GetByIdAsync(Guid id);
        Task AddAsync(RecurringTransaction recurringMovement);
        Task UpdateAsync(RecurringTransaction recurringMovement);
        Task DeleteAsync(Guid id);
        Task<List<RecurringTransaction>> GetAccountRecurringMovementsAsync (Guid accountId);
    }
}
