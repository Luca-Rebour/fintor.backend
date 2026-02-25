using Application.DTOs.RecurringTransactions;
using Application.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UseCases.Reports
{
    public interface IGetOverviewResponse
    {
        Task<OverviewResponseDTO> ExecuteAsync(Guid userId, int filter);
    }
}
