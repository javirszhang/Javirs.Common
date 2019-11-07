using System;
using System.Diagnostics;
using System.IO;
using Javirs.Common.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.X509;

namespace Javirs.Common.Tests
{
    [TestClass]
    public class RsaKeyHelperTest
    {
        [TestMethod]
        public void GenRsaKeyTest()
        {
            var helper = RsaKeyHelper.GenRsaKey(2048);
            Debug.WriteLine("PRIVATE:" + helper.ToXmlString(true));
            Debug.WriteLine("PUBLIC:" + helper.ToXmlString(false));
            Assert.IsTrue(helper != null);
        }
        [TestMethod]
        public void FromPrivateKeyTest()
        {
            string pkcs1 = @"MIIEowIBAAKCAQEA8RKBv0kUVV+CmN3i3PvfuxqguzdbQgRxOUdZDeh+3sp23Fit
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
pjg9xFMrBFaSKkx3eOgnhb7ErqJQi2E5XQ3F5IkCQTUbMB1zeGuu";
            string pkcs1_pub = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA8RKBv0kUVV+CmN3i3Pvf
uxqguzdbQgRxOUdZDeh+3sp23Fit/8TNgw6BEYts1DDWIo+SFHeligQaZA/b28Za
zdeElVR5rD7/lpue/MexT6L4V4AeqxXV3CUJ5cW5JHeSSZtWD9nK66Af4RdbdoLp
tczjC58GKiAnX0L3rcbtbAZGd5Voi7iiLXlvaoiQv7mifbwd46Dlxsw//OuaJJdj
aZ1qfrox+IcbnftVXRDdMghUz6VCNNJHAy4Mkf5eudstzis3vQmsioLSeCZMPs2q
wnjbotZbrTWeHmAicT2AMN2tbhUu9fHMSo+c5pRjVIXSNsKmYNjOWjySeE6ma79p
iwIDAQAB
-----END PUBLIC KEY-----";
            string pkcs8 = @"MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAMKA2qJK/WDExpwn
d76KhHRUIFWE53PWV3Y6WpnRb7Lqh5XQToqgWp4SYjJVSxSl6AEDxpedLRqcqeb1
SOhpk8v/2l/NFKxwCd5rjoIvo16cbqn0vK6fS3e4Jh4L7xV09W3CbK4DGSYABejd
9a6aNTnE86tSA4BLSKNnGtFGDIeZAgMBAAECgYBYOBNyOusTogynNOuF+9O545JK
154XFyfSUwNW8vxjvSZEGBgXnkWbnqD1TWRheCJzm1/O0pZrlJUnUSeBaqmCuDyl
j+wfCVcN4h3wQDUcZ+GZII+MBRIyVaj3waswtxsZ1jrduP2Nae44gyuQwTsdn0h2
zMC600BaV5R5OV0t5QJBAP+lLDJvSG0Ic4jMTJMPRpRCdZgKZpr/779FlBMw/pT6
zq031zxiiMfaPYXjCWkuh2MEB2HIyfuL6zczFMnzoocCQQDCxfVgHqbkBsFVmKbp
3KoNnZf/BgFAwo7WX6t5/v8uE6P9LkJ07/frdtMp8d4aa4fX1C8IkCZMDua+5X+u
92zfAkEA6tmkZpjQ4kl3Sd7VIexQnHcJbKlxZUdlykNzKsZBuj9Git7XvkOPAHo/
NrZBuPzjp1TWK0C4PXxmk4k7ueM2FQJATRuDhbb8yP8noNRbF/BE3jkKiGp/Kn0b
gSDEyPo7Walq3Xtn0dWCZ5RWCp/3FqyFTdtxd0lh7/BiRyaiAslvLwJBAP5Qsqgu
ncySlApFMTDDMJ/jFcea8aVBu1mcVnlJJFdYH4MnvUAW3Ty7yG3K6/EO8XZnGOfI
ShVdgu+sAdrglqA=";
            //var helper0 = RsaKeyHelper.FromPrivateKey(File.ReadAllText(@"F:\Jason\certs\gpu\166064_private_pkcs1.pem"), RsaKeyHelper.KeyFormat.pkcs1);
            var helper1 = RsaKeyHelper.FromPemKeyString(pkcs1);

            var h = RsaKeyHelper.FromPemKeyString(pkcs1_pub);
            //var helper8 = RsaKeyHelper.FromPemPrivateKey(pkcs8, RsaKeyHelper.KeyFormat.pkcs8);

            string plain = "123";
            string decode = "456";
            Assert.AreEqual(plain, decode);
            //Assert.AreEqual(helper1.Private, helper8.Private);
        }

