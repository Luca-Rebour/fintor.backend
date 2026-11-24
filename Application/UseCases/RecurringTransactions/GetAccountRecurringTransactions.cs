using Application.DTOs.Transactions;
using Application.DTOs.RecurringTransactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.RecurringTransactions;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.RecurringTransactions
{

    public class GetAccountRecurringTransactions : IGetAccountRecurringTransactions
    {
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        public GetAccountRecurringTransactions(IAccountRepository accountRepository, IRecurringTransactionRepository recurringTransactionRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _recurringTransactionRepository = recurringTransactionRepository;
            _mapper = mapper;
        }
        public async Task<List<RecurringTransactionDTO>> ExecuteAsync(Guid accountId, Guid userId)
        {
            await _accountRepository.GetAccountByIdAsync(accountId, userId);
            List<RecurringTransaction> recurringTransactions = await _recurringTransactionRepository.GetAccountRecurringTransactionsAsync(accountId, userId);
            List<RecurringTransactionDTO> ret = _mapper.Map<List<RecurringTransactionDTO>>(recurringTransactions);
            return ret;
        }
    }
}
