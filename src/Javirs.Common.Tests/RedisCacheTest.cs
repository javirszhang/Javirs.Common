using Javirs.Common.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common.Tests
{
    [TestClass]
    public class RedisCacheTest
    {
        [TestMethod]
        public void PatternGetTest()
        {
            RedisCache redisCache = new RedisCache("localhost:6379");
            redisCache.Add("tnet_user_1", new User { UserId = 1, UserCode = "12200100001", UserName = "Jason", Refer_Id = null, AuthTime = null, NodeLevel = 0 });
            redisCache.Add("tnet_user_2", new User { UserId = 2, UserCode = "dy0001", UserName = "黄月英", Refer_Id = 1, AuthTime = null, NodeLevel = 1 });
            redisCache.Add("tnet_user_3", new User { UserId = 3, UserCode = "luoxuan", UserName = "落喧", Refer_Id = 1, AuthTime = null, NodeLevel = 1 });

            var list = redisCache.GetList<User>("tnet_user_*");
            Assert.IsTrue(list != null && list.Count == 3);
        }

        public class User
        {
            public int UserId { get; set; }
            public string UserCode { get; set; }
            public string UserName { get; set; }
            public int? Refer_Id { get; set; }
            public DateTime? AuthTime { get; set; }
            public int NodeLevel { get; set; }
        }
    }
}
