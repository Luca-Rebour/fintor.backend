using Application.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Transactions
{
    public interface IGetAllTransactions
    {
        Task<List<TransactionDTO>> ExecuteAsync(Guid userId);
    }
}
