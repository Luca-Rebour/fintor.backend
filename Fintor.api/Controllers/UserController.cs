using Application.DTOs.Users;
using Application.Interfaces.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly ICreateUser _createUser;
        private readonly IChangePassword _changePassword;

        public UserController(ICreateUser createUser, IChangePassword changePassword)
        {
            _createUser = createUser;
            _changePassword = changePassword;
        }

        [HttpPost("create-user")]
        public async Task<CreateUserResponseDTO> CreateUser(CreateUserDTO createUserDTO)
        {
            CreateUserResponseDTO response = await _createUser.Execute(createUserDTO);
            return response;
        }

        [HttpPost("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _changePassword.ExecuteAsync(changePasswordDTO, userId);
            return NoContent();
        }
    }
}
