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
	public class AppointmentManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/appointment/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetItemTopImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl(string.Format("services/weixin/appointment/img/item.jpg"))));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl(string.Format("services/weixin/appointment/img/top.jpg"))));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentResultTopImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl(string.Format("services/weixin/appointment/img/result.jpg"))));
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
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/appointment/img/end.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, endImageUrl));
            }
        }

        public static string GetIndexUrl(int publishmentSystemID, int appointmentID, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", publishmentSystemID.ToString());
            attributes.Add("appointmentID", appointmentID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            string url = APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/appointment/index.html")));
            return PageUtils.AddQueryString(url, attributes);
        }

        public static string GetItemUrl(int publishmentSystemID, int appointmentID, int itemID, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", publishmentSystemID.ToString());
            attributes.Add("appointmentID", appointmentID.ToString());
            attributes.Add("itemID", itemID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            string url = APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/appointment/item.html")));
            return PageUtils.AddQueryString(url, attributes);
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<AppointmentInfo> appointmentInfoList = DataProviderWX.AppointmentDAO.GetAppointmentInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (AppointmentInfo appointmentInfo in appointmentInfoList)
            {
                Article article = null;

                if (appointmentInfo != null && appointmentInfo.StartDate < DateTime.Now)
                {
                    bool isEnd = false;

                    if (appointmentInfo.EndDate < DateTime.Now)
                    {
                        isEnd = true;
                    }

                    if (isEnd)
                    {
                        string endImageUrl = AppointmentManager.GetEndImageUrl(publishmentSystemInfo, appointmentInfo.EndImageUrl);

                        article = new Article()
                        {
                            Title = appointmentInfo.EndTitle,
                            Description = appointmentInfo.EndSummary,
                            PicUrl = endImageUrl
                        };
                    }
                    else
                    {
                        string imageUrl = AppointmentManager.GetImageUrl(publishmentSystemInfo, appointmentInfo.ImageUrl);
                        string pageUrl = AppointmentManager.GetIndexUrl(publishmentSystemInfo.PublishmentSystemID, appointmentInfo.ID, wxOpenID);
                        if (appointmentInfo.ContentIsSingle)
                        {
                            int itemID = DataProviderWX.AppointmentItemDAO.GetItemID(publishmentSystemInfo.PublishmentSystemID, appointmentInfo.ID);
                            pageUrl = AppointmentManager.GetItemUrl(publishmentSystemInfo.PublishmentSystemID, appointmentInfo.ID, itemID, wxOpenID);
                        }

                        article = new Article()
                        {
                            Title = appointmentInfo.Title,
                            Description = appointmentInfo.Summary,
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
