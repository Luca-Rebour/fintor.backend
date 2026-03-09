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
            List<Account> accounts = await _accountRepository.GetAllAccountsAsync(userId);
            List<AccountDTO> ret = _mapper.Map<List<AccountDTO>>(accounts);

            foreach (AccountDTO accountDto in ret)
            {
                List<TransactionDTO> transactions = await _getAccountTransactions.ExecuteAsync(accountDto.Id);
                List<RecurringTransactionDTO> recurringTransactions = await _getAccountRecurringTransactions.ExecuteAsync(accountDto.Id);
                accountDto.Balance = transactions.Any()
                    ? transactions
                        .Where(m => m.TransactionType == TransactionType.Income).Sum(m => m.Amount)
                        - transactions.Where(m => m.TransactionType == TransactionType.Expense).Sum(m => m.Amount)
                    : 0;

            }


            return ret;
        }
    }
}
