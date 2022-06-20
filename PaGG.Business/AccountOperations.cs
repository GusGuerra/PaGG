using PaGG.Backstage;
using PaGG.Core.Exceptions;
using PaGG.Core.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PaGG.Business
{
    public class AccountOperations : IAccountOperations
    {
        private readonly IDatabaseOperations _databaseOperations;

        public AccountOperations(IDatabaseOperations databaseOperations)
        {
            _databaseOperations = databaseOperations;
        }

        public async Task<Account> GetAccountAsync(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                throw new PaGGCustomException(HttpStatusCode.BadRequest, ExceptionMessages.InvalidAccountId);

            var acc = await _databaseOperations.GetAccountAsync(accountId);
            
            if (acc == null)
                throw new PaGGCustomException(HttpStatusCode.NotFound, ExceptionMessages.AccountNotFound);
            
            return acc;
        }

        public async Task SaveAccountAsync(Account account)
        {
            await _databaseOperations.SaveAccountAsync(account);
        }

        public async Task<Account> CreateAccountAsync(string accountOwner, IEnumerable<BillingOption> wallet)
        {
            var account = new Account()
            {
                AccountOwner = accountOwner,
                Wallet = new List<BillingOption>(wallet)
            };
            await SaveAccountAsync(account);
            return account;
        }
    }
}
