using System;
using System.Text;
using BaiRong.Core.Cryptography;
using BaiRong.Core;

namespace BaiRong.Core
{
    public class RuntimeUtils
    {
        public static string EncryptStringByTranslate(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;

            DESEncryptor encryptor = new DESEncryptor();
            encryptor.InputString = inputString;
            encryptor.EncryptKey = ConfigManager.Cipherkey;
            encryptor.DesEncrypt();

            string retval = encryptor.OutString;
            retval = retval.Replace("+", "0add0").Replace("=", "0equals0").Replace("&", "0and0").Replace("?", "0question0").Replace("'", "0quote0").Replace("/", "0slash0");

            return retval;
        }

        public static string DecryptStringByTranslate(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;

            inputString = inputString.Replace("0add0", "+").Replace("0equals0", "=").Replace("0and0", "&").Replace("0question0", "?").Replace("0quote0", "'").Replace("0slash0", "/");

            DESEncryptor encryptor = new DESEncryptor();
            encryptor.InputString = inputString;
            encryptor.DecryptKey = ConfigManager.Cipherkey;
            encryptor.DesDecrypt();

            return encryptor.OutString;
        }
    }
}
