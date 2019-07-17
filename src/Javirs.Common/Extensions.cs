using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Linq;
using System.Web;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq.Expressions;
using System.Dynamic;

#if netstandard2_0
using Microsoft.AspNetCore.Http;
#endif

namespace Javirs.Common
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 移除符合func委托的集合项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="func"></param>
        public static void Remove<T>(this ICollection<T> self, Func<T, bool> func)
        {
            T[] tsource = self.ToArray();
            foreach (T t in tsource)
            {
                if (func(t))
                    self.Remove(t);
            }
        }
        /// <summary>
        /// 合并字符串数组为字符串
        /// </summary>
        /// <param name="self"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> self, char seperator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in self)
            {
                if (!string.IsNullOrEmpty(s))
                    sb.Append(s + seperator);
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        /// <summary>
        /// 合并字符串数组为字符串
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> self)
        {
            return Join(self, ',');
        }
        /// <summary>
        /// 合并整数数组为字符串
        /// </summary>
        /// <param name="self"></param>
        /// <param name="seperator">连接符</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<int> self, char seperator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int s in self)
            {
                if (!string.IsNullOrEmpty(s.ToString()))
                    sb.Append(s.ToString() + seperator);
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        /// <summary>
        /// 合并整数数组为字符串
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<int> self)
        {
            return Join(self, ',');
        }
        /// <summary>
        /// 将集合的某个字段合并为一个字符串
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="self">源集合</param>
        /// <param name="express">表达式（it=>it.Name）</param>
        /// <param name="seperator">分隔符</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> self, Expression<Func<T, object>> express, char seperator)
        {
            PropertyInfo tarProperty = null;
            try
            {
                tarProperty = GetMemberExpressionProperty<T>(express);
            }
            catch (Exception ex)
            {
                return "";
            }
            if (tarProperty == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            int i = 1;
            foreach (T instance in self)
            {
                object value = tarProperty.GetValue(instance, null);
                builder.Append(value);
                if (i != self.Count())
                {
                    builder.Append(seperator);
                }
                i++;
            }
            return builder.ToString();
        }
        /// <summary>
        /// 汇总集合内某个属性的和值
        /// </summary>
        /// <typeparam name="T">需要汇总的集合单对象</typeparam>
        /// <param name="data">集合</param>
        /// <param name="express">lambda表达式（it=>it.Amount）</param>
        /// <returns>汇总结果，可空数字</returns>
        public static decimal? Sum<T>(this IEnumerable data, Expression<Func<T, object>> express)
        {
            PropertyInfo tarProperty = null;
            try
            {
                tarProperty = GetMemberExpressionProperty<T>(express);
            }
            catch (Exception ex)
            {
                return null;
            }
            if (tarProperty == null)
            {
                return null;
            }
            decimal sumResult = 0;
            foreach (object item in data)
            {
                decimal val = (decimal)tarProperty.GetValue(item, null);
                sumResult += val;
            }
            return sumResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="express"></param>
        /// <exception cref="InvalidOperationException">不合格的操作，不是有效的Lambda表达式</exception>
        /// <returns></returns>
        public static PropertyInfo GetMemberExpressionProperty<T>(Expression<Func<T, object>> express)
        {
            string memberName = null;
            var mexpr = express.Body as MemberExpression;
            if (mexpr != null)
            {
                memberName = mexpr.Member.Name;
            }
            else
            {
                mexpr = ((UnaryExpression)express.Body).Operand as MemberExpression;
                memberName = mexpr.Member.Name;
            }
            if (string.IsNullOrEmpty(memberName))
            {
                return null;
            }
            Type t = typeof(T);
            var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo tarProperty = null;
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == memberName)
                {
                    tarProperty = property;
                    break;
                }
            }
            return tarProperty;
        }
        /// <summary>
        /// 取得xml节点值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ElementValue(this XElement ele, string name)
        {
            if (ele == null)
                return string.Empty;
            XElement xe = ele.Element(name);
            if (xe == null)
                return string.Empty;
            return xe.Value;
        }
        /// <summary>
        /// 取得xml节点值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static string ElementValue(this XElement ele, string name, string defaultvalue)
        {
            if (ele == null)
                return defaultvalue;
            XElement xe = ele.Element(name);
            if (xe == null)
                return defaultvalue;
            return xe.Value;
        }
        /// <summary>
        /// 取得xml节点值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static decimal ElementValue(this XElement ele, string name, decimal defaultvalue)
        {
            return Convert.ToDecimal(ElementValue(ele, name, defaultvalue.ToString()));
        }
        /// <summary>
        /// 取得xml节点值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static int ElementValue(this XElement ele, string name, int defaultvalue)
        {
            return Convert.ToInt32(ElementValue(ele, name, defaultvalue.ToString()));
        }
        /// <summary>
        /// 取得xml节点值
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static bool ElementValue(this XElement ele, string name, bool defaultvalue)
        {
            string value = ele.ElementValue(name);
            if (string.IsNullOrEmpty(value))
                return defaultvalue;
            bool result = false;
            switch (value.ToLower())
            {
                case "false": result = false; break;
                case "true": result = true; break;
                default:
                    int i = 0;
                    if (!int.TryParse(value, out i))
                        result = false;
                    else
                        result = i > 0;
                    break;
            }
            return result;
        }
        /// <summary>
        /// 修改集合内元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="action"></param>
        public static void Set<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (T t in self)
            {
                action(t);
            }
        }
        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] HexString2ByteArray(this string s)
        {
            int len = s.Length;
            if (len % 2 != 0)
            {
                s += " ";
                len += 1;
            }
            int blen = len / 2;
            byte[] bytes = new byte[blen];
            for (int i = 0; i < blen; i++)
            {
                bytes[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
        public static T Find<T>(this IEnumerable<T> array, Func<T, bool> func)
        {
            foreach (T t in array)
            {
                if (func(t))
                {
                    return t;
                }
            }
            return default(T);
        }
        /// <summary>
        /// 字节数组转换16进制字符串
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static string Byte2HexString(this byte[] bs)
        {
            StringBuilder hex = new StringBuilder();
            foreach (byte b in bs)
                hex.Append(b.ToString("x").PadLeft(2, '0'));
            return hex.ToString();
        }
        /// <summary>
        /// 截断指定小数位，20180522解决溢出风险
        /// </summary>
        /// <param name="d"></param>
        /// <param name="scale">指定保留小数位数</param>
        /// <returns></returns>
        public static decimal TruncateValue(this decimal d, int scale)
        {
            decimal block = (decimal)Math.Pow(10, scale);
            decimal zs = decimal.Truncate(d);//得到整数部分
            decimal xs = d - zs;//得到小数部分
            decimal val = decimal.Truncate(xs * block) / block;//小数部分进行位数保留
            return val + zs;//最终结果，小数部分保留位数+整数部分
        }

#region 冒泡排序法,按照字母序列从a到z的顺序排列
        /// <summary>
        /// 冒泡排序法,按照字母序列从a到z的顺序排列
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static ArrayList BubbleSort(this ArrayList r)
        {
            int i, j; //交换标志 
            object temp;

            bool exchange;

            for (i = 0; i < r.Count; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Count - 2; j >= i; j--)
                {//交换条件
                    if (System.String.CompareOrdinal(r[j + 1].ToString(), r[j].ToString()) < 0)
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }
                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
            return r;
        }
#endregion
        /// <summary>
        /// 取得包含毫秒的时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToMillisecondString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
#if NET40
        /// <summary>
        /// 获取Web文件编码（Response）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetWebEncodeFileName(this string fileName)
        {
            HttpRequest request = HttpContext.Current.Request;
            string text = request.UserAgent.ToUpper();
            if (text.Contains("MS") && text.Contains("IE"))
            {
                fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            }
            else
            {
                if (text.Contains("FIREFOX"))
                {
                    fileName = "\"" + fileName + "\"";
                }
                else
                {
                    fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                }
            }
            return fileName;
        }
#endif
        /// <summary>
        /// 转换类型为Nullable&lt;DateTime&gt;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            DateTime datetime;
            if (!DateTime.TryParse(str, out datetime))
            {
                return null;
            }
            return datetime;
        }
        /// <summary>
        /// 转换为以属性分行的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToLineText(this object obj)
        {
            Type t = obj.GetType();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(t.FullName);
            var properties = t.GetProperties();
            foreach (var item in properties)
            {
                sb.AppendLine($"[{item.Name.PadRight(20, (Char)0x20)}]{item.GetValue(obj, null)}");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 保存HTML中的远程图片
        /// </summary>
        /// <param name="html">需要处理的HTML源串</param>
        /// <param name="savedLocalPaths">保存至本地的路径集合</param>
        /// <param name="localUri">本地Uri</param>
        /// <param name="saveDirectory">文件保存目录</param>
        /// <returns></returns>
        public static string SaveRemoteImage(this string html, Uri localUri, string saveDirectory, out List<string> savedLocalPaths)
        {
            const string imagePattern = @"<\s*img.*?[/]?>";
            const string srcPattern = "(.+?src\\s*=[\"'])(.+?)([\"\'].+)";
            savedLocalPaths = new List<string>();
            List<string> imgList = new List<string>();
            var mc = Regex.Matches(html, imagePattern);
            if (mc.Count == 0)
            {
                return html;
            }

            if (saveDirectory.StartsWith("/") || saveDirectory.StartsWith("\\"))
            {
                saveDirectory = saveDirectory.Substring(1, saveDirectory.Length - 1);
            }
            //完整目录名称
            string directoryName = saveDirectory;
            if (!Regex.IsMatch(saveDirectory, @"[a-zA-Z]:\\"))
            {
                directoryName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, saveDirectory);
            }
            //如果目录不存在则创建
            if (!System.IO.Directory.Exists(directoryName))
            {
                System.IO.Directory.CreateDirectory(directoryName);
            }
            foreach (Match match in mc)
            {
                imgList.Add(match.Value);
            }
            foreach (string imageSytax in imgList)
            {
                var match = Regex.Match(imageSytax, srcPattern);
                string url = match.Groups[2].Value;
                if (url.StartsWith("http") || url.StartsWith("https"))
                {
                    Uri uri = new Uri(url);
                    //如果图片域名与需要剔除的域名相同，直接进入下一次循环
                    if (uri.Host.Equals(localUri.Host, StringComparison.OrdinalIgnoreCase))
                    {
                        savedLocalPaths.Add(url);
                        continue;
                    }
                    string extName = ".jpg";
                    if (System.IO.Path.HasExtension(uri.AbsoluteUri))
                    {
                        extName = System.IO.Path.GetExtension(uri.AbsoluteUri);
                        if (extName.IndexOf('?') >= 0)
                        {
                            extName = extName.Substring(0, extName.IndexOf('?'));
                        }
                    }

                    //guid文件名
                    string filename = "auto_" + Guid.NewGuid().ToString("N") + extName;
                    //完整路径，包含盘符
                    string fullpath = System.IO.Path.Combine(directoryName, filename);
                    //调用方传入的相对路径
                    string localpath = System.IO.Path.Combine(saveDirectory, filename);

                    savedLocalPaths.Add("/" + localpath);
                    WebClient wc = new WebClient();
                    wc.DownloadFile(uri, fullpath);
                    wc.Dispose();
                    string replacedSyntax = Regex.Replace(imageSytax, srcPattern, "$1" + "/" + localpath + "$3");
                    html = html.Replace(imageSytax, replacedSyntax);
                }
                else
                {
                    savedLocalPaths.Add(url);
                }
            }
            return html;
        }
        /*
        public static string SaveRemoteImage(this string html,Uri[] localUris,string saveDirPath,out List<string> savedLocalPaths)
        {
            const string imagePattern = @"<\s*img.*?[/]?>";
            const string srcPattern = "(.+?src\\s*=[\"'])(.+?)([\"\'].+)";
            savedLocalPaths = new List<string>();
            List<string> imgList = new List<string>();
            var mc = Regex.Matches(html, imagePattern);
            if (mc.Count == 0)
            {
                return html;
            }

            if (saveDirPath.StartsWith("/") || saveDirPath.StartsWith("\\"))
            {
                saveDirPath = saveDirPath.Substring(1, saveDirPath.Length - 1);
            }
            //完整目录名称
            string directoryName = saveDirPath;
            if (!Regex.IsMatch(saveDirPath, @"[a-zA-Z]:\\"))
            {
                directoryName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, saveDirPath);
            }
            //如果目录不存在则创建
            if (!System.IO.Directory.Exists(directoryName))
            {
                System.IO.Directory.CreateDirectory(directoryName);
            }
            foreach (Match match in mc)
            {
                imgList.Add(match.Value);
            }
            foreach (string imageSytax in imgList)
            {
                var match = Regex.Match(imageSytax, srcPattern);
                string url = match.Groups[2].Value;
                if (url.StartsWith("http") || url.StartsWith("https"))
                {
                    Uri uri = new Uri(url);
                    
                    //如果图片域名与需要剔除的域名相同，直接进入下一次循环
                    if (uri.Host.Equals(localUri.Host, StringComparison.OrdinalIgnoreCase))
                    {
                        savedLocalPaths.Add(url);
                        continue;
                    }
                    string extName = ".jpg";
                    if (System.IO.Path.HasExtension(uri.AbsoluteUri))
                    {
                        extName = System.IO.Path.GetExtension(uri.AbsoluteUri);
                        if (extName.IndexOf('?') >= 0)
                        {
                            extName = extName.Substring(0, extName.IndexOf('?'));
                        }
                    }

                    //guid文件名
                    string filename = "auto_" + Guid.NewGuid().ToString("N") + extName;
                    //完整路径，包含盘符
                    string fullpath = System.IO.Path.Combine(directoryName, filename);
                    //调用方传入的相对路径
                    string localpath = System.IO.Path.Combine(saveDirPath, filename);

                    savedLocalPaths.Add("/" + localpath);
                    WebClient wc = new WebClient();
                    wc.DownloadFile(uri, fullpath);
                    wc.Dispose();
                    string replacedSyntax = Regex.Replace(imageSytax, srcPattern, "$1" + "/" + localpath + "$3");
                    html = html.Replace(imageSytax, replacedSyntax);
                }
                else
                {
                    savedLocalPaths.Add(url);
                }
            }
            return html;
        }
        */

        /// <summary>
        /// 转换为动态对象
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="selectColumns">选择的数据列</param>
        /// <returns></returns>
        public static dynamic ToDynamic(this System.Data.DataTable table, params string[] selectColumns)
        {
            if (table == null)
            {
                List<object> list = new List<object>();
                return list;
            }
            return ToDynamic(table, forceToCollection: true, selectColumns: selectColumns);
        }
        /// <summary>
        /// 转换为动态对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="selectColumns"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this System.Data.DataRow row, params string[] selectColumns)
        {
            if (row == null)
            {
                return null;
            }
            return ToDynamic(row.Table, forceToCollection: false, selectColumns: selectColumns);
        }
        /// <summary>
        /// 转换为动态对象
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="forceToCollection">是否强制转换为集合</param>
        /// <param name="selectColumns">过滤列（只要这些列，不传则全选）</param>
        /// <param name="keyCase">动态属性的字符串格式（小写，大写，驼峰，驼峰不带分隔符）</param>
        /// <param name="OnFieldGenerating">生成动态属性时</param>
        /// <param name="OnBeforeRowAdding">生成集合一行数据之前</param>
        /// <param name="OnAfterRowAdded">生产集合一行数据之后</param>
        /// <returns></returns>
        public static dynamic ToDynamic(this System.Data.DataTable table, bool forceToCollection = true,
            string[] selectColumns = null,
            DynamicConverter.KeyType keyCase = DynamicConverter.KeyType.LowerCase,
            Func<NameValuePair, NameValuePair> OnFieldGenerating = null,
            Action<List<dynamic>> OnBeforeRowAdding = null,
            Action<List<dynamic>> OnAfterRowAdded = null)
        {
            DynamicConverter converter = new DynamicConverter(table, forceToCollection);
            converter.KeyCase = keyCase;
            converter.FieldGenerating += OnFieldGenerating;
            converter.BeforeRowAdding += OnBeforeRowAdding;
            converter.AfterRowAdded += OnAfterRowAdded;
            return converter.ToDynamic(selectColumns);
        }
        //public static dynamic ToDynamic(this System.Data.DataRow row,string[] selectedColumns,DynamicConverter.KeyType keyCase=DynamicConverter.KeyType.LowerCase)

#if NET40
        /// <summary>
        /// HTTP请求参数映射到对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static T Mapping<T>(this HttpRequest request) where T : class
        {
            Type t = typeof(T);
            List<string> propertyNames = new List<string>();
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            T @this = (T)Activator.CreateInstance(t);
            foreach (PropertyInfo pinfo in properties)
            {
                string value = request[pinfo.Name];
                if (!string.IsNullOrEmpty(value))
                {
                    SetValue(value, pinfo, @this);
                }
            }
            return @this;
        }
#endif
        private static void SetValue(string value, PropertyInfo property, object target)
        {
            object realValue;
            switch (property.PropertyType.Name.ToLower())
            {
                case "string": realValue = value; break;
                default:
                    realValue = null;
                    break;
            }
            property.SetValue(realValue, target, null);
        }
        /// <summary>
        /// 转换为驼峰格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string s)
        {
            return s.ToPascalCase('_');
        }
        /// <summary>
        /// 转换为驼峰命名
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="split">字符串单词分隔符</param>
        /// <returns></returns>
        public static string ToPascalCase(this string s, char split)
        {
            return s.ToPascalCase(split, false);
        }
        /// <summary>
        /// 转化为驼峰命名
        /// </summary>
        /// <param name="s">目标字符串</param>
        /// <param name="split">字符串单词分隔符</param>
        /// <param name="ignoreSplit">输出时是否忽略分隔符</param>
        /// <returns></returns>
        public static string ToPascalCase(this string s, char split, bool ignoreSplit)
        {
            string t = string.Empty;
            Regex regex = new Regex("[^" + split + "]*");
            MatchCollection mc = regex.Matches(s);
            foreach (Match m in mc)
            {
                if (string.IsNullOrEmpty(m.Value))
                    continue;
                t += m.Value.Substring(0, 1).ToUpper() + m.Value.Substring(1, m.Value.Length - 1).ToLower();
                if (!ignoreSplit)
                {
                    t += split;
                }
            }
            t = t.TrimEnd(split);
            return t;
        }

        public static IDictionary<string, object> ToDictionary(this object obj, IDictionary<string, object> dictionary = null,
            Func<NameValuePair, NameValuePair> beforeAdding = null,
            Action<IDictionary<string, object>> afterAdded = null)
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>();
            }
            if (obj == null)
            {
                return dictionary;
            }
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in properties)
            {
                NameValuePair nvp = new NameValuePair { Name = p.Name, Value = p.GetValue(obj, null) };
                nvp = beforeAdding?.Invoke(nvp);
                if (nvp != null)
                {
                    dictionary.Add(nvp.Name, nvp.Value);
                }
                afterAdded?.Invoke(dictionary);
            }
            return dictionary;
        }
    }
}
