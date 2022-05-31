using Microsoft.AspNetCore.Mvc;
using PaGG.Business;
using PaGG.Core.Request;
using PaGG.Core.Response;
using System.Threading.Tasks;

namespace PaGG.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionsController : ControllerBase
    {
        private readonly IDatabaseOperations _databaseOperations;

        public TransactionsController(IDatabaseOperations databaseOperations)
        {
            _databaseOperations = databaseOperations;
        }

        /// <summary>
        /// Retrieves the transaction associated with the given id
        /// </summary>
        /// <returns>Information about the requested transaction</returns>
        [HttpGet("{id}")]
        public TransactionResponse GetTransactionById(string id)
        {
            // Business Layer -> Service Layer -> Fetch database
            var transaction = _databaseOperations.GetTransaction(id);

            // move this to a converter class
            return new TransactionResponse()
            {
                Status = transaction.Status,
                TransactionId = transaction.Id,
                LastUpdate = transaction.StatusTimestamp,
                CreatedAt = transaction.CreatedAt
            };
        }

        [HttpPost]
        public async Task<TransactionResponse> StartTransaction(TransactionRequest request)
        {
            // validate the request object

            // call business layer to validate sender and receiver
            
            var transaction = await _databaseOperations.CreateTransactionAsync(request.ReceiverId, request.SenderId, request.Amount);

            // move this to a converter class
            return new TransactionResponse() { Status = transaction.Status, TransactionId = transaction.Id };
        }
    }
}
