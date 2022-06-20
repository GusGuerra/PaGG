using PaGG.Core.Utilities;
using System;

namespace PaGG.Core.Models
{
    public class Transaction
    {
        public Transaction(string senderId, string receiverId, decimal amount)
        {
            CreatedAt = DateTime.UtcNow;
            SetStatusWithTimestamp(TransactionStatus.Processing.ToString());
            SenderId = senderId; ReceiverId = receiverId; Amount = amount;
            Id = RandomUtils.CreateNewUniqueId();
        }

        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public decimal Amount { get; set; }
        public long AmountAsLong => (long)(Amount * 100M);
        public DateTime CreatedAt { get; private set; }
        public DateTime StatusTimestamp { get; set; }
        public string Status { get; set; }
        public TransactionType Type { get; set; }

        public void SetStatusWithTimestamp(string status)
        {
            Status = status;
            StatusTimestamp = DateTime.Now;
        }
    }
}
