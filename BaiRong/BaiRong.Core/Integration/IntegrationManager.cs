using BaiRong.Core.Cryptography;
using BaiRong.Core.Net;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BaiRong.Core.Integration
{
    public class IntegrationManager
    {
        public static string GetIntegrationToken(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return string.Empty;

            DESEncryptor encryptor = new DESEncryptor();
            encryptor.InputString = userName;
            encryptor.EncryptKey = "M9jjr5nX";
            encryptor.DesEncrypt();

            string retval = encryptor.OutString;
            retval = retval.Replace("-", "0hz0").Replace("+", "0add0").Replace("=", "0eq0").Replace("&", "0and0").Replace("?", "0qu0").Replace("'", "0quo0").Replace("/", "0sl0");
            //retval = retval.Replace("+", "0add0").Replace("=", "0equals0").Replace("&", "0and0").Replace("?", "0question0").Replace("'", "0quote0").Replace("/", "0slash0");

            return retval;

            //return RuntimeUtils.EncryptStringByTranslate(userName);
        }

        public static string GetIntegrationUserName(string token)
        {
            if (string.IsNullOrEmpty(token)) return string.Empty;

            token = token.Replace("0hz0", "-").Replace("0add0", "+").Replace("0eq0", "=").Replace("0and0", "&").Replace("0qu0", "?").Replace("0quo0", "'").Replace("0sl0", "/");
            //token = token.Replace("0add0", "+").Replace("0equals0", "=").Replace("0and0", "&").Replace("0question0", "?").Replace("0quote0", "'").Replace("0slash0", "/");

            DESEncryptor encryptor = new DESEncryptor();
            encryptor.InputString = token;
            encryptor.DecryptKey = "M9jjr5nX";
            encryptor.DesDecrypt();

            return encryptor.OutString;

            //return RuntimeUtils.DecryptStringByTranslate(token);
        }
    }
}
