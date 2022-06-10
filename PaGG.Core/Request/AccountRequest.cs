using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PaGG.Core.Request
{
    [DataContract]
    public class AccountRequest
    {
        [DataMember]
        public string AccountOwner { get; set; }
        [DataMember]
        public List<BillingOptionRequest> Wallet;
    }
}
