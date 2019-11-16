using Microsoft.VisualStudio.TestTools.UnitTesting;
using Javirs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Javirs.Common.Security;
using System.Diagnostics;

namespace Javirs.Common.Tests
{
    [TestClass()]
    public class AsciiSortedDictionaryTests
    {
        [TestMethod()]
        public void AsciiSortedDictionaryTest()
        {
            string xml = @"<xml><return_code><![CDATA[SUCCESS]]></return_code>
<return_msg><![CDATA[OK]]></return_msg>
<appid><![CDATA[wx97a3f8c05002bd41]]></appid>
<mch_id><![CDATA[1548699391]]></mch_id>
<sub_mch_id><![CDATA[1562440151]]></sub_mch_id>
<nonce_str><![CDATA[xHY3FS0YVk6Pd18I]]></nonce_str>
<sign><![CDATA[687037AA0A52C3E8F9F65ABD097A08C3]]></sign>
<result_code><![CDATA[SUCCESS]]></result_code>
<openid><![CDATA[oGz--wRO4q1l_VPD5v_3Ff6Of6Qw]]></openid>
<is_subscribe><![CDATA[N]]></is_subscribe>
<trade_type><![CDATA[MICROPAY]]></trade_type>
<bank_type><![CDATA[CMB_CREDIT]]></bank_type>
<total_fee>1</total_fee>
<fee_type><![CDATA[CNY]]></fee_type>
<transaction_id><![CDATA[4200000434201911114804508194]]></transaction_id>
<out_trade_no><![CDATA[201911110000001268]]></out_trade_no>
<attach><![CDATA[]]></attach>
<time_end><![CDATA[20191111192944]]></time_end>
<cash_fee>1</cash_fee>
<cash_fee_type><![CDATA[CNY]]></cash_fee_type>
<version><![CDATA[1.0]]></version>
<promotion_detail><![CDATA[{}]]></promotion_detail>
</xml>";
            bool result = CheckSign(xml, "1234567890qwerasdfzxcv0987654321");
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool CheckSign(string xml, string key)
        {
            XElement root = XElement.Parse(xml);
            var eles = root.Elements();
            string sign = null;
            AsciiSortedDictionary<string> keyValues = new AsciiSortedDictionary<string>();
            foreach (var item in eles)
            {
                if (!"sign".Equals(item.Name.LocalName, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(item.Value))
                {
                    keyValues.Add(item.Name.LocalName, item.Value);
                }
                else if ("sign".Equals(item.Name.LocalName, StringComparison.OrdinalIgnoreCase))
                {
                    sign = item.Value;
                }
            }
            StringBuilder builder = new StringBuilder();
            keyValues.Aggregate(builder, (b, kv) => b.Append(kv.Key).Append("=").Append(kv.Value).Append("&"));
            builder.Append("key=").Append(key);
            Debug.WriteLine(builder.ToString());
            string mSign = MD5.Encode(builder.ToString());
            return mSign.Equals(sign, StringComparison.OrdinalIgnoreCase);
        }
    }
}