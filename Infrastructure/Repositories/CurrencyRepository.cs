using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly FintorDbContext _context;
        public CurrencyRepository(FintorDbContext context)
        {
            _context = context;
        }
        public async Task<Currency> GetCurrencyAsync(Guid id)
        {
            Currency? currency = await _context.Currencies.FindAsync(id);
            if (currency == null)
            {
                throw new NotFoundException("Currency");
            }

            return currency;
        }

        public async Task<Currency?> GetCurrencyByCodeAsync(string code)
        {
            string normalized = code.ToUpper();

            return await _context.Currencies.FirstOrDefaultAsync(c => c.Code.ToUpper() == normalized);
        }

        public void CreateCurrency(Currency currency)
        {
            _context.Currencies.Add(currency);
        }
    }
}
