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
        private readonly IGetAccountTransactions _getAccountMovements;
        private readonly IMapper _mapper;
        private readonly IGetAccountRecurringMovements _getAccountRecurringMovements;

        public GetAllAccounts(
            IAccountRepository accountRepository,
            IGetAccountTransactions getAccountMovements,
            IMapper mapper,
            IGetAccountRecurringMovements getAccountRecurringMovements)
        {
            _accountRepository = accountRepository;
            _getAccountMovements = getAccountMovements;
            _mapper = mapper;
            _getAccountRecurringMovements = getAccountRecurringMovements;
        }

        public async Task<IEnumerable<AccountDTO>> ExecuteAsync(Guid userId)
        {
            List<Account> accounts = await _accountRepository.GetAllAccountsAsync(userId);
            List<AccountDTO> ret = _mapper.Map<List<AccountDTO>>(accounts);

            foreach (AccountDTO accountDto in ret)
            {
                List<TransactionDTO> movements = await _getAccountMovements.ExecuteAsync(accountDto.Id);
                List<RecurringTransactionDTO> recurringMovements = await _getAccountRecurringMovements.ExecuteAsync(accountDto.Id);
                accountDto.Balance = movements.Any()
                    ? movements
                        .Where(m => m.TransactionType == TransactionType.Income).Sum(m => m.Amount)
                        - movements.Where(m => m.TransactionType == TransactionType.Expense).Sum(m => m.Amount)
                    : 0;

            }


            return ret;
        }
    }
}
