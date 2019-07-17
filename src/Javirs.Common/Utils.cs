using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Javirs.Common
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// 将枚举转换成Json字符串
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetEnumJson(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                return "{}";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            var values = Enum.GetValues(enumType);

            foreach (object value in values)
            {
                var name = Enum.GetName(enumType, value);
                var fi = enumType.GetField(name);
                var attrs = fi.GetCustomAttributes(true);
                if (attrs != null && attrs.Length > 0)
                {
                    foreach (var attr in attrs)
                    {
                        if (attr is IgnoreAttribute)
                        {
                            continue;
                        }
                        else if (attr is DisplayTextAttribute)
                        {
                            var dta = attr as DisplayTextAttribute;
                            sb.Append("'").Append(dta.Text).Append("':").Append((int)value).Append(",");
                        }
                        else
                        {
                            sb.Append("'").Append(name).Append("':").Append((int)value).Append(",");
                        }
                    }
                }
                else
                {
                    sb.Append("'").Append(name).Append("':").Append((int)value).Append(",");
                }

            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary>
        /// 获取枚举的显示文本
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumValueDisplayText(Type enumType, object value)
        {
            var name = Enum.GetName(enumType, value);
            var fi = enumType.GetField(name);
            var attrs = fi.GetCustomAttributes(typeof(DisplayTextAttribute), true);
            if (attrs == null || attrs.Length == 0)
            {
                return name;
            }
            else
            {
                var attr = attrs[0] as DisplayTextAttribute;
                if (attr == null)
                {
                    return name;
                }
                return attr.Text;
            }
        }
        /// <summary>
        /// 展示枚举在HTML前端页面
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ShowEnumAsHtml(Type enumType, object value)
        {
            if (enumType == null)
            {
                return string.Format("<span data-note=\"{1}\">{0}</span>", value, "传入的枚举类型为空");
            }
            if (value == null)
            {
                return null;
            }
            if (!Enum.IsDefined(enumType, value))
            {
                return string.Format("<span data-note=\"枚举值未定义在{1}\">{0}</span>", value, enumType.Name);
            }
            string name;
            if (value is int)
            {
                name = Enum.GetName(enumType, value);
            }
            else
            {
                var temp = Enum.Parse(enumType, value.ToString(), true);
                name = Enum.GetName(enumType, Convert.ToInt32(temp));
            }
            var fi = enumType.GetField(name);
            var attrs = fi.GetCustomAttributes(typeof(DisplayTextAttribute), true);
            if (attrs == null || attrs.Length == 0)
            {
                return name;
            }
            else
            {
                var attr = attrs[0] as DisplayTextAttribute;
                if (attr == null)
                {
                    return name;
                }
                return BuildTag(attr.HtmlTag, attr.Text, attr.ClassName, attr.Color);
            }
        }
        private static string BuildTag(string tagName, string text, string className, int fontColor)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return text;
            }
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append("<").Append(tagName);
            if (!string.IsNullOrEmpty(className))
            {
                sbuilder.Append(" class=\"").Append(className).Append("\" ");
            }
            if (fontColor >= 0)
            {
                sbuilder.Append(" style=\"color:").Append("#").Append(Convert.ToString(fontColor, 16).ToString().ToUpper()).Append(";\"");
            }
            sbuilder.Append(">");
            sbuilder.Append(text);
            sbuilder.Append("</").Append(tagName).Append(">");
            return sbuilder.ToString();
        }
        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetLocalIP()
        {

            string hostName = Dns.GetHostName();//本机名   
            //System.Net.IPAddress[] addressList = Dns.GetHostByName(hostName).AddressList;//会警告GetHostByName()已过期，我运行时且只返回了一个IPv4的地址   
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            return addressList;
        }
        /// <summary>
        /// 获取本机IPV4地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPV4()
        {
            var addrs = GetLocalIP();
            string localIP = "";
            foreach (var ip in addrs)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }        
    }
    /// <summary>
    /// 忽略
    /// </summary>
    public class IgnoreAttribute : Attribute
    {
    }
    /// <summary>
    /// 显示文本
    /// </summary>
    public class DisplayTextAttribute : Attribute
    {
        /// <summary>
        /// 前端展示文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 字体颜色
        /// </summary>
        public int Color { get; set; }
        /// <summary>
        /// html标签
        /// </summary>
        public string HtmlTag { get; set; }
        /// <summary>
        /// css类名
        /// </summary>
        public string ClassName { get; set; }
    }
}
