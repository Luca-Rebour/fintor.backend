using Application.DTOs.Goals;
using Application.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Interfaces.UseCases.Goals
{
    public interface IGetGoalTransactions
    {
        Task<List<TransactionDTO>> ExecuteAsync(Guid goalId, Guid userId);
    }
}
