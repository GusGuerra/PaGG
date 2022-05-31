using System;
using System.Runtime.Serialization;

namespace PaGG.Core.Response
{
    [DataContract]
    public class TransactionResponse
    {
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
