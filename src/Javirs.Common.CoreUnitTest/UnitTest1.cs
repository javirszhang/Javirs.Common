using Javirs.Common.Security;
using NUnit.Framework;

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
    }
}