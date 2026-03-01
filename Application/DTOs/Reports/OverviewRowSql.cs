using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Reports
{
    public class OverviewRowSql
    {
        public int DaysAgo { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }

        public string CategorySpendingJson { get; set; } = "[]";
        public string CategoryEarningJson { get; set; } = "[]";
    }
}
