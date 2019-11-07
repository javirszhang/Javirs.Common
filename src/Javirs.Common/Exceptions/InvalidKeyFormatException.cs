using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common.Exceptions
{
    /// <summary>
    /// 密钥格式不正确
    /// </summary>
    public class InvalidKeyFormatException : CustomizeException
    {
        /// <summary>
        /// 密钥格式不正确
        /// </summary>
        /// <param name="message"></param>
        public InvalidKeyFormatException(string message) : base(message)
        {
        }
        /// <summary>
        /// 密钥格式不正确
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerEx"></param>
        public InvalidKeyFormatException(string message, Exception innerEx) : base(message, innerEx)
        {

        }


    }
}
