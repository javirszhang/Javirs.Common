using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Javirs.Common.Exceptions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace Javirs.Common.Security
{
    /// <summary>
    /// RSA密钥帮助类
    /// </summary>
    public class RsaKeyHelper
    {
        private string _private, _public;
        /// <summary>
        /// RSA密钥帮助类
        /// </summary>
        /// <param name="keys"></param>
        public RsaKeyHelper(AsymmetricCipherKeyPair keys)
        {
            this.Keys = keys;
        }
        /// <summary>
        /// 私钥
        /// </summary>
        public string Private
        {
            get
            {
                if (string.IsNullOrEmpty(_private))
                {
                    _private = GetKeyString(Keys.Private);
                }
                return _private;
            }
        }
        /// <summary>
        /// 公钥
        /// </summary>
        public string Public
        {
            get
            {
                if (string.IsNullOrEmpty(_public))
                {
                    _public = GetKeyString(Keys.Public);
                }
                return _public;
            }
        }
        /// <summary>
        /// 密钥对
        /// </summary>
        public AsymmetricCipherKeyPair Keys { get; set; }
        private KeyFormat _keyFormat = KeyFormat.pkcs1;
        /// <summary>
        /// 密钥格式，默认PKCS1
        /// </summary>
        public KeyFormat Format
        {
            get
            {
                return _keyFormat;
            }
            set
            {
                if (_keyFormat == value)
                {
                    return;
                }
                _keyFormat = value;
                ResetKeyString();
            }
        }
        private string GetKeyString(AsymmetricKeyParameter key)
        {
            object pemObject = key;
            if (key.IsPrivate && this.Format == KeyFormat.pkcs8)
            {
                pemObject = new Pkcs8Generator(key);
            }
            StringWriter stringWriter = new StringWriter();
            PemWriter pkcs8PemWriter = new PemWriter(stringWriter);
            pkcs8PemWriter.WriteObject(pemObject);
            pkcs8PemWriter.Writer.Flush();
            return stringWriter.ToString();
        }
        /// <summary>
        /// 转换为xml密钥
        /// </summary>
        /// <param name="includePrivate"></param>
        /// <returns></returns>
        public string ToXmlString(bool includePrivate)
        {
            if (this.Keys.Private != null)
            {
                return PrivateKeyToXml((RsaPrivateCrtKeyParameters)this.Keys.Private, includePrivate);
            }
            return PublicToXml((RsaKeyParameters)this.Keys.Public);
        }
        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns></returns>
        private byte[] EncryptByPrivate(byte[] cipher)
        {
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
            engine.Init(true, this.Keys.Private);
            int blockSize = (engine.GetInputBlockSize() / 8) - 11;

            List<byte> result = new List<byte>();
            int pos = 0;
            while (pos < cipher.Length)
            {
                int len = cipher.Length - pos < blockSize ? cipher.Length - pos : blockSize;
                byte[] tmp = new byte[len];
                Array.Copy(cipher, pos, tmp, 0, len);
                var buffer = engine.ProcessBlock(tmp, 0, cipher.Length);
                result.AddRange(buffer);
                pos += len;
            }
            return result.ToArray();
        }
        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private byte[] DecryptByPublic(byte[] buffer)
        {
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
            engine.Init(false, this.Keys.Private);
            return engine.ProcessBlock(buffer, 0, buffer.Length);
        }
        private void ResetKeyString()
        {
            _public = null;
            _private = null;
        }
        /// <summary>
        /// 转换密钥格式
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public RsaKeyHelper ConvertTo(KeyFormat format)
        {
            this.Format = format;
            return this;
        }
        /// <summary>
        /// 生成RSA密钥
        /// </summary>
        /// <param name="size">1024或2048</param>
        /// <returns></returns>
        public static RsaKeyHelper GenRsaKey(int size)
        {
            if (size == 0 || (size % 1024) != 0)
            {
                throw new Exceptions.ArgumentFormatException("size数值不正确，只能在1024和2048选择一个");
            }
            RsaKeyPairGenerator rsaGen = new RsaKeyPairGenerator();
            rsaGen.Init(new KeyGenerationParameters(new SecureRandom(), size));
            AsymmetricCipherKeyPair keys = rsaGen.GenerateKeyPair();
            RsaKeyHelper helper = new RsaKeyHelper(keys);
            return helper;
        }
        /// <summary>
        /// 从私钥字符串初始化
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static RsaKeyHelper FromPemPrivateKey(string privateKey, KeyFormat format)
        {
            if (format == KeyFormat.pkcs1)
            {
                privateKey = FormatKeyString(privateKey, format);
                PemReader reader = new PemReader(new StringReader(privateKey));
                var keys = reader.ReadObject() as AsymmetricCipherKeyPair;
                if (keys == null)
                {
                    return null;
                }
                return new RsaKeyHelper(keys);
            }
            else
            {
                //pkcs8
                var tmp = RemoveKeyStringFormat(privateKey, format);
                byte[] buffer = Convert.FromBase64String(tmp);
                RsaPrivateCrtKeyParameters prvt = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(buffer);

                RsaKeyParameters pub = new RsaKeyParameters(false, prvt.Modulus, prvt.Exponent);

                var helper = new RsaKeyHelper(new AsymmetricCipherKeyPair(pub, prvt));
                return helper;
            }
        }
        /// <summary>
        /// 从xml密钥初始化
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <returns></returns>
        public static RsaKeyHelper FromXmlKey(string xmlPrivateKey)
        {
            XElement root = XElement.Parse(xmlPrivateKey);
            //Modulus
            var modulus = root.Element("Modulus");
            //Exponent
            var exponent = root.Element("Exponent");
            Org.BouncyCastle.Math.BigInteger pInteger = null, qInteger = null, dpInteger = null, dqInteger = null, iQInteger = null, dInteger = null;
            //P
            var p = root.Element("P");
            if (p != null)
            {
                pInteger = new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(p.Value));
            }
            //Q
            var q = root.Element("Q");
            if (q != null)
            {
                qInteger = new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(q.Value));
            }
            //DP
            var dp = root.Element("DP");
            if (dp != null)
            {
                dpInteger = new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(dp.Value));
            }
            //DQ
            var dq = root.Element("DQ");
            if (dq != null)
            {
                dqInteger = new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(dq.Value));
            }
            //InverseQ
            var inverseQ = root.Element("InverseQ");
            if (inverseQ != null)
            {
                iQInteger = new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(inverseQ.Value));
            }
            //D
            var d = root.Element("D");
            if (d != null)
            {
                dInteger = new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(d.Value));
            }
            RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = new RsaPrivateCrtKeyParameters(
                new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(modulus.Value)),
                new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(exponent.Value)),
                dInteger,
                pInteger,
                qInteger,
                dpInteger,
                dqInteger,
                iQInteger);

            AsymmetricKeyParameter pubc = new RsaKeyParameters(false, rsaPrivateCrtKeyParameters.Modulus, rsaPrivateCrtKeyParameters.Exponent);
            var helper = new RsaKeyHelper(new AsymmetricCipherKeyPair(pubc, rsaPrivateCrtKeyParameters));
            return helper;
        }
        private static string FormatKeyString(string key, KeyFormat format)
        {
            string flag = format == KeyFormat.pkcs1 ? "-----{0} RSA PRIVATE KEY-----" : "-----{0} PRIVATE KEY-----";
            if (key.StartsWith(string.Format(flag, "BEGIN")))
            {
                return key;
            }
            int pos = 0;
            List<string> lines = new List<string>();
            lines.Add(string.Format(flag, "BEGIN"));
            key = key.Replace("\r", "").Replace("\n", "");
            while (pos < key.Length)
            {
                var count = key.Length - pos < 64 ? key.Length - pos : 64;
                lines.Add(key.Substring(pos, count));
                pos += count;
            }
            lines.Add(string.Format(flag, "END"));
            return string.Join("\r\n", lines);
        }
        /// <summary>
        /// Format public key
        /// </summary>
        /// <param name="publicString"></param>
        /// <returns></returns>
        public static string FormatPublicKey(string publicString)
        {
            if (publicString.StartsWith("-----BEGIN PUBLIC KEY-----"))
            {
                return publicString;
            }
            publicString = publicString.Replace("\r\n", "");
            List<string> res = new List<string>();
            res.Add("-----BEGIN PUBLIC KEY-----");
            int pos = 0;

            while (pos < publicString.Length)
            {
                var count = publicString.Length - pos < 64 ? publicString.Length - pos : 64;
                res.Add(publicString.Substring(pos, count));
                pos += count;
            }
            res.Add("-----END PUBLIC KEY-----");
            var resStr = string.Join("\r\n", res);
            return resStr;
        }
        /// <summary>
        /// Remove the Pkcs1 format private key format
        /// </summary>
        /// <param name="keyString"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string RemoveKeyStringFormat(string keyString, KeyFormat format)
        {
            string flag = format == KeyFormat.pkcs1 ? "-----{0} RSA PRIVATE KEY-----" : "-----{0} PRIVATE KEY-----";
            string begin = string.Format(flag, "BEGIN");
            string end = string.Format(flag, "END");
            if (!keyString.StartsWith(begin))
            {
                return keyString.Replace("\r\n", "");
            }
            return keyString.Replace(begin, "").Replace(end, "").Replace("\r\n", "");
        }
        /// <summary>
        /// Private Key Convert to xml
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="includePrivate"></param>
        /// <returns></returns>
        private static string PrivateKeyToXml(RsaPrivateCrtKeyParameters parameters, bool includePrivate)
        {
            XElement privatElement = new XElement("RSAKeyValue");
            //Modulus
            XElement primodulus = new XElement("Modulus", Convert.ToBase64String(parameters.Modulus.ToByteArrayUnsigned())); ;
            //Exponent
            XElement priexponent = new XElement("Exponent", Convert.ToBase64String(parameters.PublicExponent.ToByteArrayUnsigned()));
            //D
            XElement prid = new XElement("D", Convert.ToBase64String(parameters.Exponent.ToByteArrayUnsigned()));
            //P
            XElement prip = new XElement("P", Convert.ToBase64String(parameters.P.ToByteArrayUnsigned()));
            //Q
            XElement priq = new XElement("Q", Convert.ToBase64String(parameters.Q.ToByteArrayUnsigned()));
            //DP
            XElement pridp = new XElement("DP", Convert.ToBase64String(parameters.DP.ToByteArrayUnsigned()));
            //DQ
            XElement pridq = new XElement("DQ", Convert.ToBase64String(parameters.DQ.ToByteArrayUnsigned()));
            //InverseQ
            XElement priinverseQ = new XElement("InverseQ", Convert.ToBase64String(parameters.QInv.ToByteArrayUnsigned()));

            privatElement.Add(primodulus);
            privatElement.Add(priexponent);
            if (includePrivate)
            {
                privatElement.Add(prip);
                privatElement.Add(priq);
                privatElement.Add(pridp);
                privatElement.Add(pridq);
                privatElement.Add(priinverseQ);
                privatElement.Add(prid);
            }
            return privatElement.ToString(SaveOptions.DisableFormatting);
        }
        /// <summary>
        /// PEM公钥转为xml公钥
        /// </summary>
        /// <param name="pemPublicKeyString"></param>
        /// <returns></returns>
        public static string PublicToXml(string pemPublicKeyString)
        {
            string tmp = FormatPublicKey(pemPublicKeyString);
            PemReader pr = new PemReader(new StringReader(tmp));
            var obj = pr.ReadObject();
            if (!(obj is RsaKeyParameters rsaKey))
            {
                throw new Exception("Public key format is incorrect");
            }
            return PublicToXml(rsaKey);
        }
        private static string PublicToXml(RsaKeyParameters parameters)
        {
            XElement publicElement = new XElement("RSAKeyValue");
            //Modulus
            XElement pubmodulus = new XElement("Modulus", Convert.ToBase64String(parameters.Modulus.ToByteArrayUnsigned()));
            //Exponent
            XElement pubexponent = new XElement("Exponent", Convert.ToBase64String(parameters.Exponent.ToByteArrayUnsigned()));

            publicElement.Add(pubmodulus);
            publicElement.Add(pubexponent);
            return publicElement.ToString();
        }
        /// <summary>
        /// xml公钥转为pem格式
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string PublicXmlToPem(string xml)
        {
            XElement root = XElement.Parse(xml);
            //Modulus
            var modulus = root.Element("Modulus");
            //Exponent
            var exponent = root.Element("Exponent");

            RsaKeyParameters rsaKeyParameters = new RsaKeyParameters(false, new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(modulus.Value)), new Org.BouncyCastle.Math.BigInteger(1, Convert.FromBase64String(exponent.Value)));

            StringWriter sw = new StringWriter();
            PemWriter pWrt = new PemWriter(sw);
            pWrt.WriteObject(rsaKeyParameters);
            pWrt.Writer.Close();
            return sw.ToString();
        }
        /// <summary>
        /// 密钥格式
        /// </summary>
        public enum KeyFormat
        {
            /// <summary>
            /// pkcs1
            /// </summary>
            pkcs1,
            /// <summary>
            /// pkcs8
            /// </summary>
            pkcs8
        }
    }
}
