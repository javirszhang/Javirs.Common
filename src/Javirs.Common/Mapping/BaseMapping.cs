using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections;

namespace Javirs.Common.Mapping
{
    public abstract class BaseMapping : IMapping
    {
        protected abstract Dictionary<string, object> CreateValueDictionary();

        public virtual T Map<T>() where T : class
        {
            T t = (T)Activator.CreateInstance(typeof(T));
            Map(t);
            return t;
        }

        public virtual void Map<T>(T t) where T : class
        {
            if (t == null)
            {
                return;
            }
            Type genericType = typeof(T);
            var properties = genericType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var valueDic = CreateValueDictionary();
            foreach (PropertyInfo pinfo in properties)
            {
                object value = null;
                string name = pinfo.Name;
                bool ignore = false;
                var attrColl = pinfo.GetCustomAttributes(typeof(MappingAttribute), true);
                MappingAttribute mattr = null;
                if (attrColl != null && attrColl.Length > 0 && (mattr = attrColl[0] as MappingAttribute) != null)
                {
                    name = mattr.Name;
                    ignore = mattr.Ignore;
                }
                if (!ignore)
                {
                    try
                    {
                        value = GetValueFromDic<object>(valueDic, name);
                    }
                    catch (Exceptions.DataNotFoundException ex)
                    {

                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }
                    if (value != null)
                    {
                        try
                        {
                            pinfo.SetValue(t, ConvertValue(value, pinfo.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessages.Add(pinfo.Name, ex);
                        }
                    }
                }
            }
        }
        public Dictionary<string, Exception> ErrorMessages = new Dictionary<string, Exception>();
        public event Func<Type, object, object> ValueConversion;
        protected object ConvertValue(object value, Type propertyType)
        {
            if (ValueConversion != null)
            {

                var delegates = ValueConversion.GetInvocationList();
                foreach (Delegate del in delegates)
                {
                    ParameterInfo[] parainfos = del.Method.GetParameters();
                    if (parainfos[0].ParameterType == propertyType)
                    {
                        
                    }

                }
            }
            return DefaultConvertValue(value, propertyType);
        }
        protected virtual object DefaultConvertValue(object value, Type propertyType)
        {
            return value;
        }
        private TValue GetValueFromDic<TValue>(Dictionary<string, TValue> dic, string key)
        {
            object ignoreCaseValue = null, CaseSenstiveValue = dic[key];
            if (CaseSenstiveValue != null)
            {
                return (TValue)CaseSenstiveValue;
            }
            foreach (string k in dic.Keys)
            {
                if (k.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    ignoreCaseValue = dic[k];
                }
            }
            if (ignoreCaseValue != null)
            {
                return (TValue)ignoreCaseValue;
            }
            throw new Exceptions.DataNotFoundException(key);
        }
        protected class KeyEqualityComparer : IEqualityComparer<string>
        {

            public bool Equals(string x, string y)
            {
                if (string.IsNullOrEmpty(x))
                {
                    return false;
                }
                return x.Equals(y, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
