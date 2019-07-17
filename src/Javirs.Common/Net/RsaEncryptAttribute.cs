using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public class RsaEncryptAttribute : Attribute,ICipherAttribute
    {
        public string Encrypt(string plainText)
        {
            Common.Security.RsaCertificate cert = Common.Security.RsaCertificate.ReadFromCert(CertificateLocation);
            return cert.Encrypt(plainText);
        }

        public string Decrypt(string encStr)
        {
            throw new NotImplementedException();
        }

        public int Priority { get; set; }
        public string CertificateLocation { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
