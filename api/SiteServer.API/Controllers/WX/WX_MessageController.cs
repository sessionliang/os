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


using MessageManager = SiteServer.WeiXin.Core.MessageManager;

namespace SiteServer.API.Controllers.WX
{
    public class WX_MessageController : ApiController
    {
        private const int PAGE_COUNT = 10;
        private MessageParameter GetMessageParameter(MessageInfo messageInfo)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(messageInfo.PublishmentSystemID);
            string poweredBy = string.Empty;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            messageInfo.ContentImageUrl = MessageManager.GetContentImageUrl(publishmentSystemInfo, messageInfo.ContentImageUrl);
            int totalNum = DataProviderWX.MessageContentDAO.GetCount(messageInfo.ID, false);
            if (string.IsNullOrEmpty(messageInfo.ContentDescription) && totalNum > 0)
            {
                messageInfo.ContentDescription = string.Format("已有<span>{0}</span>人发表留言", totalNum);
                int count = DataProviderWX.MessageContentDAO.GetCount(messageInfo.ID, true);
                if (count > 0)
                {
                    messageInfo.ContentDescription += string.Format("，<span>{0}</span>人发表评论", count);
                }
            }

            List<MessageContentInfo> contentInfoList = DataProviderWX.MessageContentDAO.GetContentInfoList(messageInfo.ID, 0, PAGE_COUNT);
            foreach (MessageContentInfo contentInfo in contentInfoList)
            {
                if (contentInfo.ReplyCount > 0)
                {
                    contentInfo.ReplyContentInfoList = DataProviderWX.MessageContentDAO.GetReplyContentInfoList(messageInfo.ID, contentInfo.ID);
                    foreach (MessageContentInfo replyContentInfo in contentInfo.ReplyContentInfoList)
                    {
                        replyContentInfo.Content = MPUtils.ParseEmotions(replyContentInfo.Content);
                    }
                }
                contentInfo.Content = MPUtils.ParseEmotions(contentInfo.Content);
            }

            bool isMore = totalNum > contentInfoList.Count ? true : false;

            return new MessageParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, MessageInfo = messageInfo, ContentInfoList = contentInfoList, IsMore = isMore };
        }

        [HttpGet]
        [ActionName("More")]
        public IHttpActionResult More(int id)
        {
            try
            {
                int startNum = RequestUtils.GetIntQueryString("startNum");

                MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(id);

                List<MessageContentInfo> contentInfoList = DataProviderWX.MessageContentDAO.GetContentInfoList(messageInfo.ID, startNum + 1, PAGE_COUNT);
                foreach (MessageContentInfo contentInfo in contentInfoList)
                {
                    if (contentInfo.ReplyCount > 0)
                    {
                        contentInfo.ReplyContentInfoList = DataProviderWX.MessageContentDAO.GetReplyContentInfoList(messageInfo.ID, contentInfo.ID);
                        foreach (MessageContentInfo replyContentInfo in contentInfo.ReplyContentInfoList)
                        {
                            replyContentInfo.Content = MPUtils.ParseEmotions(replyContentInfo.Content);
                        }
                    }
                    contentInfo.Content = MPUtils.ParseEmotions(contentInfo.Content);
                }

                int totalNum = DataProviderWX.MessageContentDAO.GetCount(messageInfo.ID, false);
                bool isMore = totalNum > startNum + contentInfoList.Count ? true : false;

                MessageParameter parameter = new MessageParameter { IsSuccess = true, ContentInfoList = contentInfoList, IsMore = isMore };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                MessageParameter parameter = new MessageParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetMessageParameter")]
        public IHttpActionResult GetMessageParameter(int id)
        {
            try
            {
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

                DataProviderWX.MessageDAO.AddPVCount(id);

                MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(id);

                MessageParameter parameter = this.GetMessageParameter(messageInfo);

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                MessageParameter parameter = new MessageParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitContent")]
        public IHttpActionResult SubmitContent(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");
                string displayName = RequestUtils.GetQueryString("displayName");
                string color = RequestUtils.GetQueryString("color");
                string content = RequestUtils.GetQueryString("content");

                MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(id);

                MessageManager.AddContent(messageInfo.PublishmentSystemID, id, displayName, color, content, PageUtils.GetIPAddress(), cookieSN, wxOpenID, UserManager.Current.UserName);

                MessageParameter parameter = this.GetMessageParameter(messageInfo);

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                MessageParameter parameter = new MessageParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitReply")]
        public IHttpActionResult SubmitReply(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                int replyContentID = RequestUtils.GetIntQueryString("replyContentID");
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");
                string displayName = RequestUtils.GetQueryString("displayName");
                string content = RequestUtils.GetQueryString("content");

                MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(id);

                MessageManager.AddReply(messageInfo.PublishmentSystemID, id, replyContentID, displayName, content, PageUtils.GetIPAddress(), cookieSN, wxOpenID, UserManager.Current.UserName);

                MessageParameter parameter = this.GetMessageParameter(messageInfo);

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                MessageParameter parameter = new MessageParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitLike")]
        public IHttpActionResult SubmitLike(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                int contentID = RequestUtils.GetIntQueryString("contentID");
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

                MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(id);

                bool isAdd = DataProviderWX.MessageContentDAO.AddLikeCount(contentID, cookieSN, wxOpenID);

                MessageParameter parameter = new MessageParameter { IsSuccess = isAdd };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                MessageParameter parameter = new MessageParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }
    }
}
