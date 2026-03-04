using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Transactions;
using AutoMapper;
using Domain.Entities;
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
        public CreateTransaction(ITransactionRepository transactionRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionDTO> ExecuteAsync(CreateTransactionDTO dto, Guid userId)
        {
            dto.Validate();
            Transaction transaction = new Transaction(dto.AccountId, dto.RecurringTransactionId, dto.CategoryId, dto.Amount, dto.Description, dto.TransactionType, dto.ExchangeRate, null);
            _transactionRepository.CreateTransaction(transaction);
            await _unitOfWork.SaveChangesAsync();
            transaction = await _transactionRepository.GetTransactionAsync(transaction.Id, userId);
            TransactionDTO ret = _mapper.Map<TransactionDTO>(transaction);
            return ret;
        }
    }
}
