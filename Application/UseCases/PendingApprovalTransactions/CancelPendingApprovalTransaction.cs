using Application.DTOs.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.PendingApproveTransactions;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.PendingApprovalTransactions
{
    public class CancelPendingApprovalTransaction : ICancelPendingApprovalTransaction
    {
        private readonly IPendingApprovalTransactionRepository _pendingApprovalTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CancelPendingApprovalTransaction(IPendingApprovalTransactionRepository pendingApprovalTransactionRepository, IUnitOfWork unitOfWork)
        {
            _pendingApprovalTransactionRepository = pendingApprovalTransactionRepository;
            _unitOfWork = unitOfWork;        }

        public async Task ExecuteAsync(Guid pendingApprovalTransactionId, Guid userId)
        {
            PendingApprovalTransaction? pendingApprovalTransaction = await _pendingApprovalTransactionRepository.GetByIdToUpdateAsync(pendingApprovalTransactionId);

            if (pendingApprovalTransaction == null)
            {
                throw new KeyNotFoundException("Pending approval transaction not found");
            }

            if (!pendingApprovalTransaction.Account.UserId.Equals(_pendingApprovalTransactionRepository))
            {
                throw new UnauthorizedAccessException("User does not have access to do the requested action");
            }

            pendingApprovalTransaction.Cancel();
            _pendingApprovalTransactionRepository.Update(pendingApprovalTransaction);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
