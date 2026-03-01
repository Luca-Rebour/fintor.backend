using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.RecurringTransactions;
using Domain.Entities;

public class GenerateRecurringTransaction : IGenerateRecurringTransactions
{
    private readonly IRecurringTransactionRepository _recurringTransactionRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
	private readonly IPendingApprovalTransactionRepository _pendingApprovalTransactionRepository;

	public GenerateRecurringTransaction(
        IRecurringTransactionRepository recurringRepo,
        ITransactionRepository transactionRepo,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        IPendingApprovalTransactionRepository pendingApprovalTransactionRepository)
    {
        _recurringTransactionRepository = recurringRepo;
        _transactionRepository = transactionRepo;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _pendingApprovalTransactionRepository = pendingApprovalTransactionRepository;
    }

    public async Task ExecuteAsync()
    {
		DateOnly today = DateOnly.FromDateTime(_dateTimeProvider.UtcNow);
        List<RecurringTransaction> recurrents = await _recurringTransactionRepository.GetRecurringTransactionsDueUpTo(today);
		List<PendingApprovalTransaction> pendingAprovalTransactions = new List<PendingApprovalTransaction>();


		foreach (RecurringTransaction r in recurrents)
		{
			while (r.NextDueDate <= today && r.NextDueDate <= r.EndDate)
			{
				DateOnly dueDate = r.ConsumeNextDueDate();

				PendingApprovalTransaction transaction = new PendingApprovalTransaction(
					r.AccountId,
					r.Id,
					r.CategoryId,
					r.Amount,
					r.Description,
					r.TransactionType,
					dueDate
				);

				_pendingApprovalTransactionRepository.CreatePendingApprovalTransaction(transaction);
			}

			_recurringTransactionRepository.Update(r);
		}

		await _unitOfWork.SaveChangesAsync();
	}
}
