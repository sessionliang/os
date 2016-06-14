using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core
{
    public class ConfigurationManager
    {
        private ConfigurationManager() { }

        private static Dictionary<int, ConfigurationInfo> dictionary = new Dictionary<int,ConfigurationInfo>();
        private static readonly object lockObject = new object();
        private static bool async = true;//缓存与数据库不同步

        /// <summary>
        /// 从数据库得到ConfigurationInfo，如果数据库改变了async将被设置为true，此方法将从数据库中重新获得ConfigurationInfo
        /// </summary>
        /// <returns></returns>
        private static ConfigurationInfo GetInstance(int publishmentSystemID)
        {
            lock (lockObject)
            {
                if (async || !dictionary.ContainsKey(publishmentSystemID))
                {
                    ConfigurationInfo configurationInfo = DataProvider.ConfigurationDAO.GetConfigurationInfo(publishmentSystemID);
                    dictionary[publishmentSystemID] = configurationInfo;
                    async = false;
                }
            }
            return dictionary[publishmentSystemID];
        }

        public static ConfigurationInfoExtend GetAdditional(int publishmentSystemID)
        {
            ConfigurationInfo configurationInfo = ConfigurationManager.GetInstance(publishmentSystemID);
            return configurationInfo.Additional;
        }

        public static void Update(int publishmentSystemID)
        {
            ConfigurationInfo configurationInfo = ConfigurationManager.GetInstance(publishmentSystemID);
            DataProvider.ConfigurationDAO.Update(configurationInfo);
        }

        public static bool IsChanged
        {
            set { async = value; }
        }
    }
}
