using System.Runtime.Serialization;

namespace PaGG.Core.Request
{
    [DataContract]
    public class BillingOptionRequest
    {
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string CardHolder { get; set; }
        [DataMember]
        public string CardExpiration { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
    }
}
