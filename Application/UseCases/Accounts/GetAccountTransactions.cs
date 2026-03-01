using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Accounts;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Accounts
{
    public class GetAccountTransactions : IGetAccountTransactions
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public GetAccountTransactions(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<List<TransactionDTO>> ExecuteAsync(Guid accountId)
        {
            List<Transaction> transactions = await _transactionRepository.GetAccountTransactionsAsync(accountId);
            List<TransactionDTO> ret = _mapper.Map<List<TransactionDTO>>(transactions);
            return ret;
        }
    }
}
