using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Javirs.Common.Json
{
    /// <summary>
    /// JSON解析对象
    /// </summary>
    public class JsonObject
    {
        private const string _NULL = "null";
        private const string _UNDEFINED = "undefined";
        private Dictionary<string, object> _jsonDic;
        private string _json;
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        private bool _ignoreCase;
        /// <summary>
        /// JSON解析对象
        /// </summary>
        /// <param name="keyvalueDic"></param>
        /// <param name="json"></param>
        /// <param name="ignorePropertyCaseSensitive">取属性是否忽略大小写,默认不忽略</param>
        protected JsonObject(Dictionary<string, object> keyvalueDic, string json, bool ignorePropertyCaseSensitive = false)
        {
            this._jsonDic = keyvalueDic;
            this._json = json;
            this._ignoreCase = ignorePropertyCaseSensitive;
        }
        /// <summary>
        /// 初始化一个空的JsonObject对象
        /// </summary>
        public JsonObject()
        {
            _jsonDic = new Dictionary<string, object>();
            _ignoreCase = false;
        }
        /// <summary>
        /// 解析JSON字符串
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ignorePropertyCaseSensitive">取属性是否忽略大小写</param>
        /// <returns></returns>
        public static JsonObject Parse(string json, bool ignorePropertyCaseSensitive)
        {
            //var dic = ParseJson(json);
            //return new JsonObject(dic, json, ignorePropertyCaseSensitive);
            return ParseJson(json, ignorePropertyCaseSensitive);
        }
        /// <summary>
        /// 解析JSON字符串,忽略key大小写
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JsonObject Parse(string json)
        {
            return Parse(json, true);
        }
        /// <summary>
        /// JSON数据的键集合
        /// </summary>
        public string[] Keys
        {
            get
            {
                return _jsonDic.Keys.ToArray();
            }
        }
        /// <summary>
        /// 使用键名称获取JSON值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                object res = GetObjectFromDictionary(key, true);
                if (res == null && _ignoreCase)
                {
                    res = GetObjectFromDictionary(key, false);
                }
                return res;
                //if (this._jsonDic.Keys.Contains(key, _propertyComparer))
                //{
                //    return this._jsonDic[key];
                //}
                //return null;
            }
        }
        private object GetObjectFromDictionary(string key, bool caseSecentive)
        {
            string originalKey = string.Empty;
            foreach (string dk in _jsonDic.Keys)
            {
                if (dk.Equals(key, caseSecentive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                {
                    originalKey = dk;
                }
            }
            if (string.IsNullOrEmpty(originalKey))
            {
                return null;
            }
            return _jsonDic[originalKey];
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            object obj = this[key];
            if (obj == null)
            {
                return null;
            }
            var valueType = obj.GetType();
            if (valueType == typeof(string) || valueType.IsPrimitive)
            {
                return obj.ToString();
            }
            //return obj != null ? obj.ToString() : null;
            return JsonSerializer.JsonSerialize(obj);
        }
        /// <summary>
        /// 获取指定key的集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonObject[] GetArray(string key)
        {
            string s = string.IsNullOrEmpty(key) ? _json : GetString(key);
            var list = ParseJsonArray(s, _ignoreCase);
            return list.ToArray();
        }
        public JsonObject[] GetArray()
        {
            return GetArray(null);
        }
        /// <summary>
        /// 获取指定key的json对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonObject GetObject(string key)
        {
            string s = GetString(key);
            if (!string.IsNullOrEmpty(s))
            {
                return JsonObject.Parse(s);
            }
            return new JsonObject();
        }
        /// <summary>
        /// 获取指定key的int值
        /// </summary>
        /// <param name="key">Json键名</param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            string value = GetString(key);
            return Convert.ToInt32(value);
        }
        /// <summary>
        /// 获取指定值的bool值
        /// </summary>
        /// <param name="key">Json键名</param>
        /// <returns></returns>
        public bool GetBoolean(string key)
        {
            string value = GetString(key);
            return Convert.ToBoolean(value);
        }
        /// <summary>
        /// 获取指定key的浮点数值
        /// </summary>
        /// <param name="key">Json键名</param>
        /// <returns></returns>
        public decimal GetDecimal(string key)
        {
            string value = GetString(key);
            return Convert.ToDecimal(value);
        }
        /// <summary>
        /// 获取指定key的datetime值
        /// </summary>
        /// <param name="key">Json键名</param>
        /// <returns></returns>
        public DateTime GetDateTime(string key)
        {
            string value = GetString(key);
            return Convert.ToDateTime(value);
        }
        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_json))
            {
                _json = JsonSerializer.JsonSerialize(_jsonDic);
            }
            return _json;
        }
        private static JsonObject ParseJson(string json, bool caseSecentive)
        {
            var resDic = new Dictionary<string, object>();//读取结果
            if (json[0] != '{')
            {
                return new JsonObject(resDic, json, caseSecentive);
            }
            Stack<char> tagStack = new Stack<char>();//标签栈
            StringReader reader = new StringReader(json);//字符串读取器
            StringBuilder propertyName = new StringBuilder();//Json属性名
            StringBuilder propertyValue = new StringBuilder();//Json属性值
            int position = 0;//索引位置
            int currentChar;//当前字符
            char lastTag = '\0';//上一个标签
            while ((currentChar = reader.Read()) > -1)
            {
                char word = (char)currentChar;
                //Debug.Assert(propertyValue.ToString() != "ALL");
                if (word == '{')
                {
                    if (position != 0)
                    {
                        tagStack.Push(word);
                        propertyValue.Append(word);
                    }
                    lastTag = '{';
                }
                else if (word == '}')
                {
                    if (tagStack.Count == 0)//结束
                    {
                        string value = propertyValue.ToString();
                        if (lastTag != '\'' && lastTag != '\"' && (value == _NULL || value == _UNDEFINED))
                        {
                            value = null;
                        }
                        object jsonValue = value;
                        if (value != null && value.StartsWith("["))
                        {
                            var tmp = JsonObject.ParseJsonArray(value, false);
                            var arrayDic = new List<Dictionary<string, object>>();
                            tmp.Aggregate(arrayDic, (d, kv) => { d.Add(kv._jsonDic); return d; });
                            jsonValue = arrayDic;
                        }
                        if (value != null && value.StartsWith("{"))
                        {
                            var tmp2 = JsonObject.ParseJson(value, false);
                            jsonValue = tmp2._jsonDic;
                        }
                        resDic.Add(propertyName.ToString(), jsonValue);

                        propertyName.Clear();
                        propertyValue.Clear();

                    }
                    else
                    {
                        if (tagStack.Peek() != '{')
                        {
                            tagStack.Push(word);
                            propertyValue.Append(word);
                        }
                        else
                        {
                            tagStack.Pop();
                            propertyValue.Append(word);
                        }
                    }
                    lastTag = '}';
                }
                else if (word == ',')
                {
                    if (tagStack.Count == 0)
                    {
                        string value = propertyValue.ToString();
                        if (lastTag != '\'' && lastTag != '\"' && (value == _NULL || value == _UNDEFINED))
                        {
                            value = null;
                        }
                        object jsonValue = value;
                        if (value != null && value.StartsWith("["))
                        {
                            var tmp = JsonObject.ParseJsonArray(value, false);
                            var arrayDic = new List<Dictionary<string, object>>();
                            tmp.Aggregate(arrayDic, (d, kv) => { d.Add(kv._jsonDic); return d; });
                            jsonValue = arrayDic;
                        }
                        if (value != null && value.StartsWith("{"))
                        {
                            var tmp2 = JsonObject.ParseJson(value, false);
                            jsonValue = tmp2._jsonDic;
                        }
                        resDic.Add(propertyName.ToString(), jsonValue);
                        //resDic.Add(propertyName.ToString(), value);

                        propertyName.Clear();
                        propertyValue.Clear();
                    }
                    else
                    {
                        propertyValue.Append(word);
                    }
                    lastTag = ',';
                }
                else if (word == '\'' || word == '\"')
                {
                    if (tagStack.Count > 0)
                    {
                        char tmpChar = tagStack.Peek();

                        if (tmpChar != word && tmpChar != '\'' && tmpChar != '"')
                        {
                            tagStack.Push(word);
                        }
                        else if(tmpChar == word)
                        {
                            tagStack.Pop();
                        }
                        if (tagStack.Count > 0)
                        {
                            propertyValue.Append(word);
                        }
                    }
                    else
                    {
                        tagStack.Push(word);
                    }
                    lastTag = word;
                }
                else if (word == '[')
                {
                    lastTag = '[';
                    tagStack.Push(word);
                    propertyValue.Append(word);
                }
                else if (word == ']')
                {

                    if (tagStack.Peek() != '[')
                    {
                        tagStack.Push(word);
                        propertyValue.Append(word);
                    }
                    else
                    {
                        tagStack.Pop();
                        propertyValue.Append(word);
                    }
                    lastTag = ']';
                }
                else if (word == ':')
                {

                    if (tagStack.Count == 0)
                    {
                        propertyName.Append(propertyValue.ToString());

                        propertyValue.Clear();
                    }
                    else
                    {
                        propertyValue.Append(word);
                    }
                    lastTag = ':';
                }
                else
                {

                    char lastestTag = tagStack.Count > 0 ? tagStack.Peek() : '\0';//上一个标签
                    if (!((word == 0x20 || word == 0x0d || word == 0x0a) && lastestTag != 0x27 && lastestTag != 0x22))
                    {
                        //丢弃空格，换行和回车
                        if (word == 0x5c)
                        {
                            ResolveEscapes(propertyValue, reader);
                        }
                        else
                        {
                            propertyValue.Append(word);
                        }
                    }
                }
                position++;
            }
            //return resDic;
            return new JsonObject(resDic, json, caseSecentive);
        }
        private static List<JsonObject> ParseJsonArray(string json, bool caseSecentive)
        {
            var resDic = new List<JsonObject>();
            StringReader reader = new StringReader(json);
            Stack<char> tagStack = new Stack<char>();
            StringBuilder temp = new StringBuilder();
            List<string> jsonArrayItem = new List<string>();
            char firstChar = (char)reader.Read();
            if (firstChar != '[')
            {
                throw new ApplicationException("不是标准的Json数据格式");
            }
            tagStack.Push(firstChar);
            int i;
            while ((i = reader.Read()) > -1)
            {
                char c = (char)i;
                if (c == '{')
                {
                    tagStack.Push(c);
                    temp.Append(c);
                }
                else if (c == '}')
                {
                    char lastTag = tagStack.Peek();
                    if (lastTag == '{')
                    {
                        tagStack.Pop();
                        temp.Append(c);
                        if (tagStack.Count == 0)
                        {
                            jsonArrayItem.Add(temp.ToString());

                            temp.Clear();

                        }
                    }
                }
                else if (c == ',')
                {
                    if (tagStack.Count > 1)
                    {
                        temp.Append(c);
                    }
                    else
                    {
                        if (temp.Length > 0)
                        {
                            jsonArrayItem.Add(temp.ToString());

                            temp.Clear();

                        }
                    }
                }
                else if (c == '"' || c == '\'')
                {
                    if (tagStack.Count > 1)
                    {
                        temp.Append(c);
                    }
                }
                else if (c == '[')
                {
                    tagStack.Push(c);
                    temp.Append(c);
                }
                else if (c == ']')
                {
                    if (tagStack.Peek() == '[')
                    {
                        tagStack.Pop();
                    }
                    if (tagStack.Count == 0)
                    {
                        if (temp.Length > 0)
                        {
                            jsonArrayItem.Add(temp.ToString());

                            temp.Clear();

                        }
                    }
                    else
                    {
                        if (c == 0x5c)// 0x5c is an asc code for '\'
                        {
                            ResolveEscapes(temp, reader);
                        }
                        else
                        {
                            temp.Append(c);
                        }
                    }
                }
                else
                {
                    temp.Append(c);
                }
            }
            foreach (string item in jsonArrayItem)
            {
                //var dic = ParseJson(item);
                var dataItem = ParseJson(item, caseSecentive); //new JsonObject(dic, item);
                resDic.Add(dataItem);
            }
            return resDic;
        }
        /// <summary>
        /// 添加Json属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(string key, object value)
        {
            if (_jsonDic == null)
            {
                _jsonDic = new Dictionary<string, object>();
            }
            if (_jsonDic.ContainsKey(key))
            {
                return;
            }
            if (value is JsonObject subJo)
            {
                //var subJo = value as JsonObject;
                _jsonDic.Add(key, subJo._jsonDic);
            }
            else if (value is JsonObject[] joArray)
            {
                var list = new List<Dictionary<string, object>>();
                //var joArray = value as JsonObject[];
                foreach (var jo in joArray)
                {
                    list.Add(jo._jsonDic);
                }
                _jsonDic.Add(key, list);
            }
            else
            {
                _jsonDic.Add(key, value);
            }
            _json = JsonSerializer.JsonSerialize(_jsonDic);
        }
        /// <summary>
        /// 移除指定的Json属性
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (_jsonDic.ContainsKey(key))
            {
                _jsonDic.Remove(key);
            }
        }
        /// <summary>
        /// 处理转义符
        /// </summary>
        private static void ResolveEscapes(StringBuilder sBuilder, StringReader reader)
        {
            char c;
            char next = (char)reader.Read();
            switch (next)
            {
                case 'r':
                    c = '\r'; break;
                case 'n':
                    c = '\n'; break;
                case 'u':
                    char[] buffer = new char[4];
                    reader.Read(buffer, 0, 4);
                    string unicodeHex = new string(buffer);
                    ushort uShort = Convert.ToUInt16(unicodeHex, 16);
                    c = Convert.ToChar(uShort);
                    break;
                default:
                    c = next;
                    break;
            }
            sBuilder.Append(c);
        }
    }
    /*
    public class JsonObjectCollection : IEnumerable<JsonObject>
    {
        private List<JsonObject> _array;
        public JsonObjectCollection()
        {
            this._array = new List<JsonObject>();
        }
        public JsonObjectCollection(List<JsonObject> array)
        {
            this._array = array;
        }
        #region enumerator
        public IEnumerator<JsonObject> GetEnumerator()
        {
            return new Enumerator(this._array);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this._array);
        }

        protected class Enumerator : IEnumerator<JsonObject>
        {
            private List<JsonObject> _array;
            private int _currentIndex;
            public Enumerator(List<JsonObject> array)
            {
                this._array = array;
                this._currentIndex = -1;
            }
            public JsonObject Current
            {
                get { return _array[_currentIndex]; }
            }

            object IEnumerator.Current
            {
                get { return _array[_currentIndex]; }
            }

            public void Dispose()
            {
                _array = null;
                _currentIndex = -1;
            }

            public bool MoveNext()
            {
                _currentIndex++;
                if (_currentIndex > _array.Count)
                {
                    return false;
                }
                return true;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }
        }
        #endregion

    }
    */
}
