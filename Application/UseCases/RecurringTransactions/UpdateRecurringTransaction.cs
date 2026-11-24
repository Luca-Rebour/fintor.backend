using Application.DTOs.RecurringTransactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.RecurringTransactions;
using Domain.Exceptions;

namespace Application.UseCases.RecurringTransactions;

public sealed class UpdateRecurringTransaction(
    IRecurringTransactionRepository recurringRepository,
    IAccountRepository accountRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IUpdateRecurringTransaction
{
    public async Task<RecurringTransactionDTO> ExecuteAsync(Guid id, CreateRecurringTransactionDTO dto, Guid userId)
    {
        dto.Validate();
        var recurring = await recurringRepository.GetBydIdToUpdateAsync(id)
            ?? throw new NotFoundException("Recurring transaction");
        if (recurring.Account.UserId != userId) throw new ForbiddenException("Recurring transaction belongs to another user.");
        var account = await accountRepository.GetAccountByIdAsync(dto.AccountId, userId);
        var category = await categoryRepository.GetTrackedByIdAsync(dto.CategoryId, userId)
            ?? throw new NotFoundException("Category");

        recurring.Edit(dto.CategoryId, dto.AccountId, dto.Name, dto.Amount, dto.Description,
            dto.transactionType, dto.Frequency, dto.StartDate, dto.EndDate);
        recurringRepository.Update(recurring);
        await unitOfWork.SaveChangesAsync();
        return new RecurringTransactionDTO
        {
            Id = recurring.Id, Name = recurring.Name, Amount = recurring.Amount,
            Description = recurring.Description, TransactionType = recurring.TransactionType,
            Frequency = recurring.Frequency, StartDate = recurring.StartDate, EndDate = recurring.EndDate,
            NextChargeDate = recurring.NextDueDate, AccountId = recurring.AccountId, CategoryId = recurring.CategoryId,
            AccountName = account.Name, CurrencyCode = account.Currency.Code, Icon = category.Icon
        };
    }
}
