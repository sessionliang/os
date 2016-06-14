using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using SiteServer.WeiXin.Model;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using Senparc.Weixin.MP.Entities;

namespace SiteServer.WeiXin.Core
{
	public class NavigationUtils
    {
        public static string GetNavigationUrl(PublishmentSystemInfo publishmentSystemInfo, ENavigationType navigationType, EKeywordType keywordType, int functionID, int channelID, int contentID, string url)
        {
            string navigationUrl = string.Empty;

            if (navigationType == ENavigationType.Url)
            {
                navigationUrl = url;
            }
            else if (navigationType == ENavigationType.Function)
            {
                navigationUrl = KeywordManager.GetFunctionUrl(publishmentSystemInfo.PublishmentSystemID, keywordType, functionID);
            }
            else if (navigationType == ENavigationType.Site)
            {
                if (contentID > 0)
                {
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, channelID);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, channelID);

                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                    navigationUrl = PageUtilityWX.GetContentUrl(publishmentSystemInfo, contentInfo);
                }
                else if (channelID > 0)
                {
                    string nodeNames = NodeManager.GetNodeNameNavigation(publishmentSystemInfo.PublishmentSystemID, channelID);
                    navigationUrl = PageUtilityWX.GetChannelUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID));
                }
            }

            return navigationUrl;
        }

        public static string ParseWebMenu(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (publishmentSystemInfo.Additional.WX_IsWebMenu && !string.IsNullOrEmpty(publishmentSystemInfo.Additional.WX_WebMenuType))
            {
                string directoryUrl = PageUtils.GetSiteFilesUrl(string.Format("Services/WeiXin/components/webMenu/{0}", publishmentSystemInfo.Additional.WX_WebMenuType));
                if (PageUtils.IsProtocolUrl(publishmentSystemInfo.PublishmentSystemUrl))
                {
                    directoryUrl = PageUtils.AddProtocolToUrl(PageUtils.ParseNavigationUrl(PageUtils.GetAbsoluteSiteFilesUrl(string.Format("Services/WeiXin/components/webMenu/{0}", publishmentSystemInfo.Additional.WX_WebMenuType))));
                } 
                 
                string menuPath = PathUtils.GetSiteFilesPath(string.Format("Services/WeiXin/components/webMenu/{0}/template.html", publishmentSystemInfo.Additional.WX_WebMenuType));
                string menuHtml = CreateCacheManager.FileContent.GetContentByFilePath(menuPath, ECharset.utf_8);

                int startIndex = menuHtml.IndexOf("<!--menu-->");
                int endIndex = menuHtml.IndexOf("<!--menu-->", startIndex + 1);
                string menuTemplate = menuHtml.Substring(startIndex, endIndex - startIndex);

                int startSubIndex = menuTemplate.IndexOf("<!--submenu-->");
                int endSubIndex = menuTemplate.IndexOf("<!--submenu-->", startSubIndex + 1);
                string subMenuTemplate = menuTemplate.Substring(startSubIndex, endSubIndex - startSubIndex);

                StringBuilder menuBuilder = new StringBuilder();
                List<WebMenuInfo> menuInfoList = DataProviderWX.WebMenuDAO.GetMenuInfoList(publishmentSystemInfo.PublishmentSystemID, 0);

                int index = 0;
                foreach (WebMenuInfo menuInfo in menuInfoList)
                {
                    StringBuilder subMenuBuilder = new StringBuilder();
                    List<WebMenuInfo> subMenuInfoList = DataProviderWX.WebMenuDAO.GetMenuInfoList(publishmentSystemInfo.PublishmentSystemID, menuInfo.ID);

                    if (subMenuInfoList != null && subMenuInfoList.Count > 0)
                    {
                        menuInfo.NavigationType = ENavigationTypeUtils.GetValue(ENavigationType.Url);
                        menuInfo.Url = PageUtils.UNCLICKED_URL;

                        foreach (WebMenuInfo subMenuInfo in subMenuInfoList)
                        {
                            string subMenu = subMenuTemplate.Replace("{{url}}", NavigationUtils.GetNavigationUrl(publishmentSystemInfo, ENavigationTypeUtils.GetEnumType(subMenuInfo.NavigationType), EKeywordTypeUtils.GetEnumType(subMenuInfo.KeywordType), subMenuInfo.FunctionID, subMenuInfo.ChannelID, subMenuInfo.ContentID, subMenuInfo.Url));
                            subMenu = subMenu.Replace("{{menuName}}", subMenuInfo.MenuName);
                            subMenuBuilder.Append(subMenu);
                        }
                    }
                    string menu = menuTemplate.Substring(0, startSubIndex) + subMenuBuilder.ToString() + menuTemplate.Substring(endSubIndex);

                    menu = menu.Replace("{{url}}", NavigationUtils.GetNavigationUrl(publishmentSystemInfo, ENavigationTypeUtils.GetEnumType(menuInfo.NavigationType), EKeywordTypeUtils.GetEnumType(menuInfo.KeywordType), menuInfo.FunctionID, menuInfo.ChannelID, menuInfo.ContentID, menuInfo.Url));
                    menu = menu.Replace("{{index}}", index.ToString());
                    menu = menu.Replace("{{menuName}}", menuInfo.MenuName);
                    menuBuilder.Append(menu);
                    index++;
                }

                menuHtml = menuHtml.Substring(0, startIndex) + menuBuilder.ToString() + menuHtml.Substring(endIndex);

                return string.Format(@"
<link rel=""stylesheet"" type=""text/css"" href=""{0}/style.css"" />
<script type=""text/javascript"" src=""{0}/script.js""></script>
{1}", directoryUrl, menuHtml);
            }
            return string.Empty;
        }
	}
}
