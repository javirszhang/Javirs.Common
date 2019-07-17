using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if net40
using System.Web;
#endif
#if netstandard2_0
using Microsoft.AspNetCore.Http;
#endif
namespace Javirs.Common.Mapping
{

    public class WebRequestMapping : BaseMapping, IMapping
    {
        private HttpRequest _request;
        private Dictionary<string, object> _keyValue;
        public WebRequestMapping(HttpRequest request)
        {
            this._request = request;
            _keyValue = new Dictionary<string, object>();
        }

        protected override Dictionary<string, object> CreateValueDictionary()
        {
            if (_keyValue == null)
            {
                _keyValue = new Dictionary<string, object>();
            }
            if (_keyValue.Count <= 0)
            {
#if net40
                foreach (string k in _request.QueryString.Keys)
                {
                    FillKeyValue(k, _request.QueryString[k]);
                }
                foreach (string k in _request.Form.Keys)
                {
                    FillKeyValue(k, _request.Form[k]);
                }
#else
                foreach (string k in _request.Query.Keys)
                {
                    FillKeyValue(k, _request.Query[k]);
                }
                foreach (string k in _request.Form.Keys)
                {
                    FillKeyValue(k, _request.Form[k]);
                }
#endif
            }
            return _keyValue;
        }
        void FillKeyValue(string k, object v)
        {
            if (_keyValue == null)
            {
                _keyValue = new Dictionary<string, object>();
            }
            if (_keyValue.ContainsKey(k))
            {
                _keyValue[k] = v;
            }
            else
            {
                _keyValue.Add(k, v);
            }
        }
    }
}
