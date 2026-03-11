using Application.DTOs.Accounts;

namespace Application.Interfaces.UseCases.Accounts
{
    public interface ICreateAccount
    {
        Task<GetAccountDTO> ExecuteAsync(CreateAccountDTO createAccountDTO, Guid userId);
    }
}
