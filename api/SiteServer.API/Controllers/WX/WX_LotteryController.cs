using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using Senparc.Weixin.MP;
using SiteServer.API.Model.WX;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;



namespace SiteServer.API.Controllers.WX
{
    public class WX_LotteryController : ApiController
    {
        [HttpGet]
        [ActionName("GetLotteryParameter")]
        public IHttpActionResult GetLotteryParameter(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string uniqueID = RequestUtils.GetQueryString("wxOpenID");
                WeiXinManager.GetCookieWXOpenID(uniqueID);
                string wxOpenID = uniqueID;

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                DataProviderWX.LotteryDAO.AddPVCount(id);

                LotteryInfo lotteryInfo = DataProviderWX.LotteryDAO.GetLotteryInfo(id);
                List<LotteryAwardInfo> awardInfoList = DataProviderWX.LotteryAwardDAO.GetLotteryAwardInfoList(publishmentSystemInfo.PublishmentSystemID, id);
                ELotteryType lotteryType = ELotteryTypeUtils.GetEnumType(lotteryInfo.LotteryType);

                lotteryInfo.ContentImageUrl = LotteryManager.GetContentImageUrl(publishmentSystemInfo, lotteryType, lotteryInfo.ContentImageUrl);
                lotteryInfo.ContentAwardImageUrl = LotteryManager.GetContentAwardImageUrl(publishmentSystemInfo, lotteryType, lotteryInfo.ContentAwardImageUrl, awardInfoList.Count);
                lotteryInfo.ContentUsage = StringUtils.ReplaceNewlineToBR(lotteryInfo.ContentUsage);
                lotteryInfo.AwardImageUrl = LotteryManager.GetAwardImageUrl(publishmentSystemInfo, lotteryType, lotteryInfo.AwardImageUrl);
                lotteryInfo.AwardUsage = StringUtils.ReplaceNewlineToBR(lotteryInfo.AwardUsage);
                lotteryInfo.EndImageUrl = LotteryManager.GetEndImageUrl(publishmentSystemInfo, lotteryType, lotteryInfo.EndImageUrl);

                LotteryAwardInfo awardInfo = null;
                LotteryWinnerInfo winnerInfo = null;
                bool isSuccess = true;
                string errorMessage = string.Empty;

                if (lotteryType == ELotteryType.Scratch)
                {
                    isSuccess = LotteryManager.Lottery(lotteryInfo, awardInfoList, cookieSN, wxOpenID, out awardInfo, out winnerInfo, out errorMessage);
                }
                else
                {
                    winnerInfo = DataProviderWX.LotteryWinnerDAO.GetWinnerInfo(lotteryInfo.PublishmentSystemID, lotteryInfo.ID, cookieSN, wxOpenID, UserManager.Current.UserName);
                    if (winnerInfo != null)
                    {
                        awardInfo = DataProviderWX.LotteryAwardDAO.GetAwardInfo(winnerInfo.AwardID);
                    }
                }

                if (lotteryInfo.IsAwardTotalNum)
                {
                    foreach (LotteryAwardInfo theAwardInfo in awardInfoList)
                    {
                        if (theAwardInfo.TotalNum == 0)
                        {
                            theAwardInfo.TotalNum = 1;
                        }
                    }
                }

                bool isEnd = false;
                if (lotteryInfo.EndDate < DateTime.Now)
                {
                    isEnd = true;
                }

                LotteryParameter parameter = new LotteryParameter { IsSuccess = isSuccess, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, LotteryInfo = lotteryInfo, AwardInfoList = awardInfoList, AwardInfo = awardInfo, WinnerInfo = winnerInfo, IsEnd = isEnd };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                LotteryParameter parameter = new LotteryParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitLottery")]
        public IHttpActionResult SubmitLottery(int id)
        {
            try
            {
                int publishmentSystemID = RequestUtils.GetIntQueryString("publishmentSystemID");
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = WeiXinManager.GetCookieWXOpenID("");

                LotteryInfo lotteryInfo = DataProviderWX.LotteryDAO.GetLotteryInfo(id);
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                List<LotteryAwardInfo> awardInfoList = DataProviderWX.LotteryAwardDAO.GetLotteryAwardInfoList(lotteryInfo.PublishmentSystemID, lotteryInfo.ID);

                LotteryAwardInfo awardInfo = null;
                LotteryWinnerInfo winnerInfo = null;
                string errorMessage = string.Empty;
                bool isSuccess = LotteryManager.Lottery(lotteryInfo, awardInfoList, cookieSN, wxOpenID, out awardInfo, out winnerInfo, out errorMessage);

                decimal angle = 0;
                if (ELotteryTypeUtils.GetEnumType(lotteryInfo.LotteryType) == ELotteryType.BigWheel)
                {
                    if (awardInfo != null && winnerInfo != null)
                    {
                        int sequence = 0;
                        foreach (LotteryAwardInfo theAwardInfo in awardInfoList)
                        {
                            sequence++;
                            if (theAwardInfo.ID == awardInfo.ID)
                            {
                                break;
                            }
                        }
                        int totalNum = awardInfoList.Count + 1;

                        angle = 360 * (totalNum - sequence);
                        angle = angle / totalNum;
                    }
                }

                LotteryParameter parameter = new LotteryParameter { IsSuccess = isSuccess, ErrorMessage = errorMessage, AwardInfo = awardInfo, WinnerInfo = winnerInfo, Angle = angle };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                LotteryParameter parameter = new LotteryParameter { IsSuccess = false, IsPoweredBy = true, PoweredBy = string.Empty, ErrorMessage = ex.Message };
                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitApplication")]
        public IHttpActionResult SubmitApplication(int id)
        {
            try
            {
                int winnerID = RequestUtils.GetIntQueryString("winnerID");
                string realName = RequestUtils.GetQueryString("realName");
                string mobile = RequestUtils.GetQueryString("mobile");
                string email = RequestUtils.GetQueryString("email");
                string address = RequestUtils.GetQueryString("address");

                if (winnerID > 0)
                {
                    LotteryManager.AddApplication(winnerID, realName, mobile, email, address);
                }

                LotteryParameter parameter = new LotteryParameter { IsSuccess = true, ErrorMessage = string.Empty };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                LotteryParameter parameter = new LotteryParameter { IsSuccess = false, IsPoweredBy = true, PoweredBy = string.Empty, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitAwardCode")]
        public IHttpActionResult SubmitAwardCode(int id)
        {
            try
            {
                int winnerID = RequestUtils.GetIntQueryString("winnerID");

                bool isSuccess = false;
                string errorMessage = "对不起，您输入的兑奖密码不正确，请重新输入！";

                if (winnerID > 0)
                {
                    DataProviderWX.LotteryWinnerDAO.UpdateStatus(EWinStatus.Cashed, TranslateUtils.ToIntList(winnerID));
                    isSuccess = true;
                    errorMessage = string.Empty;
                }

                LotteryParameter parameter = new LotteryParameter { IsSuccess = isSuccess, ErrorMessage = errorMessage };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                LotteryParameter parameter = new LotteryParameter { IsSuccess = false, IsPoweredBy = true, PoweredBy = string.Empty, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }
    }
}
