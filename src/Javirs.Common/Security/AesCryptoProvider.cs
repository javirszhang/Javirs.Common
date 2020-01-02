using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Javirs.Common.Security
{
    /// <summary>
    /// aes加解密提供程序
    /// </summary>
    public class AesCryptoProvider
    {
        private byte[] keyBytes;
        private bool useSecureRandom;
        /// <summary>
        /// 密文模式
        /// </summary>
        public CipherMode CipherMode { get; set; }
        /// <summary>
        /// 填充模式
        /// </summary>
        public PaddingMode PaddingMode { get; set; }
        /// <summary>
        /// 向量
        /// </summary>
        public byte[] IV { get; set; }
        /// <summary>
        /// aes加解密提供程序
        /// </summary>
        /// <param name="hexKey"></param>
        /// <param name="useSecureRandom">使用随机密钥，对接java</param>
        public AesCryptoProvider(string hexKey, bool useSecureRandom)
        {
            this.keyBytes = hexKey.HexString2ByteArray();
            this.useSecureRandom = useSecureRandom;
            this.CipherMode = CipherMode.ECB;
            this.PaddingMode = PaddingMode.PKCS7;
        }
        /// <summary>
        /// aes加解密提供程序
        /// </summary>
        /// <param name="key"></param>
        public AesCryptoProvider(byte[] key)
        {
            this.keyBytes = key;
            this.useSecureRandom = false;
            this.CipherMode = CipherMode.ECB;
            this.PaddingMode = PaddingMode.PKCS7;
        }
        private byte[] GetKeyBytes()
        {
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
            if (aes.Mode == CipherMode.CBC && IV == null)
            {
                IV = initIV(keyBytes.Length);
            }
            if (IV != null)
            {
                aes.IV = IV;
            }
            var encryptor = aes.CreateEncryptor();
            var bResult = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return bResult;
        }
        private static byte[] initIV(int blockSize)
        {
            byte[] iv = new byte[blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                iv[0] = 0;
            }
            return iv;
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
            if (aes.Mode == CipherMode.CBC && IV == null)
            {
                IV = initIV(keyBytes.Length);
            }
            if (IV != null)
            {
                aes.IV = IV;
            }
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
