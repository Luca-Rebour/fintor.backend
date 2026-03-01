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
    public class DeleteTransaction : IDeleteTransaction
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteTransaction(ITransactionRepository transactionRepository,IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task ExecuteAsync(Guid transactionId, Guid userId)
        {
            Transaction transaction = await _transactionRepository.GetTrackedTransactionAsync(transactionId, userId);
            _transactionRepository.RemoveTransaction(transaction);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
