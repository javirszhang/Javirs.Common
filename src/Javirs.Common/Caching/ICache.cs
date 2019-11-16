using System;
using System.Collections.Generic;

namespace Javirs.Common.Caching
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">缓存过期时间</param>
        /// <returns></returns>
        bool Add(string key, object value, TimeSpan? expiry = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetObject(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 根据通配符取出所有缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pattern"></param>
        /// <returns></returns>
        List<T> GetList<T>(string pattern);
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Delete(string key);
        /// <summary>
        /// 判定是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exist(string key);
        /// <summary>
        /// 获取缓存值或者新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        T GetOrAdd<T>(string key, Func<string, T> func);
        /// <summary>
        /// 获取缓存字符串值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);
        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool Refresh(string key, object value = null, TimeSpan? expiry = null);
    }
}