using Javirs.Common.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javirs.Common.Tests
{
    [TestClass]
    public class RsaEncryptTest
    {
        [TestMethod]
        public void EncryptTest()
        {

        }
        [TestMethod]
        public void DecryptTest()
        {
            //string privateKey = "MIICXAIBAAKBgQC+ET6Adq3QkUWkuaXGRWfM5XsaLZBJWWy7KNR0VjkNVtZenrw11yqRJ5zPN2FMDfaPgNEI7KlHD2TBmJO6nBhUYj4vb1qX5Tx0safd7pcLqtzjYcGJi7w6JINQC0twPWZsVcZJCS/7870UlgoeNMORVQd63esb2di/RbeUKgmcwQIDAQABAoGBAIQZXhRQ57s2zG2Rbesgn+UjdWybUFX6ZfyqgwacSqi/utwmfO76raYXHwBSIDiI192jDSWjvn8Z7tAy6DHhfXIsATZyflQdHJ4WLpdte2qo3TZ0QONVxu5eIBwSEx8zpStG5sgZZ1I6c83CeJ/f3NKApdiHFAjfl43OK4XT4hQ1AkEAxO5ydDAEAVPFzXfqg9M3OIbht+ymenOVUDOgWeRSLZNIs8mtU1qAP0kDymNyAFO+GDbUuta5BPDTvBGU6OD+bwJBAPcTu63JeNThtn28fToWlprKsC1oMTm/ovDK2VJYYR/yh3RzWUhdVBU5ULD/Ue8iZFWxuZhSbdaeI89VO6uyr88CQCCe/waQDZnKrrFic85yZmtOrIUzBkCydMFcS+uYDqTOCPT/K17rVuMkzSPxZSj4tjx8mLB6cRuIaQp2Pxx0aLECQCgnmdndo3idkkYPCx8UqrdPd6B5jX6AKaAOp5EdcfN6PA9t67W6DT9ByF5rsEo4Aax3rN0XkGhP3SwGAyOdb7sCQBjhcKHOCe5fgzQp3lVHWKrH8rjiN6evMgxztxGZc+upWXsRiaHNSJ1diUW8rOKQPCzRtHrpZN99EFslTEAFU0Y=";
            string keyString = @"-----BEGIN CERTIFICATE-----
MIID8DCCAtigAwIBAgIUfOURZvcxwdHsqEAKgSegT9IaSg0wDQYJKoZIhvcNAQEL
BQAwXjELMAkGA1UEBhMCQ04xEzARBgNVBAoTClRlbnBheS5jb20xHTAbBgNVBAsT
FFRlbnBheS5jb20gQ0EgQ2VudGVyMRswGQYDVQQDExJUZW5wYXkuY29tIFJvb3Qg
Q0EwHhcNMTkwODEyMDYxMzIyWhcNMjQwODEwMDYxMzIyWjCBgTETMBEGA1UEAwwK
MTU0ODY5OTM5MTEbMBkGA1UECgwS5b6u5L+h5ZWG5oi357O757ufMS0wKwYDVQQL
DCTmt7HlnLPkub7pvI7nm5vkuJblrp7kuJrmnInpmZDlhazlj7gxCzAJBgNVBAYM
AkNOMREwDwYDVQQHDAhTaGVuWmhlbjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCC
AQoCggEBAPESgb9JFFVfgpjd4tz737saoLs3W0IEcTlHWQ3oft7KdtxYrf/EzYMO
gRGLbNQw1iKPkhR3pYoEGmQP29vGWs3XhJVUeaw+/5abnvzHsU+i+FeAHqsV1dwl
CeXFuSR3kkmbVg/ZyuugH+EXW3aC6bXM4wufBiogJ19C963G7WwGRneVaIu4oi15
b2qIkL+5on28HeOg5cbMP/zrmiSXY2mdan66MfiHG537VV0Q3TIIVM+lQjTSRwMu
DJH+XrnbLc4rN70JrIqC0ngmTD7NqsJ426LWW601nh5gInE9gDDdrW4VLvXxzEqP
nOaUY1SF0jbCpmDYzlo8knhOpmu/aYsCAwEAAaOBgTB/MAkGA1UdEwQCMAAwCwYD
VR0PBAQDAgTwMGUGA1UdHwReMFwwWqBYoFaGVGh0dHA6Ly9ldmNhLml0cnVzLmNv
bS5jbi9wdWJsaWMvaXRydXNjcmw/Q0E9MUJENDIyMEU1MERCQzA0QjA2QUQzOTc1
NDk4NDZDMDFDM0U4RUJEMjANBgkqhkiG9w0BAQsFAAOCAQEADVYyBwZatxC6NHKl
IxlZSEKQWGBXmRCHWfBLHboVzfAgsjzISHoOZWoXcp7Perd1Ggh4nJZ2Mp53WaHS
d/G6T1cZ+oAccOqC8kZqfkbAr0lqkGKcz48mgBP3ZDvbYNoeeYHBZvpZDLbrVoCG
khXrCgR35zYgUAiPaxRVRfY+421ItdrUlZ57SedVUbbqnSPTpSEGAatCWjKx+Ky/
h8+pwXVeGAj0FPfXze2pIvVKAPwm9y80/WhdMDGpyOiFcPfIKocQ4K1Nd11Xw1c2
5XDfOGZL0mYzFvhIiATkne4kRjiP0/N4Z+0MwDc8TNFoZDVpKh2Rj+WKfbobjzY3
ntLd0A==
-----END CERTIFICATE-----";
            string cipher = "F6qrg0+2NQuh1dnfffv1/wkJLV3Q63ddcHbfVv2uu8BItViHVV2ecj6Mz/oTkatZnXQ4J5lw0oxPz3MOq/h9cQYB9s0MTW3ZPJ2DGWfVHABo4/s9gm6RzphhJkgtTgD1DCYVMuvEw9NDOxZJ6939x+7r+d//g45zDddPKcCf69o=";
            var rsa = Javirs.Common.Security.PemCertificate.ReadFromKeyString(keyString);
            string encode = rsa.Encrypt("123111");
            //byte[] tmp = rsa.Decrypt(Convert.FromBase64String(cipher),false);
            //string plain = Encoding.UTF8.GetString(tmp);
            Assert.AreEqual("123111", encode);
        }
        [TestMethod]
        public void ReadPemFileTest()
        {
            string path = @"G:\Jason\certs\15656590759767_pkcs1.pem";
            var rsa = PemCertificate.ReadFromPemFile(path);
            Assert.IsTrue(rsa != null);
        }

        [TestMethod]
        public void ReadFromKeyStringTest()
        {
            //case 1. private key has format declare
            string pvtKeyHasFormat = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQC+ET6Adq3QkUWkuaXGRWfM5XsaLZBJWWy7KNR0VjkNVtZenrw
11yqRJ5zPN2FMDfaPgNEI7KlHD2TBmJO6nBhUYj4vb1qX5Tx0safd7pcLqtzjYc
GJi7w6JINQC0twPWZsVcZJCS/7870UlgoeNMORVQd63esb2di/RbeUKgmcwQIDA
QABAoGBAIQZXhRQ57s2zG2Rbesgn+UjdWybUFX6ZfyqgwacSqi/utwmfO76raYX
HwBSIDiI192jDSWjvn8Z7tAy6DHhfXIsATZyflQdHJ4WLpdte2qo3TZ0QONVxu5
eIBwSEx8zpStG5sgZZ1I6c83CeJ/f3NKApdiHFAjfl43OK4XT4hQ1AkEAxO5ydD
AEAVPFzXfqg9M3OIbht+ymenOVUDOgWeRSLZNIs8mtU1qAP0kDymNyAFO+GDbUu
ta5BPDTvBGU6OD+bwJBAPcTu63JeNThtn28fToWlprKsC1oMTm/ovDK2VJYYR/y
h3RzWUhdVBU5ULD/Ue8iZFWxuZhSbdaeI89VO6uyr88CQCCe/waQDZnKrrFic85
yZmtOrIUzBkCydMFcS+uYDqTOCPT/K17rVuMkzSPxZSj4tjx8mLB6cRuIaQp2Px
x0aLECQCgnmdndo3idkkYPCx8UqrdPd6B5jX6AKaAOp5EdcfN6PA9t67W6DT9By
F5rsEo4Aax3rN0XkGhP3SwGAyOdb7sCQBjhcKHOCe5fgzQp3lVHWKrH8rjiN6ev
MgxztxGZc+upWXsRiaHNSJ1diUW8rOKQPCzRtHrpZN99EFslTEAFU0Y=
-----END RSA PRIVATE KEY-----";
            //case 2. private key has no format declare
            string pvtKeyNoFormat = @"MIICXAIBAAKBgQC+ET6Adq3QkUWkuaXGRWfM5XsaLZBJWWy7KNR0VjkNVtZenrw
11yqRJ5zPN2FMDfaPgNEI7KlHD2TBmJO6nBhUYj4vb1qX5Tx0safd7pcLqtzjYc
GJi7w6JINQC0twPWZsVcZJCS/7870UlgoeNMORVQd63esb2di/RbeUKgmcwQIDA
QABAoGBAIQZXhRQ57s2zG2Rbesgn+UjdWybUFX6ZfyqgwacSqi/utwmfO76raYX
HwBSIDiI192jDSWjvn8Z7tAy6DHhfXIsATZyflQdHJ4WLpdte2qo3TZ0QONVxu5
eIBwSEx8zpStG5sgZZ1I6c83CeJ/f3NKApdiHFAjfl43OK4XT4hQ1AkEAxO5ydD
AEAVPFzXfqg9M3OIbht+ymenOVUDOgWeRSLZNIs8mtU1qAP0kDymNyAFO+GDbUu
ta5BPDTvBGU6OD+bwJBAPcTu63JeNThtn28fToWlprKsC1oMTm/ovDK2VJYYR/y
h3RzWUhdVBU5ULD/Ue8iZFWxuZhSbdaeI89VO6uyr88CQCCe/waQDZnKrrFic85
yZmtOrIUzBkCydMFcS+uYDqTOCPT/K17rVuMkzSPxZSj4tjx8mLB6cRuIaQp2Px
x0aLECQCgnmdndo3idkkYPCx8UqrdPd6B5jX6AKaAOp5EdcfN6PA9t67W6DT9By
F5rsEo4Aax3rN0XkGhP3SwGAyOdb7sCQBjhcKHOCe5fgzQp3lVHWKrH8rjiN6ev
MgxztxGZc+upWXsRiaHNSJ1diUW8rOKQPCzRtHrpZN99EFslTEAFU0Y=";
            //case 3. public key has format declare
            string pubKeyHasFormat = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC+ET6Adq3QkUWkuaXGRWfM5Xsa
LZBJWWy7KNR0VjkNVtZenrw11yqRJ5zPN2FMDfaPgNEI7KlHD2TBmJO6nBhUYj4v
b1qX5Tx0safd7pcLqtzjYcGJi7w6JINQC0twPWZsVcZJCS/7870UlgoeNMORVQd6
3esb2di/RbeUKgmcwQIDAQAB
-----END PUBLIC KEY-----";
            //case 4. public key has no format declare
            string pubKeyNoFormat = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAisxTJJ/39RDLVjRt0VqSioxCfU2MXvHqNJCegx75i5qae2osSkKQ15dXXDNZFxMM0OludFMy6/HlGU/aDCfM2y8G255VT958CiGn8oQEnixDGkryWE1Hb/CKsXwrfykZ3zYIXL7XiQhANl+rA+C23NQBq2Tvrhhjc8rcnHxsCUoSpgtUMrd/yVlQCy0sGlml4Es/pjoT+i5GMnm49pS/1aD/+aIlb4H7N1FBOB1qm1Cfxo6NpRQvWP/Q/ZmwfHcA08Leo4phoNBksPSBE5b4UyuK1SapoIlpr4F8kqLjg0gWw+0nwUXXAWgnPXhM8mSUXLX4Rhk4ahf5ow5xqO3tIwIDAQAB";
            //case 5. pkcs8 key has no format declare
            string pkcs8key = @"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEA8RKBv0kUVV+CmN3i3PvfuxqguzdbQgRxOUdZDeh+3sp23Fit
/8TNgw6BEYts1DDWIo+SFHeligQaZA/b28ZazdeElVR5rD7/lpue/MexT6L4V4Ae
qxXV3CUJ5cW5JHeSSZtWD9nK66Af4RdbdoLptczjC58GKiAnX0L3rcbtbAZGd5Vo
i7iiLXlvaoiQv7mifbwd46Dlxsw//OuaJJdjaZ1qfrox+IcbnftVXRDdMghUz6VC
NNJHAy4Mkf5eudstzis3vQmsioLSeCZMPs2qwnjbotZbrTWeHmAicT2AMN2tbhUu
9fHMSo+c5pRjVIXSNsKmYNjOWjySeE6ma79piwIDAQABAoIBACZnFFQsiBixhlRj
xGf/l7xumXw5bUgu/Ppq6zzT9KH5DsY7OjysVTCzdswcsdF7liEbTeIEzVIXJT0b
aaKxDnYMBwri0h8mSgDr+X+7L/vHslf+COSoLdFL9S+tN7kfcyZWoHcV8sVmVK2+
0ssmP6S+ZszLIGhFhwAd4ubD8b5rUlGFLXq1fNxO0PjAKwquVWlc8P+SwaWgfhyh
oz6UjmOvhcIP67stJ6yf78MMdnnP7IqmFRhRCS7PriLepcjyY67X1ZegN7snYBDF
ikrPFrP+Ru4fQ492r+rvt/dzQ/W5IlV/SYN/i3gdp+gl1e4L2IAEA2QX8y6J7L6I
ypqtlMECgYEA/vLUdyYGFx5znWAVKVDZQH0Iy87v5LL1eeR0XT85JvgxXwG8ZGve
RZHSD7y/6llgLk42ukwywp72f/FQs25Ng71XarNO1ulbdK+Zu/AqXO0WCdIRVPGp
VrZsfs6E9sGaprhUbPqG5hONn2GINNV6pvwBODJAJYRxdMBkmjeA5R8CgYEA8hEG
zbKF08sO6TMHnU9CdwAN5IIPuUU4kfw5QGvx1EWID8PdfT2U9ASFKt/Fb3SmbN31
gEwgXPcgR7W/TujgVXyiQfe8a2ts7WAMyGcgB7nM4AcnEb/O/c2TcgZ9zIAB5ZdO
nKRK8vliHMOuQ5ZYGkOrLghC85zHgSolEQXxohUCgYEAi3Y2Nz87kaX8xJfsu38Y
eKhtCWAX5ljm41jPNrsA/opCJ6CBd8Vx/0h3SliL+xwM+weZ9Gr+UWw3l2FgLecm
y+vlR97yP6nDd58fT6le4xpmNdUN2hOf+GkuDGfYYEsj2BN5S2v4Ix+LFyZ5Jb2p
rHKqEB2c7Wj3z0/3OE0oWy0CgYAJ7LbJsjsYbufimcwDIB/Kfz8Skl6Qqwzht2Sl
hod4u40TkGVBYEf+JT1Mf7Y5lp/IMOWC6Bwfk6ZIKmR4mMWx25VPfRfFk7b4mIqN
oOTITE7hFIn+iU20TDxyXV/FBa6OOkDn3TiRc7YZFdDMRprAIOwgzAfqsw/e9Npm
M0cv7QKBgADqZPqurugoZniWmZHk4m2UPXJso8ljlKrloZ3gclJiLRUWA/86UHVJ
7sjtH0kMF0UZ9yOzhRzoB5JjRreNBYCTKFqUfl7FLhDnDp/8X/YrE8qg7ZWXyMGV
pjg9xFMrBFaSKkx3eOgnhb7ErqJQi2E5XQ3F5IkCQTUbMB1zeGuu
-----END RSA PRIVATE KEY-----";
            string text = "123111哈";
            IRsa case1 = PemCertificate.ReadFromKeyString(pvtKeyHasFormat);
            IRsa case2 = PemCertificate.ReadFromKeyString(pvtKeyNoFormat);
            IRsa case3 = PemCertificate.ReadFromKeyString(pubKeyHasFormat);
            string cipher = case3.Encrypt(text);
            IRsa case4 = PemCertificate.ReadFromKeyString(pubKeyNoFormat);
            string decode1 = case1.Decrypt(cipher);
            bool e = decode1 == text;
            IRsa case5 = PemCertificate.ReadFromKeyString(pkcs8key);
            Assert.IsTrue(case1 != null && case2 != null && case3 != null && case4 != null && e);
        }
    }
}
