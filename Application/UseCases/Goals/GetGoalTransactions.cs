using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Goals;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Goals
{
    public class GetGoalTransactions : IGetGoalTransactions
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetGoalTransactions(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<List<TransactionDTO>> ExecuteAsync(Guid goalId, Guid userId)
        {
            return _mapper.Map<List<TransactionDTO>>(await _transactionRepository.GetTransactionByGoalIdAsync(goalId, userId));
        }
    }
}
