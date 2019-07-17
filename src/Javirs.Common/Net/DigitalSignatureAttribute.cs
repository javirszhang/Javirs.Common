using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public class MD5SignatureAttribute : Attribute,ISignatureAlthrigmAttribute
    {
        public string Encrypt(Dictionary<string, object> dic)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in dic.Keys)
            {
                sb.Append(key).Append("=").Append(dic[key]).Append("&");
            }
            sb.Append(Key);
            return Javirs.Common.Security.MD5.Md5HashString(sb.ToString().ToLower());
        }
        public virtual string Key { get; set; }
    }

    public interface ISignatureAlthrigmAttribute
    {
        string Encrypt(Dictionary<string, object> dic);
    }
}
