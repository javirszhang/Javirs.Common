using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Exceptions
{
    /// <summary>
    /// 找不到数据异常
    /// </summary>
    public class DataNotFoundException : CustomizeException
    {
        /// <summary>
        /// 找不到数据
        /// </summary>
        /// <param name="message"></param>
        public DataNotFoundException(string message) : base(message)
        {

        }

    }
}
