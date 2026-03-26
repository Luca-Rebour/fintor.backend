using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPushSubscriptionRepository
    {
        Task<PushSubscription?> GetByTokenAsync(Guid userId, string token);
        Task<List<PushSubscription>> GetByUserIdAsync(Guid userId);
        Task<List<PushSubscription>> GetEnabledByUserIdAsync(Guid userId);
        Task<bool> HasEnabledByUserIdAsync(Guid userId);
        void Add(PushSubscription pushSubscription);
    }
}