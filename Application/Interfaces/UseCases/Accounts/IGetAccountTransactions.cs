using Application.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Accounts
{
    public interface IGetAccountTransactions
    {
        Task<List<TransactionDTO>> ExecuteAsync(Guid accountId);
    }
}
