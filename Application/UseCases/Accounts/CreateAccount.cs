using Application.DTOs.Accounts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Accounts;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Accounts
{
    public class CreateAccount : ICreateAccount
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        public CreateAccount(
            IMapper mapper,
            IJwtService jwtService,
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            ICurrencyRepository currencyRepository,
            IUnitOfWork unitOfWork
            )
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Account> ExecuteAsync(CreateAccountDTO createAccountDTO, Guid userId)
        {
            createAccountDTO.Validate();
            Currency? currency = await _currencyRepository.GetCurrencyByCodeAsync(createAccountDTO.CurrencyCode);
            if (currency == null)
            {
                currency = new Currency(createAccountDTO.CurrencyCode);
                _currencyRepository.CreateCurrency(currency);
            }

            Account newAccount = new Account(userId, currency.Id, createAccountDTO.Name, createAccountDTO.Icon);
            if (createAccountDTO.InitialBalance > 0)
            {
                Category category = await _categoryRepository.GetCategoryByName("General", userId);
                Transaction initialBalanceTransaction = new Transaction(newAccount.Id, null, category.Id, createAccountDTO.InitialBalance, "Initial balance", TransactionType.Income, createAccountDTO.ExchangeRate, null);
                _transactionRepository.CreateTransaction(initialBalanceTransaction);
            }
            _accountRepository.CreateAccount(newAccount);
            await _unitOfWork.SaveChangesAsync();
            return newAccount;
        }
    }
}
