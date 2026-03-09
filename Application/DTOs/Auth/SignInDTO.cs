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
                throw new ValidationException("Email is required.");
            }

            if (!new EmailAddressAttribute().IsValid(Email))
            {
                throw new ValidationException("Email format is invalid.");
            }
    
            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new ValidationException("Password is required.");
            }
                
        }
    }
}
