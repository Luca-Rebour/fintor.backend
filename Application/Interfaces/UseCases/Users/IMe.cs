using Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Users
{
    public interface IMe
    {
        Task<UserDTO> Execute(Guid userId);
    }
}
