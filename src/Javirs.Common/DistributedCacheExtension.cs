#if netstandard2_0
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common
{
    /// <summary>
    /// 分布式缓存扩展类
    /// </summary>
    public static class DistributedCacheExtension
    {
        /// <summary>
        /// 获取指定key的缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDistributedCache cache, string key)
        {
            var bytes = cache.Get(key);
            if (bytes == null)
                return default(T);
            var dataJson = Encoding.UTF8.GetString(bytes);
            if (string.IsNullOrEmpty(dataJson))
                return default(T);
            return JsonConvert.DeserializeObject<T>(dataJson);
        }
        /// <summary>
        /// 获取指定key的缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var bytes = await cache.GetAsync(key);
            if (bytes == null)
                return default(T);
            var dataJson = Encoding.UTF8.GetString(bytes);
            if (string.IsNullOrEmpty(dataJson))
                return default(T);
            return JsonConvert.DeserializeObject<T>(dataJson);
        }
        /*
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="ts"></param>
        public static void Set<T>(this IDistributedCache cache, string key, T t, TimeSpan? ts)
        {
            if (t != null)
            {
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));
                if (ts.HasValue)
                    cache.Set(key, bytes, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ts });
                else
                    cache.Set(key, bytes);
            }
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T t, TimeSpan? ts)
        {
            if (t != null)
            {
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));
                if (ts.HasValue)
                    await cache.SetAsync(key, bytes, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ts });
                else
                    await cache.SetAsync(key, bytes);
            }
        }
        */

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="options"></param>
        public static void Set<T>(this IDistributedCache cache, string key, T t, DistributedCacheEntryOptions options)
        {
            if (t != null)
            {
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));
                if (options != null)
                    cache.Set(key, bytes, options);
                else
                    cache.Set(key, bytes);
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T t, DistributedCacheEntryOptions options)
        {
            if (t != null)
            {
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));
                if (options != null)
                    await cache.SetAsync(key, bytes, options);
                else
                    await cache.SetAsync(key, bytes);
            }
        }
    }
}
#endif