using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Javirs.Common.Security
{
    public abstract class SignProvider
    {
        private object _object;
        private bool _includeNull;
        private ArrayList _originalArray;
        public SignProvider(object obj, bool includeNull)
        {
            this._object = obj;
            this._includeNull = includeNull;
            this._originalArray = new ArrayList();
        }
        public string SignData()
        {
            StringBuilder builder = new StringBuilder();
            Type t = this._object.GetType();
            var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var p in properties)
            {
                object value = p.GetValue(this._object, null);
                if ((value == null || string.IsNullOrEmpty(value.ToString())) && !this._includeNull)
                {
                    continue;
                }
                AddSignItem(p.Name, value);
            }

            ArrayList array = Sort();
            string message = Splice(array);
            return MakeSign(message);
        }

        protected virtual void AddSignItem(string name, object value)
        {
            this._originalArray.Add(string.Concat(name, "=", value));
        }

        protected virtual ArrayList Sort()
        {
            return this._originalArray.BubbleSort();
        }

        protected virtual string Splice(ArrayList array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object o in array)
            {
                builder.Append(o).Append("&");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        protected abstract string MakeSign(string message);

    }
}
