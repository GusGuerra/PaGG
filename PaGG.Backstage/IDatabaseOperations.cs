using PaGG.Core.Models;
using System.Threading.Tasks;

namespace PaGG.Backstage
{
    public interface IDatabaseOperations
    {
        Task<Transaction> CreateTransactionAsync(string receiverId, string senderId, decimal amount);
        Account GetAccount(string accountId);
        Transaction GetTransaction(string transactionId);
    }
}