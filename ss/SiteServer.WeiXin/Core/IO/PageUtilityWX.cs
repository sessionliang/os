using System;
using System.IO;
using System.Collections;
using System.Web;
using BaiRong.Core;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections.Specialized;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.Core
{
    public class PageUtilityWX
    {
        private PageUtilityWX()
        {
        }

        public static string GetWeiXinTemplateDirectoryUrl()
        {
            return PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/components/templates"));
        }

        public static string GetWeiXinFileUrl(PublishmentSystemInfo publishmentSystemInfo, int keywordID, int resourceID)
        {
            return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, string.Format("@/weixin-files/{0}-{1}.html", keywordID, resourceID)));
        }

        public static string GetContentUrl(PublishmentSystemInfo publishmentSystemInfo, ContentInfo contentInfo)
        {
            return PageUtils.AddProtocolToUrl(PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, publishmentSystemInfo.Additional.VisualType));
        }

        public static string GetContentUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
            return PageUtils.AddProtocolToUrl(PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, publishmentSystemInfo.Additional.VisualType));
        }

        public static string GetChannelUrl(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            return PageUtils.AddProtocolToUrl(PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, false, publishmentSystemInfo.Additional.VisualType));
        }

        public static string GetChannelUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
            return GetChannelUrl(publishmentSystemInfo, nodeInfo);
        }

        #region Vote

        public static string GetVoteTemplateDirectoryUrl()
        {
            return PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/vote"));
        }

        public static string GetVoteUrl(PublishmentSystemInfo publishmentSystemInfo, int keywordID, int voteID)
        {
            return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, string.Format("@/weixin-files/{0}-{1}.html", keywordID, voteID)));
        }

        #endregion

        public class API
        {
            public static string GetMPUrl(int publishmentSystemID)
            {
                string url = string.Empty;

                if (PageUtils.IsProtocolUrl(FileConfigManager.Instance.APIUrl))
                {
                    url = FileConfigManager.Instance.APIUrl;
                }
                else
                {
                    url = PageUtils.AddProtocolToUrl(PageUtils.Combine(PageUtils.GetHost(), PageUtils.ParseNavigationUrl(FileConfigManager.Instance.APIUrl), "mp/url/", publishmentSystemID.ToString()));
                }

                return PageUtils.RemovePortFromUrl(url);
            }

            public static string GetMPToken(int publishmentSystemID)
            {
                return WeiXinManager.GetAccountInfo(publishmentSystemID).Token;
            }
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetWXUrl(pageUrl), arguments, width, height);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetWXUrl(pageUrl), arguments);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetWXUrl(pageUrl), arguments, isCloseOnly);
        }

        public static string GetOpenWindowString(string title, string pageUrl, NameValueCollection arguments, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowString(title, PageUtils.GetWXUrl(pageUrl), arguments, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, textBoxID);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, textBoxID, width, height);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, textBoxID, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTextBoxValue(string title, string pageUrl, NameValueCollection arguments, string textBoxID, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTextBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, textBoxID, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, checkBoxID, alertText, width, height);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, checkBoxID, alertText);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, checkBoxID, alertText, isCloseOnly);
        }

        public static string GetOpenWindowStringWithCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID, string alertText, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, checkBoxID, alertText, width, height, isCloseOnly);
        }

        public static string GetOpenWindowStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTwoCheckBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, checkBoxID1, checkBoxID2, alertText, width, height);
        }

        public static string GetOpenWindowStringWithTwoCheckBoxValue(string title, string pageUrl, NameValueCollection arguments, string checkBoxID1, string checkBoxID2, string alertText, int width, int height, bool isCloseOnly)
        {
            return JsUtils.OpenWindow.GetOpenWindowStringWithTwoCheckBoxValue(title, PageUtils.GetWXUrl(pageUrl), arguments, checkBoxID1, checkBoxID2, alertText, width, height, isCloseOnly);
        }
    }
}
