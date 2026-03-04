using Application.DTOs.Goals;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly FintorDbContext _context;
        public GoalRepository(FintorDbContext context)
        {
            _context = context;
        }
        public void Add(Goal goal)
        {
            _context.Goals.Add(goal);
        }

        public void Delete(Goal goal)
        {
            _context.Goals.Remove(goal);
        }

        public async Task<List<GoalDTO>> GetAllAsync(Guid userId)
        {
            return await _context.Goals
                .AsNoTracking()
                .Where(g => g.Account.UserId == userId)
                .Select(g => new GoalDTO
                {
                    Id = g.Id,
                    Title = g.Title,
                    Icon = g.Icon,
                    AccentColor = g.AccentColor,
                    Description = g.Description,
                    TargetDate = g.TargetDate,
                    TargetAmount = g.TargetAmount,
                    AccountName = g.Account.Name,
                    CurrentAmount = g.Transactions.Sum(t => t.Amount)
                })
                .ToListAsync();
        }

        public async Task<GoalDTO?> GetByIdAsync(Guid goalId, Guid userId)
        {
            return await _context.Goals
                .AsNoTracking()
                .Where(g => g.Id == goalId && g.Account.UserId == userId)
                .Select(g => new GoalDTO
                {
                    Id = g.Id,
                    Title = g.Title,
                    Icon = g.Icon,
                    AccentColor = g.AccentColor,
                    Description = g.Description,
                    TargetDate = g.TargetDate,
                    TargetAmount = g.TargetAmount,
                    AccountName = g.Account.Name,
                    CurrentAmount = g.Transactions.Sum(t => t.Amount)
                })
                .FirstOrDefaultAsync();
        }

        public void Update(Goal goal)
        {
            _context.Goals.Update(goal);
        }
    }
}
