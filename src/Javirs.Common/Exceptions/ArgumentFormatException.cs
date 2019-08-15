using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Exceptions
{
    /// <summary>
    /// 参数格式异常
    /// </summary>
    public class ArgumentFormatException : CustomizeException
    {
        /// <summary>
        /// 参数格式异常
        /// </summary>
        /// <param name="message">错误信息</param>
        public ArgumentFormatException(string message) : base(message)
        {
        }
        /// <summary>
        /// 参数格式异常，默认错误信息：[参数格式不正确]
        /// </summary>
        public ArgumentFormatException() : base("参数格式不正确")
        {

        }
    }
}
