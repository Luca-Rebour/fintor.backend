using Application.DTOs.Accounts;
using Application.DTOs.Transactions;
using Application.Interfaces.UseCases.Accounts;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fintor.api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : Controller
    {
        private readonly ICreateAccount _createAccount;
        private readonly IDeleteAccount _deleteAccount;
        private readonly IGetAllAccounts _getAllAccounts;
        private readonly IGetAccountTransactions _getAccountTransactions;
        public AccountController(ICreateAccount createAccount, IDeleteAccount deleteAccount, IGetAllAccounts getAllAccounts, IGetAccountTransactions getAccountTransactions)
        {
            _createAccount = createAccount;
            _deleteAccount = deleteAccount;
            _getAllAccounts = getAllAccounts;
            _getAccountTransactions = getAccountTransactions;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAccount(CreateAccountDTO createAccountDTO)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            Account account = await _createAccount.ExecuteAsync(createAccountDTO, userId);
            return Ok(account);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            _deleteAccount.ExecuteAsync(accountId);
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAccounts()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            IEnumerable<AccountDTO> accounts = await _getAllAccounts.ExecuteAsync(userId);
            return Ok(accounts);
        }

        [HttpGet("transactions")]
        [Authorize]
        public async Task<IActionResult> GetAllTransactions(Guid accountId)
        {
            IEnumerable<TransactionDTO> transactions = await _getAccountTransactions.ExecuteAsync(accountId);
            return Ok(transactions);
        }
    }
}
