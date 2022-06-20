using PaGG.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaGG.Backstage
{
    // TODO: Refactor class into DatabaseOperations <T, TDB>
    public class DatabaseOperations : IDatabaseOperations
    {
        // Temporary "mock" databases
        private readonly List<Transaction> TransactionDatabase;
        private readonly List<Account> AccountDatabase;

        public DatabaseOperations()
        {
            TransactionDatabase = new List<Transaction>();
            AccountDatabase = new List<Account>();
        }

        public async Task<Transaction> GetTransactionAsync(string transactionId)
        {
            return TransactionDatabase.FirstOrDefault(t => t.Id == transactionId);
        }

        public async Task<Account> GetAccountAsync(string accountId)
        {
            return AccountDatabase.FirstOrDefault(a => a.Id == accountId);
        }

        public async Task SaveTransactionAsync(Transaction transaction)
        {
            var existingTransaction = TransactionDatabase.FirstOrDefault(dbTransaction => dbTransaction.Id == transaction.Id);
            if (existingTransaction == null)
                TransactionDatabase.Add(transaction);
            else
                existingTransaction = transaction;
        }

        public async Task SaveAccountAsync(Account account)
        {
            var existingAccount = AccountDatabase.FirstOrDefault(dbAccount => dbAccount.Id == account.Id);
            if (existingAccount == null)
                AccountDatabase.Add(account);
            else
                existingAccount = account;
        }
    }
}
