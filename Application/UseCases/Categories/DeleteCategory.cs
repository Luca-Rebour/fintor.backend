using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Categories;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.UseCases.Categories
{
    public class DeleteCategory : IDeleteCategory
    {
        private const string GeneralCategoryName = "General";
        private const string GeneralCategoryIcon = "ri-shapes-fill";
        private const string GeneralCategoryColor = "#64748B";

        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRecurringTransactionRepository _recurringTransactionRepository;
        private readonly IPendingApprovalTransactionRepository _pendingApprovalTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategory(
            ICategoryRepository categoryRepository,
            ITransactionRepository transactionRepository,
            IRecurringTransactionRepository recurringTransactionRepository,
            IPendingApprovalTransactionRepository pendingApprovalTransactionRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _transactionRepository = transactionRepository;
            _recurringTransactionRepository = recurringTransactionRepository;
            _pendingApprovalTransactionRepository = pendingApprovalTransactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid categoryId, Guid userId)
        {
            Category? categoryToDelete = await _categoryRepository.GetTrackedByIdAsync(categoryId, userId);
            if (categoryToDelete == null)
            {
                throw new NotFoundException("Category");
            }

            if (string.Equals(categoryToDelete.Name, GeneralCategoryName, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessRuleException("La categoria General no se puede eliminar.", ErrorCode.ValidationError);
            }

            Category? generalCategory = await _categoryRepository.GetCategoryByNameOrNullAsync(GeneralCategoryName, userId);
            if (generalCategory == null)
            {
                generalCategory = new Category(userId, GeneralCategoryName, GeneralCategoryIcon, GeneralCategoryColor);
                generalCategory = await _categoryRepository.CreateAsync(generalCategory);
            }

            await _transactionRepository.ReassignCategoryAsync(categoryToDelete.Id, generalCategory.Id, userId);
            await _recurringTransactionRepository.ReassignCategoryAsync(categoryToDelete.Id, generalCategory.Id, userId);
            await _pendingApprovalTransactionRepository.ReassignCategoryAsync(categoryToDelete.Id, generalCategory.Id, userId);

            _categoryRepository.Delete(categoryToDelete);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
