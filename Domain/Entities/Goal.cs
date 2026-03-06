using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Goal
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal TargetAmount { get; private set; }
        public string Icon { get; private set; }
        public DateTime TargetDate { get; private set; }
        public string AccentColor { get; private set; }
        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();
        public Guid AccountId { get; private set; }
        public Account Account { get; private set; }
        public Goal()
        {
        }
    }
}
