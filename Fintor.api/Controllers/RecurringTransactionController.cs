using Application.DTOs.RecurringTransactions;
using Application.DTOs.Transactions;
using Application.Interfaces.UseCases.RecurringTransactions;
using Application.Interfaces.UseCases.Transactions;
using Domain.Entities;
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
        private readonly IDeleteRecurringTransaction _deleteRecurringTransaction;
        private readonly IUpdateRecurringTransaction _updateRecurringTransaction;
        public RecurringTransactionController(ICreateRecurringTransaction createRecurringTransaction, 
            IGetRecurringTransactions getRecurringTransactions, 
            IDeleteRecurringTransaction deleteRecurringTransaction, IUpdateRecurringTransaction updateRecurringTransaction)
        {
            _updateRecurringTransaction = updateRecurringTransaction;
            _createRecurringTransaction = createRecurringTransaction;
            _getRecurringTransactions = getRecurringTransactions;
            _deleteRecurringTransaction = deleteRecurringTransaction;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRecurringTransaction([FromBody] CreateRecurringTransactionDTO createRecurringTransactionDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            RecurringTransactionDTO recurringTransactionDTO = await _createRecurringTransaction.ExecuteAsync(createRecurringTransactionDTO, userId);
            return Ok(recurringTransactionDTO);
        }

        [HttpPut("{recurringTransactionId:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateRecurringTransaction(Guid recurringTransactionId, [FromBody] CreateRecurringTransactionDTO dto)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _updateRecurringTransaction.ExecuteAsync(recurringTransactionId, dto, userId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRecurringTransactions()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            List<RecurringTransactionDTO> recurringTransactionDTOs = await _getRecurringTransactions.ExecuteAsync(userId);
            return Ok(recurringTransactionDTOs);
        }

        [HttpDelete("{recurringTransactionId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecurringTransaction([FromRoute] Guid recurringTransactionId)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _deleteRecurringTransaction.ExecuteAsync(recurringTransactionId, userId);
            return NoContent();
        }
    }
}
