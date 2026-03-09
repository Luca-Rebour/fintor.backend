using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.DTOs.Transactions
{
    public class CreateTransactionDTO
    {
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? RecurringTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal? ExchangeRate { get; set; }
        public TransactionType TransactionType { get; set; }
        public Guid? GoalId { get; set; }

        public CreateTransactionDTO() { }

        public void Validate()
        {
            if (AccountId == Guid.Empty)
                throw new BusinessRuleException("Account is required.", ErrorCode.ValidationError);

            if (CategoryId == Guid.Empty)
                throw new BusinessRuleException("Category is required.", ErrorCode.ValidationError);

            if (Amount <= 0)
                throw new BusinessRuleException("Amount must be greater than zero.", ErrorCode.ValidationError);

            if (string.IsNullOrWhiteSpace(Description))
                throw new BusinessRuleException("Description is required.", ErrorCode.ValidationError);

            if (!Enum.IsDefined(TransactionType))
                throw new BusinessRuleException("Transaction type is invalid.", ErrorCode.ValidationError);
        }

    }
}
