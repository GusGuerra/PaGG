
namespace PaGG.Core.Utilities
{
    public static class LockUtils
    {
        private static readonly string AccountIdLockPrefix = "accountId://";
        private static readonly string TransactionIdLockPrefix = "transactionId://";

        public static string GetAccountLockKey(string accountId)
            => $"{AccountIdLockPrefix}{accountId}";

        public static string GetTransactionLockKey(string transactionId)
            => $"{TransactionIdLockPrefix}{transactionId}";
    }
}
