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
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool IsRecurringTransaction { get; set; }
        public string CategoryName { get; set; }
        public string AccountName { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string CurrencyCode { get; set; }
        public string CategoryColor { get; set; }
        public string Icon { get; set; }
        public TransactionDTO()
        {

        }
    }
}
