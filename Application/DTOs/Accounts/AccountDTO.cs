using Application.DTOs.Transactions;
using Application.DTOs.RecurringTransactions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Accounts
{
    public class AccountDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public string Icon { get; set; }

        public string CurrencyCode { get; set; }

        public AccountDTO() { }
    }
}
