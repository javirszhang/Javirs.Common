using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Javirs.Common.Security
{
    /// <summary>
    /// 单倍DES加密
    /// </summary>
    public class DesEncodeDecode
    {
        static byte[] defaultIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private string _desKey = string.Empty;
        private CipherMode? _cipherMode;//加密模式
        private PaddingMode? _paddingMode;//填充模式
        private bool _isHexKey;//是否16进制密钥
        private Encoding _encoding;
        /// <summary>
        /// DES加解密
        /// </summary>
        /// <param name="deskey">ASCII编码密钥</param>
        public DesEncodeDecode(string deskey)
        {
            if (string.IsNullOrEmpty(deskey) || deskey.Length < 8)
                throw new ArgumentException("deskey参数不符合规范");
            this._desKey = deskey;
            this._encoding = Encoding.UTF8;
        }
        /// <summary>
        /// DES加解密
        /// </summary>
        /// <param name="deskey">ASCII编码密钥</param>
        /// <param name="cipherMode">密文加密模式</param>
        /// <param name="paddingMode">填充模式</param>
        public DesEncodeDecode(string deskey, CipherMode cipherMode, PaddingMode paddingMode)
            : this(deskey)
        {
            this._cipherMode = cipherMode;
            this._paddingMode = paddingMode;
        }
        /// <summary>
        /// DES加解密
        /// </summary>
        /// <param name="deskey"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="isHexKey"></param>
        public DesEncodeDecode(string deskey, CipherMode cipherMode, PaddingMode paddingMode, bool isHexKey)
            : this(deskey, cipherMode, paddingMode)
        {
            this._isHexKey = isHexKey;
        }
        /// <summary>
        /// DES加解密
        /// </summary>
        /// <param name="deskey"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="isHexKey"></param>
        /// <param name="encoding"></param>
        public DesEncodeDecode(string deskey, CipherMode cipherMode, PaddingMode paddingMode, bool isHexKey, Encoding encoding)
            : this(deskey, cipherMode, paddingMode, isHexKey)
        {
            this._encoding = encoding;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strInput"></param>
        /// <returns></returns>
        public string DesEncrypt(string p_strInput)
        {
            byte[] input = _encoding.GetBytes(p_strInput);
            byte[] output = DesEncrypt(input);
            return Convert.ToBase64String(output);
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] DesEncrypt(byte[] input)
        {
            byte[] byKey = null;
            byte[] IV = null;
            try
            {
                if (!_isHexKey)
                {
                    byKey = _encoding.GetBytes(_desKey.Substring(0, 8));
                    if (_desKey.Length > 8)
                    {
                        IV = _encoding.GetBytes(_desKey.Substring(8, 8));
                    }
                }
                else
                {
                    byKey = _desKey.Substring(0, 16).HexString2ByteArray();
                    if (_desKey.Length > 16)
                    {
                        IV = _desKey.Substring(16, 16).HexString2ByteArray();
                    }
                }
                if (IV == null)
                {
                    IV = defaultIV;
                }
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                if (_cipherMode.HasValue)
                {
                    des.Mode = this._cipherMode.Value;
                }
                if (this._paddingMode.HasValue)
                {
                    des.Padding = this._paddingMode.Value;
                }
                //byte[] inputByteArray = Encoding.UTF8.GetBytes(p_strInput);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strInput"></param>
        /// <returns></returns>
        public string DesDecrypt(string p_strInput)
        {
            var inputByteArray = Convert.FromBase64String(p_strInput);
            byte[] output = DesDecrypt(inputByteArray);
            return _encoding.GetString(output);

        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] DesDecrypt(byte[] input)
        {
            if (input == null || input.Length <= 0)
            {
                return new byte[] { 0x20 };
            }
            byte[] byKey = null;
            byte[] IV = null;
            //byte[] inputByteArray = new Byte[p_strInput.Length];

            try
            {
                if (!_isHexKey)
                {
                    byKey = _encoding.GetBytes(_desKey.Substring(0, 8));
                    if (_desKey.Length > 8)
                    {
                        IV = _encoding.GetBytes(_desKey.Substring(8, 8));
                    }
                }
                else
                {
                    byKey = _desKey.Substring(0, 16).HexString2ByteArray();
                    if (_desKey.Length > 16)
                    {
                        IV = _desKey.Substring(16, 16).HexString2ByteArray();
                    }
                }
                if (IV == null)
                {
                    IV = defaultIV;
                }
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                if (_cipherMode.HasValue)
                {
                    des.Mode = this._cipherMode.Value;
                }
                if (this._paddingMode.HasValue)
                {
                    des.Padding = this._paddingMode.Value;
                }
                //inputByteArray = Convert.FromBase64String(p_strInput);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                //return _encoding.GetString(ms.ToArray());
                return ms.ToArray();
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }
    }
}
