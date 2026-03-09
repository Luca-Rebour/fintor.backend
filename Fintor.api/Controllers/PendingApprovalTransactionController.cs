using Application.Interfaces.UseCases.PendingApproveTransactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/pending-approve-transactions")]
    public class PendingApprovalTransactionController : Controller
    {
        private readonly IApprovePendingApprovalTransaction _approvePendingApprovalTransaction;
        private readonly ICancelPendingApprovalTransaction _cancelPendingApprovalTransaction;
        private readonly IGetPendingApprovalTransactions _getPendingApprovalTransactions;

        public PendingApprovalTransactionController(IApprovePendingApprovalTransaction approvePendingApprovalTransaction, ICancelPendingApprovalTransaction cancelPendingApprovalTransaction, IGetPendingApprovalTransactions getPendingApprovalTransactions)
        {
            _approvePendingApprovalTransaction = approvePendingApprovalTransaction;
            _cancelPendingApprovalTransaction = cancelPendingApprovalTransaction;
            _getPendingApprovalTransactions = getPendingApprovalTransactions;
        }

        [HttpPost("{pendingApprovalTransactionId:guid}/approve")]
        public async Task<IActionResult> ApprovePendingApprovalTransaction([FromRoute] Guid pendingApprovalTransactionId, [FromBody] decimal? exchangeRate)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _approvePendingApprovalTransaction.ExecuteAsync(pendingApprovalTransactionId, userId, exchangeRate);
            return NoContent();
        }

        [HttpPost("{pendingApprovalTransactionId:guid}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelPendingApprovalTransaction([FromRoute] Guid pendingApprovalTransactionId)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _cancelPendingApprovalTransaction.ExecuteAsync(pendingApprovalTransactionId, userId);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetPendingApprovalTransactions()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var pendingApprovalTransactions = await _getPendingApprovalTransactions.ExecuteAsync(userId);
            return Ok(pendingApprovalTransactions);

        }
    }
}
