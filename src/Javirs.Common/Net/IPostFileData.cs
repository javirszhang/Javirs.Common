using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public interface IPostFileData:IPostData
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        string FileName { get; set; }
        /// <summary>
        /// 类型（默认image/gif）
        /// </summary>
        string ContentType { get; set; }
    }
}
