using Application.DTOs.Accounts;
using Application.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Goals
{
    public class GoalDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public string Icon { get; set; } = string.Empty;
        public DateTime TargetDate { get; set; }
        public string AccentColor { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