        [TestMethod]
        public void FromXmlKeyTest()
        {
            string xmlKey = "<RSAKeyValue><Modulus>woDaokr9YMTGnCd3voqEdFQgVYTnc9ZXdjpamdFvsuqHldBOiqBanhJiMlVLFKXoAQPGl50tGpyp5vVI6GmTy//aX80UrHAJ3muOgi+jXpxuqfS8rp9Ld7gmHgvvFXT1bcJsrgMZJgAF6N31rpo1OcTzq1IDgEtIo2ca0UYMh5k=</Modulus><Exponent>AQAB</Exponent><P>/6UsMm9IbQhziMxMkw9GlEJ1mApmmv/vv0WUEzD+lPrOrTfXPGKIx9o9heMJaS6HYwQHYcjJ+4vrNzMUyfOihw==</P><Q>wsX1YB6m5AbBVZim6dyqDZ2X/wYBQMKO1l+ref7/LhOj/S5CdO/363bTKfHeGmuH19QvCJAmTA7mvuV/rvds3w==</Q><DP>6tmkZpjQ4kl3Sd7VIexQnHcJbKlxZUdlykNzKsZBuj9Git7XvkOPAHo/NrZBuPzjp1TWK0C4PXxmk4k7ueM2FQ==</DP><DQ>TRuDhbb8yP8noNRbF/BE3jkKiGp/Kn0bgSDEyPo7Walq3Xtn0dWCZ5RWCp/3FqyFTdtxd0lh7/BiRyaiAslvLw==</DQ><InverseQ>/lCyqC6dzJKUCkUxMMMwn+MVx5rxpUG7WZxWeUkkV1gfgye9QBbdPLvIbcrr8Q7xdmcY58hKFV2C76wB2uCWoA==</InverseQ><D>WDgTcjrrE6IMpzTrhfvTueOSSteeFxcn0lMDVvL8Y70mRBgYF55Fm56g9U1kYXgic5tfztKWa5SVJ1EngWqpgrg8pY/sHwlXDeId8EA1HGfhmSCPjAUSMlWo98GrMLcbGdY63bj9jWnuOIMrkME7HZ9IdszAutNAWleUeTldLeU=</D></RSAKeyValue>";
            //string xmlKey = "<RSAKeyValue><Modulus>q+NfMvfOxSbRZU2esnFZ17icIts9Yv1teeFD7sSSKzQ8xaCKVRUbYZngsjisQXcFXpd2Z2FFZ+XFaMMb/NVrmPXkSC+b38DzLGYPpLXVNUWibzv0uO0EN4DrP59xxtx3NTGkUP7ADH9gPnZNQna9Wfar/pLKG+sUDKIdDssy6DE=</Modulus><Exponent>AQAB</Exponent><P>zHClmRF/Gr7W4fDDk+GMqKr68/qu+GoS+uxGDSRmC2sBqgrI/G37nNXsGVFzcNxnm0LxFAJZsAgOU7SM/MVh9w==</P><Q>1z0QoW1/8n27uUYq9TesmitLRE2gXMtY8Fa31LafvDbnm5uZwRg7zNszkqzMXqLYHOHmNGt5k1mAjVFOUAr9Fw==</Q><DP>F3u5CeQgnYneVQW68XmvFpDNUskw4AgPNhN92HSd5CHehxHGFHjttg48mIvqnsQygnsmBg5fDwFd4++RYlep8Q==</DP><DQ>sKigI76aC0+Danfa/lVpx0fNiQwlmMQWBX1HeMFDrdlqk19M/R2Ex4kKdVJ6kxoZQutuZIvpxzTovBSFsFC0lQ==</DQ><InverseQ>kxBQQtQ5wVvB+JOAzOuBjBOPBpNegKi+c970APIrRk7JaPzVhbxL2mvHSzF19WnD2mLzzuqB/knLfbZ0reH6vQ==</InverseQ><D>ObVtudvvBMwCk2Na+4gI5N8rNJys4HY7cicKOriuZmj0WlmK+APNHJigEtXqJ1SBbf+mFhFXTwmanJCc7ebtZ3GHtrs3hc1wafPrjEY6muKJjt+X3B/Gox1Mzz2Kxqpq7kF+EKa/duX/1HOR0U87D26cy7kwgIgk6gmQ843kIS0=</D></RSAKeyValue>";
            var helper = RsaKeyHelper.FromXmlKey(xmlKey);
            string pemPub = helper.ToXmlString(true);
            Assert.IsNotNull(pemPub);
        }

