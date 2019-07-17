using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Security
{
    public class MD5
    {
        public static string Md5HashString(string str)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString();
        }

        public static string Encode(string str)
        {
            return Encode(str, Encoding.UTF8);
        }
        public static string Encode(string str, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(str);
            byte[] resBuf = Encode(buffer);
            return resBuf.Byte2HexString();
        }
        public static byte[] Encode(byte[] bytes)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return md5.ComputeHash(bytes);
        }
    }
}
