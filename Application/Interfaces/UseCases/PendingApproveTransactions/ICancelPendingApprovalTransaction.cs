using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.PendingApproveTransactions
{
    public interface ICancelPendingApprovalTransaction
    {
        Task ExecuteAsync(Guid userId, Guid transactionId);
    }
}
