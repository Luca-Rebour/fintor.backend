using Application.DTOs.Users;

namespace Application.Interfaces.UseCases.Users
{
    public interface IChangePassword
    {
        Task ExecuteAsync(ChangePasswordDTO dto, Guid userId);
    }
}