using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        void Add(Notification notification);
    }
}