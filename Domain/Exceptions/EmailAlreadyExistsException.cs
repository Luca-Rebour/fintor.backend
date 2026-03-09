using Domain.Enums;
using Domain.Exceptions;

namespace Fintor.api.Exceptions
{
    public class EmailAlreadyInUseException : BusinessRuleException
    {
        public EmailAlreadyInUseException()
            : base(
                "An account with this email already exists.",
                ErrorCode.EmailAlreadyInUse)
        {
        }
    }

}
