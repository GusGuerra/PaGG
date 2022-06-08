using PaGG.Core.Models;
using System.Threading.Tasks;

namespace PaGG.Business
{
    public interface IAccountOperations
    {
        public Account GetAccount(string accountId);
        public Task<Account> CreateAccountAsync();
    }
}