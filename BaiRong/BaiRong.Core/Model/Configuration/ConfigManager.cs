using System.Collections;
using System.Web.Caching;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using BaiRong.Core.Cryptography;
using BaiRong.Core.Data.Provider;
using System;

namespace BaiRong.Core
{
    public class ConfigManager
    {
        private ConfigManager() { }

        private static ConfigInfo configInfo;
        private static readonly object lockObject = new object();
        private static bool async = true;//缓存与数据库不同步

        public static ConfigInfo Instance
        {
            get
            {
                if (configInfo == null)
                {
                    configInfo = BaiRongDataProvider.ConfigDAO.GetConfigInfo();
                    return configInfo;
                }
                lock (lockObject)
                {
                    if (async)
                    {
                        configInfo = BaiRongDataProvider.ConfigDAO.GetConfigInfo();
                        async = false;
                    }
                }
                return configInfo;
            }
        }

        public static ConfigInfoExtend Additional
        {
            get
            {
                return ConfigManager.Instance.Additional;
            }
        }

        public static bool IsChanged
        {
            set { async = value; }
        }

        public static string Cipherkey
        {
            get
            {
                string cipherkey = ConfigManager.Instance.Additional.Cipherkey;
                if (string.IsNullOrEmpty(cipherkey))
                {
                    char[] s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'I', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };//枚举数组
                    Random r = new Random();
                    for (int i = 0; i < 8; i++)
                    {
                        cipherkey += s[r.Next(0, s.Length)].ToString();
                    }

                    ConfigManager.Instance.Additional.Cipherkey = cipherkey;

                    BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                }
                return cipherkey;
            }
        }

        private const string SYS = "sys";

        public static bool IsSysAdministrator(string userName)
        {
            if (userName == SYS)
            {
                return true;
            }
            return false;
        }
    }
}
