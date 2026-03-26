using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<Category> GetCategoryByName(string name, Guid userId);
        Task<Category?> GetCategoryByNameOrNullAsync(string name, Guid userId);
        Task<List<Category>> GetAllAsync(Guid userId);
        Task<Category?> GetTrackedByIdAsync(Guid categoryId, Guid userId);
        void Delete(Category category);
    }
}
