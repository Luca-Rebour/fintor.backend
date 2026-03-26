using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Users;

namespace Application.UseCases.Users
{
    public class GetNotificationsEnabled : IGetNotificationsEnabled
    {
        private readonly IPushSubscriptionRepository _pushSubscriptionRepository;

        public GetNotificationsEnabled(IPushSubscriptionRepository pushSubscriptionRepository)
        {
            _pushSubscriptionRepository = pushSubscriptionRepository;
        }

        public async Task<GetNotificationsEnabledResponseDTO> ExecuteAsync(Guid userId)
        {
            bool enabled = await _pushSubscriptionRepository.HasEnabledByUserIdAsync(userId);

            return new GetNotificationsEnabledResponseDTO
            {
                Enabled = enabled
            };
        }
    }
}
