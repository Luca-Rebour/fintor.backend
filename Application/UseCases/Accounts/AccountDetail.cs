using Application.DTOs.Accounts;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Accounts;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Accounts
{
    public class AccountDetail : IAccountDetail
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        public AccountDetail(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
        public async Task<AccountDetailDTO> ExecuteAsync(Guid accountId, Guid userId)
        {
            AccountDetailDTO? accountDetail = await _accountRepository.GetAccountDetailAsync(userId, accountId);
            if (accountDetail == null)
            {
                throw new NotFoundException("Account");
            }

            return accountDetail;

        }
    }
}
