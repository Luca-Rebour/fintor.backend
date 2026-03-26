using Application.DTOs.Users;

namespace Application.Interfaces.UseCases.Users
{
    public interface IGetNotificationsEnabled
    {
        Task<GetNotificationsEnabledResponseDTO> ExecuteAsync(Guid userId);
    }
}
