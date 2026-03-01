using Application.DTOs.Auth;
using Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Auth
{
    public interface ISignIn
    {
        Task<LoginResponseDTO> ExecuteAsync(SignInDTO signInDTO);
    }
}
