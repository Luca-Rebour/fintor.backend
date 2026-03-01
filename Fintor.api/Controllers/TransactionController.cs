using Application.DTOs.Accounts;
using Application.DTOs.Transactions;
using Application.Interfaces.UseCases.Transactions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : Controller
    {
        private readonly ICreateTransaction _createTransaction;
        private readonly IGetAllTransactions _getAllTransactions;
        private readonly IDeleteTransaction _deleteTransaction;
        public TransactionController(ICreateTransaction createTransaction, IGetAllTransactions getAllTransactions, IDeleteTransaction deleteTransaction)
        {
            _createTransaction = createTransaction;
            _getAllTransactions = getAllTransactions;
            _deleteTransaction = deleteTransaction;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDTO createTransactionDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            TransactionDTO transactionDto = await _createTransaction.ExecuteAsync(createTransactionDTO, userId);
            return Ok(transactionDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTransactions()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            List<TransactionDTO> transactionDto = await _getAllTransactions.ExecuteAsync(userId);
            return Ok(transactionDto);
        }

        [HttpDelete("{transactionId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid transactionId)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _deleteTransaction.ExecuteAsync(transactionId, userId);
            return NoContent();
        }


    }
}
