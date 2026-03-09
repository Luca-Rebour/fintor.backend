using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string message, ErrorCode code)
            : base(message, code)
        {
        }
    }
}
