using PaGG.Core.Models;
using System.Threading.Tasks;

namespace PaGG.Business
{
    public interface ITransactionOperations
    {
        Task<Transaction> GetTransactionAsync(string transactionId);
        Task<Transaction> CreateTransactionAsync(Account receiver, Account sender, decimal amount);
    }
}