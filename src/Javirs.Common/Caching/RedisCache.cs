using Javirs.Common.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Caching
{
    /// <summary>
    /// redis缓存操作
    /// </summary>
    public class RedisCache : ICache
    {
        private static object _connectionLock = new object();
        private static ConnectionMultiplexer Connection { get; set; }
        /// <summary>
        /// redis缓存
        /// </summary>
        /// <param name="server">redis连接字符串，对于内网redis直接输入ip地址（10.14.0.148:3363），外网redis使用连接字符串</param>
        public RedisCache(string server)
        {
            InitConnectionMultiplexer(server);
        }
        private IDatabase _db;
        private IDatabase DB
        {
            get
            {
                if (_db == null)
                {
                    _db = Connection.GetDatabase();
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
        /// <returns></returns>
        public bool Add(string key, object value)
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
            return DB.StringSet(key, json);
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
        /// <returns></returns>
        public bool Refresh(string key, object value = null)
        {
            if (value == null)
            {
                return this.Delete(key);
            }
            return Add(key, value);
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
    }
}
