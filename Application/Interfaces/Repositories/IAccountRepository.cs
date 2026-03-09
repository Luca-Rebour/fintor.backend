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
        Task<List<AccountDTO>> GetAllAccountsAsync(Guid userId);
        Task<AccountDetailDTO?> GetAccountDetailAsync(Guid userId, Guid accountId);
        Task<decimal> GetAvailableBalance(Guid accountId, Guid userId);

    }
}
