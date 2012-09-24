using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.Security;

namespace BetEx247.Core.Common.Utils
{
    public class Crypter
    {
        #region Member Variables
        private static Crypter m_instance = null;
        private static string m_monitor = "lock";
        #endregion

        #region Constructors
        private Crypter()
        {
        }
        public static Crypter Instance()
        {
            if (m_instance == null)
            {
                lock (m_monitor)
                {
                    if (m_instance == null)
                    {
                        m_instance = new Crypter();
                    }
                }
            }

            return m_instance;
        }
        #endregion

        #region Public Static Methods
        public static string EncryptQTEK(string pvInput, string pvKey)
        {
            return Instance().Encrypt("QTEK", pvInput, pvKey);
        }
        public static string DecryptQTEK(string pvInput, string pvKey)
        {
            return Instance().Decrypt("QTEK", pvInput, pvKey);
        }

        public static string EncryptHQ(string pvInput)
        {
            return Instance().Encrypt("HQ", pvInput, null);
        }
        public static string DecryptHQ(string pvInput)
        {
            return Instance().Decrypt("HQ", pvInput, null);
        }

        public static string EncryptRSA(string pvInput)
        {
            return Instance().Encrypt("RSA", pvInput, null);
        }
        public static string DecryptRSA(string pvInput)
        {
            return Instance().Decrypt("RSA", pvInput, null);
        }

        public static string EncryptDES(string pvInput)
        {
            return Instance().Encrypt("DES", pvInput, null);
        }
        public static string DecryptDES(string pvInput)
        {
            return Instance().Decrypt("DES", pvInput, null);
        }

        public static string EncryptRC2(string pvInput)
        {
            return Instance().Encrypt("RC2", pvInput, null);
        }
        public static string DecryptRC2(string pvInput)
        {
            return Instance().Decrypt("RC2", pvInput, null);
        }

        public static string GetHash(string texts, string key)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(key + texts, "SHA1");
        }

