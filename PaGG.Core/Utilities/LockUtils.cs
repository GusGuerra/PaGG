
using System;

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

        public static int GetRetryDelay(double factor)
        {
            double y = Math.Round(Math.Exp(factor) * 0.01, MidpointRounding.ToEven);
            y = Math.Min(1000, y);

            return (int)y;
        }

        public static double UpdateRetryFactor(int retry, double factor, int retryMod)
        {
            if (retry % retryMod == 0)
                factor += 1;

            return factor;
        }
    }
}
