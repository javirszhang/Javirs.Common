using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Xml
{
    public class XmlSerializeSetting
    {
        public static XmlSerializeSetting Default
        {
            get
            {
                return new XmlSerializeSetting
                {
                    OutputDeclaretion = true,
                    OutputNamespace = true,
                    IsFullTag = false,
                    Encoding = Encoding.ASCII
                };
            }
        }
        public static XmlSerializeSetting Clean
        {
            get
            {
                return new XmlSerializeSetting
                {
                    OutputDeclaretion = false,
                    OutputNamespace = false,
                    IsFullTag = false,
                    Encoding = Encoding.ASCII
                };
            }
        }
        /// <summary>
        /// 是否输出XML格式版本声明
        /// </summary>
        public bool OutputDeclaretion { get; set; }
        /// <summary>
        /// 是否输出命名空间
        /// </summary>
        public bool OutputNamespace { get; set; }
        /// <summary>
        /// 是否启用完整标签。 闭合标签&lt;text /&gt;，完整标签&lt;text&gt;&lt;/text&gt;
        /// </summary>
        public bool IsFullTag { get; set; }
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 是否在xml声明头加入Standalone属性
        /// </summary>
        public bool? Standalone { get; set; }
        /// <summary>
        /// 是否输出&lt;![CDATA[ text ]]&gt;
        /// </summary>
        public bool IsOutputCData { get; set; }
    }
}
