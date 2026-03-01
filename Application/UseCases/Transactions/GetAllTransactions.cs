using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using AutoMapper;

namespace Application.UseCases.Transactions
{
    public class GetAllTransactions : IGetAllTransactions
    {
        private ITransactionRepository _transactionRepository;
        private IMapper _mapper;
        public GetAllTransactions(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<List<TransactionDTO>> ExecuteAsync(Guid userId)
        {
            List<Transaction> transactions = await _transactionRepository.GetAllTransactionsAsync(userId);
            return _mapper.Map<List<TransactionDTO>>(transactions);
        }
    }
}
