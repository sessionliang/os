using System;
using System.Xml;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using BaiRong.Model;
using BaiRong.Core.IO;
using System.Collections;

using SiteServer.CMS.Model;
using SiteServer.WeiXin.Model;
using System.Text;
using SiteServer.CMS.Core;

namespace SiteServer.WeiXin.Core
{
    public class FileUtilityWX
    {
        private FileUtilityWX()
        {
        }

        public static void DeleteWeiXinContent(PublishmentSystemInfo publishmentSystemInfo, int keywordID, int resourceID)
        {
            string filePath = PathUtilityWX.GetWeiXinFilePath(publishmentSystemInfo, keywordID, resourceID);
            FileUtils.DeleteFileIfExists(filePath);
        }

        public static void CreateWeiXinContent(PublishmentSystemInfo publishmentSystemInfo, int keywordID, int resourceID)
        {
            SiteServer.WeiXin.Model.KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(keywordID);

            if (keywordInfo != null)
            {
                string filePath = PathUtilityWX.GetWeiXinFilePath(publishmentSystemInfo, keywordID, resourceID);
                string templateFilePath = PathUtilityWX.GetWeiXinTemplateFilePath();
                string serviceUrl = PageUtilityWX.GetWeiXinTemplateDirectoryUrl();

                StringBuilder builder = new StringBuilder(FileUtils.ReadText(templateFilePath, ECharset.utf_8));

                KeywordResourceInfo resourceInfo = DataProviderWX.KeywordResourceDAO.GetResourceInfo(resourceID);
                if (resourceInfo != null)
                {
                    builder.Replace("{serviceUrl}", serviceUrl);
                    builder.Replace("{title}", resourceInfo.Title);
                    if (resourceInfo.IsShowCoverPic && !string.IsNullOrEmpty(resourceInfo.ImageUrl))
                    {
                        builder.Replace("{image}", string.Format(@"<img src=""{0}"" />", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, resourceInfo.ImageUrl))));
                    }
                    else
                    {
                        builder.Replace("{image}", string.Empty);
                    }
                    builder.Replace("{addDate}", DateUtils.GetDateString(keywordInfo.AddDate));
                    builder.Replace("{weixinName}", publishmentSystemInfo.PublishmentSystemName);
                    builder.Replace("{content}", resourceInfo.Content);
                    //builder.Replace("{poweredBy}", FileConfigManager.Instance.OEMConfig.IsOEM ? string.Empty : "技术支持：阁下");
                    #region 张浩然 2014-8-25 修改自定义版权

                    if (string.IsNullOrEmpty(publishmentSystemInfo.Additional.WX_PoweredBy))
                    {
                        publishmentSystemInfo.Additional.WX_PoweredBy =string.Format(@"<a href='http://www.gexia.com/home/login.html'>{0}</a>", "技术支持：阁下");
                    }

                    builder.Replace("{poweredBy}", publishmentSystemInfo.Additional.WX_PoweredBy);
                    #endregion
                }

                FileUtils.WriteText(filePath, ECharset.utf_8, builder.ToString());
            }
        }
    }
}
