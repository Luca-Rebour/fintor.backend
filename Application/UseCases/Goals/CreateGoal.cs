using Application.DTOs.Goals;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Goals;
using Application.Interfaces.UseCases.Transactions;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Goals
{
    public class CreateGoal : ICreateGoal
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateGoal(IGoalRepository goalRepository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository)
        {
            _goalRepository = goalRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<GoalDTO> ExecuteAsync(CreateGoalDTO dto, Guid userId)
        {
            dto.Validate();

            Goal goal = _mapper.Map<Goal>(dto);

            if (dto.CurrentAmount is > 0)
            {
                Category category = await _categoryRepository.GetCategoryByName("General", userId);

                Transaction tx = new Transaction(
                    dto.AccountId,
                    null,
                    category.Id,
                    dto.CurrentAmount,
                    $"{dto.Title}: Initial amount",
                    Domain.Enums.TransactionType.Income,
                    dto.ExchangeRate,
                    goal.Id
                );

                tx.SetGoal(goal);

                _transactionRepository.CreateTransaction(tx);
            }

            _goalRepository.Add(goal);

            await _unitOfWork.SaveChangesAsync();

            GoalDTO? createdGoal = await _goalRepository.GetByIdAsync(goal.Id, userId);
            if (createdGoal == null)
            {
                throw new NotFoundException("Goal");
            }

            return createdGoal;
        }
    }
}
