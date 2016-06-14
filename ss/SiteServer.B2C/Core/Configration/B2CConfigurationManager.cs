using System.Collections;
using System.Web.Caching;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using SiteServer.B2C.Model;
using BaiRong.Core.Cryptography;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
    public class B2CConfigurationManager
    {
        private B2CConfigurationManager() { }

        private static Dictionary<int, B2CConfigurationInfo> dictionary = new Dictionary<int,B2CConfigurationInfo>();
        private static readonly object lockObject = new object();
        private static bool async = true;//缓存与数据库不同步

        public static B2CConfigurationInfo GetInstance(int nodeID)
        {
            if (dictionary.ContainsKey(nodeID))
            {
                lock (lockObject)
                {
                    if (async)
                    {
                        B2CConfigurationInfo configurationInfo = DataProviderB2C.B2CConfigurationDAO.GetConfigurationInfo(nodeID);
                        dictionary[nodeID] = configurationInfo;

                        async = false;
                    }
                }

                return dictionary[nodeID];
            }
            else
            {
                B2CConfigurationInfo configurationInfo = DataProviderB2C.B2CConfigurationDAO.GetConfigurationInfo(nodeID);
                dictionary[nodeID] = configurationInfo;

                return configurationInfo;
            }
        }

        public static B2CConfigurationInfoExtend GetPublishmentSystemAdditional(int publishmentSystemID)
        {
            return B2CConfigurationManager.GetInstance(publishmentSystemID).Additional;
        }

        public static bool IsChanged
        {
            set { async = value; }
        }
    }
}
