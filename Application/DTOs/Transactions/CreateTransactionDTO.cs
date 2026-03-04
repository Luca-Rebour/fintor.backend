using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transactions
{
    public class CreateTransactionDTO
    {
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? RecurringTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal? ExchangeRate { get; set; }
        public TransactionType TransactionType { get; set; }
        public Guid? GoalId { get; set; }

        public CreateTransactionDTO() { }

        public void Validate()
        {
            if (Amount <= 0)
                throw new ArgumentException("El monto debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("La descripción no puede estar vacía.");
        }

    }
}
