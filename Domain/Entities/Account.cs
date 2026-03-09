using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public Guid CurrencyId { get; private set; }
        public string Name { get; private set; } = null!;
        public string Icon { get; private set; }

        public User User { get; private set; } = null!;
        public Currency Currency { get; private set; } = null!;
        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();
        public List<Goal> Goals { get; private set; } = new List<Goal>();


        private Account() { }

        public Account(Guid userId, Guid currencyId, string name)
        {
            Id = Guid.NewGuid();
            CurrencyId = currencyId;
            UserId = userId;
            Name = name;
        }

        public void Rename(string newName)
        {
            Name = newName;
        }

        public void SetCurrency(Guid currencyId)
        {
            CurrencyId = currencyId;
        }

        public void SetUser(Guid userId)
        {
            UserId = userId;
        }
    } 
}
