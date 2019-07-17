using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Exceptions
{
    /// <summary>
    /// 缺少配置项异常
    /// </summary>
    public class MissingConfigurationException : CustomizeException
    {
        /// <summary>
        /// 缺少配置项异常
        /// </summary>
        /// <param name="message"></param>
        public MissingConfigurationException(string message) : base(message)
        {
        }
        /// <summary>
        /// 缺少配置项异常
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        public MissingConfigurationException(string node, string key) : base(string.Concat(node, "节点缺少名为", key, "的配置"))
        {
        }
    }
}
