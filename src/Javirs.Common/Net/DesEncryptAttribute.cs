using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public class DesEncryptAttribute : Attribute, ICipherAttribute
    {
        public string Key { get; set; }
        public int Priority { get; set; }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(Key))
                Key = Guid.NewGuid().ToString("N");
            var des = new Common.Security.DesEncodeDecode(Key);
            return des.DesEncrypt(plainText);
        }

        public string Decrypt(string encStr)
        {
            throw new NotImplementedException();
        }
    }
}
