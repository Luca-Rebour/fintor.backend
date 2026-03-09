using Application.DTOs.Accounts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Accounts
{
    public interface ICreateAccount
    {
        Task<Account> ExecuteAsync(CreateAccountDTO createAccountDTO, Guid userId);
    }
}
