using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.AsynNotify
{
    /// <summary>
    /// 异步通知基类
    /// </summary>
    public abstract class NotifyUnitBase
    {
        /// <summary>
        /// 产生GET访问参数
        /// </summary>
        /// <returns></returns>
        public abstract string CreateQueryString();
        /// <summary>
        /// 异步通知结果
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isContinue"></param>
        /// <param name="notifyResult"></param>
        /// <returns></returns>
        public abstract bool AsynNotifyResult(bool isSuccess,bool isContinue, string notifyResult);
        /// <summary>
        /// 是否可以发出通知
        /// </summary>
        /// <returns></returns>
        public abstract bool CanNotify();
        /// <summary>
        /// 异步通知地址
        /// </summary>
        public abstract string NotifyUrl { get; }
        /// <summary>
        /// 异步通知方式
        /// </summary>
        public virtual HttpMethod Method
        {
            get { return HttpMethod.GET; }
        }
    }
}
