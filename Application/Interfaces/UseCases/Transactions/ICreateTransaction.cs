using Application.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Transactions
{
    public interface ICreateTransaction
    {
        Task<TransactionDTO> ExecuteAsync(CreateTransactionDTO dto, Guid userId);
    }
}
