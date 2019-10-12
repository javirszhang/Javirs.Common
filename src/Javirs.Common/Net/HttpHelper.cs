using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Javirs.Common.Net
{

    /// <summary>
    /// http帮助类，主要用于post文件，也可以post常规表单
    /// </summary>
    [DebuggerDisplay("Url={_requestData.Url},Encoding={_requestData.Encoding},StatusCode={StatusCode}")]
    public class HttpHelper
    {
        private Request _requestData;
        private Response _responseData;
        /// <summary>
        /// 初始化Http帮助类，默认UTF8编码
        /// </summary>
        /// <param name="url"></param>
        public HttpHelper(string url)
            : this(url, Encoding.UTF8)
        {

        }
        /// <summary>
        /// 初始化http帮助类
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        public HttpHelper(string url, Encoding encoding)
        {
            this._requestData = new Request(this);
            this._requestData.Url = url;
            this._requestData.Encoding = encoding;
        }
        /// <summary>
        /// 添加Http头信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public HttpHelper AddHeaderData(string name, string value)
        {
            this._requestData.AddHeader(name, value);
            return this;
        }
        /// <summary>
        /// 添加Post表单数据
        /// </summary>
        /// <param name="data"></param>
        public HttpHelper AddPostData(IPostData data)
        {
            this._requestData.AddPostData(data);
            return this;
        }
        /// <summary>
        /// 添加常规表单数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public HttpHelper AddPostData(string name, object value)
        {
            PostData data = new PostData
            {
                Name = name,
                Value = value
            };
            return AddPostData(data);
        }
        /// <summary>
        /// 批量添加表单数据，可以是常规表单，也可以是带文件的表单，也可以是混合表单
        /// </summary>
        /// <param name="data"></param>
        public HttpHelper AddPostData(IEnumerable<IPostData> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    this._requestData.AddPostData(item);
                }
            }
            return this;
        }
        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public HttpHelper AddCookie(Cookie cookie)
        {
            this._requestData.AddCookie(cookie);
            return this;
        }
        /// <summary>
        /// 证书地址
        /// </summary>
        public CertificationInfo Certification
        {
            get
            {
                return this._requestData.Certification;
            }
            set
            {
                this._requestData.Certification = value;
            }
        }
        /// <summary>
        /// SSL请求的回调认证
        /// </summary>
        public RemoteCertificateValidationCallback SslCallback
        {
            get { return this._requestData.SslCallback; }
            set { this._requestData.SslCallback = value; }
        }
        /// <summary>
        /// 上传数据
        /// </summary>
        /// <returns></returns>
        public string Post()
        {
            this._requestData.Method = "POST";
            using (this._requestData)
            {
                this._responseData = this._requestData.GetResponse();
                return this._responseData.Result;
            }
        }
        /// <summary>
        /// 获取http响应的更多信息
        /// </summary>
        /// <returns></returns>
        public Response GetResponse()
        {
            return this._responseData;
        }

        /// <summary>
        /// 提交POST请求
        /// </summary>
        /// <param name="s">POST数据</param>
        /// <param name="timeout">超时时间,单位秒</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <returns></returns>
        public string Post(string s, int timeout, bool isUseCert)
        {
            return Post(s, timeout, isUseCert, "text/xml");
        }
        /// <summary>
        /// 提交POST请求，默认20秒超时时间
        /// </summary>
        /// <param name="s">POST数据</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <returns></returns>
        public string Post(string s, bool isUseCert)
        {
            return Post(s, 20, isUseCert);
        }
        /// <summary>
        /// 获取POST请求结果
        /// </summary>
        /// <param name="s">post数据</param>
        /// <param name="timeout">超时时间，单位为秒</param>
        /// <param name="isUseCert">是否使用证书，如需使用证书请设置Certificate证书信息</param>
        /// <param name="content_type">请求内容类型</param>
        /// <returns></returns>
        public string Post(string s, int timeout, bool isUseCert, string content_type)
        {
            timeout = timeout < 10 ? 10 : timeout;

            this._requestData.SetPostString(s, content_type);
            this._requestData.Timeout = timeout;
            this._requestData.UseCert = isUseCert;
            using (this._requestData)
            {
                this._responseData = this._requestData.GetResponse();
                return this._responseData.Result;
            }
        }
        /// <summary>
        /// 直接post字节数组
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        /// <param name="isUseCert"></param>
        /// <returns></returns>
        public string Post(byte[] buffer, string contentType, int timeout, bool isUseCert)
        {
            return SendRequest("POST", buffer, timeout, isUseCert, contentType);
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="buffer"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        /// <param name="isUseCert"></param>
        /// <returns></returns>
        public string SendRequest(string method, byte[] buffer, int timeout, bool isUseCert, string contentType = null)
        {
            timeout = timeout < 10 ? 10 : timeout;
            this._requestData.SetRequestBody(buffer);
            if (!string.IsNullOrEmpty(contentType))
            {
                this._requestData.ContentType = contentType;
            }
            this._requestData.Timeout = timeout;
            this._requestData.UseCert = isUseCert;
            this._requestData.Method = method;
            using (this._requestData)
            {
                this._responseData = this._requestData.GetResponse();
                return this._responseData.Result;
            }
        }
        /// <summary>
        /// 向http接口post数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Post(object obj)
        {
            Type t = obj.GetType();
            PropertyInfo[] proInfos = t.GetProperties();
            var signAlthmDic = new Dictionary<string, IEnumerable<ISignatureAlthrigmAttribute>>();
            Dictionary<string, object> nameValuePairs = new Dictionary<string, object>();
            List<string> ignorePostFields = new List<string>();
            var urlencodelist = new Dictionary<string, Encoding>();
            foreach (PropertyInfo pi in proInfos)
            {
                bool isEnc = false;
                string key = pi.Name;
                object value = pi.GetValue(obj, null);
                object[] attrs = pi.GetCustomAttributes(true);
                if (attrs != null)
                {
                    var ignore_attrs = attrs.Where(it => it is IgnorePostAttribute);
                    if (ignore_attrs != null && ignore_attrs.Count() > 0)
                    {
                        nameValuePairs.Add(key, value);
                        ignorePostFields.Add(key);
                        continue;
                    }
                    var cipher_attrs = attrs.Where(it => it is ICipherAttribute);
                    if (cipher_attrs != null && cipher_attrs.Count() > 0)
                    {
                        var ciphers = cipher_attrs.Cast<ICipherAttribute>();
                        if (ciphers != null)
                        {
                            var sortciphers = ciphers.OrderByDescending(it => it.Priority);
                            if (sortciphers != null)
                            {
                                foreach (var item in sortciphers)
                                {
                                    value = item.Encrypt(value + "");
                                    isEnc = true;
                                }
                            }
                        }
                    }
                    var dataname_attrs = attrs.Where(it => it is PostDataAttribute);
                    if (dataname_attrs != null && dataname_attrs.Count() > 0)
                    {
                        var postname_attrs = dataname_attrs.Cast<PostDataAttribute>();
                        if (postname_attrs != null)
                        {
                            foreach (PostDataAttribute pda in postname_attrs)
                            {
                                if (pda != null)
                                {
                                    if (!string.IsNullOrEmpty(pda.Name))
                                        key = pda.Name;
                                    if (pda.UrlEncode)
                                    {
                                        string charset = string.IsNullOrEmpty(pda.Charset) ? "UTF-8" : pda.Charset;
                                        urlencodelist.Add(key, Encoding.GetEncoding(charset));
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    var sign_attrs = attrs.Where(it => it is ISignatureAlthrigmAttribute);
                    if (sign_attrs != null && sign_attrs.Count() > 0)
                    {
                        signAlthmDic.Add(key, sign_attrs.Cast<ISignatureAlthrigmAttribute>());
                        continue;
                    }
                }
                if (isEnc && !urlencodelist.Keys.Contains(key))
                {
                    urlencodelist.Add(key, Encoding.GetEncoding("UTF-8"));
                }
                nameValuePairs.Add(key, value);
            }
            if (signAlthmDic.Count > 1)
            {
                throw new ArgumentException("有多个字段被标记为ISignatureAlthrigmAttribute");
            }
            foreach (var signkey in signAlthmDic.Keys)
            {
                var signAlthm = signAlthmDic[signkey];
                if (signAlthm == null || signAlthm.Count() != 1)
                {
                    throw new ArgumentException("字段被多次标记为ISignatureAlthrigmAttribute");
                }
                foreach (ISignatureAlthrigmAttribute ISAA in signAlthm)
                {
                    string enc = ISAA.Encrypt(nameValuePairs);
                    nameValuePairs.Add(signkey, enc);
                    break;
                }
            }
            foreach (string key in nameValuePairs.Keys)
            {
                if (ignorePostFields.Contains(key))
                {
                    continue;
                }
                string postvalue = nameValuePairs[key] + "";
                if (urlencodelist.Keys.Contains(key))
                    postvalue = HttpUtility.UrlEncode(postvalue.ToString(), urlencodelist[key]);
                AddPostData(key, postvalue);
            }
            return Post();
        }
        /// <summary>
        /// 使用Get方式
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string Get(string query = null)
        {
            this._requestData.Method = "GET";
            string path = this._requestData.Url;
            if (!string.IsNullOrEmpty(query))
            {
                string sep = this._requestData.Url.IndexOf("?") > 0 ? "&" : "?";
                if (sep.Equals("&"))
                {
                    query = query.TrimStart('?');
                }
                path = string.Concat(path, sep, query);
            }
            this._requestData.Url = path;
            using (this._requestData)
            {
                this._responseData = this._requestData.GetResponse();
                return _responseData.Result;
            }
        }
        /// <summary>
        /// 下载，使用GET
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetFile()
        {
            this._requestData.Method = "GET";
            using (this._requestData)
            {
                this._responseData = this._requestData.GetResponse();
                if (this._responseData.StatusCode == 200)
                {
                    return new MemoryStream(this._responseData.Buffer);
                }
                return null;
            }
        }
        /// <summary>
        /// Http状态码
        /// </summary>
        public int StatusCode
        {
            get
            {
                if (this._responseData != null)
                {
                    return this._responseData.StatusCode;
                }
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        private class Request : IDisposable
        {
            private HttpHelper _http;
            public Request(HttpHelper http)
            {
                this._http = http;
            }
            public string ContentType { get; set; }
            public Encoding Encoding { get; set; }
            public Dictionary<string, string> Headers { get; set; }
            public List<Cookie> Cookies { get; set; }
            public string Url { get; set; }
            public string Method { get; set; }
            private byte[] _buffer;
            public byte[] Buffer
            {
                get
                {
                    if (_buffer == null)
                    {
                        _buffer = ConstructPostData();
                    }
                    return _buffer;
                }
            }
            public void SetPostString(string s, string contentType)
            {
                this.Method = "POST";
                this.ContentType = contentType;
                if (!string.IsNullOrEmpty(s))
                {
                    if (Encoding == null)
                    {
                        Encoding = Encoding.UTF8;
                    }
                    this._buffer = this.Encoding.GetBytes(s);
                }
            }
            public void SetRequestBody(byte[] buffer)
            {
                this._buffer = buffer;
            }
            public List<IPostData> PostDataCollection { get; set; }
            public void AddHeader(string name, string value)
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                {
                    return;
                }
                if (Headers == null)
                {
                    Headers = new Dictionary<string, string>();
                }
                if (!Headers.ContainsKey(name))
                {
                    Headers.Add(name, value);
                }
                else
                {
                    Headers[name] = value;
                }
            }
            public void AddPostData(IPostData data)
            {
                if (data == null)
                {
                    return;
                }
                if (PostDataCollection == null)
                {
                    PostDataCollection = new List<IPostData>();
                }
                PostDataCollection.Add(data);
            }
            public void AddCookie(Cookie cookie)
            {
                if (cookie == null)
                {
                    return;
                }
                if (Cookies == null)
                {
                    Cookies = new List<Cookie>();
                }
                this.Cookies.Add(cookie);
            }
            private byte[] ConstructPostData()
            {
                if (PostDataCollection == null || PostDataCollection.Count <= 0)
                {
                    return null;
                }
                List<byte> postBuffer = new List<byte>();
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] bytesBoundary = this.Encoding.GetBytes("\r\n--" + boundary + "\r\n");
                var textData = this.PostDataCollection.FindAll((it) => { return !(it is PostFileData); });
                var fileData = this.PostDataCollection.FindAll((it) => { return (it is PostFileData); });
                if (fileData.Count > 0)
                {
                    this.ContentType = "multipart/form-data; boundary=" + boundary;
                    if (textData.Count > 0)
                    {
                        StringBuilder textpoststring = new StringBuilder();
                        foreach (var text in textData)
                        {
                            textpoststring.Append(string.Concat("\r\n--", boundary, "\r\n"));
                            textpoststring.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", text.Name, text.Value);
                        }
                        byte[] textByteData = this.Encoding.GetBytes(textpoststring.ToString());
                        postBuffer.AddRange(textByteData);
                    }
                    foreach (var data in fileData)
                    {
                        IPostFileData ipfd = data as IPostFileData;
                        byte[] filebytes = data.Value as byte[];
                        if (filebytes == null)
                            continue;
                        string contenttype = "image/gif";
                        postBuffer.AddRange(bytesBoundary);
                        string filedataComposite = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                        byte[] compositeBytes = this.Encoding.GetBytes(string.Format(filedataComposite, data.Name, ipfd.FileName, contenttype));
                        postBuffer.AddRange(compositeBytes);
                        postBuffer.AddRange(filebytes);
                    }
                    byte[] lastboundary = this.Encoding.GetBytes("\r\n--" + boundary + "--");
                    postBuffer.AddRange(lastboundary);
                }
                else
                {
                    this.ContentType = "application/x-www-form-urlencoded";
                    StringBuilder textpoststring = new StringBuilder();
                    foreach (var text in textData)
                    {
                        textpoststring.Append(string.Concat(text.Name, "=", text.Value, "&"));
                    }
                    textpoststring.Remove(textpoststring.Length - 1, 1);
                    byte[] textByteData = this.Encoding.GetBytes(textpoststring.ToString());
                    postBuffer.AddRange(textByteData);
                }
                return postBuffer.ToArray();
            }
            public RemoteCertificateValidationCallback SslCallback { get; set; }
            public CertificationInfo Certification { get; set; }
            public int Timeout { get; set; }
            public bool UseCert { get; set; }
            private HttpWebRequest _webRequest;
            private HttpWebResponse _webRepsonse;
            private void SetRequestHeader(HttpWebRequest request)
            {
                if (this.Headers == null || this.Headers.Count <= 0)
                {
                    return;
                }
                var header = this.Headers;
                if (header.Count > 0)
                {
                    foreach (string key in header.Keys)
                    {
                        switch (key.ToUpper())
                        {
                            case "CONTENT-LENGTH": break;//drop content-length
                            case "ACCEPT": request.Accept = header[key]; break;
                            case "CONTENT-TYPE": this.ContentType = request.ContentType = header[key]; break;
                            case "USER-AGENT": request.UserAgent = header[key]; break;
                            case "CACHE-CONTROL":
                            case "ACCEPT-LANGUAGE":
                            case "ACCEPT-ENCODING":
                            default:
                                request.Headers.Add(key, header[key]);
                                break;
                        }

                    }
                }
            }
            private void SetRequestCookie(HttpWebRequest request)
            {
                if (this.Cookies == null)
                {
                    return;
                }
                if (request.CookieContainer == null)
                {
                    request.CookieContainer = new CookieContainer();
                }
                foreach (Cookie cookie in this.Cookies)
                {
                    request.CookieContainer.Add(cookie);
                }
            }
            private HttpWebRequest BuildRequest()
            {
                int timeout = this.Timeout < 10 ? 10 : this.Timeout;
                if (this.Url.StartsWith("https", StringComparison.CurrentCultureIgnoreCase))
                {
                    RemoteCertificateValidationCallback remoteCallBack = new RemoteCertificateValidationCallback(CheckValidationResult);
                    if (this.SslCallback != null)
                    {
                        remoteCallBack = this.SslCallback;
                    }
                    ServicePointManager.ServerCertificateValidationCallback = remoteCallBack;
                }
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.Url);
                request.KeepAlive = true;
                request.ContentType = this.ContentType;
                request.Method = this.Method;
                request.Timeout = timeout * 1000;
                //是否使用证书
                if (this.UseCert && this.Certification != null)
                {
                    X509Certificate2 cert = new X509Certificate2(this.Certification.Path, this.Certification.Password);
                    request.ClientCertificates.Add(cert);
                }
                SetRequestCookie(request);
                SetRequestHeader(request);
                if (this.Buffer != null && this.Buffer.Length > 0)
                {
                    request.ContentLength = this.Buffer.Length;
                    request.ContentType = this.ContentType;
                    Stream reqStream = request.GetRequestStream();
                    reqStream.Write(this.Buffer, 0, this.Buffer.Length);
                    reqStream.Close();
                }
                return request;
            }

            public Response GetResponse()
            {
                _webRequest = this.BuildRequest();
                try
                {
                    _webRepsonse = (HttpWebResponse)_webRequest.GetResponse();
                    return Response.ParseFrom(_webRepsonse, this.Encoding);
                }
                catch (Exception e)
                {
                    return Response.ParseFrom(e, this.Encoding);
                }
                finally
                {
                    this.Reset();
                }
            }
            public void Reset()
            {
                this._buffer = null;
                this.PostDataCollection?.Clear();
            }
            public void Dispose()
            {
                this._webRequest?.Abort();
                this._webRequest = null;
                this._webRepsonse?.Close();
                this._webRepsonse = null;
            }
        }
        /// <summary>
        /// http请求响应
        /// </summary>
        public class Response
        {
            private Encoding encoding;
            private string _result;
            public Response(Encoding encoding)
            {
                this.encoding = encoding;
            }
            /// <summary>
            /// http请求的字符串结果
            /// </summary>
            public string Result
            {
                get
                {
                    if (string.IsNullOrEmpty(_result) && this.Buffer != null && this.Buffer.Length > 0)
                    {
                        _result = encoding.GetString(this.Buffer);
                    }
                    return _result;
                }
            }
            /// <summary>
            /// http请求的字节数组结果
            /// </summary>
            public byte[] Buffer { get; private set; }
            /// <summary>
            /// http请求状态码
            /// </summary>
            public int StatusCode { get; private set; }
            /// <summary>
            /// http请求状态码描述
            /// </summary>
            public string Description { get; private set; }
            /// <summary>
            /// 响应头信息
            /// </summary>
            public Dictionary<string, string> Headers { get; private set; }
            /// <summary>
            /// 响应cookie集合
            /// </summary>
            public CookieCollection Cookies { get; private set; }
            /// <summary>
            /// 异常信息
            /// </summary>
            public Exception Exception { get; private set; }
            internal static Response ParseFrom(HttpWebResponse response, Encoding encoding)
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                var res = new Response(encoding);
                Stream stream = response.GetResponseStream();
                List<byte> bs = new List<byte>();
                int nextByte;
                while ((nextByte = stream.ReadByte()) > -1)
                {
                    bs.Add((byte)nextByte);
                }
                stream.Close();
                res.Buffer = bs.ToArray();
                res.StatusCode = (int)response.StatusCode;
                res.Description = response.StatusDescription;
                if (response.Headers.Count > 0)
                {
                    res.Headers = new Dictionary<string, string>();
                    foreach (string key in response.Headers.Keys)
                    {
                        res.Headers.Add(key, response.Headers[key]);
                    }
                }
                res.Cookies = response.Cookies;
                return res;
            }

            internal static Response ParseFrom(Exception ex, Encoding encoding)
            {
                WebException we;
                if ((we = (ex as WebException)) != null && we.Response != null)
                {
                    return ParseFrom((HttpWebResponse)we.Response, encoding);
                }
                var res = new Response(encoding);
                res.StatusCode = 0;
                res.Exception = ex;
                return res;
            }
        }
    }
}
