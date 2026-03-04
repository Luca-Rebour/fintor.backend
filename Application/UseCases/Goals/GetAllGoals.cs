using Application.DTOs.Goals;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Goals;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Goals
{
    public class GetAllGoals : IGetAllGoals
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IMapper _mapper;

        public GetAllGoals(IGoalRepository goalRepository,IMapper mapper)
        {
            _goalRepository = goalRepository;
            _mapper = mapper;
        }
        public async Task<List<GoalDTO>> ExecuteAsync(Guid userId)
        {
            List<GoalDTO> goals = await _goalRepository.GetAllAsync(userId);
            return goals;
        }
    }
}
