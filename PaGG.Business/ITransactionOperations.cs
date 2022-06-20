using PaGG.Core.Models;
using System.Threading.Tasks;

namespace PaGG.Business
{
    public interface ITransactionOperations
    {
        Task<Transaction> GetTransactionAsync(string transactionId);
        Task<Transaction> CreateTransactionAsync(string receiverId, string senderId, decimal amount);
        Task ValidateTransactionAsync(Account sender, Account receiver, Transaction transaction);
    }
}