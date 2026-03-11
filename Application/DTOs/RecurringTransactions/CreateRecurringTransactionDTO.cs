using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.DTOs.RecurringTransactions
{
    public class CreateRecurringTransactionDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public TransactionType transactionType { get; set; }
        public Frequency Frequency { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new BusinessRuleException("Recurring transaction name is required.", ErrorCode.ValidationError);
            }

            if (Amount <= 0)
            {
                throw new BusinessRuleException("Amount must be greater than zero.", ErrorCode.ValidationError);
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                throw new BusinessRuleException("Description is required.", ErrorCode.ValidationError);
            }

            if (!Enum.IsDefined(transactionType))
            {
                throw new BusinessRuleException("Transaction type is invalid.", ErrorCode.ValidationError);
            }

            if (!Enum.IsDefined(Frequency))
            {
                throw new BusinessRuleException("Frequency is invalid.", ErrorCode.ValidationError);
            }

            if (AccountId == Guid.Empty)
            {
                throw new BusinessRuleException("Account is required.", ErrorCode.ValidationError);
            }

            if (CategoryId == Guid.Empty)
            {
                throw new BusinessRuleException("Category is required.", ErrorCode.ValidationError);
            }

            if (StartDate == default)
            {
                throw new BusinessRuleException("Start date is required.", ErrorCode.ValidationError);
            }

            if (EndDate == default)
            {
                throw new BusinessRuleException("End date is required.", ErrorCode.ValidationError);
            }

            if (EndDate < StartDate)
            {
                throw new BusinessRuleException("End date must be greater than or equal to start date.", ErrorCode.ValidationError);
            }
        }
    }
}
