using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Javirs.Common.Xml
{
    /// <summary>
    /// 完整标签xml写入对象
    /// </summary>
    public class FullTagXmlWriter : XmlTextWriter
    {
        private XmlWriterSettings _settings;
        /// <summary>
        /// XML完整标签Writer
        /// </summary>
        /// <param name="stream">需要写入的流</param>
        public FullTagXmlWriter(Stream stream, Encoding encoding)
            : base(stream, encoding)
        { }
        /// <summary>
        /// XML完整标签Writer
        /// </summary>
        /// <param name="stream">需要写入的流</param>
        /// <param name="settings">xml序列化配置</param>
        public FullTagXmlWriter(Stream stream, XmlWriterSettings settings)
            : base(stream, settings.Encoding)
        {
            this._settings = settings;
        }
        /// <summary>
        /// 序列化设置
        /// </summary>
        public override XmlWriterSettings Settings
        {
            get
            {
                return _settings;
            }
        }
        /// <summary>
        /// 文档声明是否包含standone属性
        /// </summary>
        public bool? Standalone { get; set; }
        /// <summary>
        /// 写入文档声明
        /// </summary>
        public override void WriteStartDocument()
        {
            if (this.Settings == null || !this.Settings.OmitXmlDeclaration)
            {
                if (Standalone.HasValue)
                {
                    base.WriteStartDocument(this.Standalone.Value);
                }
                else
                {
                    base.WriteStartDocument();
                }
            }
        }
        /// <summary>
        /// 写入文档声明
        /// </summary>
        /// <param name="standalone"></param>
        public override void WriteStartDocument(bool standalone)
        {
            if (this.Settings == null || !this.Settings.OmitXmlDeclaration)
                base.WriteStartDocument(standalone);
        }
        /// <summary>
        /// 写入结束标签
        /// </summary>
        public override void WriteEndElement()
        {
            base.WriteFullEndElement();
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="text"></param>
        public override void WriteString(string text)
        {
            base.WriteString(text);
        }
        /// <summary>
        /// 写入原字符串
        /// </summary>
        /// <param name="data"></param>
        public override void WriteRaw(string data)
        {
            base.WriteRaw(data);
        }
    }
}
