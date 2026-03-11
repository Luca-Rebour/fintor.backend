using Application.DTOs.Accounts;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Accounts;

namespace Application.UseCases.Accounts
{
    public class GetAllAccounts : IGetAllAccounts
    {
        private readonly IAccountRepository _accountRepository;

        public GetAllAccounts(
            IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<GetAccountDTO>> ExecuteAsync(Guid userId)
        {
            return await _accountRepository.GetAccountSummariesAsync(userId);
        }
    }
}
