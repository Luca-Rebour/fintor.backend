using Application.DTOs;
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
    public class GetPendingApprovalTransactions : IGetPendingApprovalTransactions
    {
        private readonly IPendingApprovalTransactionRepository _pendingApprovalTransactionRepository;
        private readonly IMapper _mapper;
        public GetPendingApprovalTransactions(IPendingApprovalTransactionRepository pendingApprovalTransactionRepository, IMapper mapper)
        {
            _pendingApprovalTransactionRepository = pendingApprovalTransactionRepository;
            _mapper = mapper;
        }

        public async Task<List<PendingApprovalTransactionDTO>> ExecuteAsync(Guid userId)
        {
            List<PendingApprovalTransaction> pendingApprovalTransactions = await _pendingApprovalTransactionRepository.GetAsync(userId);
            return _mapper.Map<List<PendingApprovalTransactionDTO>>(pendingApprovalTransactions);
        }
    }
}
