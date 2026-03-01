using Application.DTOs.Users;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(Guid id);
    }
}
