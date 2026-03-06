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
    public class DeleteRecurringTransaction : IDeleteRecurringTransaction
    {
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRecurringTransaction(IRecurringTransactionRepository recurringTransactionRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _recurringTransactionRepository = recurringTransactionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task ExecuteAsync(Guid recurringTransactionId, Guid userId)
        {
            RecurringTransaction? recurringTransaction = await _recurringTransactionRepository.GetBydIdToUpdateAsync(recurringTransactionId);
            if(recurringTransaction == null)
            {
                throw new KeyNotFoundException("Recurring transaction not found.");
            }

            if (!recurringTransaction.Account.UserId.Equals(userId))
            {
                throw new UnauthorizedAccessException("The user does not have access to the requested action.");
            }
            _recurringTransactionRepository.Delete(recurringTransaction);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
