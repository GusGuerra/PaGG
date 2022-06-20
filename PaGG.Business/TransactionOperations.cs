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
        private readonly IAccountOperations _accountOperations;

        public TransactionOperations(IDatabaseOperations databaseOperations, ILockOperations lockOperations,
            IAccountOperations accountOperations)
        {
            _databaseOperations = databaseOperations;
            _lockOperations = lockOperations;
            _accountOperations = accountOperations;
        }

        public async Task<Transaction> GetTransactionAsync(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new PaGGCustomException(HttpStatusCode.BadRequest, ExceptionMessages.InvalidTransactionId);

            var tx = await _databaseOperations.GetTransactionAsync(transactionId);
            
            if (tx == null)
                throw new PaGGCustomException(HttpStatusCode.NotFound, ExceptionMessages.TransactionNotFound);
            
            return tx;
        }

        public async Task<Transaction> CreateTransactionAsync(Account receiver, Account sender, decimal amount)
        {
            // check if both accounts are valid
            var transaction = new Transaction(receiver.Id, sender.Id, amount);

            await Task.WhenAll(
                _lockOperations.PerformAccountLock(sender.Id),
                _lockOperations.PerformAccountLock(receiver.Id),
                _lockOperations.PerformTransactionLock(transaction.Id));

            await _databaseOperations.SaveTransactionAsync(transaction);
            _ = PerformTransactionAsync(receiver, sender, transaction);

            return transaction;
        }

        private async Task PerformTransactionAsync(Account receiver, Account sender, Transaction transaction)
        {
            if (sender.Balance >= transaction.AmountAsLong) // external call to bank is not necessary
            {
                receiver.AddBalance(transaction.AmountAsLong);
                sender.SubtractBalance(transaction.AmountAsLong);

                transaction.SetStatusWithTimestamp(TransactionStatus.Authorized.ToString());
            }
            else if (sender.Wallet.Count != 0)
            {
                // decrease balance from sender through external call
                receiver.AddBalance(transaction.AmountAsLong);
                transaction.SetStatusWithTimestamp(TransactionStatus.Authorizing.ToString());
            }
            else
            {
                // cannot grab money from sender
                transaction.SetStatusWithTimestamp(TransactionStatus.Canceled.ToString());
            }

            _ = Task.Run(() =>
            {
                _databaseOperations.SaveTransactionAsync(transaction);
                _accountOperations.SaveAccountAsync(sender);
                _accountOperations.SaveAccountAsync(receiver);
            });
        }
    }
}
