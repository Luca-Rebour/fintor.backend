using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Transactions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Transactions
{
    public class CreateTransaction : ICreateTransaction
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CreateTransaction(ITransactionRepository transactionRepository, IAccountRepository accountRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionDTO> ExecuteAsync(CreateTransactionDTO dto, Guid userId)
        {
            dto.Validate();
            if(dto.TransactionType == TransactionType.Expense && await _accountRepository.GetAvailableBalance(dto.AccountId, userId) < dto.Amount)
            {
                throw new BusinessRuleException("Insufficient funds.", ErrorCode.InsufficientBalance);
            }
            Transaction transaction = new Transaction(dto.AccountId, dto.RecurringTransactionId, dto.CategoryId, dto.Amount, dto.Description, dto.TransactionType, dto.ExchangeRate, null);
            _transactionRepository.CreateTransaction(transaction);
            await _unitOfWork.SaveChangesAsync();
            transaction = await _transactionRepository.GetTransactionAsync(transaction.Id, userId);
            TransactionDTO ret = _mapper.Map<TransactionDTO>(transaction);
            return ret;
        }
    }
}
