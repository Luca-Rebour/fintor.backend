using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RecurringTransactions
{
    public class CreateRecurringTransactionDTO
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public TransactionType transactionType { get; set; }
        public Frequency Frequency { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
