using Application.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Accounts
{
    public interface IGetAccountDetail
    {
        Task<AccountDetailDTO> ExecuteAsync(Guid accountId, Guid userId);
    }
}
