using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Javirs.Common
{
    public class SecureRandomKeyGenerator
    {
        private byte[] seed;
        public SecureRandomKeyGenerator(string hexSeed)
        {
            this.seed = hexSeed.HexString2ByteArray();
        }
        public SecureRandomKeyGenerator(byte[] seed)
        {
            this.seed = seed;
        }
        public byte[] GenerateKey()
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                using (var sha2 = new SHA1CryptoServiceProvider())
                {
                    var hash = sha2.ComputeHash(sha1.ComputeHash(this.seed));
                    return hash.Take(16).ToArray();
                }
            }
        }
    }
}
