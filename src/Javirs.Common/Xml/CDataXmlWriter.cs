using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Javirs.Common.Xml
{
    /// <summary>
    /// 
    /// </summary>
    public class CDataXmlWriter : FullTagXmlWriter
    {
        /// <summary>
        /// XML完整标签Writer
        /// </summary>
        /// <param name="stream">需要写入的流</param>
        public CDataXmlWriter(Stream stream, Encoding encoding)
            : base(stream, encoding)
        { }
        /// <summary>
        /// XML完整标签Writer
        /// </summary>
        /// <param name="stream">需要写入的流</param>
        /// <param name="settings">xml序列化配置</param>
        public CDataXmlWriter(Stream stream, XmlWriterSettings settings)
            : base(stream, settings)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public override void WriteString(string text)
        {
            //base.WriteString(text);
            base.WriteCData(text);
        }
    }
}
