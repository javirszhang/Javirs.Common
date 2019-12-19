using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common.Caching
{
    /// <summary>
    /// 本地内存缓存
    /// </summary>
    public class MemoryCache : ICache
    {
#if net40
        private static readonly System.Runtime.Caching.MemoryCache Cache = System.Runtime.Caching.MemoryCache.Default;
#endif
#if netstandard2_0
        private static readonly Microsoft.Extensions.Caching.Memory.IMemoryCache Cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
#endif
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan? expiry = null)
        {
#if net40
            var cachePolicy = new System.Runtime.Caching.CacheItemPolicy();
            if (expiry.HasValue)
            {
                cachePolicy.AbsoluteExpiration = DateTimeOffset.Now.Add(expiry.Value);
            }
            var item = new System.Runtime.Caching.CacheItem(key, value);
            Cache.Set(item, cachePolicy);
#endif
#if netstandard2_0
            using (var entry = Cache.CreateEntry(key))
            {
                entry.Value = value;
                entry.AbsoluteExpirationRelativeToNow = expiry;
            }
#endif
            return true;
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            Cache.Remove(key);
            return true;
        }
        /// <summary>
        /// 判断缓存是否已存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exist(string key)
        {
#if netstandard2_0
            Cache.TryGetValue(key, out object value);
            return value != null;
#endif
#if net40
            return Cache.Get(key) != null;
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            try
            {
                object value = GetObject(key);
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetObject(string key)
        {
            object value;
#if net40
            value = Cache.Get(key);
#endif
#if netstandard2_0
            Cache.TryGetValue(key, out value);
#endif
            return value;
        }

        /// <summary>
        /// 新增或者获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public T GetOrAdd<T>(string key, Func<string, T> func)
        {
            object value;
#if netstandard2_0
            Cache.TryGetValue(key, out value);
#endif
#if net40
            value = Cache.Get(key);
#endif
            T val = (T)value;
            if (val == null)
            {
                val = func(key);
                Add(key, val);
            }
            return val;
        }
        /// <summary>
        /// 获取缓存字符串值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
#if net40
            return Cache.Get(key)?.ToString();
#endif
#if netstandard2_0
            object value;
            Cache.TryGetValue(key, out value);
            return value?.ToString();
#endif
        }
        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool Refresh(string key, object value = null, TimeSpan? expiry = null)
        {
            Cache.Remove(key);
            if (value != null)
            {
                return Add(key, value, expiry);
            }
            return true;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string pattern)
        {
            return Get<List<T>>(pattern);
        }
        /// <summary>
        /// 按通配符获取匹配的key
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string[] GetKeys(string pattern)
        {
            return new string[0];
        }
    }
}
