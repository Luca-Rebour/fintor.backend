using Application.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Reports
{
    public class OverviewResponseDTO
    {
        public int DaysAgo { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public IReadOnlyList<CategorySummaryDto> CategorySpending { get; set; }
        public IReadOnlyList<CategorySummaryDto> CategoryEarning { get; set; }

        public OverviewResponseDTO(decimal totalBalance, decimal totalIncome, decimal totalExpense, IReadOnlyList<CategorySummaryDto> categorySpending, IReadOnlyList<CategorySummaryDto> categoryEarning)
        {
            TotalBalance = totalBalance;
            TotalIncome = totalIncome;
            TotalExpense = totalExpense;
            CategorySpending = categorySpending;
            CategoryEarning = categoryEarning;
        }
    }
}
