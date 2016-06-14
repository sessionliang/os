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
	public class VoteManager
    {
        //http://www.bootply.com/85625
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/vote/img/start.jpg")));
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
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/vote/img/top.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentImageUrl));
            }
        }

        public static string GetEndImageUrl(PublishmentSystemInfo publishmentSystemInfo, string endImageUrl)
        {
            if (string.IsNullOrEmpty(endImageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/vote/img/end.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, endImageUrl));
            }
        }

        private static string GetVoteUrl(VoteInfo voteInfo)
        {
            if (voteInfo.ContentIsImageOption)
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/vote/image.html")));
            }
            else
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/vote/text.html")));
            }
        }

        public static string GetVoteUrl(VoteInfo voteInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("voteID", voteInfo.ID.ToString());
            attributes.Add("publishmentSystemID", voteInfo.PublishmentSystemID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetVoteUrl(voteInfo), attributes);
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<VoteInfo> voteInfoList = DataProviderWX.VoteDAO.GetVoteInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (VoteInfo voteInfo in voteInfoList)
            {
                Article article = null;

                if (voteInfo != null && voteInfo.StartDate < DateTime.Now)
                {
                    bool isEnd = false;

                    if (voteInfo.EndDate < DateTime.Now)
                    {
                        isEnd = true;
                    }

                    if (isEnd)
                    {
                        string endImageUrl = VoteManager.GetEndImageUrl(publishmentSystemInfo, voteInfo.EndImageUrl);

                        article = new Article()
                        {
                            Title = voteInfo.EndTitle,
                            Description = voteInfo.EndSummary,
                            PicUrl = endImageUrl
                        };
                    }
                    else
                    {
                        string imageUrl = VoteManager.GetImageUrl(publishmentSystemInfo, voteInfo.ImageUrl);
                        string pageUrl = VoteManager.GetVoteUrl(voteInfo, wxOpenID);

                        article = new Article()
                        {
                            Title = voteInfo.Title,
                            Description = voteInfo.Summary,
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

        public static void Vote(int publishmentSystemID, int voteID, List<int> itemIDList, string ipAddress, string cookieSN, string wxOpenID, string userName)
        {
            DataProviderWX.VoteDAO.AddUserCount(voteID);
            DataProviderWX.VoteItemDAO.AddVoteNum(voteID, itemIDList);
            VoteLogInfo logInfo = new VoteLogInfo { ID = 0, PublishmentSystemID = publishmentSystemID, VoteID = voteID, ItemIDCollection = TranslateUtils.ObjectCollectionToString(itemIDList), IPAddress = ipAddress, CookieSN = cookieSN, WXOpenID = wxOpenID, UserName = userName, AddDate = DateTime.Now };
            DataProviderWX.VoteLogDAO.Insert(logInfo);
        }
	}
}
