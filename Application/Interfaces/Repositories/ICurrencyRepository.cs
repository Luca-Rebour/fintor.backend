using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        void CreateCurrency(Currency currency);
        Task<Currency> GetCurrencyAsync(Guid id);
        Task<Currency?> GetCurrencyByCodeAsync(string code);
    }
}
