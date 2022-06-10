using PaGG.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PaGG.Core.Response
{
    [DataContract]
    public class AccountResponse
    {
        public AccountResponse(Account source)
        {
            Id = source.Id;
            Balance = source.Balance;
            AccountOwner = source.AccountOwner;
            Wallet = new List<BillingOptionResponse>(source.Wallet.Select(x => new BillingOptionResponse(x)));
        }

        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public long Balance { get; set; }
        [DataMember]
        public string AccountOwner { get; set; }
        [DataMember]
        public List<BillingOptionResponse> Wallet { get; set; }
    }
}
