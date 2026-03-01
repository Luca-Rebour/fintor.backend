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
        private readonly ITransactionRepository _transactionRepository;

        public GetOverviewResponse(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IReadOnlyList<OverviewResponseDTO>> ExecuteAsync(Guid userId){

         return await _transactionRepository.GetOverview(userId);

        }
    }
}