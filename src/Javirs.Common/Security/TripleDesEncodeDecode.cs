using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Javirs.Common.Security
{
    /// <summary>
    /// 3des加密解密
    /// </summary>
    public class TripleDesEncodeDecode
    {
        /// <summary>
        /// 3des双倍长加密字符串版本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TwiceDesEncrypt(string key, string source)
        {
            byte[] mm = key.HexString2ByteArray();
            byte[] data = source.HexString2ByteArray();
            byte[] iv = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            return TwiceDesEncrypt(mm, iv, data).Byte2HexString();
        }
        public static string TwiceDesDescrypt(string key, string source)
        {
            byte[] mm = key.HexString2ByteArray();
            byte[] data = source.HexString2ByteArray();
            byte[] iv = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            return TwiceDesDescrypt(mm, iv, data).Byte2HexString();
        }
        /// <summary>
        /// 3des二倍长加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] TwiceDesEncrypt(byte[] key, byte[] iv, byte[] data)
        {
            byte[] key1 = new byte[8];
            Array.Copy(key, 0, key1, 0, 8);
            byte[] key2 = new byte[8];
            Array.Copy(key, 8, key2, 0, 8);
            byte[] data1 = EncryptECB(data, key1, iv);
            data1 = DecryptECB(data1, key2, iv);
            data1 = EncryptECB(data1, key1, iv);
            return data1;
        }
        /// <summary>
        /// 3des二倍长解密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] TwiceDesDescrypt(byte[] key, byte[] iv, byte[] data)
        {

            byte[] key1 = new byte[8];
            Array.Copy(key, 0, key1, 0, 8);
            byte[] key2 = new byte[8];
            Array.Copy(key, 8, key2, 0, 8);

            byte[] data1 = DecryptECB(data, key1, iv);
            data1 = EncryptECB(data1, key2, iv);
            data1 = DecryptECB(data1, key1, iv);
            return data1;
        }

        /// <summary>
        /// ECB解密
        /// </summary>
        /// <param name="encryptedDataBytes"></param>
        /// <param name="keys"></param>
        /// <param name="iv"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        private static byte[] DecryptECB(byte[] encryptedDataBytes, byte[] keys, Byte[] iv, CipherMode mode = CipherMode.ECB, PaddingMode padding = PaddingMode.PKCS7)
        {
            MemoryStream tempStream = new MemoryStream(encryptedDataBytes, 0, encryptedDataBytes.Length);
            DESCryptoServiceProvider decryptor = new DESCryptoServiceProvider();
            decryptor.Mode = mode;
            decryptor.Padding = padding;
            CryptoStream decryptionStream = new CryptoStream(tempStream, decryptor.CreateDecryptor(keys, iv), CryptoStreamMode.Read);
            byte[] data = new byte[encryptedDataBytes.Length];
            decryptionStream.Read(data, 0, data.Length);
            decryptionStream.Close();
            tempStream.Close();
            return data;

        }
        /// <summary>
        /// ECB加密
        /// </summary>
        /// <param name="sourceDataBytes"></param>
        /// <param name="keys"></param>
        /// <param name="iv"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        private static byte[] EncryptECB(byte[] sourceDataBytes, byte[] keys, Byte[] iv, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {

            //Byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            MemoryStream tempStream = new MemoryStream();
            //get encryptor and encryption stream
            DESCryptoServiceProvider encryptor = new DESCryptoServiceProvider();
            encryptor.Mode = cipherMode;
            encryptor.Padding = paddingMode;
            CryptoStream encryptionStream = new CryptoStream(tempStream, encryptor.CreateEncryptor(keys, iv), CryptoStreamMode.Write);
            encryptionStream.Write(sourceDataBytes, 0, sourceDataBytes.Length);
            encryptionStream.FlushFinalBlock();
            encryptionStream.Close();
            byte[] encryptedDataBytes = tempStream.ToArray();
            tempStream.Close();
            return encryptedDataBytes;
        }
    }
}
