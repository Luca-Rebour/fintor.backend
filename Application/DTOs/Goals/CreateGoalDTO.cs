using Domain.Enums;
using Domain.Exceptions;

namespace Application.DTOs.Goals
{
    public class CreateGoalDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public string Icon { get; set; } = string.Empty;
        public DateTime TargetDate { get; set; }
        public string AccentColor { get; set; } = string.Empty;
        public Guid AccountId { get; set; }
        public decimal? ExchangeRate { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                throw new BusinessRuleException("Title is required.", ErrorCode.ValidationError);
            }
            if (string.IsNullOrWhiteSpace(Description))
            {
                throw new BusinessRuleException("Description is required.", ErrorCode.ValidationError);
            }
            if (TargetAmount <= 0)
            {
                throw new BusinessRuleException("Target amount must be greater than zero.", ErrorCode.ValidationError);
            }
            if (CurrentAmount < 0)
            {
                throw new BusinessRuleException("Current amount cannot be negative.", ErrorCode.ValidationError);
            }
            if (string.IsNullOrWhiteSpace(Icon))
            {
                throw new BusinessRuleException("Icon is required.", ErrorCode.ValidationError);
            }
            if (string.IsNullOrWhiteSpace(AccentColor))
            {
                throw new BusinessRuleException("Accent color is required.", ErrorCode.ValidationError);
            }
            if (AccountId == Guid.Empty)
            {
                throw new BusinessRuleException("Account is required.", ErrorCode.ValidationError);
            }
            if (TargetDate == default)
            {
                throw new BusinessRuleException("Target date is required.", ErrorCode.ValidationError);
            }
        }
    }
}
