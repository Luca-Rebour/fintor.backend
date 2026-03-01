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
    public class GetRecurringTransactions : IGetRecurringTransactions
    {
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;
        private readonly IMapper _mapper;
        public GetRecurringTransactions(IRecurringTransactionRepository recurringTransactionRepository, IMapper mapper)
        {
            _recurringTransactionRepository = recurringTransactionRepository;
            _mapper = mapper;
        }
        public async Task<List<RecurringTransactionDTO>> ExecuteAsync(Guid userId)
        {
            List<RecurringTransaction> recurringTransactionDTOs = await _recurringTransactionRepository.GetAsync(userId);
            return _mapper.Map<List<RecurringTransactionDTO>>(recurringTransactionDTOs);
        }
    }
}
