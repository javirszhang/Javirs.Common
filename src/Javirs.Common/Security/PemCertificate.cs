using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Javirs.Common.Security
{
    /// <summary>
    /// PEM证书
    /// </summary>
    public class PemCertificate : RSAServiceProvider, IRsa
    {
        const string PEM_PRIVATE_KEY_HEADER = "-----BEGIN RSA PRIVATE KEY-----";
        const string PEM_PRIVATE_KEY_FOOTER = "-----END RSA PRIVATE KEY-----";
        const string PEM_PUBLIC_KEY_HEADER = "-----BEGIN PUBLIC KEY-----";
        const string PEM_PUBLIC_KEY_FOOTER = "-----END PUBLIC KEY-----";
        /// <summary>
        /// 默认构造
        /// </summary>
        protected PemCertificate() { }
        /// <summary>
        /// 从PEM密钥文件初始化PEM证书
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static PemCertificate ReadFromPemFile(string fullpath)
        {
            PemCertificate pemcert = new PemCertificate();
            using (System.IO.FileStream fs = System.IO.File.OpenRead(fullpath))
            {
                byte[] data = new byte[fs.Length];
                byte[] res = null;
                fs.Read(data, 0, data.Length);
                string pem = Encoding.UTF8.GetString(data);
                string type = pem.StartsWith(PEM_PRIVATE_KEY_HEADER) ? "RSA PRIVATE KEY" : pem.StartsWith(PEM_PUBLIC_KEY_HEADER) ? "PUBLIC KEY" : "PLAIN TEXT";
                if (type == "PLAIN TEXT")
                {
                    res = Convert.FromBase64String(Encoding.UTF8.GetString(data));
                }
                else
                {
                    res = GetPem(type, data);
                }

                if (type.Equals("RSA PRIVATE KEY"))//私钥
                {
                    pemcert._provider = DecodeRSAPrivateKey(res);
                }
                else if (type.Equals("PUBLIC KEY"))
                {
                    pemcert._provider = DecodeX509PublicKey(res);
                }
                else if (type.Equals("PLAIN TEXT"))
                {
                    if (res.Length == 608 || res.Length == 1193) //PKCS#1 PRIVATE KEY,keysize=1024,bytes=608
                    {
                        pemcert._provider = DecodeRSAPrivateKey(res);
                    }
                    else if (res.Length == 634 || res.Length == 635 || res.Length == 1217 || res.Length == 1218)//ASN.1 PRIVATE KEY
                    {
                        pemcert._provider = DecodeASN1PrivateKey(res);
                    }
                    else
                    {
                        pemcert._provider = DecodeX509PublicKey(res);
                    }
                }
                return pemcert;
            }
        }

        public static PemCertificate ReadFromKeyString(string pem)
        {
            PemCertificate pemcert = new PemCertificate();
            byte[] res = null;
            string type = pem.StartsWith(PEM_PRIVATE_KEY_HEADER) ? "RSA PRIVATE KEY" : pem.StartsWith(PEM_PUBLIC_KEY_HEADER) ? "PUBLIC KEY" : "PLAIN TEXT";
            if (type == "PLAIN TEXT")
            {
                res = Convert.FromBase64String(pem);
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(pem);
                res = GetPem(type, data);
            }

            if (type.Equals("RSA PRIVATE KEY"))//私钥
            {
                pemcert._provider = DecodeRSAPrivateKey(res);
            }
            else if (type.Equals("PUBLIC KEY"))
            {
                pemcert._provider = DecodeX509PublicKey(res);
            }
            else if (type.Equals("PLAIN TEXT"))
            {
                if (res.Length == 608 || res.Length == 611 || res.Length == 1193) //PKCS#1 PRIVATE KEY,keysize=1024,bytes=608
                {
                    pemcert._provider = DecodeRSAPrivateKey(res);
                }
                else if (res.Length == 634 || res.Length == 635 || res.Length == 1217 || res.Length == 1218)//ASN.1 PRIVATE KEY
                {
                    pemcert._provider = DecodeASN1PrivateKey(res);
                }
                else
                {
                    pemcert._provider = DecodeX509PublicKey(res);
                    if (pemcert._provider == null)
                    {
                        pemcert._provider = DecodeRSAPrivateKey(res);
                        if (pemcert._provider == null)
                        {
                            pemcert._provider = DecodeASN1PrivateKey(res);
                        }
                    }
                }
            }
            return pemcert;
        }
        private static byte[] GetPem(string type, byte[] data)
        {
            string pem = Encoding.UTF8.GetString(data);
            string header = String.Format("-----BEGIN {0}-----\\n", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);
            string base64 = pem.Substring(start, (end - start));
            return Convert.FromBase64String(base64);
        }

        private static RSACryptoServiceProvider DecodeASN1PrivateKey(byte[] asnPrvtKey)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            MemoryStream mem = new MemoryStream(asnPrvtKey);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)	//expect an Octet string 
                    return null;

                bt = binr.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                return rsacsp;
            }

            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }
        }
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            byte tempbyte = 0;
            short tempshort = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    tempbyte = binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    tempshort = binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);
                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }
        private static RSACryptoServiceProvider DecodeX509PublicKey(byte[] x509key)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(x509key);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            byte tempByte = 0;
            short tempShort = 0;
            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    tempByte = binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    tempShort = binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                    tempByte = binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8203)
                    tempShort = binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x00)		//expect null byte next
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    tempByte = binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    tempShort = binr.ReadInt16();	//advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102)	//data read as little endian order (actual data order for Integer is 02 81)
                    lowbyte = binr.ReadByte();	// read next bytes which is bytes in modulus
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte();	//advance 2 bytes
                    lowbyte = binr.ReadByte();
                }
                else
                    return null;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modsize = BitConverter.ToInt32(modint, 0);

                byte firstbyte = binr.ReadByte();
                binr.BaseStream.Seek(-1, SeekOrigin.Current);

                if (firstbyte == 0x00)
                {	//if first byte (highest order) of modulus is zero, don't include it
                    binr.ReadByte();	//skip this null byte
                    modsize -= 1;	//reduce modulus buffer size by 1
                }

                byte[] modulus = binr.ReadBytes(modsize);	//read the modulus bytes

                if (binr.ReadByte() != 0x02)			//expect an Integer for the exponent data
                    return null;
                int expbytes = (int)binr.ReadByte();		// should only need one byte for actual exponent data (for all useful values)
                byte[] exponent = binr.ReadBytes(expbytes);
                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo.Modulus = modulus;
                RSAKeyInfo.Exponent = exponent;
                RSA.ImportParameters(RSAKeyInfo);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }

        }
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        public static string ToPemString(RSAParameters rsaParams, bool includePrivate)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            
            if (includePrivate)
            {
                bw.Write((ushort)0x8230);
                bw.Write((short)0x5F02);
                bw.Write((ushort)0x0102);
                bw.Write((byte)0x00);
                WriteParameter(bw, rsaParams.Modulus);
                WriteParameter(bw, rsaParams.Exponent);
                WriteParameter(bw, rsaParams.D);
                WriteParameter(bw, rsaParams.P);
                WriteParameter(bw, rsaParams.Q);
                WriteParameter(bw, rsaParams.DP);
                WriteParameter(bw, rsaParams.DQ);
                WriteParameter(bw, rsaParams.InverseQ);
            }
            else
            {
                bw.Write((ushort)0x8130);
                bw.Write((byte)0x9F);
                byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
                bw.Write(SeqOID);
                bw.Write((ushort)0x8103);
                bw.Write((byte)0x8d);
                bw.Write((byte)0x00);
                bw.Write((ushort)0x8130);
                bw.Write((byte)0x89);
                ushort flag = rsaParams.Modulus.Length > byte.MaxValue ? (ushort)0x8202 : (ushort)0x8102;
                bw.Write(flag);
                if (flag == 0x8102)
                {
                    byte len = (byte)(rsaParams.Modulus.Length + 1);
                    bw.Write(len);
                    bw.Write((byte)0x00);
                }
                else
                {
                    bw.Write((ushort)rsaParams.Modulus.Length);
                }

                bw.Write(rsaParams.Modulus);
                bw.Write((byte)0x02);
                bw.Write((byte)rsaParams.Exponent.Length);
                bw.Write(rsaParams.Exponent);
            }
            bw.Flush();
            bw.Close();
            byte[] buffer = ms.ToArray();
            ms.Close();
            return Convert.ToBase64String(buffer);
        }

        private static void WriteParameter(BinaryWriter bw, byte[] buffer)
        {
            if (buffer == null || buffer.Length <= 0)
            {
                return;
            }
            int len = buffer.Length;
            bw.Write((byte)0x02);
            if (len > byte.MaxValue)
            {
                bw.Write((byte)0x82);
                bw.Write((ushort)len);
            }
            else
            {
                bw.Write((byte)0x81);
                byte bytelen = (byte)(len + 1);
                bw.Write(bytelen);
                bw.Write((byte)0x00);
            }
            bw.Write(buffer);
        }
    }
}
