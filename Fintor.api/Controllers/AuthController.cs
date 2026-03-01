using Application.DTOs.Auth;
using Application.DTOs.Users;
using Application.Interfaces.UseCases.Auth;
using Application.Interfaces.UseCases.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly ISignIn _signIn;
        private readonly IMe _me;

        public AuthController(ISignIn signIn, IMe me)
        {
            _signIn = signIn;
            _me = me;
        }
        [HttpPost("login")]
        public async Task<LoginResponseDTO> SignIn(SignInDTO signInDTO)
        {
            LoginResponseDTO response = await _signIn.ExecuteAsync(signInDTO);
            return response;
        }

        [HttpPost("me")]
        public async Task<UserDTO> me()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            UserDTO userDTO = await _me.Execute(userId);
            return userDTO;
        }
    }
}
