using Application.DTOs.Accounts;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transactions
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool IsRecurringTransaction { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal? ExchangeRate { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public TransactionDTO()
        {

        }
    }
}
