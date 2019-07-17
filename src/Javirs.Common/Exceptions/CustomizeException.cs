using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Exceptions
{
    /// <summary>
    /// 自定义异常，所有自定义的Exception类必须从此类派生，try块优先catch自定义异常，catch到自定义异常允许直接输出message到UI
    /// </summary>
    public class CustomizeException : Exception
    {
        protected string _message;
        /// <summary>
        /// 自定义异常
        /// </summary>
        /// <param name="message"></param>
        public CustomizeException(string message)
        {
            this._message = message;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }
}
