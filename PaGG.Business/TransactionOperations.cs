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

            await PerformLocksForTransaction(receiver, sender, transaction);

            await Task.WhenAll(
                _databaseOperations.SaveTransactionAsync(transaction),
                _databaseOperations.SaveAccountAsync(sender),
                _databaseOperations.SaveAccountAsync(receiver)
            );

            await PrepareTransactionAsync(sender, transaction);

            return transaction;
        }

        private async Task PerformLocksForTransaction(Account receiver, Account sender, Transaction transaction)
        {
            var lockSender = Task.Run(async () => { sender.LockId = await _lockOperations.PerformAccountLock(sender.Id); });
            var lockReceiver = Task.Run(async () => { receiver.LockId = await _lockOperations.PerformAccountLock(receiver.Id); });
            var lockTransaction = Task.Run(async () => { transaction.LockId = await _lockOperations.PerformTransactionLock(transaction.Id); });

            await Task.WhenAll(lockSender, lockReceiver, lockTransaction);
        }

        private Task PrepareTransactionAsync(Account sender, Transaction transaction)
        {
            transaction.SetStatusWithTimestamp(TransactionStatus.Authorizing.ToString());

            if (sender.Balance >= transaction.AmountAsLong) // external call to bank is not necessary
            {
                transaction.Type = TransactionType.Internal;
                // raise pipeline event for internal transaction
            }
            else if (sender.Wallet.Count != 0)
            {
                transaction.Type = TransactionType.External;
                // raise pipeline event for external transaction
            }
            else
            {
                // cannot grab money from sender
                transaction.SetStatusWithTimestamp(TransactionStatus.Canceled.ToString());
            }

            return _databaseOperations.SaveTransactionAsync(transaction);
        }

        public async Task<Transaction> FinishTransactionAsync(Transaction transaction, string status)
        {
            transaction.SetStatusWithTimestamp(status);

            var receiver = await _accountOperations.GetAccountAsync(transaction.ReceiverId);
            var sender = await _accountOperations.GetAccountAsync(transaction.SenderId);

            if (transaction.Type == TransactionType.Internal)
            {
                receiver.AddBalance(transaction.AmountAsLong);
                sender.AddBalance(-transaction.AmountAsLong);
            }
            else if (transaction.Type == TransactionType.External)
            {
                // sender balance already decreased through external call
                receiver.AddBalance(transaction.AmountAsLong);
            }

            await ReleaseLockForTransaction(transaction, receiver, sender);

            await Task.WhenAll(
                _databaseOperations.SaveTransactionAsync(transaction),
                _accountOperations.SaveAccountAsync(sender),
                _accountOperations.SaveAccountAsync(receiver)
            );

            return transaction;
        }

        private async Task ReleaseLockForTransaction(Transaction transaction, Account receiver, Account sender)
        {
            await Task.WhenAll(
                _lockOperations.ReleaseAccountLock(receiver.Id, receiver.LockId),
                _lockOperations.ReleaseAccountLock(sender.Id, sender.LockId),
                _lockOperations.ReleaseTransactionLock(transaction.Id, transaction.LockId));

            receiver.LockId = null;
            sender.LockId = null;
            transaction.LockId = null;
        }
    }
}
