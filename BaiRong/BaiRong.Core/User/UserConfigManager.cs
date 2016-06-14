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
        private static bool async = true;//���������ݿⲻͬ��

        /// <summary>
        /// �����ݿ�õ�configInfo��������ݿ�ı���async��������Ϊtrue���˷����������ݿ������»��configInfo
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
        /// ����ͬʱ���web��api�Ļ���
        /// </summary>
        public static void Clear()
        {
            configInfo = null;            
        }
    }
}
