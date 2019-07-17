using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    /// <summary>
    /// 程序配置管理
    /// </summary>
    public static class ProviderContainer
    {
        private static List<Type> Providers = new List<Type>();
        private static object lockobj = new object();
        /// <summary>
        /// 注册应用程序，建议注册的类型T包含无参构造，否则无法调用GetProvider获取实例，若不包含无参构造请使用Find找到类型，然后自行初始化实例
        /// </summary>
        /// <param name="force">是否强制注册，若程序已被注册，重复注册默认忽略。若需强制重复注册，请指定force参数为true</param>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>(bool force = false)
        {
            lock (lockobj)
            {
                Type t = typeof(T);
                if (Find<T>() != t || force)
                {
                    Providers.Add(t);
                }
            }
        }
        /// <summary>
        /// 查找T程序是否有注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type Find<T>()
        {
            var list = FindAll<T>();
            return list.FirstOrDefault();
            //Type t = typeof(T);
            //Type provider = null;
            //foreach (Type item in Providers)
            //{
            //    if (t.IsInterface)
            //    {
            //        Type @interface = item.GetInterface(t.FullName);
            //        if (@interface != null)
            //        {
            //            provider = item;
            //            break;
            //        }
            //    }
            //    else
            //    {
            //        if (IsEqualsOrInherits<T>(item))
            //        {
            //            provider = item;
            //            break;
            //        }
            //    }
            //}
            //return provider;
        }
        /// <summary>
        /// 获取所有注册的T程序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Type> FindAll<T>()
        {
            Type t = typeof(T);
            List<Type> providers = new List<Type>();
            foreach (Type item in Providers)
            {
                if (t.IsInterface)
                {
                    Type @interface = item.GetInterface(t.FullName);
                    if (@interface != null)
                    {
                        providers.Add(item);
                    }
                }
                else
                {
                    if (IsEqualsOrInherits<T>(item))
                    {
                        providers.Add(item);
                    }
                }
            }
            return providers;
        }
        /// <summary>
        /// 获得注册的应用程序实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetProvider<T>()
        {
            var provider = Find<T>();
            if (provider == null)
            {
                return default(T);
            }
            var obj = Activator.CreateInstance(provider);
            return (T)obj;
        }
        /// <summary>
        /// 获取全部提供程序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetProviders<T>()
        {
            var providers = FindAll<T>();
            List<T> list = new List<T>();
            foreach (Type t in providers)
            {
                var obj = Activator.CreateInstance(t);
                list.Add((T)obj);
            }
            return list;
        }
        /// <summary>
        /// 是否相等或者继承自T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsEqualsOrInherits<T>(Type t)
        {
            Type sourceType = typeof(T);
            if (sourceType == t)
            {
                return true;
            }
            else if (t == typeof(object))
            {
                return false;
            }
            return IsEqualsOrInherits<T>(t.BaseType);


        }
    }
}
