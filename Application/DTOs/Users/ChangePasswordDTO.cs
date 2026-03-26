using Domain.Enums;
using Domain.Exceptions;

namespace Application.DTOs.Users
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                throw new BusinessRuleException("Current password is required.", ErrorCode.ValidationError);
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                throw new BusinessRuleException("New password is required.", ErrorCode.ValidationError);
            }

            if (NewPassword.Length < 6)
            {
                throw new BusinessRuleException("New password must be at least 6 characters long.", ErrorCode.ValidationError);
            }
        }
    }
}