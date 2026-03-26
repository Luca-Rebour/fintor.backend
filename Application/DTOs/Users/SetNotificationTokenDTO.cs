using Domain.Enums;
using Domain.Exceptions;

namespace Application.DTOs.Users
{
    public class SetNotificationTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public Platform Platform { get; set; }
        public string? Provider { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                throw new BusinessRuleException("Notification token is required.", ErrorCode.ValidationError);
            }

            if (!Token.StartsWith("ExponentPushToken[") && !Token.StartsWith("ExpoPushToken["))
            {
                throw new BusinessRuleException("Invalid Expo push token format.", ErrorCode.ValidationError);
            }

        }
    }
}