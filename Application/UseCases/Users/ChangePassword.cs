using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases.Users;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.Users
{
    public class ChangePassword : IChangePassword
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePassword(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(ChangePasswordDTO dto, Guid userId)
        {
            dto.Validate();

            User? user = await _userRepository.GetTrackedUserByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (!_passwordService.VerifyPassword(user.PasswordHash, dto.CurrentPassword))
            {
                throw new UnauthorizedAccessException("Current password is invalid.");
            }

            user.SetPassword(_passwordService.HashPassword(dto.NewPassword));
            await _unitOfWork.SaveChangesAsync();
        }
    }
}