        [TestMethod]
        public void ToXmlStringTest()
        {
            string privateKey = "<RSAKeyValue><Modulus>woDaokr9YMTGnCd3voqEdFQgVYTnc9ZXdjpamdFvsuqHldBOiqBanhJiMlVLFKXoAQPGl50tGpyp5vVI6GmTy//aX80UrHAJ3muOgi+jXpxuqfS8rp9Ld7gmHgvvFXT1bcJsrgMZJgAF6N31rpo1OcTzq1IDgEtIo2ca0UYMh5k=</Modulus><Exponent>AQAB</Exponent><P>/6UsMm9IbQhziMxMkw9GlEJ1mApmmv/vv0WUEzD+lPrOrTfXPGKIx9o9heMJaS6HYwQHYcjJ+4vrNzMUyfOihw==</P><Q>wsX1YB6m5AbBVZim6dyqDZ2X/wYBQMKO1l+ref7/LhOj/S5CdO/363bTKfHeGmuH19QvCJAmTA7mvuV/rvds3w==</Q><DP>6tmkZpjQ4kl3Sd7VIexQnHcJbKlxZUdlykNzKsZBuj9Git7XvkOPAHo/NrZBuPzjp1TWK0C4PXxmk4k7ueM2FQ==</DP><DQ>TRuDhbb8yP8noNRbF/BE3jkKiGp/Kn0bgSDEyPo7Walq3Xtn0dWCZ5RWCp/3FqyFTdtxd0lh7/BiRyaiAslvLw==</DQ><InverseQ>/lCyqC6dzJKUCkUxMMMwn+MVx5rxpUG7WZxWeUkkV1gfgye9QBbdPLvIbcrr8Q7xdmcY58hKFV2C76wB2uCWoA==</InverseQ><D>WDgTcjrrE6IMpzTrhfvTueOSSteeFxcn0lMDVvL8Y70mRBgYF55Fm56g9U1kYXgic5tfztKWa5SVJ1EngWqpgrg8pY/sHwlXDeId8EA1HGfhmSCPjAUSMlWo98GrMLcbGdY63bj9jWnuOIMrkME7HZ9IdszAutNAWleUeTldLeU=</D></RSAKeyValue>";
            string publicKey = "<RSAKeyValue><Modulus>woDaokr9YMTGnCd3voqEdFQgVYTnc9ZXdjpamdFvsuqHldBOiqBanhJiMlVLFKXoAQPGl50tGpyp5vVI6GmTy//aX80UrHAJ3muOgi+jXpxuqfS8rp9Ld7gmHgvvFXT1bcJsrgMZJgAF6N31rpo1OcTzq1IDgEtIo2ca0UYMh5k=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            var helper = RsaKeyHelper.FromXmlKey(privateKey);
            string xmlPvt = helper.ToXmlString(true);
            string xmlPub = helper.ToXmlString(false);
            Assert.AreEqual(privateKey, helper.ToXmlString(true));
            Assert.AreEqual(publicKey, xmlPub);
        }

        [TestMethod]
        public void PublicXmlToPemTest()
        {
            string xml = "<RSAKeyValue><Modulus>pNxHZASsv0YJIIfrD5FihVLRTOvEAcQUqcWUu3uYK4+yxxNIXMQBJx0bDllVn/hr15oIh2Kjhe6LcrXM1lZ02DF20jr+eswd3QObO9oQ5g52Gj9TWziBdasfNZPXv3i9sQ0UWI0KMGsbm6Zkl/r8P+/zZXPn/L6H7zsQfR6wu3k=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string pub = RsaKeyHelper.PublicXmlToPem(xml);

            Assert.IsTrue(pub.EndsWith("AQAB"));
        }

