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
        public string LockId { get; set; }
        public long Balance { get; set; }
        public string AccountOwner { get; set; }
        public List<BillingOption> Wallet { get; set; }
        
        public void AddBalance(long delta) => Balance += delta;
        public void SubtractBalance(long delta) => Balance -= delta;
    }
}
