using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Currency
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public Currency() { }
        public Currency (string code)
        {
            Id = Guid.NewGuid();
            Code = code;
        }

        public Currency(Guid id, string code)
        {
            Id = id;
            Code = code;
        }
    }
}
