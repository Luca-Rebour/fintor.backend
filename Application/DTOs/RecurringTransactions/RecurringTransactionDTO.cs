using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RecurringTransactions
{
    public class RecurringTransactionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public TransactionType TransactionType { get; set; }
        public string Icon { get; set; } = string.Empty;
        public Frequency Frequency { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly LastGeneratedAt { get; set; }
        public DateOnly NextChargeDate { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;

    }
}
