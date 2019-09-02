using Javirs.Common.Json;
using Javirs.Common.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common.Tests
{
    [TestClass]
    public class HttpHelperTest
    {
        [TestMethod]
        public void PostStringTest()
        {
            string url = "https://localhost:44308/api/values";
            var data = new
            {
                merchantNo = "000000000",
                timestamp = "000000",
                token = "123123123123123123"
            };
            string json = JsonSerializer.JsonSerialize(data);
            json = "biz_content=%7b%22bank_account_type%22%3a%22personal%22%2c%22bank_name%22%3a%22%22%2c%22bank_type%22%3a%221031000%22%2c%22buyer_card_number%22%3a%226225887854054596%22%2c%22buyer_mobile%22%3a%2218675534882%22%2c%22buyer_name%22%3a%22%e5%bc%a0%e6%85%a7%e5%86%9b%22%2c%22currency%22%3a%22CNY%22%2c%22out_trade_no%22%3a%22UTST2019082300003945%22%2c%22pyerIDNo%22%3a%22xBMv7wPlUgDjhOHmBdCGHXlhKOA8TZkG%22%2c%22pyerIDTp%22%3a%22mSkT7BxMKlA%3d%22%2c%22seller_id%22%3a%22zj15986784985%22%2c%22seller_name%22%3a%22%e4%b8%ad%e6%99%b6%e5%87%ba%e8%a1%8c%e7%a7%91%e6%8a%80%ef%bc%88%e6%b7%b1%e5%9c%b3%ef%bc%89%e6%9c%89%e9%99%90%e5%85%ac%e5%8f%b8%22%2c%22shopdate%22%3a%2220190823%22%2c%22subject%22%3a%22%e5%bf%ab%e6%8d%b7%e6%94%af%e4%bb%98%e6%b5%8b%e8%af%95%22%2c%22support_card_type%22%3a%22debit%22%2c%22timeout_express%22%3a%2219m%22%2c%22total_amount%22%3a%221%22%7d&charset=utf-8&method=ysepay.online.fastpay&notify_url=http%3a%2f%2f120.24.63.144%3a18080%2fnotify%2fall%2fquickpay%2fauthcode%2fysepay_ali_h5&partner_id=zj15986784985&sign_type=RSA&sign=Elc5k7G9O3l5v701N9C44zBhkeJG9klwEMPpgoGzrJdVuhyFovctZNowejTtT6g8O9XlCZSedF%2fUJ0YCegcmwDwff2CbRRLz5paiBh%2bDOi57ZuoBh8DoGrqPNVKzSiuMb7Q%2fh0%2bt%2b6wgxZUsF5z7OU%2fFtZ3%2f%2fzd%2fS390EIoMnIXBfYAMAIllFcdCZAJ3b%2fV%2f5Ty%2bHGKlnKxpoc3VcGbm6YWoBD%2bc18ZQ2p3QJuyKvCcLLzz7iTEufgOzA1Nu%2bI03ta%2bZb%2bHCJLalCTMDLC0IGsnZU8s8QdfmtG9k70POOpNyNzl0Y0LBZKiL7yLEENVH518S4c603CVb9XSsQgwkSg%3d%3d&timestamp=2019-08-23+11%3a24%3a52&tran_type=1&version=3.0";
            HttpHelper http = new HttpHelper(url);
            var response = http.Post(json, 20, false, "application/json");
            //var response = http.Get();
            Assert.IsTrue(http.StatusCode == 200);
        }
        [TestMethod]
        public void PostBufferTest()
        {
            string url = "https://localhost:44308/api/values";
            var data = new
            {
                merchantNo = "000000000",
                timestamp = "000000",
                token = "123123123123123123"
            };
            string json = JsonSerializer.JsonSerialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            HttpHelper http = new HttpHelper(url);
            string response = http.SendRequest("post", buffer, 10, false);
            Assert.IsTrue(http.StatusCode == 200);
        }
    }
}
