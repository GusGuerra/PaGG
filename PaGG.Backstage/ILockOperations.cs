using System.Threading.Tasks;

namespace PaGG.Backstage
{
    public interface ILockOperations
    {
        Task<string> PerformAccountLock(string key);
        Task<string> PerformTransactionLock(string key);
        Task ReleaseAccountLock(string key, string lockId);
        Task ReleaseTransactionLock(string key, string lockId);
    }
}
