using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IWebsiteMessageClassifyDAO : ITreeDAO
    {
        int InsertWebsiteMessageClassifyInfo(int publishmentSystemID, int parentID, string itemName, string itemIndexName);

        int InsertWebsiteMessageClassifyInfo(WebsiteMessageClassifyInfo websiteMessageClassifyInfo);

        void UpdateWebsiteMessageClassifyInfo(WebsiteMessageClassifyInfo websiteMessageClassifyInfo);

        WebsiteMessageClassifyInfo GetWebsiteMessageClassifyInfo(int itemID);

        void Delete(int deleteID);

        /// <summary>
        /// 设置默认分类
        /// </summary>
        /// <returns></returns>
        int SetDefaultWebsiteMessageClassifyInfo(int publishmentSystemID);

        /// <summary>
        /// 获取默认分类ID
        /// </summary>
        /// <returns></returns>
        int GetDefaultClassifyID();
    }
}
