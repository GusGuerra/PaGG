using PaGG.Core.Models;
using System.Threading.Tasks;

namespace PaGG.Backstage
{
    public interface IDatabaseOperations
    {
        Account GetAccount(string accountId);
        Transaction GetTransaction(string transactionId);
        Task SaveTransactionAsync(Transaction transaction);
        Task SaveAccountAsync(Account account);
    }
}