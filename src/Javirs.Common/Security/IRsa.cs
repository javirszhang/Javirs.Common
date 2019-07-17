using System;
namespace Javirs.Common.Security
{
    public interface IRsa
    {
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="value">base64密文</param>
        /// <returns></returns>
        string Decrypt(string value);
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="value">base64密文</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        string Decrypt(string value, System.Text.Encoding encoding);
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="value">base64密文</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="doOAEPPadding">OAE填充</param>
        /// <returns></returns>
        string Decrypt(string value, System.Text.Encoding encoding, bool doOAEPPadding);
        /// <summary>
        /// 解密RSA密文字节数组，返回结果为明文字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="doOAEPPadding"></param>
        /// <param name="pvtKey"></param>
        /// <returns></returns>
        byte[] Decrypt(byte[] bytes, bool doOAEPPadding);
        /// <summary>
        /// RSA加密，使用UTF8,不填充OAE加密
        /// </summary>
        /// <param name="value">明文</param>
        /// <returns></returns>
        string Encrypt(string value);
        /// <summary>
        /// RSA加密，不填充OAE加密
        /// </summary>
        /// <param name="value">明文</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        string Encrypt(string value, System.Text.Encoding encoding);
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="value">明文</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="doOAEPPadding">是否填充OAE</param>
        /// <returns></returns>
        string Encrypt(string value, System.Text.Encoding encoding, bool doOAEPPadding);
        /// <summary>
        /// RSA加密字节数组，返回字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="doOAEPPadding"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        byte[] Encrypt(byte[] bytes, bool doOAEPPadding);
        /// <summary>
        /// 错误事件
        /// </summary>
        event Action<Exception> ErrorOccurs;
        /// <summary>
        /// 是否包含私钥
        /// </summary>
        bool HasPrivateKey { get; }
        /// <summary>
        /// 私钥XML
        /// </summary>
        string PrivateKey { get; }
        /// <summary>
        /// 公钥XML
        /// </summary>
        string PublicKey { get; }
        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="buffer">需要签名的数据</param>
        /// <param name="althm">签名算法</param>
        /// <returns></returns>
        string SignData(byte[] buffer);
        string SignData(byte[] buffer, string hashAlthm);
        /// <summary>
        /// RSA签名验证
        /// </summary>
        /// <param name="buffer">需要签名的数据</param>
        /// <param name="althm">签名算法</param>
        /// <param name="signature">需要验证的签名</param>
        /// <returns></returns>
        bool VerifyData(byte[] buffer, byte[] signature);
        bool VerifyData(byte[] buffer, byte[] signature, string hashAlthm);
    }
}
