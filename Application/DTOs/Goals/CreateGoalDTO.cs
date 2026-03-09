using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Goals
{
    public class CreateGoalDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public string Icon { get; set; }
        public DateTime TargetDate { get; set; }
        public string AccentColor { get; set; }
        public Guid AccountId { get; set; }
        public decimal? ExchangeRate { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Title))
            {
                throw new BusinessRuleException("Title is required.", ErrorCode.ValidationError);
            }
            if (TargetAmount <= 0)
            {
                throw new BusinessRuleException("Target amount must be greater than zero.", ErrorCode.ValidationError);
            }
            if (CurrentAmount < 0)
            {
                throw new BusinessRuleException("Current amount cannot be negative.", ErrorCode.ValidationError);
            }
            if (string.IsNullOrEmpty(Icon))
            {
                throw new BusinessRuleException("Icon is required.", ErrorCode.ValidationError);
            }
            if (string.IsNullOrEmpty(AccentColor))
            {
                throw new BusinessRuleException("Accent color is required.", ErrorCode.ValidationError);
            }
        }
    }
}
