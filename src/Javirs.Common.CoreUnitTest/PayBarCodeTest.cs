using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Javirs.Common.CoreUnitTest
{
    public class PayBarCodeTest
    {
        [Test]
        public void EncodeTest()
        {

            PaymentBarCode barCode = new PaymentBarCode("68");
            string cipher = barCode.Encode(100201, "RBvA8epWQudBJjbeSXtNcU");
            Debug.WriteLine("barCode：" + cipher);
            bool success = barCode.Decode(cipher, "RBvA8epWQudBJjbeSXtNcU", out long uid);
            Assert.IsTrue(success && uid == 100201);
        }
    }
    public class PaymentBarCode
    {
        private const int duration = 30;
        private const int n = 7654321;
        private readonly string prefix;
        public PaymentBarCode(string prefix)
        {
            this.prefix = prefix;
        }

        public string Encode(long uid, string token)
        {
            DynamicPassword otp = new DynamicPassword(token, duration);
            string pwd = otp.GetPassword(n.ToString().Length - 1);
            long l = Convert.ToInt64(pwd);

            long res = uid * n + l;
            return prefix + res.ToString().PadLeft(16, '0');
        }

        public bool Decode(string barCode, string token, out long uid)
        {
            long cat = Convert.ToInt64(barCode.Substring(prefix.Length));
            uid = cat / n;
            long pwd = cat % n;
            DynamicPassword otp = new DynamicPassword(token, duration);
            return otp.Verify(pwd.ToString());
        }

    }
    public class DynamicPassword
    {
        private readonly string key;
        private readonly int duration;
        public DynamicPassword(string key, int duration)
        {
            this.key = key;
            this.duration = duration;
        }

        public string GetPassword(int digits = 6)
        {
            TimeStamp ts = new TimeStamp();
            long seconds = (long)ts.Seconds;
            long count = seconds / duration;
            byte[] buffer = BitConverter.GetBytes(count);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }
            HMACSHA1 hmac = new HMACSHA1(Encoding.ASCII.GetBytes(key), true);
            byte[] hash = hmac.ComputeHash(buffer);
            int offset = hash[hash.Length - 1] & 0xf;
            int binary =
            ((hash[offset] & 0x7f) << 24)
             | ((hash[offset + 1] & 0xff) << 16)
             | ((hash[offset + 2] & 0xff) << 8)
             | (hash[offset + 3] & 0xff);
            int password = binary % (int)Math.Pow(10, digits); // 6 digits
            return password.ToString(new string('0', digits));
        }

        public bool Verify(string pwd)
        {
            string g = GetPassword(pwd.Length);
            return g == pwd;
        }
    }
}
