using Javirs.Common.Security;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Text;

namespace Javirs.Common.CoreUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
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
            Assert.Pass();
        }
        [Test]
        public void PrivateEncryptTest()
        {
            string privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICWwIBAAKBgQCWqgFvHUkg87sGnrWZ8KLfTWhZLt+wC7ewn3HB6K7FWVsA6NzE
/7JS6UEOhEwEko2SAS3E/gOUcu8icKk+Z9xYzPD/m/HqcpMWZR0v8Z2/C2rYJbtW
at/tYIo/Avr1acKWIDrVlaJNRitFPwCLp/3lGe+LM++yDAPSgxorxsoWHQIDAQAB
AoGAbVWL2t2uvvoApCoycTceitv6uJV6gJ/QOp1KiURY6CATGVLjOxavitl4e5do
Lv1HPUgbtdP8NxM9FIobV405ah6radDb0p56sk3+SLRN+zW08TQCzB8W29nLLdg8
Ehbp03wktPx9xsUw3mGqvXMMuid+kqF6RbbvsDVw0s4L7cUCQQDDLx/DoVYBJjyC
09NveGO1zqM3b3WqudfbPnx0JXcwMsBGIivRDzTXn/2XSNJxjuRVYJLjzpVkAp4X
CfHcro1rAkEAxZu9eBKNKfYsLXsfGS7p8TnLMm6FVda8mVHa8fP8HLipYohnW/5i
PaNTvfQMOiQygkrdw8uSsb1wqQM0NZ0ElwJATIE0GiIq69ho737H6XFu49xS7W+q
13hwu2cKsRveU+4Qn2zVb+Rd+gakB6BrnEc+CJkJ+nEG7WR5Qt6LBs0EcQJAfhWZ
Fvv/Rj7kgUCF3zOn4VrW5B7QxKx3OfyYjrj2q/zHVy9hg7kURe/ohtMo2hAuiiPq
y/sgZnhRYgXBUlQVkQJAcDdDGN+SScKh7BANlDmdDqgOhRl34kbwslGs372xh7Fn
90kOt7X2IZPxYnmW5OWKLGP2V1totk0X0wv7hR81HA==
-----END RSA PRIVATE KEY-----
";
            string plain = "sdasdfasdfasdf";
            byte[] buffer = Encoding.UTF8.GetBytes(plain);
            var rsa = PemCertificate.ReadFromKeyString(privateKey);
            var res = rsa.EncryptByPrivate(buffer);
            string base64 = Convert.ToBase64String(res);
            Debug.WriteLine(base64);
        }
        [Test]
        public void PublicEncryptTest()
        {
            string publicKey = @"MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAJcoaZUeOHeS0+Fkw0jIj1QiNUxOSKgSYszQJX2aq5txZPnsAPmo6tmodjLJdzDu+7srQ7nBo07FVXZwUBCHjz0CAwEAAQ==";
            //string cipher = "MzVsWMoZcwyZuSg2byHaJubMU8BDaB9RGz8wYpZEjXjCTdNCdcmJbMGE+mCsVJWqsgHStidEL71CuNMeDnE38gGPoUHQpuEUW0cV0fPcuAlyiApJ48f3dakZ4cJrXZ4db+NeI3hstbQ8fMh3NXFZtWJqJXQYfbNYoe5CCMegkpY=";
            string cipher = "Cx4OcHmVO2kiIbecibjEq4naWPC+vBfUpmYFw2Al1n/pRBzjWi6NPSL3FI1FkyD68+7s1b3F0uHVbc2QtIhdTt8Eh0lvcUnwohpE6Ku961wk1K6LTiLV9DyxPlV0O2Slmzx1jqSIS4S2HcSXa28mv2TTtpV9bH29b55mBlt/rAA=";
            byte[] buffer = Convert.FromBase64String(cipher);
            var rsa = PemCertificate.ReadFromKeyString(publicKey);
            byte[] res = rsa.Decrypt(buffer, false, true);
            //byte[] res = rsa.DecryptByPublic(buffer);
            string s = Encoding.UTF8.GetString(res);
            Assert.AreEqual("sdasdfasdfasdf", s);
        }

        [Test]
        public void Pkcs1AndPkcs8Test()
        {
            string pkcs1 = @"MIICXAIBAAKBgQDBHHARgZjkBrwfN4yx7+HeF9Xj0tfHjR1U5veoZjV1AfErlAoW
hvQ1asQlWt9Kid7K0dyy1tdYwoHUhK6DCWKwGJrluaVgat9Bt5DkvrkE58f+8Uun
EfAFN/pbpDHjGITjNwc47tGmNbt5iVf27+B0WXNPv/SJHZ342/oC23qOCQIDAQAB
AoGASsCQXSZ2oaY9wjPFoceC/m7pnBQs9REaDpiNu8E85jtHOSBIO2ooNFlM7kzb
7MWr2YUdUpk5qSE6EqL9zrmeWnH+3KD/l6qhaLkHq6uVT/9UJgXbuzq4L/dAWWQP
YLcm0AbLEXBLSRc9iq3/6Fe+DPflOC2wBORpKG0ulQYECaUCQQDTQfKaVLsvsx8D
IKBPt3nXkAljMGrBFIl1XhJCy1v4e2Jhz2CbtpgRE72h8SkxSMPom7s5uPPdbI9S
SdXB/ZJ7AkEA6gKdSgnPQhWCAqun/UpDeyRWsQi1IarZBG5n/htaCwWv4aESfF7s
foo6gsxEC6TUqHt34HRzSj5CgyYrN3KsSwJAaTsPYorUVxVNXFxzHmJRYewQkQT8
GENnmTwLspPjsymavwfyon3Yz6DatAERuMf7NjHkmAMmDmTWG3JF9QSfYwJAO3kO
oJJ9qj3tHOCjgSwumk1R+wxLfJL/NLEanOo2qDZ5zM4y9Ijrcf2PgU6IvKzXnzpB
7TmrU2MfZFV/BkiOoQJBAK4XWqA6dyJ3fkz2siolohzNQY3NE2GwYI57eNH0qGbf
+BOWG8oKAsgeG8hSwDsR1tt2DQJA20HwzgSFUPyb3GA=";
            string pkcs8 = @"MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAKJYfqECuvkShdI0
1l7vnmkRifV6d0SQ8zFLd3Qpb6AgSF3kXNEsXWEfYYLHRV12QSaIxl80hmAHDYXW
m2R9GKOanMOkBOXOaDKLJFpxNor4gtGsx6HN6WkxX064w5Is4XYZAM4qN64WzaOu
KgGKXwZNSEmY4xSTJz1f04eC1R1dAgMBAAECgYAbdjjFZ77VDysP6dwxZvs27r6V
hcfTE2nv9sIYJkI5pfxX1Z9Vytlo6nQGOUskijJvIEfeJvZsHAVPlIPotbiK/03h
nw5ZgpS6XdPWXhdFo6UW0ZHumBtdEEImppaWaGb+5uzuvKoWn3hiqjC8C55TEvno
TgIkJSNBgmbRRnXCSQJBAMxFpzPKhlJ3qw29BeCAxtOIOosAKiPI5bKtUuZEXAkC
VR7KWKgM68JuS5m9AqhZVec13XCZbWDfw3aT/jmO9QMCQQDLdODuZ4c6NrUnQ9sx
w2WVd5nLoMLaET7rekOrBuDQkZnvo+x9HSayy9P6qzNXF1DEW5hblpe9xRUw32NL
DSYfAkA5TdzvAYSXA+0fiIRqi5W7Z78MmXo42bXeXxMfd9PdzyKz5Y3jbuSAgdKB
iW2CbrVTjF/Xo6L0hzFdRX0PBpSjAkA33I1tp8s/Nooij8T9MtMPaNk3SZp2WUnX
SaiOqsLLQU7stytVZs2bRc+cAamE/gfVAfkhHIXpOtWIp60RjDitAkBTLd1buw7F
A/0Nhw1bTWYhv644nvtaL9Ctpr3awpwT3rG4JWpTul++grHFB8VJmXZzyv57ToJM
FvVNjmzv1yHF";
            byte[] cs1 = Convert.FromBase64String(pkcs1);
            byte[] cs2 = Convert.FromBase64String(pkcs8);
            byte s1 = cs1[7];
            byte s2 = cs2[7];

        }
    }
}