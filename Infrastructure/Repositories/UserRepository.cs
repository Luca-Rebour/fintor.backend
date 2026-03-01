using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FintorDbContext _context;
        public UserRepository(FintorDbContext context)
        {
            _context = context;
        }
        public void CreateUser(User user)
        {
            _context.Users.Add(user);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.BaseCurrency)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));
        }
    }
}
