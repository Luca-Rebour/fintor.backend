using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PushSubscriptionRepository : IPushSubscriptionRepository
    {
        private readonly FintorDbContext _context;

        public PushSubscriptionRepository(FintorDbContext context)
        {
            _context = context;
        }

        public async Task<PushSubscription?> GetByTokenAsync(Guid userId, string token)
        {
            return await _context.PushSubscriptions
                .FirstOrDefaultAsync(p => p.UserId == userId && p.DeviceId == token);
        }

        public async Task<List<PushSubscription>> GetByUserIdAsync(Guid userId)
        {
            return await _context.PushSubscriptions
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<PushSubscription>> GetEnabledByUserIdAsync(Guid userId)
        {
            return await _context.PushSubscriptions
                .Where(p => p.UserId == userId && p.Enabled)
                .ToListAsync();
        }

        public async Task<bool> HasEnabledByUserIdAsync(Guid userId)
        {
            return await _context.PushSubscriptions
                .AnyAsync(p => p.UserId == userId && p.Enabled);
        }

        public void Add(PushSubscription pushSubscription)
        {
            _context.PushSubscriptions.Add(pushSubscription);
        }
    }
}