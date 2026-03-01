using Application.DTOs.Transactions;
using Application.DTOs.RecurringTransactions;
using Application.Interfaces.UseCases.Transactions;
using Application.Interfaces.UseCases.RecurringTransactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/recurring-transactions")]
    public class RecurringTransactionController : Controller
    {
        private readonly ICreateRecurringTransaction _createRecurringTransaction;
        public RecurringTransactionController(ICreateRecurringTransaction createRecurringTransaction)
        {
            _createRecurringTransaction = createRecurringTransaction;
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
            return Ok();
        }
    }
}
