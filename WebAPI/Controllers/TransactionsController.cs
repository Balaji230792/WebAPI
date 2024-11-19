using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController(ITransactionService transactionService) : ControllerBase
    {
        private readonly ITransactionService _transactionService = transactionService;
        [Route("gettransactionlist")]
        [Authorize]
        public IActionResult GetTransactionList([FromQuery] string accountNumber)
        {
            var result = _transactionService.GetTransactions(accountNumber);
            return Ok(result);
        }
    }
}
