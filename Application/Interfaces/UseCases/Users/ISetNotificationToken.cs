using Application.DTOs.Users;

namespace Application.Interfaces.UseCases.Users
{
    public interface ISetNotificationToken
    {
        Task ExecuteAsync(SetNotificationTokenDTO dto, Guid userId);
    }
}