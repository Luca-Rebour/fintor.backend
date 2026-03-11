using Application.DTOs.Accounts;
using Application.DTOs.Categories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        void CreateAccount(Account account);
        Task DeleteAccountAsync(Guid accountId);
        Task<List<GetAccountDTO>> GetAccountSummariesAsync(Guid userId);
        Task<List<Account>> GetAllAccountsAsync(Guid userId);
        Task<AccountDetailDTO> GetAccountDetailAsync(Guid accountId, Guid userId);
         Task<Account> GetAccountByIdAsync(Guid accountId, Guid userId);
         Task<Account> GetAccountByIdToUpdateAsync(Guid accountId, Guid userId);
         void UpdateAccount(Account account);
         Task<List<Transaction>> GetTransactions(Guid accountId);
    }
}
