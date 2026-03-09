using Application.DTOs.Accounts;
using Application.DTOs.Transactions;
using Application.DTOs.RecurringTransactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Accounts;
using Application.Interfaces.UseCases.RecurringTransactions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel;

namespace Application.UseCases.Accounts
{
    public class GetAllAccounts : IGetAllAccounts
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGetAccountTransactions _getAccountTransactions;
        private readonly IMapper _mapper;
        private readonly IGetAccountRecurringTransactions _getAccountRecurringTransactions;

        public GetAllAccounts(
            IAccountRepository accountRepository,
            IGetAccountTransactions getAccountTransactions,
            IMapper mapper,
            IGetAccountRecurringTransactions getAccountRecurringTransactions)
        {
            _accountRepository = accountRepository;
            _getAccountTransactions = getAccountTransactions;
            _mapper = mapper;
            _getAccountRecurringTransactions = getAccountRecurringTransactions;
        }

        public async Task<IEnumerable<AccountDTO>> ExecuteAsync(Guid userId)
        {
            List<AccountDTO> accounts = await _accountRepository.GetAllAccountsAsync(userId);
            return accounts;
        }
    }
}
