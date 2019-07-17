using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Security
{
    public class SignatureVerify
    {
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        public void Put(string name, string value)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                this.parameters.Add(name.ToLower(), value.ToLower());
        }

        public string Signature()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in parameters.Keys)
            {
                sb.Append(key);
                sb.Append("=");
                sb.Append(parameters[key]);
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return MD5.Md5HashString(sb.ToString());
        }
        public bool Verify(string sign)
        {
            return false;
        }
    }
}
