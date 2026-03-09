using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class AuthenticationException : DomainException
    {
        public AuthenticationException()
            : base("Credenciales inválidas.", ErrorCode.InvalidCredentials)
        {
        }
    }
}
