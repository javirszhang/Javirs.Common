using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    /// <summary>
    /// 密文处理接口
    /// </summary>
    public interface ICipherAttribute
    {
        /// <summary>
        /// 优先级，当属性被多个<see cref="ICipherAttribute"/>标记时的执行顺序
        /// </summary>
        int Priority { get; set; }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        string Encrypt(string plainText);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encStr"></param>
        /// <returns></returns>
        string Decrypt(string encStr);
    }
}
