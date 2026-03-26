using Application.DTOs.Users;

namespace Application.Interfaces.UseCases.Users
{
    public interface ISendNotification
    {
        Task ExecuteAsync(SendNotificationDTO dto, Guid userId, CancellationToken cancellationToken = default);
    }
}