        [TestMethod]
        public void ReadPemCertificateTest()
        {
            string pemKey = @"-----BEGIN CERTIFICATE-----
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
            string pkcs1_key = @"MIIEpAIBAAKCAQEAz/+myUn4kfGtsonvGTxoPiCv1v1OVT2csx/qBjtLHTytc5LM
YfZmFPTlZdu/mR44xzkqNqHDgSPXxEezgmhXUD6VtydPsx479glWhyhaYFVFNguE
T/G8sg3Mq/fgmcCnATTxS3sjH6D+oQI9gKhiLZkOuQrRP5X+XzxwkU10lDnD1vPj
h68a9AlvaddbJEF+kGo7lHRAVxirRRy3ZFgesFNHdhZ94Iyk/qqh909dXbzDIAvm
+/PTD2JMFKGLo210EY1/+sVc4bWgFMm95uJPTQU+eoBKmVrVh/fTGz6VBZl9on43
DeYJ/y4C9A63d/hLf7Afm8TDYbruK7pDPUJD8wIDAQABAoIBAQCeZ82mqJ4Uz+EA
AspEBtxAzHX+HmWkx0pqpJh+7HM1LfvO8/KRfKybszJtU2DkhL6rAt8iQin5VMnD
IvQUEdDFPRSaKucou5LgupXaUyIZEXenRDgdBjY1yAqiwSGdFrpZUe+eE4ZZVSv2
hRU/wX0byuAKjrFzYpEaCJaWIk3xf3jwCZAM+eR3nDgWGFrPQxsqwBaGqDv2XaiZ
e1j2OsYDyuaOMYZNnZhlf952Q/TocYPpfp5odjG4l6pscvffPdGA3Uh26sYJDEFF
IXHBxiUON63F8vxqS/6XbyCKEI0prSE/jAESmkyQ1QiLZprE5ZeyrLP4Q12dg8NZ
YmoGEX8ZAoGBAO9JGgOZhUqwB7fUacXLMsXmP9WvMPwRlH/D1yTq35f6qTcuFl2G
vxGldONDKzrCIU7vS7sRLt9N5grHNBfI2dYacwpzUAFoPr/Jv7XR59VroVm+a3zp
xxRFKqsO4l3EU1zxkqgBfISYFP/qNzMYA/zQ35uR5aQuiweZo3VLrxrfAoGBAN6H
E/twseHHb+hkEXRWuWQUuiP2Px0x27gf3V8iUrfYAmfsFgkURzfMU6cw6QQVFAhi
5oxW06a85d/Bjd30NA67FVf6rD/gI0ZbDxWXSNMNuk8ioRKyZBXafeF4cNbCbdZ6
uCbGeX9a1LSPfveIDqCC+poj4THVrl7NRStx+Y1tAoGAFVv3IH8hsMsJxVUGpAtB
Wvz0kcRzoOyIzubSW49DqjNLy4snn+2ZPKACUQ772uDdTEh8ABTGlFRFxoyFj+Hz
3/K4diRY7ec6QrBllntIMHrNCk+/FIVhqeOKrX5Eoo4VyuQdbTXEwak5pqZniv4H
zRdv/lkFoKQu8Ny8BIpe9Q0CgYAzcAvpakJkC0LN+Bukxcsy3Cuu1mP+bqD4bb0x
GXD/eBoORZ9OV/aiakPH+OFUBT9NNPdP514jQ7TrRQVzEdjastobLSGV3kypHbFd
70txJdI0KrAK71t9RbUlYj7LFcRn+JQKuoQpMSjL2AOkWjnvVndxG+wZJeiZl19u
zHi//QKBgQDGg4yHwHX35Q+PcLQLjs4FekSfUtkEalyEAy4AsiPFTSXqJggcn0tZ
4YPjO+oLccab6hTd9CT17YLiFAKn/IjzMctaWCqlZHfMn7DKbwpAXA4CfkIYECCP
9IjKw6JEBXwXZ7kUJapQ6UJXDU2IvmyfuFwKp7swkdTAUbg7VZEk9A==";
            string pkcs8_key = @"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDxEoG/SRRVX4KY
3eLc+9+7GqC7N1tCBHE5R1kN6H7eynbcWK3/xM2DDoERi2zUMNYij5IUd6WKBBpk
D9vbxlrN14SVVHmsPv+Wm578x7FPovhXgB6rFdXcJQnlxbkkd5JJm1YP2crroB/h
F1t2gum1zOMLnwYqICdfQvetxu1sBkZ3lWiLuKIteW9qiJC/uaJ9vB3joOXGzD/8
65okl2NpnWp+ujH4hxud+1VdEN0yCFTPpUI00kcDLgyR/l652y3OKze9CayKgtJ4
Jkw+zarCeNui1lutNZ4eYCJxPYAw3a1uFS718cxKj5zmlGNUhdI2wqZg2M5aPJJ4
TqZrv2mLAgMBAAECggEAJmcUVCyIGLGGVGPEZ/+XvG6ZfDltSC78+mrrPNP0ofkO
xjs6PKxVMLN2zByx0XuWIRtN4gTNUhclPRtporEOdgwHCuLSHyZKAOv5f7sv+8ey
V/4I5Kgt0Uv1L603uR9zJlagdxXyxWZUrb7SyyY/pL5mzMsgaEWHAB3i5sPxvmtS
UYUterV83E7Q+MArCq5VaVzw/5LBpaB+HKGjPpSOY6+Fwg/ruy0nrJ/vwwx2ec/s
iqYVGFEJLs+uIt6lyPJjrtfVl6A3uydgEMWKSs8Ws/5G7h9Dj3av6u+393ND9bki
VX9Jg3+LeB2n6CXV7gvYgAQDZBfzLonsvojKmq2UwQKBgQD+8tR3JgYXHnOdYBUp
UNlAfQjLzu/ksvV55HRdPzkm+DFfAbxka95FkdIPvL/qWWAuTja6TDLCnvZ/8VCz
bk2DvVdqs07W6Vt0r5m78Cpc7RYJ0hFU8alWtmx+zoT2wZqmuFRs+obmE42fYYg0
1Xqm/AE4MkAlhHF0wGSaN4DlHwKBgQDyEQbNsoXTyw7pMwedT0J3AA3kgg+5RTiR
/DlAa/HURYgPw919PZT0BIUq38VvdKZs3fWATCBc9yBHtb9O6OBVfKJB97xra2zt
YAzIZyAHuczgBycRv879zZNyBn3MgAHll06cpEry+WIcw65DllgaQ6suCELznMeB
KiURBfGiFQKBgQCLdjY3PzuRpfzEl+y7fxh4qG0JYBfmWObjWM82uwD+ikInoIF3
xXH/SHdKWIv7HAz7B5n0av5RbDeXYWAt5ybL6+VH3vI/qcN3nx9PqV7jGmY11Q3a
E5/4aS4MZ9hgSyPYE3lLa/gjH4sXJnklvamscqoQHZztaPfPT/c4TShbLQKBgAns
tsmyOxhu5+KZzAMgH8p/PxKSXpCrDOG3ZKWGh3i7jROQZUFgR/4lPUx/tjmWn8gw
5YLoHB+TpkgqZHiYxbHblU99F8WTtviYio2g5MhMTuEUif6JTbRMPHJdX8UFro46
QOfdOJFzthkV0MxGmsAg7CDMB+qzD9702mYzRy/tAoGAAOpk+q6u6ChmeJaZkeTi
bZQ9cmyjyWOUquWhneByUmItFRYD/zpQdUnuyO0fSQwXRRn3I7OFHOgHkmNGt40F
gJMoWpR+XsUuEOcOn/xf9isTyqDtlZfIwZWmOD3EUysEVpIqTHd46CeFvsSuolCL
YTldDcXkiQJBNRswHXN4a64=";
            string public_key = @"-----BEGIN CERTIFICATE-----
MIID3DCCAsSgAwIBAgIUGL/XGN8xPHGDe57WT0U005JRZMMwDQYJKoZIhvcNAQEL
BQAwXjELMAkGA1UEBhMCQ04xEzARBgNVBAoTClRlbnBheS5jb20xHTAbBgNVBAsT
FFRlbnBheS5jb20gQ0EgQ2VudGVyMRswGQYDVQQDExJUZW5wYXkuY29tIFJvb3Qg
Q0EwHhcNMTkwODEyMDYxMzIyWhcNMjQwODEwMDYxMzIyWjBuMRgwFgYDVQQDDA9U
ZW5wYXkuY29tIHNpZ24xEzARBgNVBAoMClRlbnBheS5jb20xHTAbBgNVBAsMFFRl
bnBheS5jb20gQ0EgQ2VudGVyMQswCQYDVQQGDAJDTjERMA8GA1UEBwwIU2hlblpo
ZW4wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDDmb7jBAvG3bN7a4/d
6mM6wKefEjcO+Bl85EgdxoXCDkN+0mSxyGW+03+wePYGqs24U6Ab1m1VqrfnCWuC
guKEJSAk8mtRRki4gfs4OBhePVYGSEFkzKdtbF0ucw10pPElHG5Yrvd+L3hwbMnj
D4m0MfXzURcxR7CZ/LUmizK/lkMGvt734iATcb3vvZpYpTRW34mQXXpnzifV5RXg
h0wwMn28cpUqy9EwkgWEEdgRb/s0REeQFEPqFsPjnNffe8/aTETQJb+0W8cRK2IH
GINuhP3f8RSbGWmwKzI14b3xZEqBQlPFFDrvELvjtdo6OEHGq0k7JJsiyp0hmWIL
dWplAgMBAAGjgYEwfzAJBgNVHRMEAjAAMAsGA1UdDwQEAwIE8DBlBgNVHR8EXjBc
MFqgWKBWhlRodHRwOi8vZXZjYS5pdHJ1cy5jb20uY24vcHVibGljL2l0cnVzY3Js
P0NBPTFCRDQyMjBFNTBEQkMwNEIwNkFEMzk3NTQ5ODQ2QzAxQzNFOEVCRDIwDQYJ
KoZIhvcNAQELBQADggEBAFGyHl1bYh3qdh5Ssb6SSrVlLjzjoP3iPYckM6jFv78O
i24k+IsI8wAyM1dY3WkmI8bn8n47QjMqow6TxxtvvbEYzX0vAb6elev4c66diknb
P9jR+zu5H0NKR1HnoFrzgACG9Lf38P+1P2FNWbetCMUAjAAcw/7rlHbqYfOYz+YW
nLBkx4T4aJQduXLXpzVQx1xz6yZzq21oDqVZQq040XCoqVCGyfR4a+owV3cphOUJ
7EiV0Kkq9nN1vmEYSamINKx4h13pA5uYPxBTnZIXKcW/85IV2JF0Gv6XtUFmRlZo
eSBP4f3F+9aPs4tu2eOVxTLsWwia8ydXikFdK1zDubA=
-----END CERTIFICATE-----";
            //string formatKey = RsaKeyHelper.FormatKeyString(public_key);
            
            var helper = RsaKeyHelper.FromPemKeyString(public_key);
            var n = helper.RSACryptoServiceProvider();
            string pubKeyString = helper.GetKeyString();
            /*
            var rsaService = helper.RSACryptoServiceProvider();
            object pemObject;
            using (StringReader sReader = new StringReader(pemKey))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sReader);
                pemObject = pemReader.ReadObject();
            }
            X509Certificate rsaKey = pemObject as X509Certificate;
            var cer = new System.Security.Cryptography.X509Certificates.X509Certificate2();
            cer.Import(rsaKey.GetEncoded());
            var provider = new System.Security.Cryptography.RSACryptoServiceProvider();
            provider.FromXmlString(cer.PublicKey.Key.ToXmlString(false));
            Assert.IsTrue(rsaKey != null);
            */
        }

