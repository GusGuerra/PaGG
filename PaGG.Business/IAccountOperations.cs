using PaGG.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaGG.Business
{
    public interface IAccountOperations
    {
        public Task<Account> GetAccountAsync(string accountId);
        public Task SaveAccountAsync(Account account);
        public Task<Account> CreateAccountAsync(string accountOwner, IEnumerable<BillingOption> wallet);
    }
}