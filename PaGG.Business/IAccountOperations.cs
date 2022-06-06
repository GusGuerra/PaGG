using PaGG.Core.Models;

namespace PaGG.Business
{
    public interface IAccountOperations
    {
        public Account GetAccount(string accountId);
    }
}