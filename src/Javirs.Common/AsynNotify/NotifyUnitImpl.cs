using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.AsynNotify
{
    /// <summary>
    /// 按键名的ASC排序规则的加密实现
    /// </summary>
    public abstract class NotifyUnitImpl : NotifyUnitBase
    {
        string _key;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public NotifyUnitImpl(string key)
        {
            this._key = key;
        }
        /// <summary>
        /// 排序字典
        /// </summary>
        private SortedDictionary<string, object> _parasDic = new SortedDictionary<string, object>();
        /// <summary>
        /// 排序字典
        /// </summary>
        protected SortedDictionary<string, object> ParasDic
        {
            get
            {
                if (_parasDic == null)
                {
                    _parasDic = new SortedDictionary<string, object>();
                }
                return _parasDic;
            }
        }
        /// <summary>
        /// 添加加密项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Put(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                //ignore when name is null
                return;
            }
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                //won't join the signature message if value is null
                return;
            }
            if (!this.ParasDic.Keys.Contains(name))
            {
                ParasDic.Add(name, value);
            }
            else
            {
                ParasDic[name] = value;
            }
        }
        /// <summary>
        /// 生成GET访问的URL参数
        /// </summary>
        /// <returns></returns>
        public override string CreateQueryString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string name in this.ParasDic.Keys)
            {
                builder.Append(string.Format("{0}={1}&", name, this.ParasDic[name]));
            }
            builder.AppendFormat("sign={0}", SignMsg());
            return builder.ToString();
        }
        /// <summary>
        /// 对消息进行签名
        /// </summary>
        /// <returns></returns>
        protected virtual string SignMsg()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string name in this.ParasDic.Keys)
            {
                builder.Append(string.Format("{0}={1}&", name, ParasDic[name]));
            }
            builder.Append(this._key);
            string msg = builder.ToString().ToLower();
            OnLog(msg);
            string sign = Security.MD5.Md5HashString(msg);
            OnLog(sign);
            return sign;
        }
        /// <summary>
        /// 订阅日志
        /// </summary>
        public event Action<string> Log;
        /// <summary>
        /// 执行事件通知
        /// </summary>
        /// <param name="s"></param>
        protected void OnLog(string s)
        {
            if (Log != null)
            {
                Log(s);
            }
        }
    }
}
