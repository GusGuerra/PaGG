using System;
using System.Security.Cryptography;

namespace PaGG.Core.Utilities
{
    public static class RandomUtils
    {
        private readonly static RandomNumberGenerator _random;

        static RandomUtils()
        {
            _random = RandomNumberGenerator.Create();
        }

        public static string CreateNewUniqueId()
        {
            byte[] randomBytes = new byte[16];
            _random.GetBytes(randomBytes);
            return new Guid(randomBytes).ToString();
        }
    }
}
