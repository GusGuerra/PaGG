using Microsoft.AspNetCore.Mvc;
using PaGG.Business;
using PaGG.Core.Request;
using PaGG.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaGG.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountOperations _accountOperations;

        public AccountsController(IAccountOperations accountOperations)
        {
            _accountOperations = accountOperations;
        }

        /// <summary>
        /// Retrieves the account associated with the given id
        /// </summary>
        /// <returns>Information about the requested account</returns>
        [HttpGet("{id}")]
        public AccountResponse GetAccountById(string id)
        {
            var account = _accountOperations.GetAccount(id);

            return new AccountResponse();
        }

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The newly created account</returns>
        [HttpPost]
        public async Task<AccountResponse> CreateAccount(AccountRequest request)
        {
            // validate the request object

            var account = await _accountOperations.CreateAccountAsync();
            return new AccountResponse();
        }
    }
}
