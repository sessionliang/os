using System.Collections;
using System.Web.Caching;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using BaiRong.Core.Cryptography;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public class UserConfigManager
    {
        private UserConfigManager() { }

        private static UserConfigInfo configInfo;
        private static readonly object lockObject = new object();
        private static bool async = true;//缓存与数据库不同步

        /// <summary>
        /// 从数据库得到configInfo，如果数据库改变了async将被设置为true，此方法将从数据库中重新获得configInfo
        /// </summary>
        /// <returns></returns>
        public static UserConfigInfo Instance
        {
            get
            {
                if (configInfo == null)
                {
                    configInfo = BaiRongDataProvider.UserConfigDAO.GetUserConfigInfo();
                    return configInfo;
                }
                lock (lockObject)
                {
                    if (async)
                    {
                        configInfo = BaiRongDataProvider.UserConfigDAO.GetUserConfigInfo();
                        async = false;
                    }
                }
                return configInfo;
            }
        }

        public static UserConfigInfoExtend Additional
        {
            get
            {
                return UserConfigManager.Instance.Additional;
            }
        }

        public static bool IsChanged
        {
            set
            {
                async = value;
                AjaxUrlManager.AddAjaxUrl(PageUtils.API.GetUserConfigClearCacheUrl());
            }
        }

        /// <summary>
        /// 可以同时清楚web和api的缓存
        /// </summary>
        public static void Clear()
        {
            configInfo = null;            
        }
    }
}