        public static string GetHashSHA1(string input)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] data = Encoding.Default.GetBytes(input);
            byte[] hash = sha1.ComputeHash(data);
            //transform as hexa
            string hexHash = string.Empty;
            foreach (byte b in hash)
            {
                hexHash += string.Format("{0:x2}", b);
            }
            return hexHash.ToUpper();
        }
        #endregion

        #region Internal Methods
        //pvMode: QTEK=Phuc1Method; HQ=Phuc2Method; RSA; DES; RC2
        //pvKey: only use when pvMode=QTEK
        public string Encrypt(string pvMode, string pvInput, string pvKey)
        {
            switch (pvMode)
            {
                case "QTEK":
                    {
                        return QTekCrypter.Crypting(pvKey, pvInput);
                    }
                case "HQ":
                    {
                        return HQCrypter.Encrypt(pvInput);
                    }
                case "RSA":
                    {
                        return RSACrypter.Encrypt(pvInput);
                    }
                case "DES":
                    {
                        return DESCrypter.Encrypt(pvInput);
                    }
                case "RC2":
                    {
                        return RC2Crypter.Encrypt(pvInput);
                    }
            }
            return pvInput;
        }
        public string Decrypt(string pvMode, string pvInput, string pvKey)
        {
            switch (pvMode)
            {
                case "QTEK":
                    {
                        return QTekCrypter.Crypting(pvKey, pvInput);
                    }
                case "HQ":
                    {
                        return HQCrypter.Decrypt(pvInput);
                    }
                case "RSA":
                    {
                        return RSACrypter.Decrypt(pvInput);
                    }
                case "DES":
                    {
                        return DESCrypter.Decrypt(pvInput);
                    }
                case "RC2":
                    {
                        return RC2Crypter.Decrypt(pvInput);
                    }
            }
            return pvInput;
        }
        #endregion

        #region Internal Classes
        internal class QTekCrypter
        {
            #region "Private Method"
            private static string GetChar(int pvAssci)
            {
                ASCIIEncoding fvAscii = new ASCIIEncoding();
                Byte[] fvByte;
                fvByte = new Byte[1];
                fvByte[0] = (Byte)pvAssci;
                string fvSReturn;
                fvSReturn = fvAscii.GetString(fvByte, 0, 1);
                return fvSReturn;
            }
            private static int GetAsci(string pvLetter)
            {
                ASCIIEncoding fvAscii = new ASCIIEncoding();
                Byte[] fvBytes;
                fvBytes = new Byte[1];
                int bytesEncodedCount = fvAscii.GetBytes(pvLetter, 0, 1, fvBytes, 0);
                foreach (Byte b in fvBytes)
                {
                    int k;
                    k = b % 256;
                    return k;
                }
                return -1;
            }
            #endregion

            #region "Encrypt and DeEncrypt Method"
            //type = 1: Encrypt for username and password
            //type = 2: Encrypt for card number
            public static string Crypting(string pvKey, string pvInput)
            {
                if (pvInput == null || pvInput == String.Empty || pvInput == "")
                {
                    return "";
                }

                ASCIIEncoding fvAscii = new ASCIIEncoding();
                string fvResult = "";
                if (pvKey == "")
                {
                    pvKey = "QTek.Crypter";
                }

                int inputLen = pvInput.Length;
                int keyLen = pvKey.Length;
                int[] fvSBox = new int[inputLen];
                int[] fvSBox2 = new int[inputLen];

                //initialize string
                int j = 0;
                for (int i = 0; i < inputLen; i++)
                {
                    fvSBox[i] = i;
                    if (j == keyLen)
                    {
                        j = 0;
                    }
                    fvSBox2[i] = GetAsci(pvKey[j++].ToString());
                }

                //Random shift in fvSBox and fvSBox2
                j = 0;
                int temp = 0;
                for (int i = 0; i < inputLen; i++)
                {
                    j = (j + fvSBox[i] + fvSBox2[inputLen - (i + 1)]) % inputLen;
                    temp = fvSBox[i];
                    fvSBox[i] = fvSBox[j];
                    fvSBox[j] = temp;
                }

                int i1 = 0, j1 = 0, t = 0, k = 0;
                for (int x = 0; x < inputLen; x++)
                {
                    i1 = (i1 + 1) % inputLen;
                    j1 = (keyLen + fvSBox[i1]) % inputLen;
                    t = (fvSBox[i1] + fvSBox[j1]) % inputLen;
                    k = (GetAsci(pvInput[x].ToString()) ^ fvSBox[t]);
                    if (k != 0)
                    {
                        fvResult += GetChar(k);
                    }
                    else
                    {
                        fvResult += pvInput[x];
                    }
                }
                return fvResult;
            }
            #endregion
        }

        internal class HQCrypter
        {
            #region Member Variables
            private const String SZ_CHARACTERSO = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789`!#$*.?;:^-<>%&,+~@/|";
            private const String SZ_CHARACTERSA = "Hz:0Za^y1C%5D?E8F>&GwJuKPcQVp;qBW*6X,Yb+d#e7fTg|Uh<AiMj/k!SlRm~@nLo$rsItx2-3N`O49v.";
            private const String SZ_CHARACTERSB = "Y@bFGd#e7f+~/Za^yz:1C|H5D?E8nLw-,JuK>%0s3N`PcQ<&Vp;qBW*6XTg2O49v.iUhAMjk!SlRmo$rItx";
            private const String SZ_CHARACTERSC = "cQ;qByVpW*-<6XTg2s3N`O49mo$rI>%&txYb|Fd#e7fZa^z:1CH,+/5D?E8n~@v.iUhAMjk!SlRLwJGuK0P";
            private const String SZ_CHARACTERSD = "-d<#e7f>%TAph;iMjk!SlR+mG&,n~Lo$rsW*6ItxqB|XgU23N`O49v.Hz:0Za^y1C5D?E@FwJuKPcQ/V8Yb";
            #endregion

            #region Private Methods
            // determine character pattern...
            private static String GetEncodePattern(int pvSeed)
            {
                pvSeed = Math.Abs(pvSeed);
                switch (pvSeed % 4)
                {
                    case 0: return SZ_CHARACTERSA;
                    case 1: return SZ_CHARACTERSB;
                    case 2: return SZ_CHARACTERSC;
                    default: return SZ_CHARACTERSD;
                }
            }
            // get encryption pvSeed...
            private static int GetEncodeSeed()
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                int fvRandom = Math.Abs(rand.Next());
                if (fvRandom > 99)
                {
                    while (fvRandom > 99)
                        fvRandom = (int)(fvRandom / 10);
                }
                if (fvRandom < 10)
                    fvRandom += 10;
                return fvRandom;
            }
            // simple string reverse function...
            private static string Rev(String pvS)
            {
                char[] fvC = pvS.ToCharArray();
                int l = pvS.Length - 1;
                for (int j = 0; j < l; j++, l--)
                {
                    fvC[j] ^= fvC[l];
                    fvC[l] ^= fvC[j];
                    fvC[j] ^= fvC[l];
                }
                String fvStr = new String(fvC);
                return fvStr;
            }
            #endregion

            #region Public Methods
            // encrypt string...
            public static String Encrypt(String pvS)
            {
                int fvMode = 0; // someday, i will add other modes
                pvS.Trim();
                if (pvS.Length == 0)
                {
                    return "";
                }

                String fvSzCharacters, fvSztmp;
                int pvSeed = 0;

                pvSeed = (fvMode <= 0) ? GetEncodeSeed() : pvSeed;
                pvS = Rev(pvS);

                int i;
                StringBuilder str = new StringBuilder("");
                for (i = 0; i < pvS.Length; i += 2)
                {
                    fvSztmp = pvS.Substring(i, 2);
                    fvSztmp = Rev(fvSztmp);
                    str.Append(fvSztmp);
                }
                pvS = str.ToString();
                str.Remove(0, str.Length);

                char fvUcChar;
                int fvIPos;

                fvSzCharacters = GetEncodePattern(pvSeed);
                for (i = 0; i < pvS.Length; i++)
                {
                    fvUcChar = pvS[i];
                    fvIPos = SZ_CHARACTERSO.IndexOf(fvUcChar);
                    if (fvIPos > 0)
                    {
                        fvUcChar = fvSzCharacters[fvIPos];
                    }
                    else if (fvIPos == 0 && SZ_CHARACTERSO[0].CompareTo(fvUcChar) != 0)
                    {
                        fvUcChar = fvSzCharacters[0];
                    }
                    str.Append(fvUcChar);
                }
                pvS = Rev(str.ToString());

                if (fvMode <= 0)
                {
                    String fvSeed1, fvSeed2;
                    int fvSeedbyte = (pvSeed / 10);
                    fvSeed1 = fvSeedbyte.ToString();
                    fvSeedbyte = pvSeed % 10;
                    fvSeed2 = fvSeedbyte.ToString();
                    pvS = fvSeed1 + pvS + fvSeed2;
                }
                return pvS;
            }
            // decrypt string...
            public static String Decrypt(String pvS)
            {
                int fvMode = 0;
                if (pvS.Length == 0)
                {
                    return "";
                }

                String fvSztmp;
                int pvSeed = fvMode;
                if (fvMode <= 0)
                {
                    fvSztmp = pvS[0].ToString();
                    fvSztmp += pvS[pvS.Length - 1];
                    pvSeed = Convert.ToInt32(fvSztmp);
                    pvS = pvS.Substring(1, pvS.Length - 2);
                }
                pvS = Rev(pvS);

                String fvSzCharacters;
                char fvUcChar;
                int fvPos;
                int i;

                fvSzCharacters = GetEncodePattern(pvSeed).ToString();

                StringBuilder szcopy = new StringBuilder("");
                for (i = 0; i < pvS.Length; i++)
                {
                    fvUcChar = pvS[i];
                    fvPos = fvSzCharacters.IndexOf(fvUcChar);
                    if (fvPos >= 0)
                    {
                        fvUcChar = SZ_CHARACTERSO[fvPos];
                    }
                    else if (fvSzCharacters[0].Equals(fvUcChar))
                    {
                        fvUcChar = SZ_CHARACTERSO[0];
                    }
                    szcopy.Append(fvUcChar);
                }
                StringBuilder faststring = new StringBuilder("");
                pvS = "";
                String szcopy1 = szcopy.ToString();
                for (i = 0; i < szcopy1.Length; i += 2)
                {
                    fvSztmp = szcopy1.Length >= i + 2 ? szcopy1.Substring(i, 2) : szcopy1.Substring(i);
                    fvSztmp = Rev(fvSztmp);
                    faststring.Append(fvSztmp);
                }
                pvS = faststring.ToString(); //this is not very costly in time cause its a single allocation
                pvS = Rev(pvS);
                return pvS;
            }
            #endregion
        }

        internal class RSACrypter
        {
            #region Public Methods
            public static string Encrypt(string pvDataString)
            {
                ASCIIEncoding fvByteConverter = new ASCIIEncoding();
                byte[] fvDataToEncrypt = fvByteConverter.GetBytes(pvDataString);
                byte[] fvEncryptedData;

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                fvEncryptedData = RSA.Encrypt(fvDataToEncrypt, false);
                RSA.PersistKeyInCsp = false;

                return Convert.ToBase64String(fvEncryptedData);
            }
            public static string Decrypt(string pvDataString)
            {
                try
                {
                    ASCIIEncoding fvByteConverter = new ASCIIEncoding();
                    byte[] fvDataToDecrypt = Convert.FromBase64String(pvDataString);
                    byte[] fvDecryptedData;

                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                    fvDecryptedData = RSA.Decrypt(fvDataToDecrypt, false);
                    return fvByteConverter.GetString(fvDecryptedData);
                }
                catch
                {
                    return "BAD DATA";
                }
            }
            #endregion
        }

        internal class DESCrypter
        {
            #region Public Methods
            public static string Encrypt(string pvDataString)
            {
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                MemoryStream fvMs = new MemoryStream();
                CryptoStream fvEncStream = new CryptoStream(fvMs, DES.CreateEncryptor(), CryptoStreamMode.Write);
                StreamWriter fvSw = new StreamWriter(fvEncStream);
                fvSw.WriteLine(pvDataString);
                fvSw.Close();
                fvEncStream.Close();
                ASCIIEncoding fvByteConverter = new ASCIIEncoding();
                return Convert.ToBase64String(fvMs.ToArray());
            }
            public static string Decrypt(string pvDataString)
            {
                try
                {
                    DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                    MemoryStream fvMs = new MemoryStream(Convert.FromBase64String(pvDataString));
                    CryptoStream fvEncStream = new CryptoStream(fvMs, DES.CreateDecryptor(), CryptoStreamMode.Read);
                    StreamReader fvSr = new StreamReader(fvEncStream);
                    string fvVal = fvSr.ReadLine();
                    fvSr.Close();
                    fvEncStream.Close();
                    fvMs.Close();
                    return fvVal;
                }
                catch
                {
                    return "BAD DATA";
                }
            }
            #endregion
        }

        internal class RC2Crypter
        {
            #region Public Methods
            public static string Encrypt(string pvDataString)
            {
                RC2CryptoServiceProvider RC2 = new RC2CryptoServiceProvider();
                byte[] fvKey = RC2.Key;
                byte[] IV = RC2.IV;

                ASCIIEncoding fvByteConverter = new ASCIIEncoding();
                ICryptoTransform fvEncryptorr = RC2.CreateEncryptor(fvKey, IV);
                MemoryStream fvMsEncrypt = new MemoryStream();
                CryptoStream fvCsEncrypt = new CryptoStream(fvMsEncrypt, fvEncryptorr, CryptoStreamMode.Write);
                byte[] toEncrypt = Encoding.ASCII.GetBytes(pvDataString);
                fvCsEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                fvCsEncrypt.FlushFinalBlock();
                return Convert.ToBase64String(fvMsEncrypt.ToArray());
            }
            public static string Decrypt(string pvDataString)
            {
                try
                {
                    RC2CryptoServiceProvider RC2 = new RC2CryptoServiceProvider();
                    byte[] fvKey = RC2.Key;
                    byte[] IV = RC2.IV;
                    ICryptoTransform fvDecryptor = RC2.CreateDecryptor(fvKey, IV);
                    MemoryStream fvMsDecrypt = new MemoryStream(Convert.FromBase64String(pvDataString));
                    CryptoStream csDecrypt = new CryptoStream(fvMsDecrypt, fvDecryptor, CryptoStreamMode.Read);
                    StreamReader fvSr = new StreamReader(csDecrypt);
                    string fvVal = fvSr.ReadLine();
                    fvSr.Close();
                    csDecrypt.Close();
                    fvMsDecrypt.Close();
                    return fvVal;
                }
                catch
                {
                    return "BADDATA";
                }
            }
            #endregion
        }
        #endregion
    }
}
