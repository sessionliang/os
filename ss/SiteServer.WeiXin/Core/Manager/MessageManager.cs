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
	public class MessageManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/message/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentImageUrl(PublishmentSystemInfo publishmentSystemInfo, string contentImageUrl)
        {
            if (string.IsNullOrEmpty(contentImageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/message/img/top.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentImageUrl));
            }
        }

        private static string GetMessageUrl(MessageInfo messageInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/message/index.html")));
        }

        public static string GetMessageUrl(MessageInfo messageInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", messageInfo.PublishmentSystemID.ToString());
            attributes.Add("messageID", messageInfo.ID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetMessageUrl(messageInfo), attributes);
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<MessageInfo> messageInfoList = DataProviderWX.MessageDAO.GetMessageInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (MessageInfo messageInfo in messageInfoList)
            {
                Article article = null;

                if (messageInfo != null && !messageInfo.IsDisabled)
                {
                    string imageUrl = MessageManager.GetImageUrl(publishmentSystemInfo, messageInfo.ImageUrl);
                    string pageUrl = MessageManager.GetMessageUrl(messageInfo, wxOpenID);

                    article = new Article()
                    {
                        Title = messageInfo.Title,
                        Description = messageInfo.Summary,
                        PicUrl = imageUrl,
                        Url = pageUrl
                    };
                }

                if (article != null)
                {
                    articleList.Add(article);
                }
            }

            return articleList;
        }

        public static void AddContent(int publishmentSystemID, int messageID, string displayName, string color, string content, string ipAddress, string cookieSN, string wxOpenID, string userName)
        {
            DataProviderWX.MessageDAO.AddUserCount(messageID);
            MessageContentInfo contentInfo = new MessageContentInfo { ID = 0, PublishmentSystemID = publishmentSystemID, MessageID = messageID, IPAddress = ipAddress, CookieSN = cookieSN, WXOpenID = wxOpenID, UserName = userName, ReplyCount = 0, LikeCount = 0, IsReply = false, ReplyID = 0, DisplayName = displayName, Color = color, Content = content, AddDate = DateTime.Now };
            DataProviderWX.MessageContentDAO.Insert(contentInfo);
        }

        public static void AddReply(int publishmentSystemID, int messageID, int replyContentID, string displayName, string content, string ipAddress, string cookieSN, string wxOpenID, string userName)
        {
            DataProviderWX.MessageDAO.AddUserCount(messageID);
            MessageContentInfo contentInfo = new MessageContentInfo { ID = 0, PublishmentSystemID = publishmentSystemID, MessageID = messageID, IPAddress = ipAddress, CookieSN = cookieSN, WXOpenID = wxOpenID, UserName = userName, ReplyCount = 0, LikeCount = 0, IsReply = true, ReplyID = replyContentID, DisplayName = displayName, Color = string.Empty, Content = content, AddDate = DateTime.Now };
            DataProviderWX.MessageContentDAO.Insert(contentInfo);
            DataProviderWX.MessageContentDAO.AddReplyCount(replyContentID);
        }
	}
}
