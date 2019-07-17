/***************************************************
* Name:RSA CRYPTO OBJECT                           *
* CreateTime : 2014-02-20 16:51:42                 *
* Author:Jason                                     *
* E_Mail : zhj.pavel@gmail.com                     *
***************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
#if !V35
using System.Numerics;
#endif

namespace Javirs.Common.Security
{
    /// <summary>
    /// RSA证书对象
    /// </summary>
    public class RsaCertificate : RSAServiceProvider, IRsa
    {
        /// <summary>
        /// x509的RSA证书对象
        /// </summary>
        protected X509Certificate2 _certificate2;
        /// <summary>
        /// RSA证书对象构造
        /// </summary>
        protected RsaCertificate() { }
        /// <summary>
        /// 从本机当前用户证书存储区域读取证书
        /// </summary>
        /// <param name="certName"></param>
        public RsaCertificate(string certName)
        {
            this._certificate2 = GetCertificateFromStore(certName);
            SetRSACryptoServiceProvider(this._certificate2);
        }

        private void SetRSACryptoServiceProvider(X509Certificate2 cert)
        {
            this._provider = new RSACryptoServiceProvider();
            if (cert.HasPrivateKey)
            {
                this._provider.FromXmlString(cert.PrivateKey.ToXmlString(true));
            }
            else
            {
                this._provider.FromXmlString(cert.PublicKey.Key.ToXmlString(false));
            }
        }

        /// <summary>
        /// 从机器证书存储器创建密钥对
        /// </summary>
        /// <param name="pvtKey"></param>
        /// <param name="pbkKey"></param>
        /// <returns></returns>
        public static bool BuildRsaKey(out string pvtKey, out string pbkKey)
        {
            try
            {
                CspParameters para = new CspParameters();
                para.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024, para);
                pvtKey = provider.ToXmlString(true);
                pbkKey = provider.ToXmlString(false);
                return true;
            }
            catch
            {
                pvtKey = pbkKey = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// 从pfx证书文件中读取
        /// </summary>
        /// <param name="path"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static RsaCertificate ReadFromPfx(string path, string password)
        {
            var rsa = new RsaCertificate();
            rsa._provider = new RSACryptoServiceProvider();
            rsa._certificate2 = new X509Certificate2(path, password, X509KeyStorageFlags.Exportable);
            if (rsa._certificate2.HasPrivateKey)
            {
                rsa._provider.FromXmlString(rsa._certificate2.PrivateKey.ToXmlString(true));
            }
            else
            {
                rsa._provider.FromXmlString(rsa._certificate2.PublicKey.Key.ToXmlString(false));
            }
            return rsa;
        }

        /// <summary>
        /// 从Cert证书文件中读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static RsaCertificate ReadFromCert(string path)
        {
            //var x509cert = new X509Certificate2(path);
            var rsa = new RsaCertificate();
            rsa._certificate2 = new X509Certificate2(path);
            rsa._provider = new RSACryptoServiceProvider();
            rsa._provider.FromXmlString(rsa._certificate2.PublicKey.Key.ToXmlString(false));
            return rsa;
        }

        /// <summary>
        /// 创建证书到本机当前用户的个人证书存储区域
        /// </summary>
        /// <returns></returns>
        public static bool CreateCertificate(string makecertpath, string x509name)
        {
            try
            {
                IDictionary<string, string> args = GetMakecertArgs(x509name, 120);
                args.Add("r", string.Format("\"{0}\"", x509name + ".pfx"));
                args.Add("$", "commercial");
                CallMakeCert(makecertpath, args);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 从本机当前用户证书存储区域导出证书
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="pfxFileName"></param>
        /// <param name="password"></param>
        /// <param name="isDeleteFromStore"></param>
        /// <returns></returns>
        public static bool ExportPfxFile(string subject, string pfxFileName, string password, bool isDeleteFromStore)
        {
            return ExportFile(subject, pfxFileName, password, isDeleteFromStore, X509ContentType.Pfx);
        }

        /// <summary>
        /// 从本机当前用户证书存储区域导出cert证书（即公钥证书）
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="cerFileName"></param>
        /// <returns></returns>
        public static bool ExportCertFile(string subjectName, string cerFileName)
        {
            return ExportFile(subjectName, cerFileName, string.Empty, false, X509ContentType.Cert);
        }

        #region private helper
        /// <summary>
        /// 从本机当前用户证书存储区域导出证书
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="fileName"></param>
        /// <param name="password"></param>
        /// <param name="isDeleteFromStore"></param>
        /// <param name="x509type"></param>
        /// <returns></returns>
        private static bool ExportFile(string subjectName, string fileName, string password, bool isDeleteFromStore, X509ContentType x509type)
        {
            subjectName = "CN=" + subjectName;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            X509Certificate2Collection storecollection = (X509Certificate2Collection)store.Certificates;
            foreach (X509Certificate2 x509 in storecollection)
            {
                if (x509.Subject == subjectName)
                {
                    Debug.Print(string.Format("certificate name: {0}", x509.Subject));
                    byte[] certificateBytes = null;
                    if (x509type == X509ContentType.Pfx)
                        certificateBytes = x509.Export(x509type, password);
                    else
                        certificateBytes = x509.Export(x509type);
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        for (int i = 0; i < certificateBytes.Length; i++)
                            fileStream.WriteByte(certificateBytes[i]);
                        fileStream.Seek(0, SeekOrigin.Begin);
                        for (int i = 0; i < fileStream.Length; i++)
                        {
                            if (certificateBytes[i] != fileStream.ReadByte())
                            {
                                fileStream.Close();
                                return false;
                            }
                        }
                        fileStream.Close();
                    }
                    if (isDeleteFromStore == true)
                        store.Remove(x509);
                }
            }
            store.Close();
            return true;
        }
        /// <summary>
        /// 调用系统MakeCert.exe程序
        /// </summary>
        /// <param name="makecertPath"></param>
        /// <param name="args"></param>
        private static void CallMakeCert(string makecertPath, IDictionary<string, string> args)
        {
            try
            {
                string arg = JoinMakecertParams(args);
                Process p = new Process();
                p.StartInfo.FileName = makecertPath;
                p.StartInfo.Arguments = arg;
                //Process p = Process.Start(makecertPath, JoinMakecertParams(args));
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从本机当前用户的个人证书存储区域读取证书
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        private static X509Certificate2 GetCertificateFromStore(string subjectName)
        {
            string x509name = "CN=" + subjectName;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            X509Certificate2Collection certcoll = store.Certificates;
            foreach (var item in certcoll)
            {
                if (item.Subject == x509name)
                    return item;
            }
            store.Close();
            return null;
        }
        /// <summary>
        /// 构造MakeCert程序参数
        /// </summary>
        /// <param name="x509name"></param>
        /// <param name="expireMonth"></param>
        /// <returns></returns>
        private static IDictionary<string, string> GetMakecertArgs(string x509name, int expireMonth)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("pe", string.Empty);
            args.Add("ss", "my");
            args.Add("n", string.Format("\"CN={0}\"", x509name));
            args.Add("m", expireMonth.ToString());
            return args;
        }
        /// <summary>
        /// 连结参数成字符串
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private static string JoinMakecertParams(IDictionary<string, string> args)
        {
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;
            foreach (var arg in args)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(" ");
                sb.AppendFormat("-{0} {1}", arg.Key, arg.Value);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        private static void WriteToFile(byte[] buffer, string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                // Write the data to the file, byte by byte.  
                for (int i = 0; i < buffer.Length; i++)
                    fileStream.WriteByte(buffer[i]);
                // Set the stream position to the beginning of the file.   
                fileStream.Seek(0, SeekOrigin.Begin);
                // Read and verify the data.   
                for (int i = 0; i < fileStream.Length; i++)
                {
                    if (buffer[i] != fileStream.ReadByte())
                    {
                        fileStream.Close();
                        throw new Exception("Export pfx error while verify the pfx file");
                    }
                }
                fileStream.Close();
            }
        }
        #endregion
#if !V35
        /// <summary>
        /// 证书ID
        /// </summary>
        public string GetCertificateID()
        {
            if (this._certificate2 == null)
            {
                return null;
            }
            return BigInteger.Parse(this._certificate2.SerialNumber, System.Globalization.NumberStyles.HexNumber).ToString();
        }
#else
        /// <summary>
        /// 证书ID
        /// </summary>
        public string GetCertificateID()
        {
            if (this._certificate2 == null)
            {
                return null;
            }
            return Javirs.Common.Numeric.BigNum.ToDecimalStr(Javirs.Common.Numeric.BigNum.ConvertFromHex(this._certificate2.SerialNumber)); 
        }
#endif
    }
}
