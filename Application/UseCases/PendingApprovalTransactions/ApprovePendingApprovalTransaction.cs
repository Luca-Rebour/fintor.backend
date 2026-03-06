using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.PendingApproveTransactions;
using Application.Interfaces.UseCases.Transactions;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.PendingApprovalTransactions
{
    public class ApprovePendingApprovalTransaction : IApprovePendingApprovalTransaction
    {
        private readonly IPendingApprovalTransactionRepository _pendingApprovalTransactionRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ApprovePendingApprovalTransaction(IPendingApprovalTransactionRepository pendingApprovalTransactionRepository, IMapper mapper, IUnitOfWork unitOfWork, ITransactionRepository transactionRepository)
        {
            _pendingApprovalTransactionRepository = pendingApprovalTransactionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
        }

        public async Task ExecuteAsync(Guid pendingApprovalTransactionId, Guid userId, decimal? exchangeRate)
        {
            PendingApprovalTransaction? pendingApprovalTransaction = await _pendingApprovalTransactionRepository.GetByIdToUpdateAsync(pendingApprovalTransactionId);
            
            if (pendingApprovalTransaction == null)
            {
                throw new KeyNotFoundException("Pending approval transaction not found");
            }

            if (!pendingApprovalTransaction.Account.UserId.Equals(userId))
            {
                throw new UnauthorizedAccessException("User does not have access to do the requested action");
            }

            Transaction transaction = new Transaction(pendingApprovalTransaction.AccountId, pendingApprovalTransaction.Id, pendingApprovalTransaction.CategoryId, pendingApprovalTransaction.Amount, pendingApprovalTransaction.Description, pendingApprovalTransaction.TransactionType, exchangeRate, null);
            _transactionRepository.CreateTransaction(transaction);
            pendingApprovalTransaction.Approve(DateOnly.FromDateTime(DateTime.UtcNow), transaction.Id);
            _pendingApprovalTransactionRepository.Update(pendingApprovalTransaction);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
