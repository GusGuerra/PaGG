using PaGG.Core.Models;
using System;
using System.Runtime.Serialization;

namespace PaGG.Core.Response
{
    [DataContract]
    public class TransactionResponse
    {
        public TransactionResponse(Transaction source)
        {
            Status = source.Status;
            TransactionId = source.Id;
            CreatedAt = source.CreatedAt;
            LastUpdate = source.StatusTimestamp;
            Type = source.Type;
        }

        public string Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }
        public TransactionType Type { get; set; }
    }
}
