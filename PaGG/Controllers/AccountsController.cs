﻿using Microsoft.AspNetCore.Mvc;
using PaGG.Business;
using PaGG.Core.Models;
using PaGG.Core.Request;
using PaGG.Core.Response;
using System.Linq;
using System.Threading.Tasks;

namespace PaGG.Controllers
{
    [ApiController]
    [Route("api/account")]
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
        public async Task<AccountResponse> GetAccountById(string id)
        {
            var account = await _accountOperations.GetAccountAsync(id);
            return new AccountResponse(account);
        }

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The newly created account</returns>
        [HttpPost]
        public async Task<AccountResponse> CreateAccount(AccountRequest request)
        {
            var wallet = request.Wallet.Select(bo => new BillingOption(bo));
            var account = await _accountOperations.CreateAccountAsync(request.AccountOwner, wallet);
            return new AccountResponse(account);
        }

        /// <summary>
        /// Adds balance to account
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The newly created account</returns>
        [HttpPost("{id}/balance")]
        public async Task<AccountResponse> AddBalance(BalanceRequest request, string id)
        {
            var account = await _accountOperations.GetAccountAsync(id);

            account.AddBalance(request.Amount);
            await _accountOperations.SaveAccountAsync(account);
            
            return new AccountResponse(account);
        }
    }
}
