using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid CurrencyId { get; private set; }
        public string Name { get; private set; } = null!;

        public User User { get; private set; } = null!;
        public Currency Currency { get; private set; } = null!;

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
    }
}
