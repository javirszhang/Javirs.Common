using Javirs.Common.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if netstandard2_0
using Microsoft.Extensions.Options;
#endif

namespace Javirs.Common.Caching
{
    /// <summary>
    /// redis缓存操作
    /// </summary>
    public class RedisCache : ICache
    {
        private static object _connectionLock = new object();
        private readonly int redisDB;
        private static ConnectionMultiplexer Connection { get; set; }
        /// <summary>
        /// redis缓存
        /// </summary>
        /// <param name="server">redis连接字符串，对于内网redis直接输入ip地址（10.14.0.148:3363），外网redis使用连接字符串</param>
        /// <param name="db">redis数据库索引</param>
        public RedisCache(string server, int db = -1)
        {
            this.redisDB = db;
            InitConnectionMultiplexer(server);
        }
#if netstandard2_0
        /// <summary>
        /// DI
        /// </summary>
        /// <param name="options"></param>
        public RedisCache(IOptions<RedisConfig> options)
        {
            var config = options.Value;
            this.redisDB = config.DBIndex;
            InitConnectionMultiplexer(config.Server);
        }
#endif
        private IDatabase _db;
        private IDatabase DB
        {
            get
            {
                if (_db == null)
                {
                    _db = Connection.GetDatabase(this.redisDB);
                }
                return _db;
            }
        }
        /// <summary>
        /// 获取或添加缓存，缓存key存在时直接返回缓存值，否则执行func委托，加入返回并存入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public T GetOrAdd<T>(string key, Func<string, T> func)
        {
            object value = GetString(key);
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                Type t = typeof(T);
                if (t == typeof(string) || t.IsPrimitive)
                {
                    return (T)Convert.ChangeType(value, t);
                }
                else
                {
                    return JsonSerializer.Deserializer<T>(value.ToString());
                }
            }
            value = func(key);
            Add(key, value);
            return (T)value;
        }
        /// <summary>
        /// 获得缓存字符串值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return DB.StringGet(key);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan? expiry = null)
        {
            if (value == null)
            {
                return false;
            }
            var valType = value.GetType();
            string json = null;
            if (valType == typeof(string) || valType.IsPrimitive)
            {
                json = value.ToString();
            }
            else
            {
                json = JsonSerializer.JsonSerialize(value);
            }
            return DB.StringSet(key, json, expiry);
        }
        /// <summary>
        /// 判定缓存key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exist(string key)
        {
            return DB.KeyExists(key);
        }
        /// <summary>
        /// 删除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            return DB.KeyDelete(key);
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
            if (value == null)
            {
                return this.Delete(key);
            }
            return Add(key, value, expiry);
        }

        private void InitConnectionMultiplexer(string server)
        {
            if (Connection != null && Connection.IsConnected)
            {
                return;
            }
            lock (_connectionLock)
            {
                if (Connection != null && Connection.IsConnected)
                {
                    return;
                }
                Connection = ConnectionMultiplexer.Connect(server);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetObject(string key)
        {
            return GetString(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            string json = GetString(key);
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return JsonSerializer.Deserializer<T>(json);
        }
    }
    /// <summary>
    /// redis配置
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// redis服务连接字符串
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// redis db索引
        /// </summary>
        public int DBIndex { get; set; }
    }
}
