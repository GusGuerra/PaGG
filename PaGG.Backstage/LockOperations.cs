using PaGG.Core.Exceptions;
using PaGG.Core.Utilities;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace PaGG.Backstage
{
    public class LockOperations : ILockOperations
    {
        private readonly ConnectionMultiplexer connection;

        private readonly int NumberOfRetries = 2;
        private readonly int RetryModulo = 5;
        private readonly double RetryDelayFactor = 0.1;
        private readonly string endpoint = "localhost";

        public LockOperations()
        {
            ConfigurationOptions config = new()
            {
                EndPoints = { endpoint }, 
                AbortOnConnectFail = false,
                KeepAlive = 10,
                Proxy = Proxy.Twemproxy
            };

            connection = ConnectionMultiplexer.Connect(config);
        }

        public async Task<string> PerformAccountLock(string key)
        {
            key = LockUtils.GetAccountLockKey(key);
            return await PerformLock(key);
        }

        public async Task<string> PerformTransactionLock(string key)
        {
            key = LockUtils.GetTransactionLockKey(key);
            return await PerformLock(key);
        }

        private async Task<string> PerformLock(string key)
        {
            string lockId = Guid.NewGuid().ToString("n");

            string storedLockId = await TryLock(key, lockId);

            int retries = NumberOfRetries;
            double factor = RetryDelayFactor;

            do
            {
                retries -= 1;

                if (storedLockId == lockId)
                    return lockId;

                int delay = LockUtils.GetRetryDelay(factor);
                await Task.Delay(delay);
                factor = LockUtils.UpdateRetryFactor(retries, factor, RetryModulo);

            } while (retries >= 0);

            throw new PaGGCustomException(ExceptionMessages.ObjectLocked);
        }

        public async Task ReleaseAccountLock(string key, string lockId)
        {
            key = LockUtils.GetAccountLockKey(key);
            await ReleaseLock(key, lockId);
        }

        public async Task ReleaseTransactionLock(string key, string lockId)
        {
            key = LockUtils.GetTransactionLockKey(key);
            await ReleaseLock(key, lockId);
        }

        private async Task ReleaseLock(string key, string lockId)
        {
            string storedLockId = await Get(key);
            if (storedLockId == lockId)
                await Remove(key);
        }

        private async Task<string> TryLock(string key, string operationId)
        {
            await SetEntryIfNotExists(key, operationId);
            return await Get(key);
        }

        private async Task<string> Get(string key)
        {
            IDatabase database = GetDatabase();
            return await database.StringGetAsync(key, CommandFlags.None);
        }

        private async Task Remove(string key)
        {
            IDatabase database = GetDatabase();
            await database.KeyDeleteAsync(key, CommandFlags.None);
        }

        private async Task SetEntryIfNotExists(string key, string operationId)
        {
            IDatabase database = GetDatabase();
            await database.StringSetAsync(key, operationId, null, When.NotExists, CommandFlags.None);
        }

        private IDatabase GetDatabase()
        {
            return this.connection.GetDatabase();
        }
    }
}
