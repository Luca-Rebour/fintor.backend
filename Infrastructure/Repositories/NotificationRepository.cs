using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly FintorDbContext _context;

        public NotificationRepository(FintorDbContext context)
        {
            _context = context;
        }

        public void Add(Notification notification)
        {
            _context.Notifications.Add(notification);
        }
    }
}