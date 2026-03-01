using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.RecurringTransactions;
using Domain.Entities;

public class GenerateRecurringTransaction : IGenerateRecurringMovements
{
    private readonly IRecurringTransactionRepository _recurringTransactionRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateRecurringTransaction(
        IRecurringTransactionRepository recurringRepo,
        ITransactionRepository transactionRepo,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        _recurringTransactionRepository = recurringRepo;
        _transactionRepository = transactionRepo;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync()
    {
        var today = DateOnly.FromDateTime(_dateTimeProvider.UtcNow);
        var recurrents = await _recurringTransactionRepository.GetAllAsync();

        foreach (RecurringTransaction r in recurrents)
        {
            if (r.ShouldGenerate(today))
            {
                var transaction = new Transaction(r.AccountId,r.Id, r.CategoryId, r.Amount, r.Description, r.TransactionType, null);
                

                _transactionRepository.CreateTransactionAsync(transaction);
                await _unitOfWork.SaveChangesAsync();
                r.SetLastGenerated(today);
                await _recurringTransactionRepository.UpdateAsync(r);
            }
        }
    }
}
