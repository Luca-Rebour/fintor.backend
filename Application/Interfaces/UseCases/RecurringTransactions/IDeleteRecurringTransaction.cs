using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.RecurringTransactions
{
    public interface IDeleteRecurringTransaction
    {
        Task ExecuteAsync(Guid recurringTransactionId, Guid userId);
    }
}
