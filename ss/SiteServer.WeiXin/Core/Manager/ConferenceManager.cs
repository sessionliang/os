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
	public class ConferenceManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/conference/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetEndImageUrl(PublishmentSystemInfo publishmentSystemInfo, string endImageUrl)
        {
            if (string.IsNullOrEmpty(endImageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/conference/img/end.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, endImageUrl));
            }
        }

        private static string GetConferenceUrl(ConferenceInfo conferenceInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/conference/index.html")));
        }

        public static string GetConferenceUrl(ConferenceInfo conferenceInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", conferenceInfo.PublishmentSystemID.ToString());
            attributes.Add("conferenceID", conferenceInfo.ID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetConferenceUrl(conferenceInfo), attributes);
        }

        public static void AddContent(int publishmentSystemID, int conferenceID, string realName, string mobile, string email, string company, string position, string note, string ipAddress, string cookieSN, string wxOpenID, string userName)
        {
            DataProviderWX.ConferenceDAO.AddUserCount(conferenceID);
            ConferenceContentInfo contentInfo = new ConferenceContentInfo { ID = 0, PublishmentSystemID = publishmentSystemID, ConferenceID = conferenceID, IPAddress = ipAddress, CookieSN = cookieSN, WXOpenID = wxOpenID, UserName = userName, RealName = realName, Mobile = mobile, Email = email, Company = company, Position = position, Note = note, AddDate = DateTime.Now };
            DataProviderWX.ConferenceContentDAO.Insert(contentInfo);
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<ConferenceInfo> conferenceInfoList = DataProviderWX.ConferenceDAO.GetConferenceInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (ConferenceInfo conferenceInfo in conferenceInfoList)
            {
                Article article = null;

                if (conferenceInfo != null && conferenceInfo.StartDate < DateTime.Now)
                {
                    bool isEnd = false;

                    if (conferenceInfo.EndDate < DateTime.Now)
                    {
                        isEnd = true;
                    }

                    if (isEnd)
                    {
                        string endImageUrl = ConferenceManager.GetEndImageUrl(publishmentSystemInfo, conferenceInfo.EndImageUrl);

                        article = new Article()
                        {
                            Title = conferenceInfo.EndTitle,
                            Description = conferenceInfo.EndSummary,
                            PicUrl = endImageUrl
                        };
                    }
                    else
                    {
                        string imageUrl = ConferenceManager.GetImageUrl(publishmentSystemInfo, conferenceInfo.ImageUrl);
                        string pageUrl = ConferenceManager.GetConferenceUrl(conferenceInfo, wxOpenID);

                        article = new Article()
                        {
                            Title = conferenceInfo.Title,
                            Description = conferenceInfo.Summary,
                            PicUrl = imageUrl,
                            Url = pageUrl
                        };
                    }
                }

                if (article != null)
                {
                    articleList.Add(article);
                }
            }

            return articleList;
        }
	}
}
