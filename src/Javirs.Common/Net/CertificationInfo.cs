using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    /// <summary>
    /// SSL证书信息
    /// </summary>
    public class CertificationInfo
    {
        /// <summary>
        /// 证书存放地址
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 证书访问密码
        /// </summary>
        public string Password { get; set; }
    }
}
