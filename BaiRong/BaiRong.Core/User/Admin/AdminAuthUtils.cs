using System;

using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using BaiRong.Model;
using BaiRong.Core;
using BaiRong.Core.Cryptography;

namespace BaiRong.Core
{
    public class AdminAuthUtils
    {
        public static string EncodePassword(string password, EPasswordFormat passwordFormat, out string passwordSalt)
        {
            string retval = string.Empty;
            passwordSalt = string.Empty;

            if (passwordFormat == EPasswordFormat.Clear)
            {
                retval = password;
            }
            else if (passwordFormat == EPasswordFormat.Hashed)
            {
                passwordSalt = GenerateSalt();

                byte[] src = Encoding.Unicode.GetBytes(password);
                byte[] buffer2 = Convert.FromBase64String(passwordSalt);
                byte[] dst = new byte[buffer2.Length + src.Length];
                byte[] inArray = null;
                Buffer.BlockCopy(buffer2, 0, dst, 0, buffer2.Length);
                Buffer.BlockCopy(src, 0, dst, buffer2.Length, src.Length);
                HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
                inArray = algorithm.ComputeHash(dst);

                retval = Convert.ToBase64String(inArray);
            }
            else if (passwordFormat == EPasswordFormat.Encrypted)
            {
                passwordSalt = GenerateSalt();

                DESEncryptor encryptor = new DESEncryptor();
                encryptor.InputString = password;
                encryptor.EncryptKey = passwordSalt;
                encryptor.DesEncrypt();

                retval = encryptor.OutString;
            }
            return retval;
        }

        public static string DecodePassword(string password, EPasswordFormat passwordFormat, string passwordSalt)
        {
            string retval = string.Empty;
            if (passwordFormat == EPasswordFormat.Clear)
            {
                retval = password;
            }
            else if (passwordFormat == EPasswordFormat.Hashed)
            {
                throw new Exception("can not decode hashed password");
            }
            else if (passwordFormat == EPasswordFormat.Encrypted)
            {
                DESEncryptor encryptor = new DESEncryptor();
                encryptor.InputString = password;
                encryptor.DecryptKey = passwordSalt;
                encryptor.DesDecrypt();

                retval = encryptor.OutString;
            }
            return retval;
        }

        public static string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public static HttpCookie GetUserNameCookie(string userName, bool persistent)
        {
            HttpCookie cookie = new HttpCookie(AdminAuthConfig.UserNameCookieName, PageUtils.UrlEncode(userName));
            cookie.Path = AdminAuthConfig.FormsCookiePath;
            cookie.Secure = false;
            if (!persistent)
            {
                cookie.Expires = DateTime.Now.AddMinutes((double)AdminAuthConfig.Timeout);
            }
            else
            {
                cookie.Expires = DateTime.Now.AddYears(50);
            }
            return cookie;
        }

        public static HttpCookie GetAuthCookie(string userName, bool persistent)
        {
            if (userName == null)
            {
                userName = "";
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, persistent ? DateTime.Now.AddYears(50) : DateTime.Now.AddMinutes((double)AdminAuthConfig.Timeout), persistent, "", AdminAuthConfig.FormsCookiePath);
            string str = FormsAuthentication.Encrypt(ticket);
            if ((str == null) || (str.Length < 1))
            {
                throw new HttpException("Unable to encrypt cookie ticket");
            }
            HttpCookie cookie = new HttpCookie(AdminAuthConfig.AuthCookieName, str);
            cookie.Path = AdminAuthConfig.FormsCookiePath;
            cookie.Secure = false;
            if (!persistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            else
            {
                cookie.Expires = DateTime.Now.AddYears(50);
            }
            return cookie;
        }
    }
}
