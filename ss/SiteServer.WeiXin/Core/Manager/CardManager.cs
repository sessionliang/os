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
	public class CardManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/card/img/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentFrontImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/card/img/front.png")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentBackImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/card/img/back.png")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        private static string GetCardUrl(CardInfo CardInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/card/index.html")));
        }

        public static string GetCardUrl(CardInfo CardInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("publishmentSystemID", CardInfo.PublishmentSystemID.ToString());
            attributes.Add("cardID", CardInfo.ID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetCardUrl(CardInfo), attributes);
        }

        public static int GetSignCredits(PublishmentSystemInfo publishmentSystemInfo, string userName)
        {
            int credits = 0;
            string signCreditsConfigure = publishmentSystemInfo.Additional.Card_SignCreditsConfigure;
            if (!string.IsNullOrEmpty(signCreditsConfigure))
            {
                int days = 0;
                DateTime curDateTime = DateTime.Now;
                List<DateTime> signDateList = DataProviderWX.CardSignLogDAO.GetSignDateList(publishmentSystemInfo.PublishmentSystemID, userName);
                foreach (DateTime dateTime in signDateList)
                {
                    days++;
                    curDateTime= curDateTime.AddDays(-1);
                    if ((curDateTime.Day - dateTime.Day) > 0)
                    { 
                        break;
                    }
               }

                int index = 0;
                ArrayList configureInfoArrayList = TranslateUtils.StringCollectionToArrayList(signCreditsConfigure);
                foreach (string configureInfo in configureInfoArrayList)
                {
                    if (!string.IsNullOrEmpty(configureInfo))
                    {
                        index++;
                        int dayFrom = TranslateUtils.ToInt(configureInfo.Split('&')[0]);
                        int dayTo = TranslateUtils.ToInt(configureInfo.Split('&')[1]);
                        int singCredits = TranslateUtils.ToInt(configureInfo.Split('&')[2]);
                        if (days == 0)
                        {
                            credits = singCredits;
                            break;
                        }
                        else if (days >= dayFrom && days < dayTo)
                        {
                            credits = singCredits;
                            break;
                        }
                        else if (days > dayTo)
                        {
                            if (index == configureInfoArrayList.Count)
                            {
                                credits = singCredits;
                            }
                        }
                    }
                }
            }

            return credits;

        }
        
        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string wxOpenID)
        { 
            List<Article> articleList = new List<Article>();
             
            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<CardInfo> CardInfoList = DataProviderWX.CardDAO.GetCardInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (CardInfo CardInfo in CardInfoList)
            {
                Article article = null;

                if (CardInfo != null)
                {
                    string imageUrl = CardManager.GetImageUrl(publishmentSystemInfo, CardInfo.ImageUrl);
                    string pageUrl = CardManager.GetCardUrl(CardInfo, wxOpenID);

                    article = new Article()
                    {
                        Title = CardInfo.Title,
                        Description = CardInfo.Summary,
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
