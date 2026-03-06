using Application.DTOs.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Goals
{
    public interface ICreateGoal
    {
        Task<GoalDTO> ExecuteAsync(CreateGoalDTO createGoalDTO, Guid userId);
    }
}
