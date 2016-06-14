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
	public class CollectManager
    {
        //http://www.bootply.com/85625
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/collect/img/start.jpg")));
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
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/collect/img/top.jpg")));
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
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/collect/img/end.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, endImageUrl));
            }
        }

        private static string GetCollectUrl(CollectInfo collectInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/collect/index.html")));
        }

        public static string GetCollectUrl(CollectInfo collectInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", collectInfo.PublishmentSystemID.ToString());
            attributes.Add("collectID", collectInfo.ID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetCollectUrl(collectInfo), attributes);
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<CollectInfo> collectInfoList = DataProviderWX.CollectDAO.GetCollectInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (CollectInfo collectInfo in collectInfoList)
            {
                Article article = null;

                if (collectInfo != null && collectInfo.StartDate < DateTime.Now)
                {
                    bool isEnd = false;

                    if (collectInfo.EndDate < DateTime.Now)
                    {
                        isEnd = true;
                    }

                    if (isEnd)
                    {
                        string endImageUrl = CollectManager.GetEndImageUrl(publishmentSystemInfo, collectInfo.EndImageUrl);

                        article = new Article()
                        {
                            Title = collectInfo.EndTitle,
                            Description = collectInfo.EndSummary,
                            PicUrl = endImageUrl
                        };
                    }
                    else
                    {
                        string imageUrl = CollectManager.GetImageUrl(publishmentSystemInfo, collectInfo.ImageUrl);
                        string pageUrl = CollectManager.GetCollectUrl(collectInfo, wxOpenID);

                        article = new Article()
                        {
                            Title = collectInfo.Title,
                            Description = collectInfo.Summary,
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

        public static void Vote(int publishmentSystemID, int collectID, int itemID, string ipAddress, string cookieSN, string wxOpenID, string userName)
        {
            DataProviderWX.CollectDAO.AddUserCount(collectID);
            DataProviderWX.CollectItemDAO.AddVoteNum(collectID, itemID);

            CollectLogInfo logInfo = new CollectLogInfo { ID = 0, PublishmentSystemID = publishmentSystemID, CollectID = collectID, ItemID = itemID, IPAddress = ipAddress, CookieSN = cookieSN, WXOpenID = wxOpenID, UserName = userName, AddDate = DateTime.Now };
            DataProviderWX.CollectLogDAO.Insert(logInfo);
        }
	}
}
