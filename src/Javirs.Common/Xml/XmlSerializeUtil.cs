using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Javirs.Common.Xml
{
    public class XmlSerializeUtil
    {
        public static string XmlSerialize(object arg, XmlSerializeSetting setting)
        {
            if (setting == null)
            {
                setting = XmlSerializeSetting.Default;
            }
            XmlSerializer xmlserializer = new XmlSerializer(arg.GetType());
            MemoryStream stream = new MemoryStream();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            if (!setting.OutputNamespace)
                ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings();
            if (!setting.OutputDeclaretion)
                settings.OmitXmlDeclaration = true;
            settings.Encoding = setting.Encoding;
            XmlWriter writer = XmlTextWriter.Create(stream, settings);
            if (setting.IsFullTag)
            {
                writer = new FullTagXmlWriter(stream, settings);
                ((FullTagXmlWriter)writer).Standalone = setting.Standalone;
            }
            if (setting.IsOutputCData)
            {
                writer = new CDataXmlWriter(stream, settings);
                ((FullTagXmlWriter)writer).Standalone = setting.Standalone;
            }
            xmlserializer.Serialize(writer, arg, ns);
            return settings.Encoding.GetString(stream.ToArray());
        }

        public static T XmlDeserialize<T>(string xmlStr, Encoding encoding)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            byte[] buffer = encoding.GetBytes(xmlStr);
            MemoryStream ms = new MemoryStream(buffer);
            return (T)serializer.Deserialize(ms);
        }
    }
}
