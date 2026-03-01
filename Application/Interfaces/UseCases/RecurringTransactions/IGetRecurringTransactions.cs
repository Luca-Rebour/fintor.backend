using Application.DTOs.RecurringTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.RecurringTransactions
{
    public interface IGetRecurringTransactions
    {
        Task<List<RecurringTransactionDTO>> ExecuteAsync(Guid userId);
    }
}
