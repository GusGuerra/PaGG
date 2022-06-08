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
        private readonly ITransactionOperations _transactionOperations;

        public TransactionsController(ITransactionOperations transactionOperations)
        {
            _transactionOperations = transactionOperations;
        }

        /// <summary>
        /// Retrieves the transaction associated with the given id
        /// </summary>
        /// <returns>Information about the requested transaction</returns>
        [HttpGet("{id}")]
        public TransactionResponse GetTransactionById(string id)
        {
            var transaction = _transactionOperations.GetTransaction(id);

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
            
            var transaction = await _transactionOperations.CreateTransactionAsync(request.ReceiverId, request.SenderId, request.Amount);

            // move this to a converter class
            return new TransactionResponse() { Status = transaction.Status, TransactionId = transaction.Id };
        }
    }
}
