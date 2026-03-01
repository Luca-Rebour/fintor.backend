using Application.DTOs.Transactions;
using Application.DTOs.RecurringTransactions;
using Application.Interfaces.UseCases.Transactions;
using Application.Interfaces.UseCases.RecurringTransactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/recurring-transactions")]
    public class RecurringTransactionController : Controller
    {
        private readonly ICreateRecurringTransaction _createRecurringTransaction;
        private readonly IGetRecurringTransactions _getRecurringTransactions;
        public RecurringTransactionController(ICreateRecurringTransaction createRecurringTransaction, IGetRecurringTransactions getRecurringTransactions)
        {
            _createRecurringTransaction = createRecurringTransaction;
            _getRecurringTransactions = getRecurringTransactions;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRecurringTransaction([FromBody] CreateRecurringTransactionDTO createRecurringTransactionDTO)
        {
            RecurringTransactionDTO recurringTransactionDTO = await _createRecurringTransaction.ExecuteAsync(createRecurringTransactionDTO);
            return Ok(recurringTransactionDTO);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRecurringTransactions()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            List<RecurringTransactionDTO> recurringTransactionDTOs = await _getRecurringTransactions.ExecuteAsync(userId);
            return Ok(recurringTransactionDTOs);
        }
    }
}
