using PaGG.Core.Models;
using System.Threading.Tasks;

namespace PaGG.Business
{
    public interface ITransactionOperations
    {
        Transaction GetTransaction(string transactionId);
        Task<Transaction> CreateTransactionAsync(string receiverId, string senderId, decimal amount);
    }
}