using Javirs.Common.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
