using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }

        public string? Provider { get; private set; }
        public string? ProviderId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid BaseCurrencyId { get; private set; }
        public Currency BaseCurrency { get; private set; }
        public User()
        {

        }

        public User(string email, DateOnly dateOfBirth, string name, string lastName, string? provider, string? providerId, Guid baseCurrencyId)
        {
            Email = email;
            DateOfBirth = dateOfBirth;
            Name = name;
            LastName = lastName;
            Provider = provider;
            ProviderId = providerId;
            CreatedAt = DateTime.UtcNow;
            BaseCurrencyId = baseCurrencyId;
        }

        public void SetPassword(string password)
        {
            PasswordHash = password;
        }

        public void setBaseCurrencyId(Guid baseCurrencyId)
        {
            BaseCurrencyId = baseCurrencyId;
        }
    }
}
