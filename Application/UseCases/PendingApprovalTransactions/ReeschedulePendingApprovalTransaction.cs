using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.PendingApproveTransactions;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.PendingApprovalTransactions
{
    public class ReschedulePendingApprovalTransaction : IReschedulePendingApprovalTransaction
    {
        private readonly IPendingApprovalTransactionRepository _pendingApprovalTransactionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ReschedulePendingApprovalTransaction(IPendingApprovalTransactionRepository pendingApprovalTransactionRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _pendingApprovalTransactionRepository = pendingApprovalTransactionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task ExecuteAsync(Guid transactionId, Guid userId, DateOnly newDate)
        {
            PendingApprovalTransaction? pendingApprovalTransaction = await _pendingApprovalTransactionRepository.GetByIdToUpdateAsync(transactionId);
            if (pendingApprovalTransaction == null)
            {
                throw new NotFoundException("Pending Transaction Not Found");
            }
            pendingApprovalTransaction.Reschedule(newDate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
