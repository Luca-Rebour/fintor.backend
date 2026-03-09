using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Users
{
    public class CreateUserDTO
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string BaseCurrencyCode { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }

        public void Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add("Name is required.");

            if (string.IsNullOrWhiteSpace(LastName))
                errors.Add("Last name is required.");

            if (string.IsNullOrWhiteSpace(Email))
                errors.Add("Email is required.");
            else if (!new EmailAddressAttribute().IsValid(Email))
                errors.Add("Email format is invalid.");

            if (string.IsNullOrWhiteSpace(Password))
                errors.Add("Password is required.");
            else if (Password.Length < 6)
                errors.Add("Password must be at least 6 characters long.");

            if (DateOfBirth == default)
                errors.Add("Birth date is required.");

            if (errors.Any())
                throw new ValidationException(string.Join(" ", errors));
        }


    }
}
