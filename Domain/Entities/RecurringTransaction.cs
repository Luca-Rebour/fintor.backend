using Domain.Enums;

namespace Domain.Entities
{
    public class RecurringTransaction
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public decimal Amount { get; private set; }
        public string Description { get; private set; } = null!;
        public TransactionType TransactionType { get; private set; }
        public Frequency Frequency { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }

        // próxima ocurrencia a crear PendingApprovalTransaction
        public DateOnly NextDueDate { get; private set; }

        public Guid AccountId { get; private set; }
        public Account Account { get; private set; } = null!;

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public List<PendingApprovalTransaction> PendingApprovalTransactions { get; private set; } = new List<PendingApprovalTransaction>();

        private RecurringTransaction() { }

        public RecurringTransaction(
            Guid categoryId,
            Guid accountId,
            string name,
            decimal amount,
            string description,
            TransactionType transactionType,
            Frequency frequency,
            DateOnly startDate,
            DateOnly endDate)
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
            NextDueDate = startDate;
        }

        // Indica si hoy hay al menos una ocurrencia que ya debería existir como Pending
        public bool ShouldGenerate(DateOnly today){
            return NextDueDate <= today && NextDueDate <= EndDate;
        }

        // Devuelve el DueDate actual y avanza al siguiente (se usa en el job)
        public DateOnly ConsumeNextDueDate()
        {
            if (NextDueDate > EndDate)
                throw new InvalidOperationException("No hay más ocurrencias dentro del rango.");

            var dueDate = NextDueDate;
            NextDueDate = CalculateNext(NextDueDate);
            return dueDate;
        }
        private DateOnly CalculateNext(DateOnly date) =>
            Frequency switch
            {
                Frequency.Daily => date.AddDays(1),
                Frequency.Weekly => date.AddDays(7),
                Frequency.BiWeekly => date.AddDays(14),
                Frequency.Monthly => date.AddMonths(1),
                Frequency.Quarterly => date.AddMonths(3),
                Frequency.Yearly => date.AddYears(1),
                _ => throw new NotSupportedException("Frecuencia no soportada")
            };
    }
}