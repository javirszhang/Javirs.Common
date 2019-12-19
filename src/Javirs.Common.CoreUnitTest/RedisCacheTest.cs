using Javirs.Common.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Javirs.Common.CoreUnitTest
{
    public class RedisCacheTest
    {
        [Test]
        public void PatternGetTest()
        {
            RedisCache redisCache = new RedisCache("localhost:6379");            
            var list = redisCache.GetList<User>("tnet_user_*");
            Assert.IsTrue(list != null && list.Count == 3);
        }
        [Test]
        public void PatternDeleteTest()
        {
            RedisCache redis = new RedisCache("localhost:6379");
            string[] keys = redis.GetKeys("*_1");
            for (int i = 0; i < keys.Length; i++)
            {
                redis.Delete(keys[i]);
            }
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
