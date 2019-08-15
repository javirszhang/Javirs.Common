using System;
using System.Diagnostics;
using System.IO;
using Javirs.Common.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Javirs.Common.Tests
{
    [TestClass]
    public class RsaKeyHelperTest
    {
        [TestMethod]
        public void GenRsaKeyTest()
        {
            var helper = RsaKeyHelper.GenRsaKey(1024);
            Debug.WriteLine("PRIVATE:" + helper.Private);
            Debug.WriteLine("PUBLIC:" + helper.Public);
            Assert.IsTrue(helper != null);
        }
        [TestMethod]
        public void FromPrivateKeyTest()
        {
            string pkcs1 = @"MIICXQIBAAKBgQDCgNqiSv1gxMacJ3e+ioR0VCBVhOdz1ld2OlqZ0W+y6oeV0E6K
oFqeEmIyVUsUpegBA8aXnS0anKnm9UjoaZPL/9pfzRSscAnea46CL6NenG6p9Lyu
n0t3uCYeC+8VdPVtwmyuAxkmAAXo3fWumjU5xPOrUgOAS0ijZxrRRgyHmQIDAQAB
AoGAWDgTcjrrE6IMpzTrhfvTueOSSteeFxcn0lMDVvL8Y70mRBgYF55Fm56g9U1k
YXgic5tfztKWa5SVJ1EngWqpgrg8pY/sHwlXDeId8EA1HGfhmSCPjAUSMlWo98Gr
MLcbGdY63bj9jWnuOIMrkME7HZ9IdszAutNAWleUeTldLeUCQQD/pSwyb0htCHOI
zEyTD0aUQnWYCmaa/++/RZQTMP6U+s6tN9c8YojH2j2F4wlpLodjBAdhyMn7i+s3
MxTJ86KHAkEAwsX1YB6m5AbBVZim6dyqDZ2X/wYBQMKO1l+ref7/LhOj/S5CdO/3
63bTKfHeGmuH19QvCJAmTA7mvuV/rvds3wJBAOrZpGaY0OJJd0ne1SHsUJx3CWyp
cWVHZcpDcyrGQbo/Rore175DjwB6Pza2Qbj846dU1itAuD18ZpOJO7njNhUCQE0b
g4W2/Mj/J6DUWxfwRN45Cohqfyp9G4EgxMj6O1mpat17Z9HVgmeUVgqf9xashU3b
cXdJYe/wYkcmogLJby8CQQD+ULKoLp3MkpQKRTEwwzCf4xXHmvGlQbtZnFZ5SSRX
WB+DJ71AFt08u8htyuvxDvF2ZxjnyEoVXYLvrAHa4Jag";
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
            var helper1 = RsaKeyHelper.FromPemPrivateKey(pkcs1, RsaKeyHelper.KeyFormat.pkcs1);
            var helper8 = RsaKeyHelper.FromPemPrivateKey(pkcs8, RsaKeyHelper.KeyFormat.pkcs8);
            var rsa = PemCertificate.ReadFromKeyString(helper8.Private);
            var pub = PemCertificate.ReadFromKeyString(helper8.Public);

            string plain = "123";
            string cipher = pub.Encrypt(plain);
            string decode = rsa.Decrypt(cipher);
            Assert.AreEqual(plain, decode);
            //Assert.AreEqual(helper1.Private, helper8.Private);
        }

        [TestMethod]
        public void FromXmlKeyTest()
        {
            var helper = RsaKeyHelper.FromXmlKey("<RSAKeyValue><Modulus>woDaokr9YMTGnCd3voqEdFQgVYTnc9ZXdjpamdFvsuqHldBOiqBanhJiMlVLFKXoAQPGl50tGpyp5vVI6GmTy//aX80UrHAJ3muOgi+jXpxuqfS8rp9Ld7gmHgvvFXT1bcJsrgMZJgAF6N31rpo1OcTzq1IDgEtIo2ca0UYMh5k=</Modulus><Exponent>AQAB</Exponent><P>/6UsMm9IbQhziMxMkw9GlEJ1mApmmv/vv0WUEzD+lPrOrTfXPGKIx9o9heMJaS6HYwQHYcjJ+4vrNzMUyfOihw==</P><Q>wsX1YB6m5AbBVZim6dyqDZ2X/wYBQMKO1l+ref7/LhOj/S5CdO/363bTKfHeGmuH19QvCJAmTA7mvuV/rvds3w==</Q><DP>6tmkZpjQ4kl3Sd7VIexQnHcJbKlxZUdlykNzKsZBuj9Git7XvkOPAHo/NrZBuPzjp1TWK0C4PXxmk4k7ueM2FQ==</DP><DQ>TRuDhbb8yP8noNRbF/BE3jkKiGp/Kn0bgSDEyPo7Walq3Xtn0dWCZ5RWCp/3FqyFTdtxd0lh7/BiRyaiAslvLw==</DQ><InverseQ>/lCyqC6dzJKUCkUxMMMwn+MVx5rxpUG7WZxWeUkkV1gfgye9QBbdPLvIbcrr8Q7xdmcY58hKFV2C76wB2uCWoA==</InverseQ><D>WDgTcjrrE6IMpzTrhfvTueOSSteeFxcn0lMDVvL8Y70mRBgYF55Fm56g9U1kYXgic5tfztKWa5SVJ1EngWqpgrg8pY/sHwlXDeId8EA1HGfhmSCPjAUSMlWo98GrMLcbGdY63bj9jWnuOIMrkME7HZ9IdszAutNAWleUeTldLeU=</D></RSAKeyValue>");
            Assert.IsNotNull(helper.Private);
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
    }
}
