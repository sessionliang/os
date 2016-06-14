using BaiRong.Core;
using BaiRong.Model;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SiteServer.API.Controllers.CMS
{
    public class CommentController : ApiController
    {
        [HttpGet]
        [ActionName("GetCommentParameter")]
        public IHttpActionResult GetCommentParameter()
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                if (publishmentSystemInfo != null)
                {
                    int channelID = RequestUtils.GetIntQueryString("channelID");
                    int contentID = RequestUtils.GetIntQueryString("contentID");

                    int totalNum = DataProvider.CommentDAO.GetCountChecked(RequestUtils.PublishmentSystemID, channelID, contentID);

                    List<CommentInfo> commentInfoList = DataProvider.CommentDAO.GetCommentInfoListChecked(RequestUtils.PublishmentSystemID, channelID, contentID);
                    List<object> list = new List<object>();
                    int i = commentInfoList.Count;
                    foreach (CommentInfo commentInfo in commentInfoList)
                    {
                        string addDate = DateUtils.GetRelatedDateTimeString(commentInfo.AddDate, "前");
                        string dateTime = DateUtils.GetDateAndTimeString(commentInfo.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                        string userName = string.IsNullOrEmpty(commentInfo.UserName) ? "匿名用户" : commentInfo.UserName;
                        string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, commentInfo.UserName, EAvatarSize.Small));
                        int floor = i--;
                        string content = TranslateUtils.ParseCommentContent(commentInfo.Content);

                        list.Add(new { CommentID = commentInfo.CommentID, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl, IsRecommend = commentInfo.IsRecommend, Good = commentInfo.Good, Floor = floor, Content = content });
                    }

                    bool isAnonymous = BaiRongDataProvider.UserDAO.IsAnonymous;

                    User user = SiteServer.API.Model.User.GetInstance();

                    var parameter = new { IsSuccess = true, TotalNum = totalNum, IsAnonymous = isAnonymous, User = user, Comments = list };

                    return Ok(parameter);
                }
            }
            catch (Exception ex)
            {
                return Ok(new { IsSuccess = false, ErrorMessage = ex.Message });
            }

            return Ok(new { IsSuccess = false });
        }

        [HttpGet]
        [ActionName("Good")]
        public IHttpActionResult Good(int id)
        {
            DataProvider.CommentDAO.AddGood(RequestUtils.PublishmentSystemID, id);
            return Ok(new { IsSuccess = true });
        }

        [HttpGet]
        [ActionName("SubmitComment")]
        public IHttpActionResult SubmitComment()
        {
            int channelID = RequestUtils.GetIntQueryString("channelID");
            int contentID = RequestUtils.GetIntQueryString("contentID");
            string content = RequestUtils.GetQueryStringNoSqlAndXss("content");

            //!RequestUtils.IsAnonymous && 评论允许匿名评论
            if (RequestUtils.PublishmentSystemInfo != null)
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;

                try
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);
                    if (nodeInfo != null)
                    {
                        CommentInfo commentInfo = new CommentInfo();

                        commentInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                        commentInfo.NodeID = channelID;
                        commentInfo.ContentID = contentID;
                        commentInfo.AddDate = DateTime.Now;
                        commentInfo.IPAddress = PageUtils.GetIPAddress();
                        commentInfo.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                        commentInfo.IsChecked = true;
                        commentInfo.Content = content;

                        int commentID = DataProvider.CommentDAO.Insert(publishmentSystemInfo.PublishmentSystemID, commentInfo);

                        string message = "评论添加成功。";

                        string addDate = DateUtils.GetRelatedDateTimeString(commentInfo.AddDate, "前");
                        string dateTime = DateUtils.GetDateAndTimeString(commentInfo.AddDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                        string userName = string.IsNullOrEmpty(commentInfo.UserName) ? "匿名用户" : commentInfo.UserName;
                        string avatarUrl = APIPageUtils.ParseUrl(PageUtils.GetUserAvatarUrl(publishmentSystemInfo.GroupSN, commentInfo.UserName, EAvatarSize.Small));
                        int floor = 1;
                        content = TranslateUtils.ParseCommentContent(commentInfo.Content);

                        var comment = new { CommentID = commentID, AddDate = addDate, DateTime = dateTime, UserName = userName, AvatarUrl = avatarUrl, IsRecommend = commentInfo.IsRecommend, Good = commentInfo.Good, Floor = floor, Content = content };

                        var parameter = new { IsSuccess = true, SuccessMessage = message, Comment = comment };

                        return Ok(parameter);
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { IsSuccess = false, ErrorMessage = ex.Message });
                }
            }

            return Ok(new { IsSuccess = false });
        }
    }
}
