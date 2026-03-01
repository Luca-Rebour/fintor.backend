using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.PendingApproveTransactions
{
    public interface IGetPendingApprovalTransactions
    {
        Task<List<PendingApprovalTransactionDTO>> ExecuteAsync(Guid userId);
    }
}
