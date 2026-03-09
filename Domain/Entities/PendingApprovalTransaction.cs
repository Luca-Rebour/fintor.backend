using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PendingApprovalTransaction
    {
        public Guid Id { get; private set; }
        public DateOnly DueDate { get; private set; } // Fecha programada para crear la transaction
        public PendingTransactionStatus Status { get; private set; }
        public string? Description { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public DateOnly ConfirmedAt { get; private set; }
        public Decimal Amount { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;
        public Guid? TransactionId { get; private set; }
        public Transaction? Transaction { get; private set; }
        public Guid RecurringTransactionId { get; private set; }
        public RecurringTransaction RecurringTransaction { get; private set; } = null!;
        public Guid AccountId { get; private set; }
        public Account Account { get; private set; } = null!;

        public PendingApprovalTransaction()
        {
        }

        public PendingApprovalTransaction(Guid accountId, Guid recurringTransactionId, Guid categoryId, decimal amount, string description, TransactionType transactionType, DateOnly? dueDate)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            RecurringTransactionId = recurringTransactionId;
            CategoryId = categoryId;
            Amount = amount;
            Status = PendingTransactionStatus.Pending;
            DueDate = dueDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            Description = description;
            TransactionType = transactionType;
        }

         public void Approve(DateOnly confirmedAt, Guid transactionId)
         {
                if (Status != PendingTransactionStatus.Pending)
                    throw new BusinessRuleException("Only pending transactions can be approved.", ErrorCode.ValidationError);
    
                Status = PendingTransactionStatus.Approved;
                ConfirmedAt = confirmedAt;
                TransactionId = transactionId;
        }
        public void Cancel()
        {
            if (Status == PendingTransactionStatus.Approved)
                throw new BusinessRuleException("Approved transactions cannot be rejected.", ErrorCode.ValidationError);
    
            Status = PendingTransactionStatus.Cancelled;
        }

        public void Reschedule(DateOnly newDate)
        {
            DueDate = newDate;
            Status = PendingTransactionStatus.Rescheduled;
        }
    }
}
