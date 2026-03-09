using Application.DTOs.Categories;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FintorDbContext _context;
        public AccountRepository(FintorDbContext context)
        {
            _context = context;
        }
        public void CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
        }

        public async Task DeleteAccountAsync(Guid accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                throw new NotFoundException("Acount not found");
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Account>> GetAllAccountsAsync(Guid userId)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .Include(a => a.Currency)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

    }
}
