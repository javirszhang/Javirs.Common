using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Javirs.Common.Security
{
    public class AesCryptoProvider
    {
        private string hexKey;
        private bool useSecureRandom;
        private CipherMode cipherMode;
        private PaddingMode paddingMode;

        public CipherMode CipherMode
        {
            get
            {
                return cipherMode;
            }

            set
            {
                cipherMode = value;
            }
        }

        public PaddingMode PaddingMode
        {
            get
            {
                return paddingMode;
            }

            set
            {
                paddingMode = value;
            }
        }

        public AesCryptoProvider(string hexKey, bool useSecureRandom)
        {
            this.hexKey = hexKey;
            this.useSecureRandom = useSecureRandom;
            this.cipherMode = CipherMode.ECB;
            this.paddingMode = PaddingMode.PKCS7;
        }
        private byte[] GetKeyBytes()
        {
            var keyBytes = hexKey.HexString2ByteArray();
            if (useSecureRandom)
            {
                SecureRandomKeyGenerator srkg = new SecureRandomKeyGenerator(keyBytes);
                return srkg.GenerateKey();
            }
            return keyBytes;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainBytes">明文字节数组</param>
        /// <returns>返回密文字节数组</returns>
        public byte[] Encrypt(byte[] plainBytes)
        {
            var aes = new System.Security.Cryptography.AesCryptoServiceProvider();
            aes.Mode = this.CipherMode;
            aes.Padding = this.PaddingMode;
            aes.Key = GetKeyBytes();
            var encryptor = aes.CreateEncryptor();
            var bResult = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return bResult;
        }
        /// <summary>
        /// 加密，使用UTF8编码明文，密文以base64返回
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <returns>base64编码密文</returns>
        public string Encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] result = Encrypt(plainBytes);
            return Convert.ToBase64String(result);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cipherBytes">密文字节数组</param>
        /// <returns>返回明文字节数组</returns>
        public byte[] Decrypt(byte[] cipherBytes)
        {
            var aes = new AesCryptoServiceProvider();
            aes.Mode = this.CipherMode;
            aes.Padding = this.PaddingMode;
            aes.Key = GetKeyBytes();
            var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return plainBytes;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="base64">base64编码的密文</param>
        /// <returns>范围UTF8编码明文</returns>
        public string Decrypt(string base64)
        {
            byte[] cipherBytes = Convert.FromBase64String(base64);
            byte[] plainBytes = Decrypt(cipherBytes);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
