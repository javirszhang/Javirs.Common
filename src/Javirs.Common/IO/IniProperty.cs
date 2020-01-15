using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common.IO
{
    /// <summary>
    /// ini配置属性
    /// </summary>
    [DebuggerDisplay("{Name}={Value}")]
    public class IniProperty
    {
        private readonly string str;
        /// <summary>
        /// 
        /// </summary>
        public IniProperty() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public IniProperty(string s)
        {
            this.str = s;
            Resolve(s);
        }
        private void Resolve(string s)
        {
            string[] kv = s.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (kv.Length == 2)
            {
                this.Name = kv[0].Trim();
                this.Value = kv[1].Trim();
            }
            else
            {
                this.Value = kv[0];
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string[] Comments { get; set; }
        /// <summary>
        /// 转字符串
        /// </summary>
        /// <param name="containComment">是否包含注释</param>
        /// <returns></returns>
        public string ToString(bool containComment)
        {
            string s = str;
            if (string.IsNullOrEmpty(s))
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    s = this.Name + " = ";
                }
                s += this.Value;
            }
            StringBuilder builder = new StringBuilder();
            if (containComment && this.Comments != null && this.Comments.Length > 0)
            {
                foreach (var c in Comments)
                {
                    builder.AppendLine($"# {c}");
                }
            }
            builder.AppendLine(s);
            return builder.ToString();
        }
        /// <summary>
        /// 转字符串，默认不包含注释
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(false);
        }
    }
}
