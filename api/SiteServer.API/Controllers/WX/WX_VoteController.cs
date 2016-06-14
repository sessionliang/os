using BaiRong.Core;
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
    public class WX_VoteController : ApiController
    {
        [HttpGet]
        [ActionName("GetVoteParameter")]
        public IHttpActionResult GetVoteParameter(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string uniqueID = RequestUtils.GetQueryString("wxOpenID");
                WeiXinManager.GetCookieWXOpenID(uniqueID);
                string wxOpenID = uniqueID;

                DataProviderWX.VoteDAO.AddPVCount(id);

                VoteInfo voteInfo = DataProviderWX.VoteDAO.GetVoteInfo(id);

                bool isVoted = DataProviderWX.VoteLogDAO.IsVoted(id, cookieSN, wxOpenID);

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(voteInfo.PublishmentSystemID);
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                voteInfo.ContentImageUrl = VoteManager.GetContentImageUrl(publishmentSystemInfo, voteInfo.ContentImageUrl);
                voteInfo.EndImageUrl = VoteManager.GetEndImageUrl(publishmentSystemInfo, voteInfo.EndImageUrl);
                if (string.IsNullOrEmpty(voteInfo.ContentDescription))
                {
                    int count = DataProviderWX.VoteLogDAO.GetCount(voteInfo.ID);
                    voteInfo.ContentDescription = string.Format("点击选择投票项目（{0}），共有{1}人参与投票", voteInfo.ContentIsCheckBox ? "可多选" : "单选", count == 0 ? 1 : count);
                }

                List<VoteItemInfo> itemInfoList = DataProviderWX.VoteItemDAO.GetVoteItemInfoList(id);
                foreach (VoteItemInfo itemInfo in itemInfoList)
                {
                    itemInfo.ImageUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.ImageUrl));
                    itemInfo.NavigationUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.NavigationUrl));
                }

                bool isEnd = false;
                if (voteInfo.EndDate < DateTime.Now)
                {
                    isEnd = true;
                }

                VoteParameter parameter = new VoteParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, IsVoted = isVoted, VoteInfo = voteInfo, ItemList = itemInfoList, IsEnd = isEnd };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                VoteParameter parameter = new VoteParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitVote")]
        public IHttpActionResult SubmitVote(int id)
        {
            try
            {
                VoteParameter parameter;
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = WeiXinManager.GetCookieWXOpenID("");
                string itemIDCollection = RequestUtils.GetQueryString("itemIDCollection");
                string realName = RequestUtils.GetQueryString("realName");
                string agentsType = RequestUtils.GetQueryString("agentsType");

                if (string.IsNullOrEmpty(wxOpenID))
                {
                    parameter = new VoteParameter { IsSuccess = false, ErrorMessage = "请在微信客户端进行投票." };

                    return Ok(parameter);
                }

                if (string.IsNullOrEmpty(realName))
                {
                    realName = "匿名";
                }

                if (agentsType == "false")
                {
                    string iPAddress = PageUtils.GetIPAddress();
                    string dateTime = DateTime.Now.ToString();


                    int ipVoteCount = DataProviderWX.VoteLogDAO.GetCount(id, iPAddress);
                    if (ipVoteCount > 0)
                    {
                        parameter = new VoteParameter { IsSuccess = false, ErrorMessage = "请不要重复的参与投票." };

                        return Ok(parameter);
                    }

                    string oldDateTime = DbCacheManager.Get("SiteServer.API.Controllers.WX.SubmitVote.dateTime");

                    if (oldDateTime == "")
                    {
                        DbCacheManager.Insert("SiteServer.API.Controllers.WX.SubmitVote.dateTime", dateTime);
                    }
                    else
                    {
                        DateTime timeFir = Convert.ToDateTime(oldDateTime);
                        DateTime timeSec = Convert.ToDateTime(dateTime);

                        TimeSpan sub = timeSec.Subtract(timeFir);
                        //int minutes = sub.Minutes;
                        //if (minutes < 1)
                        //{
                        //    parameter = new VoteParameter { IsSuccess = false, ErrorMessage = "投票时间间隔太短,请稍后重试." };

                        //    return Ok(parameter);
                        //}

                        DbCacheManager.Insert("SiteServer.API.Controllers.WX.SubmitVote.dateTime", dateTime);
                    }

                    VoteInfo voteInfo = DataProviderWX.VoteDAO.GetVoteInfo(id);

                    VoteManager.Vote(voteInfo.PublishmentSystemID, id, TranslateUtils.StringCollectionToIntList(itemIDCollection), PageUtils.GetIPAddress(), cookieSN, wxOpenID, realName);

                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(voteInfo.PublishmentSystemID);
                    voteInfo.ContentImageUrl = VoteManager.GetContentImageUrl(publishmentSystemInfo, voteInfo.ContentImageUrl);

                    if (string.IsNullOrEmpty(voteInfo.ContentDescription))
                    {
                        int count = DataProviderWX.VoteLogDAO.GetCount(voteInfo.ID);
                        voteInfo.ContentDescription = string.Format("点击选择投票项目（{0}），共有{1}人参与投票", voteInfo.ContentIsCheckBox ? "可多选" : "单选", count == 0 ? 1 : count);
                    }

                    List<VoteItemInfo> itemInfoList = DataProviderWX.VoteItemDAO.GetVoteItemInfoList(id);
                    foreach (VoteItemInfo itemInfo in itemInfoList)
                    {
                        itemInfo.ImageUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.ImageUrl));
                        itemInfo.NavigationUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.NavigationUrl));
                    }

                    parameter = new VoteParameter { IsSuccess = true, IsVoted = true, ErrorMessage = string.Empty, VoteInfo = voteInfo, ItemList = itemInfoList };

                    return Ok(parameter);
                }
                else
                {
                    parameter = new VoteParameter { IsSuccess = false, ErrorMessage = "请不要在PC端进行投票." };

                    return Ok(parameter);
                }
            }
            catch (Exception ex)
            {
                VoteParameter parameter = new VoteParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }
    }
}
