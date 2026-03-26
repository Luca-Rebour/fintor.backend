using Domain.Enums;
using Domain.Exceptions;

namespace Application.DTOs.Users
{
    public class SendNotificationDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                throw new BusinessRuleException("Notification title is required.", ErrorCode.ValidationError);
            }

            if (string.IsNullOrWhiteSpace(Body))
            {
                throw new BusinessRuleException("Notification body is required.", ErrorCode.ValidationError);
            }
        }
    }
}