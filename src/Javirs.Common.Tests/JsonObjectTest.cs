using Javirs.Common.Json;
using Javirs.Common.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common.Tests
{
    [TestClass]
    public class JsonObjectTest
    {
        [TestMethod]
        public void JsonAppendTest()
        {
            PageReturnInfo pageInfo = new PageReturnInfo();
            pageInfo.Count = 100;
            pageInfo.Data = new List<PageReturnInfo.Entity>
            {
                new PageReturnInfo.Entity{ Amount="1010",Subject="test1" },
                new PageReturnInfo.Entity{ Amount="1011",Subject="test2"}
            };
            FuncResult funcResult = new FuncResult
            {
                Success = true,
                StatusCode = 0,
                Message = "ok",
                Content = pageInfo
            };
            string json = JsonSerializer.JsonSerialize(funcResult);
            JsonObject jsonObject = JsonObject.Parse(json);
            var content = jsonObject.GetObject("Content");
            content.Put("resCode", "0000");
            content.Put("resMsg", "ok");
            string value = content.ToString();
            Debug.WriteLine(value);

        }
        [TestMethod]
        public void JsonParseTest()
        {
            var m = new
            {
                text = JsonSerializer.JsonSerialize(new { name = "Jason" })
            };
            string json = File.ReadAllText(@"g:\jsonfiles\json.txt");
            //string json = JsonSerializer.JsonSerialize(m);
            var obj = JsonObject.Parse(json);
            string text = obj.GetString("text");
            var data = obj.GetObject("data");
            var array = data.GetArray("array");
            string content = array[1].GetString("content");
            string code = obj.GetString("retCode");
            //string code = obj.GetArray()[0].GetString("code");
            Assert.AreEqual("who's right", code);
        }
        [TestMethod]
        public void ComplexJsonParseTest()
        {
            HttpHelper http = new HttpHelper("http://gpu-rest.52stark.cn/openapi/Channel/auth/template/duolab?merchantNo=2019110200001032");
            string json = http.Get();
            JsonObject jo = JsonObject.Parse(json);
            JsonObject data = jo.GetObject("data");
            string value = data.GetString("value");
            JsonObject v = JsonObject.Parse(value);
            JsonObject[] array = v.GetArray();
            foreach (var a in array)
            {
                string regular = a.GetString("regular");
                Assert.AreEqual("^\\d+$", regular);
            }
        }
    }

    public class PageReturnInfo
    {
        public int Count { get; set; }
        public List<Entity> Data { get; set; }

        public class Entity
        {
            public string Amount { get; set; }
            public string Subject { get; set; }
        }
    }

    public class FuncResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public Object Content { get; set; }
    }
}
