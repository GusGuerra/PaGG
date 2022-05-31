using System.Runtime.Serialization;

namespace PaGG.Core.Request
{
    [DataContract]
    public class TransactionRequest
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public decimal Amount { get; set; }

        [IgnoreDataMember]
        public long AmountAsLong => (long)(Amount * 100M);
    }
}