        [TestMethod]
        public void GetPublicKeyLengthTest()
        {
            string pub1024 = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC+ET6Adq3QkUWkuaXGRWfM5XsaLZBJWWy7KNR0VjkNVtZenrw11yqRJ5zPN2FMDfaPgNEI7KlHD2TBmJO6nBhUYj4vb1qX5Tx0safd7pcLqtzjYcGJi7w6JINQC0twPWZsVcZJCS/7870UlgoeNMORVQd63esb2di/RbeUKgmcwQIDAQAB";
            string pub2048 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAz3aS2cDTwKPq94r0IpFsqyLeS6xilUfbiBADJkZPEQyuTVcgKm5gvbKwR3Mo3AMxgnoqMrRfLCxubcLN0xWobEfwLfQTCcO+VCDvxupjqDYYhX79qd1QSZ5TueOei47KHdQRBRLvLfKDZG1u86QCWuwvUpsaMGFIwTV22bE6u0WjhY7qgC0mdl7Pt2MDk49Ldyq+h86l3+cQVqNoTUjK8WXMjZ48sFQChU04Jz+SokmBWQw824xHYf3y5nTyYmhkHjvYJQ4zGHMZah1JECEmiArunyDQm6ZM89hESMRSp/FHYOiEZerFQnSeJ6vKMopzrmC9F19zCouYiGmaFG+YzQIDAQAB";
            string pvt2048 = @"MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDxEoG/SRRVX4KY
3eLc+9+7GqC7N1tCBHE5R1kN6H7eynbcWK3/xM2DDoERi2zUMNYij5IUd6WKBBpk
D9vbxlrN14SVVHmsPv+Wm578x7FPovhXgB6rFdXcJQnlxbkkd5JJm1YP2crroB/h
F1t2gum1zOMLnwYqICdfQvetxu1sBkZ3lWiLuKIteW9qiJC/uaJ9vB3joOXGzD/8
65okl2NpnWp+ujH4hxud+1VdEN0yCFTPpUI00kcDLgyR/l652y3OKze9CayKgtJ4
Jkw+zarCeNui1lutNZ4eYCJxPYAw3a1uFS718cxKj5zmlGNUhdI2wqZg2M5aPJJ4
TqZrv2mLAgMBAAECggEAJmcUVCyIGLGGVGPEZ/+XvG6ZfDltSC78+mrrPNP0ofkO
xjs6PKxVMLN2zByx0XuWIRtN4gTNUhclPRtporEOdgwHCuLSHyZKAOv5f7sv+8ey
V/4I5Kgt0Uv1L603uR9zJlagdxXyxWZUrb7SyyY/pL5mzMsgaEWHAB3i5sPxvmtS
UYUterV83E7Q+MArCq5VaVzw/5LBpaB+HKGjPpSOY6+Fwg/ruy0nrJ/vwwx2ec/s
iqYVGFEJLs+uIt6lyPJjrtfVl6A3uydgEMWKSs8Ws/5G7h9Dj3av6u+393ND9bki
VX9Jg3+LeB2n6CXV7gvYgAQDZBfzLonsvojKmq2UwQKBgQD+8tR3JgYXHnOdYBUp
UNlAfQjLzu/ksvV55HRdPzkm+DFfAbxka95FkdIPvL/qWWAuTja6TDLCnvZ/8VCz
bk2DvVdqs07W6Vt0r5m78Cpc7RYJ0hFU8alWtmx+zoT2wZqmuFRs+obmE42fYYg0
1Xqm/AE4MkAlhHF0wGSaN4DlHwKBgQDyEQbNsoXTyw7pMwedT0J3AA3kgg+5RTiR
/DlAa/HURYgPw919PZT0BIUq38VvdKZs3fWATCBc9yBHtb9O6OBVfKJB97xra2zt
YAzIZyAHuczgBycRv879zZNyBn3MgAHll06cpEry+WIcw65DllgaQ6suCELznMeB
KiURBfGiFQKBgQCLdjY3PzuRpfzEl+y7fxh4qG0JYBfmWObjWM82uwD+ikInoIF3
xXH/SHdKWIv7HAz7B5n0av5RbDeXYWAt5ybL6+VH3vI/qcN3nx9PqV7jGmY11Q3a
E5/4aS4MZ9hgSyPYE3lLa/gjH4sXJnklvamscqoQHZztaPfPT/c4TShbLQKBgAns
tsmyOxhu5+KZzAMgH8p/PxKSXpCrDOG3ZKWGh3i7jROQZUFgR/4lPUx/tjmWn8gw
5YLoHB+TpkgqZHiYxbHblU99F8WTtviYio2g5MhMTuEUif6JTbRMPHJdX8UFro46
QOfdOJFzthkV0MxGmsAg7CDMB+qzD9702mYzRy/tAoGAAOpk+q6u6ChmeJaZkeTi
bZQ9cmyjyWOUquWhneByUmItFRYD/zpQdUnuyO0fSQwXRRn3I7OFHOgHkmNGt40F
gJMoWpR+XsUuEOcOn/xf9isTyqDtlZfIwZWmOD3EUysEVpIqTHd46CeFvsSuolCL
YTldDcXkiQJBNRswHXN4a64=";
            //pub1024 = RsaKeyHelper.FormatPublicKey(pub1024);
            //pub2048 = RsaKeyHelper.FormatPublicKey(pub2048);
            int len = Convert.FromBase64String(pub1024).Length;
            int len2048 = Convert.FromBase64String(pub2048).Length;
            Assert.IsTrue(len2048 > len);
        }
    }
}
