using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PendingApprovalTransactionDTO
    {
        public Guid Id { get; set; }
        public DateOnly DueDate { get; set; } // Fecha programada para crear la transaction
        public PendingTransactionStatus Status { get; set; }
        public string? Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public RecurringTransaction RecurringTransaction { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string CurrencyCode { get; set; } = null!;

        public PendingApprovalTransactionDTO()
        {
        }
         public PendingApprovalTransactionDTO(PendingApprovalTransaction pending)
        {
            Id = pending.Id;
            DueDate = pending.DueDate;
            Status = pending.Status;
            Description = pending.Description;
            TransactionType = pending.TransactionType;
            Amount = pending.Amount;
            CategoryName = pending.Category.Name;
            Icon = pending.Category.Icon;
            RecurringTransaction = pending.RecurringTransaction;
            AccountName = pending.RecurringTransaction.Account.Name;
            CurrencyCode = pending.RecurringTransaction.Account.Currency.Code;
        }
    }
}
