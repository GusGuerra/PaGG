using PaGG.Backstage;
using PaGG.Core.Exceptions;
using PaGG.Core.Models;
using System.Net;

namespace PaGG.Business
{
    public class AccountOperations : IAccountOperations
    {
        private readonly IDatabaseOperations _databaseOperations;
        public AccountOperations(IDatabaseOperations databaseOperations)
        {
            _databaseOperations = databaseOperations;
        }

        public Account GetAccount(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                throw new PaGGCustomException(HttpStatusCode.BadRequest, ExceptionMessages.InvalidAccountId);

            var acc = _databaseOperations.GetAccount(accountId);
            
            if (acc == null)
                throw new PaGGCustomException(HttpStatusCode.NotFound, ExceptionMessages.AccountNotFound);
            
            return acc;
        }
    }
}
