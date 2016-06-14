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
    public class LotteryManager
    {
        public static string GetImageUrl(PublishmentSystemInfo publishmentSystemInfo, ELotteryType lotteryType, string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                string directoryName = "img" + ELotteryTypeUtils.GetValue(lotteryType);
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/lottery/" + directoryName + "/start.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl));
            }
        }

        public static string GetContentImageUrl(PublishmentSystemInfo publishmentSystemInfo, ELotteryType lotteryType, string contentImageUrl)
        {
            if (string.IsNullOrEmpty(contentImageUrl))
            {
                string directoryName = "img" + ELotteryTypeUtils.GetValue(lotteryType);
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/lottery/" + directoryName + "/content.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentImageUrl));
            }
        }

        public static string GetContentAwardImageUrl(PublishmentSystemInfo publishmentSystemInfo, ELotteryType lotteryType, string contentImageUrl, int awardCount)
        {
            if (string.IsNullOrEmpty(contentImageUrl))
            {
                string fileName = awardCount + ".png";
                if (awardCount < 2 || awardCount > 9)
                {
                    fileName = "contentAward.png";
                }
                string directoryName = "img" + ELotteryTypeUtils.GetValue(lotteryType);
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/lottery/" + directoryName + "/" + fileName)));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentImageUrl));
            }
        }

        public static string GetAwardImageUrl(PublishmentSystemInfo publishmentSystemInfo, ELotteryType lotteryType, string contentImageUrl)
        {
            if (string.IsNullOrEmpty(contentImageUrl))
            {
                string directoryName = "img" + ELotteryTypeUtils.GetValue(lotteryType);
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/lottery/" + directoryName + "/award.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentImageUrl));
            }
        }

        public static string GetEndImageUrl(PublishmentSystemInfo publishmentSystemInfo, ELotteryType lotteryType, string endImageUrl)
        {
            if (string.IsNullOrEmpty(endImageUrl))
            {
                string directoryName = "img" + ELotteryTypeUtils.GetValue(lotteryType);
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/lottery/" + directoryName + "/end.jpg")));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, endImageUrl));
            }
        }

        private static string GetLotteryUrl(LotteryInfo lotteryInfo)
        {
            string fileName = ELotteryTypeUtils.GetValue(ELotteryTypeUtils.GetEnumType(lotteryInfo.LotteryType)).ToLower();
            return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/lottery/" + fileName + ".html")));
        }

        public static string GetLotteryUrl(LotteryInfo lotteryInfo, string wxOpenID)
        {
            NameValueCollection attributes = new NameValueCollection();
            
            attributes.Add("lotteryID", lotteryInfo.ID.ToString());
            attributes.Add("publishmentSystemID", lotteryInfo.PublishmentSystemID.ToString());
            attributes.Add("wxOpenID", wxOpenID);
            return PageUtils.AddQueryString(GetLotteryUrl(lotteryInfo), attributes);
        }

        public static string GetLotteryTemplateDirectoryUrl()
        {
            return PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl("services/weixin/lottery"));
        }

        private static LotteryAwardInfo GetAwardInfo(List<LotteryAwardInfo> awardInfoList, int awardID)
        {
            LotteryAwardInfo awardInfo = null;
            foreach (LotteryAwardInfo lotteryAwardInfo in awardInfoList)
            {
                if (lotteryAwardInfo.ID == awardID)
                {
                    awardInfo = lotteryAwardInfo;
                    break;
                }
            }
            return awardInfo;
        }

        public static bool Lottery(LotteryInfo lotteryInfo, List<LotteryAwardInfo> awardInfoList, string cookieSN, string wxOpenID, out LotteryAwardInfo awardInfo, out LotteryWinnerInfo winnerInfo, out string errorMessage)
        {
            errorMessage = string.Empty;

            awardInfo = null;
            winnerInfo = DataProviderWX.LotteryWinnerDAO.GetWinnerInfo(lotteryInfo.PublishmentSystemID, lotteryInfo.ID, cookieSN, wxOpenID, UserManager.Current.UserName);
            if (winnerInfo != null)
            {
                awardInfo = LotteryManager.GetAwardInfo(awardInfoList, winnerInfo.AwardID);
            }
            else
            {
                bool isMaxCount = false;
                bool isMaxDailyCount = false;

                DataProviderWX.LotteryLogDAO.AddCount(lotteryInfo.PublishmentSystemID, lotteryInfo.ID, cookieSN, wxOpenID, UserManager.Current.UserName, lotteryInfo.AwardMaxCount, lotteryInfo.AwardMaxDailyCount, out isMaxCount, out isMaxDailyCount);

                if (isMaxCount)
                {
                    errorMessage = string.Format("对不起，每人最多允许抽奖{0}次", lotteryInfo.AwardMaxCount);
                    return false;
                }
                else if (isMaxDailyCount)
                {
                    errorMessage = string.Format("对不起，每人每天最多允许抽奖{0}次", lotteryInfo.AwardMaxDailyCount);
                    return false;
                }
                else
                {
                    if (awardInfoList != null && awardInfoList.Count > 0)
                    {
                        Dictionary<int, decimal> idWithProbabilityDictionary = new Dictionary<int, decimal>();
                        foreach (LotteryAwardInfo lotteryAwardInfo in awardInfoList)
                        {
                            idWithProbabilityDictionary.Add(lotteryAwardInfo.ID, lotteryAwardInfo.Probability);
                        }

                        int awardID = WeiXinManager.Lottery(idWithProbabilityDictionary);
                        if (awardID > 0)
                        {
                            LotteryAwardInfo lotteryAwardInfo = LotteryManager.GetAwardInfo(awardInfoList, awardID);

                            if (lotteryAwardInfo != null && lotteryAwardInfo.TotalNum > 0)
                            {
                                int wonNum = DataProviderWX.LotteryWinnerDAO.GetTotalNum(awardID);
                                if (lotteryAwardInfo.TotalNum > wonNum)
                                {
                                    awardInfo = lotteryAwardInfo;
                                    winnerInfo = new LotteryWinnerInfo { PublishmentSystemID = lotteryInfo.PublishmentSystemID, LotteryType = lotteryInfo.LotteryType, LotteryID = lotteryInfo.ID, AwardID = awardID, Status = EWinStatusUtils.GetValue(EWinStatus.Won), CookieSN = cookieSN, WXOpenID = wxOpenID, UserName = UserManager.Current.UserName, AddDate = DateTime.Now };
                                    winnerInfo.ID = DataProviderWX.LotteryWinnerDAO.Insert(winnerInfo);

                                    DataProviderWX.LotteryAwardDAO.UpdateWonNum(awardID);

                                    DataProviderWX.LotteryDAO.AddUserCount(winnerInfo.LotteryID);
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public static void AddApplication(int winnerID, string realName, string mobile, string email, string address)
        {
            LotteryWinnerInfo winnerInfo = DataProviderWX.LotteryWinnerDAO.GetWinnerInfo(winnerID);

            string oldCashSN = winnerInfo.CashSN;

            winnerInfo.RealName = realName;
            winnerInfo.Mobile = mobile;
            winnerInfo.Email = email;
            winnerInfo.Address = address;
            winnerInfo.Status = EWinStatusUtils.GetValue(EWinStatus.Applied);
            winnerInfo.CashSN = StringUtils.GetShortGUID(true);

            if (string.IsNullOrEmpty(oldCashSN))
            {
                DataProviderWX.LotteryWinnerDAO.Update(winnerInfo);
            }
        }

        public static List<Article> Trigger(SiteServer.WeiXin.Model.KeywordInfo keywordInfo, ELotteryType lotteryType, string requestFromUserName)
        {
            List<Article> articleList = new List<Article>();

            DataProviderWX.CountDAO.AddCount(keywordInfo.PublishmentSystemID, SiteServer.WeiXin.Model.ECountType.RequestNews);

            List<LotteryInfo> lotteryInfoList = DataProviderWX.LotteryDAO.GetLotteryInfoListByKeywordID(keywordInfo.PublishmentSystemID, lotteryType, keywordInfo.KeywordID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(keywordInfo.PublishmentSystemID);

            foreach (LotteryInfo lotteryInfo in lotteryInfoList)
            {
                Article article = null;

                if (lotteryInfo != null && lotteryInfo.StartDate < DateTime.Now)
                {
                    bool isEnd = false;

                    if (lotteryInfo.EndDate < DateTime.Now)
                    {
                        isEnd = true;
                    }

                    if (isEnd)
                    {
                        string endImageUrl = LotteryManager.GetEndImageUrl(publishmentSystemInfo, lotteryType, lotteryInfo.EndImageUrl);

                        article = new Article()
                        {
                            Title = lotteryInfo.EndTitle,
                            Description = lotteryInfo.EndSummary,
                            PicUrl = endImageUrl
                        };
                    }
                    else
                    {
                        string imageUrl = LotteryManager.GetImageUrl(publishmentSystemInfo, lotteryType, lotteryInfo.ImageUrl);
                        string pageUrl = LotteryManager.GetLotteryUrl(lotteryInfo, requestFromUserName);

                        article = new Article()
                        {
                            Title = lotteryInfo.Title,
                            Description = lotteryInfo.Summary,
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
