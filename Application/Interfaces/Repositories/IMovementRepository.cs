using Application.DTOs.Categories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IMovementRepository
    {
        Task<Transaction> CreateMovementAsync(Transaction movement);
        Task<List<Transaction>> GetAccountMovementsAsync(Guid accountId);
        Task<decimal> GetTotalIncome(Guid userId, int filter);
        Task<decimal> GetTotalExpense(Guid userId, int filter);
        Task<List<CategorySummaryDto>> GetCategorySpending(Guid userId, int filter);
        Task<List<CategorySummaryDto>> GetCategoryEarning(Guid userId, int filter);
    }
}
