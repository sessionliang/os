using System.Collections;
using System.Web.Caching;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using BaiRong.Core.Cryptography;
using SiteServer.CRM.Model;

namespace SiteServer.CRM.Core
{
    public class ConfigurationManager
    {
        private ConfigurationManager() { }

        private static ConfigurationInfo configurationInfo;
        private static readonly object lockObject = new object();
        private static bool async = true;//缓存与数据库不同步

        /// <summary>
        /// 从数据库得到ConfigurationInfo，如果数据库改变了async将被设置为true，此方法将从数据库中重新获得ConfigurationInfo
        /// </summary>
        /// <returns></returns>
        public static ConfigurationInfo Instance
        {
            get
            {
                if (configurationInfo == null)
                {
                    configurationInfo = DataProvider.ConfigurationDAO.GetConfigurationInfo();
                    return configurationInfo;
                }
                lock (lockObject)
                {
                    if (async)
                    {
                        configurationInfo = DataProvider.ConfigurationDAO.GetConfigurationInfo();
                        async = false;
                    }
                }
                return configurationInfo;
            }
        }

        public static ConfigurationInfoExtend Additional
        {
            get
            {
                return ConfigurationManager.Instance.Additional;
            }
        }

        public const string ConnectionStringRedirect = "";

        public static bool IsChanged
        {
            set { async = value; }
        }
    }
}
