using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Users;
using Domain.Entities;

namespace Application.UseCases.Users
{
    public class SetNotificationToken : ISetNotificationToken
    {
        private readonly IPushSubscriptionRepository _pushSubscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetNotificationToken(
            IPushSubscriptionRepository pushSubscriptionRepository,
            IUnitOfWork unitOfWork)
        {
            _pushSubscriptionRepository = pushSubscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(SetNotificationTokenDTO dto, Guid userId)
        {
            dto.Validate();

            PushSubscription? existingSubscription = await _pushSubscriptionRepository.GetByTokenAsync(userId, dto.Token);
            if (existingSubscription == null)
            {
                PushSubscription newSubscription = new PushSubscription(
                    userId,
                    dto.Platform,
                    "expo",
                    dto.Token,
                    DateOnly.FromDateTime(DateTime.UtcNow));

                newSubscription.SetEnabled(true);
                _pushSubscriptionRepository.Add(newSubscription);
            }
            else
            {
                existingSubscription.UpdateToken(
                    dto.Token,
                    dto.Platform,
                    "expo",
                    DateOnly.FromDateTime(DateTime.UtcNow));
                existingSubscription.SetEnabled(true);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}