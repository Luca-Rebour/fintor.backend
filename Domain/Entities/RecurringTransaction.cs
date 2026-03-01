using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RecurringTransaction
    {
        public Guid Id { get; private set; } 
        public string Name { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public Frequency Frequency { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public DateOnly? LastGeneratedAt { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public Guid AccountId { get; private set; }
        public Account Account { get; private set; } = null!;
        
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public RecurringTransaction()
        {

        }
        public RecurringTransaction(Guid categoryId, Guid accountId, string name, decimal amount, string description, TransactionType transactionType, Frequency frequency, DateOnly startDate, DateOnly endDate)
        {
            Id = Guid.NewGuid();
            CategoryId = categoryId;
            AccountId = accountId;
            Name = name;
            Amount = amount;
            Description = description;
            TransactionType = transactionType;
            Frequency = frequency;
            StartDate = startDate;
            EndDate = endDate;
        }
        public bool ShouldGenerate(DateOnly today)
        {
            if (today < StartDate || today > EndDate)
                return false;

            if (LastGeneratedAt == null)
                return today == StartDate;

            var nextDueDate = GetNextDueDate(LastGeneratedAt.Value);
            return today >= nextDueDate;

        }

        private DateOnly GetNextDueDate(DateOnly lastDate)
        {
            return Frequency switch
            {
                Frequency.Daily => lastDate.AddDays(1),
                Frequency.Weekly => lastDate.AddDays(7),
                Frequency.Monthly => lastDate.AddMonths(1),
                _ => throw new NotSupportedException("Frecuencia no soportada")
            };
        }
        public void SetLastGenerated(DateOnly date)
        {
            LastGeneratedAt = date;
        }
    }
}
