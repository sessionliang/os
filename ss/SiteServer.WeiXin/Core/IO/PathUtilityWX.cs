using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO.FileManagement;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.WeiXin.Core
{
    public class PathUtilityWX
    {
        private PathUtilityWX()
        {
        }

        public static string GetWeiXinFilePath(PublishmentSystemInfo publishmentSystemInfo, int keywordID, int resourceID)
        {
            return PathUtils.Combine(PathUtility.GetPublishmentSystemPath(publishmentSystemInfo), "weixin-files", string.Format("{0}-{1}.html", keywordID, resourceID));
        }

        public static string GetWeiXinTemplateFilePath()
        {
            return PathUtils.GetSiteFilesPath("services/weixin/components/templates/content.html");
        }
    }
}
