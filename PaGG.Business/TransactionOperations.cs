using PaGG.Backstage;
using PaGG.Core.Exceptions;
using PaGG.Core.Models;
using System.Net;
using System.Threading.Tasks;

namespace PaGG.Business
{
    public class TransactionOperations : ITransactionOperations
    {
        private readonly IDatabaseOperations _databaseOperations;
        private readonly ILockOperations _lockOperations;

        public TransactionOperations(IDatabaseOperations databaseOperations)
        {
            _databaseOperations = databaseOperations;
            _lockOperations = new LockOperations(); // TODO: Figure if I can do this another way
        }

        public Transaction GetTransaction(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new PaGGCustomException(HttpStatusCode.BadRequest, ExceptionMessages.InvalidTransactionId);

            var tx = _databaseOperations.GetTransaction(transactionId);
            
            if (tx == null)
                throw new PaGGCustomException(HttpStatusCode.NotFound, ExceptionMessages.TransactionNotFound);
            
            return tx;
        }

        public async Task<Transaction> CreateTransactionAsync(string receiverId, string senderId, decimal amount)
        {
            // check if both accounts are valid
            var transaction = new Transaction(receiverId, senderId, amount);

            await Task.WhenAll(
                _lockOperations.PerformAccountLock(senderId),
                _lockOperations.PerformAccountLock(receiverId),
                _lockOperations.PerformTransactionLock(transaction.Id));

            await _databaseOperations.SaveTransactionAsync(transaction);
            _ = PerformTransactionAsync(transaction);

            return transaction;
        }

        private async Task PerformTransactionAsync(Transaction transaction)
        {
            // decide between internal or external operation
            // perform operation
            // modify balances
        }
    }
}
