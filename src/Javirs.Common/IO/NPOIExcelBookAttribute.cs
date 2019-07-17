using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.IO
{
    public class NPOIExcelBookAttribute : Attribute
    {
        /// <summary>
        /// 表头
        /// </summary>
        public string TitleName { get; set; }
        /// <summary>
        /// 导出时是否忽略
        /// </summary>
        public bool Ignore { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Range { get; set; }
    }
}
