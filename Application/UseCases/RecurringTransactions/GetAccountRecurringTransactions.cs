using Application.DTOs.Transactions;
using Application.DTOs.RecurringTransactions;
using Application.Interfaces.Repositories;
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

    public class GetAccountRecurringTransactions : IGetAccountRecurringMovements
    {
        private readonly IRecurringTransactionRepository _recurringMovementRepository;
        private readonly IMapper _mapper;
        public GetAccountRecurringTransactions(IRecurringTransactionRepository recurringMovementRepository, IMapper mapper)
        {
            _recurringMovementRepository = recurringMovementRepository;
            _mapper = mapper;
        }
        public async Task<List<RecurringTransactionDTO>> ExecuteAsync(Guid accountId)
        {
            List<RecurringTransaction> recurringMovements = await _recurringMovementRepository.GetAccountRecurringMovementsAsync(accountId);
            List<RecurringTransactionDTO> ret = _mapper.Map<List<RecurringTransactionDTO>>(recurringMovements);
            return ret;
        }
    }
}
