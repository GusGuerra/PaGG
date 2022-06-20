using Microsoft.AspNetCore.Mvc;
using PaGG.Business;
using PaGG.Core.Exceptions;
using PaGG.Core.Models;
using PaGG.Core.Request;
using PaGG.Core.Response;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PaGG.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionOperations _transactionOperations;
        private readonly IAccountOperations _accountOperations;

        public TransactionsController(ITransactionOperations transactionOperations,
            IAccountOperations accountOperations)
        {
            _accountOperations = accountOperations;
            _transactionOperations = transactionOperations;
        }

        /// <summary>
        /// Retrieves the transaction associated with the given id
        /// </summary>
        /// <returns>Information about the requested transaction</returns>
        [HttpGet("{id}")]
        public async Task<TransactionResponse> GetTransactionById(string id)
        {
            var transaction = await _transactionOperations.GetTransactionAsync(id);

            // move this to a converter class
            return new TransactionResponse(transaction);
        }

        /// <summary>
        /// Creates (starts) a new transaction between two existing accounts
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Information about the created transaction</returns>
        [HttpPost]
        public async Task<TransactionResponse> StartTransaction(TransactionRequest request)
        {
            // validate the request object
            var getSender = _accountOperations.GetAccountAsync(request.SenderId);
            var getReceiver = _accountOperations.GetAccountAsync(request.ReceiverId);

            await Task.WhenAll(getSender, getReceiver);
            
            var transaction = await _transactionOperations.CreateTransactionAsync(getSender.Result, getReceiver.Result, request.Amount);

            // move this to a converter class
            return new TransactionResponse(transaction);
        }

        /// <summary>
        /// Called by external bank system. Updates the status of an ongoing transaction
        /// </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns>If <paramref name="id"/> and <paramref name="status"/> are valid: 200 ("OK") with empty response;
        /// Otherwise, 400 ("Bad Request")</returns>
        [HttpPost("{id}/callback/{status}")]
        public async Task TransactionCallback(string id, string status)
        {
            var getTransaction = _transactionOperations.GetTransactionAsync(id);

            if (!Enum.IsDefined(typeof(TransactionStatus), status))
                throw new PaGGCustomException(HttpStatusCode.BadRequest, ExceptionMessages.InvalidTransactionStatus);
            
        }
    }
}
