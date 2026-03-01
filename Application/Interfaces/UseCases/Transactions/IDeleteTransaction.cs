using Application.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Transactions
{
    public interface IDeleteTransaction
    {
        Task ExecuteAsync(Guid transactionId, Guid userId);
    }
}
