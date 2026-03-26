using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Users;
using Domain.Entities;

namespace Application.UseCases.Users
{
    public class SendNotification : ISendNotification
    {
        private readonly IPushSubscriptionRepository _pushSubscriptionRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IUnitOfWork _unitOfWork;

        public SendNotification(
            IPushSubscriptionRepository pushSubscriptionRepository,
            INotificationRepository notificationRepository,
            IPushNotificationService pushNotificationService,
            IUnitOfWork unitOfWork)
        {
            _pushSubscriptionRepository = pushSubscriptionRepository;
            _notificationRepository = notificationRepository;
            _pushNotificationService = pushNotificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(SendNotificationDTO dto, Guid userId, CancellationToken cancellationToken = default)
        {
            dto.Validate();

            Notification notification = new Notification(userId, dto.Title, false, DateTime.UtcNow, DateTime.UtcNow);
            _notificationRepository.Add(notification);

            List<PushSubscription> enabledSubscriptions = await _pushSubscriptionRepository.GetEnabledByUserIdAsync(userId);
            List<string> tokens = enabledSubscriptions
                .Select(s => s.DeviceId)
                .Distinct()
                .ToList();

            if (tokens.Count > 0)
            {
                await _pushNotificationService.SendAsync(tokens, dto.Title, dto.Body, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}