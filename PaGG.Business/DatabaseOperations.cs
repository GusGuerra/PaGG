using PaGG.Core;
using PaGG.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaGG.Business
{
    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly List<Transaction> TransactionDatabase;
        private readonly List<Account> AccountDatabase;

        public DatabaseOperations()
        {
            TransactionDatabase = new List<Transaction>();
            AccountDatabase = new List<Account>();
        }

        public Transaction GetTransaction(string transactionId)
        {
            var tx = TransactionDatabase.FirstOrDefault(t => t.Id == transactionId);
            if (tx == null)
                throw new PaGGCustomException(HttpStatusCode.NotFound, ExceptionMessages.TransactionNotFound);
            return tx;
        }

        public Account GetAccount(string accountId)
        {
            var acc = AccountDatabase.FirstOrDefault(a => a.Id == accountId);
            if (acc == null)
                throw new PaGGCustomException(HttpStatusCode.NotFound, ExceptionMessages.AccountNotFound);
            return acc;
        }

        // TODO: move this to TransactionOperations
        public async Task<Transaction> CreateTransactionAsync(string receiverId, string senderId, decimal amount)
        {
            var transaction = new Transaction(receiverId, senderId, amount);
            // lock senderId
            // lock receiverId
            // lock transaction.Id
            await SaveTransactionAsync(transaction);
            _ = PerformTransactionAsync(transaction);
            return transaction;
        }

        // TODO: move this to TransactionOperations
        private async Task SaveTransactionAsync(Transaction transaction)
        {
            TransactionDatabase.Add(transaction);
        }

        // TODO: move this to TransactionOperations
        private async Task PerformTransactionAsync(Transaction transaction)
        {
            // decide between internal or external operation
            // perform operation
            // modify balances
        }
    }
}
