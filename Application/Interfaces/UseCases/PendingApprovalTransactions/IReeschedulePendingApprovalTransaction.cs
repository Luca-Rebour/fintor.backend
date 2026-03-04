using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.PendingApproveTransactions
{
    public interface IReschedulePendingApprovalTransaction
    {
        Task ExecuteAsync(Guid transactionId, Guid userId, DateOnly newDate);
    }
}
