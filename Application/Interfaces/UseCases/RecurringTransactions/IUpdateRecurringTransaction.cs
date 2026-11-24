using Application.DTOs.RecurringTransactions;

namespace Application.Interfaces.UseCases.RecurringTransactions;

public interface IUpdateRecurringTransaction
{
    Task<RecurringTransactionDTO> ExecuteAsync(Guid id, CreateRecurringTransactionDTO dto, Guid userId);
}
