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
    public class CreateRecurringTransaction : ICreateRecurringMovement
    {
        private readonly IRecurringTransactionRepository _recurringMovementRepository;
        private readonly IMapper _mapper;
        public CreateRecurringTransaction(IRecurringTransactionRepository recurringMovementRepository, IMapper mapper)
        {
            _recurringMovementRepository = recurringMovementRepository;
            _mapper = mapper;
        }
        public async Task<RecurringTransactionDTO> ExecuteAsync(CreateRecurringTransactionDTO createRecurringMovementDTO)
        {
            RecurringTransaction recurringMovement = new RecurringTransaction(createRecurringMovementDTO.CategoryId, createRecurringMovementDTO.AccountId, createRecurringMovementDTO.Name, createRecurringMovementDTO.Amount, createRecurringMovementDTO.Description, createRecurringMovementDTO.movementType, createRecurringMovementDTO.Frequency, createRecurringMovementDTO.StartDate, createRecurringMovementDTO.EndDate);
            await _recurringMovementRepository.AddAsync(recurringMovement);
            RecurringTransactionDTO recurringMovementDTO = _mapper.Map<RecurringTransactionDTO>(recurringMovement);
            return recurringMovementDTO;
        }
    }
}
