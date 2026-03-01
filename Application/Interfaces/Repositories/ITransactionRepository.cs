using Application.DTOs.Categories;
using Application.DTOs.Reports;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        void CreateTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetAccountTransactionsAsync(Guid accountId);
        Task<List<Transaction>> GetAllTransactionsAsync(Guid userId);
        Task<Transaction> GetTransactionAsync(Guid transactionId, Guid userId);
        Task<IReadOnlyList<OverviewResponseDTO>> GetOverview(Guid userId);
        Task<Transaction> GetTrackedTransactionAsync(Guid transactionId, Guid userId);
        void RemoveTransaction(Transaction transaction);

    }
}
