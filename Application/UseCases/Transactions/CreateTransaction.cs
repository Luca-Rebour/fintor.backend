using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Transactions;
using AutoMapper;
using Domain.Entities;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly IGoalRepository _goalRepository;
        public CreateTransaction(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper, IUnitOfWork unitOfWork, IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionDTO> ExecuteAsync(CreateTransactionDTO dto, Guid userId)
        {
            dto.Validate();
            await _accountRepository.GetAccountByIdAsync(dto.AccountId, userId);
            if (dto.GoalId is Guid goalId && !await _goalRepository.BelongsToAccountAsync(goalId, dto.AccountId, userId))
            {
                throw new NotFoundException("Goal for the selected account");
            }
            Transaction transaction = new Transaction(dto.AccountId, dto.RecurringTransactionId, dto.CategoryId, dto.Amount, dto.Description, dto.TransactionType, dto.ExchangeRate, dto.GoalId);
            _transactionRepository.CreateTransaction(transaction);
            await _unitOfWork.SaveChangesAsync();
            transaction = await _transactionRepository.GetTransactionAsync(transaction.Id, userId);
            TransactionDTO ret = _mapper.Map<TransactionDTO>(transaction);
            return ret;
        }
    }
}
