using Application.DTOs.RecurringTransactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.RecurringTransactions;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.RecurringTransactions
{
    public class CreateRecurringTransaction : ICreateRecurringTransaction
    {
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CreateRecurringTransaction(IRecurringTransactionRepository recurringTransactionRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _recurringTransactionRepository = recurringTransactionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<RecurringTransactionDTO> ExecuteAsync(CreateRecurringTransactionDTO createRecurringTransactionDTO)
        {
            RecurringTransaction recurringTransaction = new RecurringTransaction(createRecurringTransactionDTO.CategoryId, createRecurringTransactionDTO.AccountId, createRecurringTransactionDTO.Name, createRecurringTransactionDTO.Amount, createRecurringTransactionDTO.Description, createRecurringTransactionDTO.transactionType, createRecurringTransactionDTO.Frequency, createRecurringTransactionDTO.StartDate, createRecurringTransactionDTO.EndDate);
            _recurringTransactionRepository.AddAsync(recurringTransaction);
            RecurringTransactionDTO recurringTransactionDTO = _mapper.Map<RecurringTransactionDTO>(recurringTransaction);
            await _unitOfWork.SaveChangesAsync();
            return recurringTransactionDTO;
        }
    }
}
