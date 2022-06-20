using PaGG.Core.Models;
using System.Threading.Tasks;

namespace PaGG.Backstage
{
    public interface IDatabaseOperations
    {
        Task<Account> GetAccountAsync(string accountId);
        Task<Transaction> GetTransactionAsync(string transactionId);
        Task SaveTransactionAsync(Transaction transaction);
        Task SaveAccountAsync(Account account);
    }
}