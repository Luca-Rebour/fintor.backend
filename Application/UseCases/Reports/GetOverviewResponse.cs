using Application.DTOs.Categories;
using Application.DTOs.Reports;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Reports
{
    public class GetOverviewResponse : IGetOverviewResponse
    {
        private readonly IMovementRepository _transactionRepository;

        public GetOverviewResponse(IMovementRepository transactionRepository) {
            _transactionRepository = transactionRepository;
        }
        public async Task<OverviewResponseDTO> ExecuteAsync(Guid userId, int filter)
        {
            decimal totalIncome = await _transactionRepository.GetTotalIncome(userId, filter);
            decimal totalExpense = await _transactionRepository.GetTotalExpense(userId, filter);
            decimal totalBalance = totalIncome - totalExpense;
            List<CategorySummaryDto> categorySpending = await _transactionRepository.GetCategorySpending(userId, filter);
            List<CategorySummaryDto> categoryEarning = await _transactionRepository.GetCategoryEarning(userId, filter);
            return new OverviewResponseDTO(totalBalance, totalIncome, totalExpense, categorySpending, categoryEarning);
        }
    }
}
