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
	public class View360Manager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/view360/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl, int sequence)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl(string.Format("services/weixin/view360/img/pic{0}.jpg", sequence))));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        private static string GetView360Url(View360Info view360Info)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/view360/index.html")));
        }

        public static string GetView360Url(View360Info view360Info, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", view360Info.PublishmentSystemID.ToString());
            attributes.Add("view360ID", view360Info.ID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetView360Url(view360Info), attributes);
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<View360Info> view360InfoList = DataProviderWX.View360DAO.GetView360InfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (View360Info view360Info in view360InfoList)
            {
                Article article = null;

                if (view360Info != null)
                {
                    string imageUrl = View360Manager.GetImageUrl(publishmentSystemInfo, view360Info.ImageUrl);
                    string pageUrl = View360Manager.GetView360Url(view360Info, wxOpenID);

                    article = new Article()
                    {
                        Title = view360Info.Title,
                        Description = view360Info.Summary,
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
	}
}
