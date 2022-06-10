using PaGG.Core.Utilities;
using System.Collections.Generic;

namespace PaGG.Core.Models
{
    public class Account
    {
        public Account()
        {
            Id = RandomUtils.CreateNewUniqueId();
            Balance = 0;
        }

        public string Id { get; set; }
        public long Balance { get; set; }
        public string AccountOwner { get; set; }
        public List<BillingOption> Wallet { get; set; }
        public AccountType Type => Wallet.Count != 0
            ? AccountType.External
            : AccountType.Internal;

        public bool IsValid()
        {
            foreach (var billingOption in Wallet)
                if (!billingOption.IsValid()) return false;
            
            return true;
        }
    }
}
