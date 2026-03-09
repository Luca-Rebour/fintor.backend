using Domain.Enums;
using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Users
{
    public class SignInDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new BusinessRuleException("Email is required.", ErrorCode.ValidationError);
            }

            if (!new EmailAddressAttribute().IsValid(Email))
            {
                throw new BusinessRuleException("Email format is invalid.", ErrorCode.ValidationError);
            }
    
            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new BusinessRuleException("Password is required.", ErrorCode.ValidationError);
            }
                
        }
    }
}
