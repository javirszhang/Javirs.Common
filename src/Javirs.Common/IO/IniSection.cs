using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common.IO
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Name={Name},items={Properties.Count}")]
    public class IniSection
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string[] Comments { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public List<IniProperty> Properties { get; set; } = new List<IniProperty>();
        /// <summary>
        /// 转字符串
        /// </summary>
        /// <param name="containComment">是否包含注释</param>
        /// <returns></returns>
        public string ToString(bool containComment)
        {
            StringBuilder builder = new StringBuilder();
            if (containComment && this.Comments != null && this.Comments.Length > 0)
            {
                foreach (var cmt in Comments)
                {
                    builder.AppendLine($"# {cmt}");
                }
            }
            builder.AppendLine($"[{this.Name}]");
            if (this.Properties != null)
            {
                foreach (var p in Properties)
                {
                    builder.Append(p.ToString(containComment));
                }
            }
            return builder.ToString();
        }
        /// <summary>
        /// 转字符串，不包含注释
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(false);
        }
    }
}
