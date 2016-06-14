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
    public class CouponManager
    {

        public static List<string> GetCouponSN(int totalNum)
        {
            List<string> list = new List<string>();

            for (int i = 1; i <= totalNum; i++)
            {
                list.Add(StringUtils.GetShortGUID(true));
            }

            return list;
        }

        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/coupon/images/start.jpg")));
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
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/coupon/images/content.jpg")));
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
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/coupon/images/end.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, endImageUrl));
            }
        }

        public static string GetCouponUrl(PublishmentSystemInfo publishmentSystemInfo)
        {
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/coupon/application.html")));
        }

        public static string GetCouponHoldUrl(PublishmentSystemInfo publishmentSystemInfo, int keywordID, int actID, int snID, string keyword)
        {
            NameValueCollection attributes = new NameValueCollection();
            attributes.Add("snID", snID.ToString());
            attributes.Add("keyword", keyword);
            return PageUtils.AddQueryString(GetCouponUrl(publishmentSystemInfo), attributes);
        }
        public static string GetCouponHoldUrl(PublishmentSystemInfo publishmentSystemInfo, int actID)
        {
            NameValueCollection attributes = new NameValueCollection();

            attributes.Add("actID", actID.ToString());
            attributes.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());

            return PageUtils.AddQueryString(GetCouponUrl(publishmentSystemInfo), attributes);
        }

        public static string GetCouponActTemplateDirectoryUrl()
        {
            return PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/coupon"));
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, string keyword, string requestFromUserName)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<CouponActInfo> actInfoList = DataProviderWX.CouponActDAO.GetActInfoListByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (CouponActInfo actInfo in actInfoList)
            {
                Article article = null;

                if (actInfo != null && actInfo.StartDate < DateTime.Now)
                {
                    bool isEnd = false;
                    int snID = 0;

                    if (actInfo.EndDate < DateTime.Now)
                    {
                        isEnd = true;
                    }
                    else
                    {
                        snID = DataProviderWX.CouponSNDAO.Hold(keywordInfo.PublishmentSystemID, actInfo.ID, requestFromUserName);
                        if (snID == 0)
                        {
                            isEnd = true;
                        }
                    }

                    if (isEnd)
                    {
                        string endImageUrl = CouponManager.GetEndImageUrl(publishmentSystemInfo, actInfo.EndImageUrl);

                        article = new Article()
                        {
                            Title = actInfo.EndTitle,
                            Description = actInfo.EndSummary,
                            PicUrl = endImageUrl
                        };
                    }
                    else
                    {
                        string imageUrl = CouponManager.GetImageUrl(publishmentSystemInfo, actInfo.ImageUrl);
                        string pageUrl = CouponManager.GetCouponHoldUrl(publishmentSystemInfo, actInfo.ID);

                        article = new Article()
                        {
                            Title = actInfo.Title,
                            Description = actInfo.Summary,
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

        public static int AddApplication(int publishmentSystemID, int actID, string uniqueID, string realName, string mobile, string email, string address)
        {
            try
            {
                List<CouponInfo> couponInfoList = DataProviderWX.CouponDAO.GetCouponInfoList(publishmentSystemID, actID);

                int snID = 0;
                int couponID = 0;
                string cookieSN = WeiXinManager.GetCookieSN();
                snID = DataProviderWX.CouponSNDAO.Hold(publishmentSystemID, actID, cookieSN);

                CouponSNInfo newCouponSNInfo = DataProviderWX.CouponSNDAO.GetSNInfo(snID);
                couponID = newCouponSNInfo.CouponID;

                CouponSNInfo couponSNInfo = new CouponSNInfo();

                couponSNInfo.PublishmentSystemID = publishmentSystemID;
                couponSNInfo.CookieSN = cookieSN;
                couponSNInfo.CouponID = couponID;
                couponSNInfo.ID = snID;
                couponSNInfo.HoldDate = DateTime.Now;
                couponSNInfo.HoldRealName = realName;
                couponSNInfo.HoldMobile = mobile;
                couponSNInfo.HoldEmail = email;
                couponSNInfo.HoldAddress = address;
                couponSNInfo.CashDate = DateTime.Now;
                couponSNInfo.Status = ECouponStatusUtils.GetValue(ECouponStatus.Hold);
                couponSNInfo.WXOpenID = uniqueID;

                if (newCouponSNInfo.Status == ECouponStatusUtils.GetValue(ECouponStatus.Cash))
                {
                    couponSNInfo.Status = ECouponStatusUtils.GetValue(ECouponStatus.Cash);
                }
                DataProviderWX.CouponSNDAO.Update(couponSNInfo);

                return newCouponSNInfo.ID;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}
