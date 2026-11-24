using Application.DTOs.Goals;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGoalRepository
    {
        Task<bool> BelongsToAccountAsync(Guid goalId, Guid accountId, Guid userId);
            Task<GoalDTO?> GetByIdAsync(Guid goalId, Guid userId);
            Task<List<GoalDTO>> GetAllAsync(Guid userId);
            void Add(Goal goal);
            void Update(Goal goal);
        void Delete(Goal goal);
    }
}
