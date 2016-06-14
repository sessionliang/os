using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using BaiRong.Core;

namespace BaiRong.Core
{
    public class SSOUtils
    {
        public static string AuthCode(string data, bool isDecode, string Key, int Expiry)
        {
            string retval = string.Empty;
            try
            {
                retval = SSOUtils.AuthCodeInternal(StringUtils.ValueFromUrl(data), isDecode, Key, Expiry);
            }
            catch { }
            if (!string.IsNullOrEmpty(retval))
            {
                byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(retval);
                retval = Encoding.GetEncoding("utf-8").GetString(bytes);
            }

            //byte[] temp;
            //string try0 = SSOUtils.AuthCode(Data, isDecode, Key, Expiry);
            //if (try0 != "")
            //{
            //    temp = Encoding.GetEncoding("iso-8859-1").GetBytes(try0);
            //    return Encoding.GetEncoding(SSOClient.CHARSET).GetString(temp);
            //}
            //string try1 = SSOUtils.AuthCode(Data + "=", isDecode, Key, 0);
            //if (try1 != "")
            //{
            //    temp = Encoding.GetEncoding("iso-8859-1").GetBytes(try1);
            //    return Encoding.GetEncoding(SSOClient.CHARSET).GetString(temp);
            //}
            //string try2 = SSOUtils.AuthCode(Data + "==", isDecode, Key, 0);
            //if (try2 != "")
            //{
            //    temp = Encoding.GetEncoding("iso-8859-1").GetBytes(try2);
            //    return Encoding.GetEncoding(SSOClient.CHARSET).GetString(temp);
            //}
            return retval;
        }

        private static string AuthCodeInternal(string data, bool isDecode, string authKey, int expiry)
        {
            string KeyC;
            DateTime now;
            DateTime gmt;
            TimeSpan ts;
            int i;
            int tmp;
            int cKey_Length = 4;
            authKey = FormsAuthentication.HashPasswordForStoringInConfigFile(authKey, "md5").ToLower();
            string KeyA = FormsAuthentication.HashPasswordForStoringInConfigFile(authKey.Substring(0, 0x10), "md5").ToLower();
            string KeyB = FormsAuthentication.HashPasswordForStoringInConfigFile(authKey.Substring(0x10, 0x10), "md5").ToLower();
            if (cKey_Length > 0)
            {
                if (isDecode)
                {
                    KeyC = data.Substring(0, cKey_Length);
                }
                else
                {
                    KeyC = FormsAuthentication.HashPasswordForStoringInConfigFile(DateTime.Now.Millisecond.ToString(), "md5").ToLower().Substring(0x20 - cKey_Length);
                }
            }
            else
            {
                KeyC = "";
            }
            string CryptKey = KeyA + FormsAuthentication.HashPasswordForStoringInConfigFile(KeyA + KeyC, "md5").ToLower();
            int Key_Length = CryptKey.Length;
            if (isDecode)
            {
                byte[] buffer2 = new Base64Decoder().GetDecoded(data.Substring(cKey_Length));
                data = Encoding.GetEncoding("iso-8859-1").GetString(buffer2);
            }
            else
            {
                now = DateTime.Now.AddHours(-8.0);
                gmt = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                ts = (TimeSpan)(now - gmt);
                data = string.Format("{0:D10}", (expiry > 0) ? (expiry + Convert.ToInt32(ts.TotalSeconds)) : 0) + FormsAuthentication.HashPasswordForStoringInConfigFile(data + KeyB, "md5").ToLower().Substring(0, 0x10) + data;
            }
            int Data_Length = data.Length;
            string Result = "";
            int[] Box = new int[0x100];
            for (i = 0; i < Box.Length; i++)
            {
                Box[i] = i;
            }
            char[] rndkey = CryptKey.ToCharArray();
            int[] RndKey = new int[0x100];
            for (i = 0; i < 0x100; i++)
            {
                RndKey[i] = Convert.ToInt32(rndkey[i % Key_Length]);
            }
            int j = 0;
            for (i = 0; i < 0x100; i++)
            {
                j = ((j + Box[i]) + RndKey[i]) % 0x100;
                tmp = Box[i];
                Box[i] = Box[j];
                Box[j] = tmp;
            }
            char[] DataCharArray = data.ToCharArray();
            int a = 0;
            j = 0;
            for (i = 0; i < Data_Length; i++)
            {
                a = (a + 1) % 0x100;
                j = (j + Box[a]) % 0x100;
                tmp = Box[a];
                Box[a] = Box[j];
                Box[j] = tmp;
                Result = Result + Convert.ToString(Convert.ToChar((int)(Convert.ToInt32(DataCharArray[i]) ^ Box[(Box[a] + Box[j]) % 0x100])));
            }
            if (isDecode)
            {
                bool condition1 = false;
                try
                {
                    condition1 = Convert.ToInt32(Result.Substring(0, 10)) == 0;
                }
                catch
                {
                }
                bool condition2 = false;
                try
                {
                    now = DateTime.Now.AddHours(-8.0);
                    gmt = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                    ts = (TimeSpan)(now - gmt);
                    condition2 = (Convert.ToInt32(Result.Substring(0, 10)) - Convert.ToInt32(ts.TotalSeconds)) > 0;
                }
                catch
                {
                }
                bool condition3 = false;
                string md5 = SSOUtils.MD5(Result.Substring(0x1a) + KeyB).ToLower();
                int tab = Convert.ToInt32(Result.Substring(0x1a)[1]);
                if (Result.Substring(10, 0x10) == FormsAuthentication.HashPasswordForStoringInConfigFile(Result.Substring(0x1a) + KeyB, "md5").ToLower().Substring(0, 0x10))
                {
                    condition3 = true;
                }
                if ((condition1 || condition2) && condition3)
                {
                    return Result.Substring(0x1a);
                }
                return "";
            }
            char[] buffer = Result.ToCharArray();
            byte[] temp = new byte[buffer.Length];
            for (i = 0; i < buffer.Length; i++)
            {
                temp[i] = Convert.ToByte(buffer[i]);
            }
            char[] base64char = new Base64Encoder(temp).GetEncoded();
            string base64string = "";
            for (i = 0; i < base64char.Length; i++)
            {
                base64string = base64string + base64char[i];
            }
            //base64string = base64string.Replace("=", "");
            base64string = StringUtils.ValueToUrl(base64string);
            return (KeyC + base64string);
        }

        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.Default.GetBytes(str);
            byte[] result = md5.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < result.Length; i++)
            {
                ret = ret + result[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }

        public static string GenerateAuthKey()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public static string ParseUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                url = url.Trim().TrimEnd('/').ToLower();
                url = PageUtils.AddProtocolToUrl(url);
            }
            return url;
        }
    }
}
