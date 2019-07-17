using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Exceptions
{
    /// <summary>
    /// Json格式异常
    /// </summary>
    public class JsonFormatException : CustomizeException
    {
        /// <summary>
        /// Json格式异常
        /// </summary>
        /// <param name="message"></param>
        public JsonFormatException(string message) : base(message)
        {
        }
        /// <summary>
        /// Json格式异常
        /// </summary>
        public JsonFormatException() : base("Json格式有误，无法解析")
        {

        }
    }
}
