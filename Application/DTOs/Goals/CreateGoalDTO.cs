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
                throw new ArgumentException("Title is required.");
            }
            if (TargetAmount <= 0)
            {
                throw new ArgumentException("Target amount must be greater than zero.");
            }
            if (CurrentAmount < 0)
            {
                throw new ArgumentException("Current amount cannot be negative.");
            }
            if (string.IsNullOrEmpty(Icon))
            {
                throw new ArgumentException("Icon is required.");
            }
            if (string.IsNullOrEmpty(AccentColor))
            {
                throw new ArgumentException("Accent color is required.");
            }
        }
    }
}
