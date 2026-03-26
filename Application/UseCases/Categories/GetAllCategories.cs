using Application.DTOs.Categories;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Categories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Categories
{
    public class GetAllCategories : IGetAllCategories
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public GetAllCategories(ICategoryRepository categoryRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<List<CategoryDTO>> Execute(Guid userId)
        {
            List<Category> categories = await _categoryRepository.GetAllAsync(userId);
            List<CategoryDTO> ret = _mapper.Map<List<CategoryDTO>>(categories);

            List<Transaction> transactions = await _transactionRepository.GetAllTransactionsAsync(userId);

            Dictionary<Guid, decimal> totalSpentByCategory = transactions
                .Where(t => t.TransactionType == TransactionType.Expense)
                .GroupBy(t => t.CategoryId)
                .ToDictionary(group => group.Key, group => group.Sum(t => t.Amount));

            foreach (CategoryDTO category in ret)
            {
                if (totalSpentByCategory.TryGetValue(category.Id, out decimal totalSpent))
                {
                    category.TotalSpent = totalSpent;
                }
            }
            return ret;
        }
    }
}
