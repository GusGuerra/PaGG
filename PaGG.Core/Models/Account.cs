using System.Collections.Generic;

namespace PaGG.Core.Models
{
    public class Account
    {
        public string Id { get; set; }
        public long Balance { get; set; }
        public AccountType Type;
        public List<BillingOption> Wallet;
    }
}
