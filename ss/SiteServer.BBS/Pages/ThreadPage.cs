using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser;
using System.Collections.Specialized;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using BaiRong.Model;

namespace SiteServer.BBS.Pages
{
    public class ThreadPage : BasePage
    {
        protected int publishmentSystemID;
        protected int forumID;
        protected int threadID;
        protected int postID;
        protected ForumInfo forumInfo;
        protected ThreadInfo threadInfo;
        protected System.Web.UI.WebControls.Image imgIdentify;
       
        public Repeater rptPosts;
        protected int pageNum;
        protected PagerInfo pagerInfo;

        private bool isModerator;
        private int startNum;

        #region poll

        protected PollInfo pollInfo;
        protected bool isPolled;

        #endregion

        public void Page_Load(object sender, EventArgs e)
        {
            this.forumID = base.GetIntQueryString("forumID");
            this.threadID = base.GetIntQueryString("threadID");

            UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
            if (groupInfo.Additional.IsAllowRead)
            {
                base.OnInit(e);
            }
            else
            {
                string redirectUrl = LoginPage.GetLoginUrl(base.PublishmentSystemID, AccessManager.GetErrorMessage(EPermission.View, groupInfo), base.Request.RawUrl);
                PageUtils.Redirect(redirectUrl);
            }

            this.threadInfo = DataProvider.ThreadDAO.GetThreadInfo(base.PublishmentSystemID, threadID);
            if (threadInfo != null)
            {
                GetIdentifyImg(threadInfo.IdentifyID);

            }
            if (threadInfo == null || threadInfo.ForumID < 0)
            {
                Page.Response.Redirect(PageUtilityBBS.GetBBSUrl(base.PublishmentSystemID, "errorPage.htm"));
            }
            this.forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);
            if (this.forumInfo != null)
            {
                string redirectUrl = string.Empty;
                bool isViewable = AccessManager.IsViewable(base.PublishmentSystemID, this.forumInfo, this.Page.Request.RawUrl, out redirectUrl);
                if (isViewable)
                {
                    this.isModerator = BBSUserManager.IsModerator(this.forumInfo);
                    this.threadInfo = DataProvider.ThreadDAO.GetThreadInfo(base.PublishmentSystemID, this.threadID);
                    this.pageNum = TranslateUtils.ToInt(base.Request.QueryString["page"], 1);

                    string orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Post, EBBSTaxisType.OrderByTaxis);
                    this.startNum = (this.pageNum - 1) * base.Additional.PostPageNum + 1;
                    rptPosts.DataSource = DataUtility.GetPostsDataSource(base.PublishmentSystemID, this.threadID, startNum, base.Additional.PostPageNum, false, false, orderByString, string.Empty);
                    rptPosts.ItemDataBound += new RepeaterItemEventHandler(rptPosts_ItemDataBound);
                    rptPosts.DataBind();

                    if (this.pageNum == 1)
                    {
                        DataProvider.ThreadDAO.AddHits(this.threadID);
                        ThreadManager.MarkOldThread(this.threadInfo);
                    }
                    string urlFormat = PageUtilityBBS.GetThreadUrlFormat(base.PublishmentSystemID, this.forumID, this.threadID);
                    this.pagerInfo = PagerInfo.GetPagerInfo(this.threadInfo.Replies + 1, base.Additional.PostPageNum, base.Request, urlFormat);
                }
                else
                {
                    PageUtils.Redirect(redirectUrl);
                }
            }
            if (this.threadInfo == null)
            {
                this.threadInfo = new ThreadInfo(this.publishmentSystemID);
            }
        }

        private string GetPoll()
        {
            this.pollInfo = DataProvider.PollDAO.GetPollInfo(this.threadID);
            if (!BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                this.isPolled = DataProvider.PollDAO.IsUserExists(this.pollInfo.ID, BaiRongDataProvider.UserDAO.CurrentUserName);
            }
            ArrayList pollItems = DataProvider.PollDAO.GetPollItemInfoArrayList(this.pollInfo.ID);
            ArrayList pollItemIDArrayList = new ArrayList();
            if (this.isPolled)
            {
                pollItemIDArrayList = DataProvider.PollDAO.GetPollItemIDArrayList(this.pollInfo.ID, BaiRongDataProvider.UserDAO.CurrentUserName);
            }
            StringBuilder builder = new StringBuilder();
            int totalNum = 0;
            foreach (PollItemInfo itemInfo in pollItems)
            {
                totalNum += itemInfo.Num;
            }

            int index = 1;
            foreach (PollItemInfo itemInfo in pollItems)
            {
                int px = 0;
                int percent = 0;
                if (totalNum > 0)
                {
                    if (pollInfo.IsVoteFirst)
                    {
                        if (isPolled)
                        {
                            px = (itemInfo.Num * 180) / totalNum;
                            percent = (itemInfo.Num * 100) / totalNum;
                        }
                    }
                    else
                    {
                        px = (itemInfo.Num * 180) / totalNum;
                        percent = (itemInfo.Num * 100) / totalNum;
                    }
                }
                if (px == 0)
                {
                    itemInfo.Num = 0;
                }
                string select = string.Empty;
                if (pollItemIDArrayList.Contains(itemInfo.ID))
                {
                    select = @"<span class=""oi_right""></span>";
                }
                else
                {
                    if (this.pollInfo.MaxNum == 1)
                    {
                        select = string.Format(@"<input type=""radio"" name=""pollItemID"" value=""{0}"" />", itemInfo.ID);
                    }
                    else
                    {
                        select = string.Format(@"<input type=""checkbox"" name=""pollItemID"" value=""{0}"" />", itemInfo.ID);
                    }
                }

                builder.AppendFormat(@"
<tr>
    <th class=""oi_text""> <span>{0}</span>： </th>
    <td class=""oi_numline""><div style=""width: {1}px"" class=""oi_numline{2}""> <span></span> </div></td>
    <td class=""oi_num""><span>{3}</span><span>({4}%)</span></td>
    <td class=""oi_input"">{5}</td>
</tr>
", itemInfo.Title, px, index % 5, itemInfo.Num, percent, select);
                index++;
            }
            return builder.ToString();
        }

        private void rptPosts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PostInfo postInfo = new PostInfo(e.Item.DataItem);
                if (postInfo != null)
                {
                    System.Web.UI.WebControls.Image imgUserImage = e.Item.FindControl("imgUserImage") as System.Web.UI.WebControls.Image;
                    HtmlTable postList = e.Item.FindControl("postList") as HtmlTable;
                    Literal ltlOnline = e.Item.FindControl("ltlOnline") as Literal;
                    HyperLink hlUserName = e.Item.FindControl("hlUserName") as HyperLink;
                    Literal ltlGroupName = e.Item.FindControl("ltlGroupName") as Literal;
                    Literal ltlStars = e.Item.FindControl("ltlStars") as Literal;
                    Literal ltlCredits = e.Item.FindControl("ltlCredits") as Literal;
                    Literal ltlPostCount = e.Item.FindControl("ltlPostCount") as Literal;
                    Literal ltlPrestige = e.Item.FindControl("ltlPrestige") as Literal;
                    Literal ltlContribution = e.Item.FindControl("ltlContribution") as Literal;
                    Literal ltlCurrency = e.Item.FindControl("ltlCurrency") as Literal;
                    Literal ltlOnlineTotal = e.Item.FindControl("ltlOnlineTotal") as Literal;
                    Literal ltlCreationDate = e.Item.FindControl("ltlCreationDate") as Literal;
                    Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                    Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                    Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;
                    Literal ltlEditUserName = e.Item.FindControl("ltlEditUserName") as Literal;
                    Literal ltlEditDate = e.Item.FindControl("ltlEditDate") as Literal;
                    Literal ltlPollTrs = e.Item.FindControl("ltlPollTrs") as Literal;
                   
                    Literal ltlAttachment = e.Item.FindControl("ltlAttachment") as Literal;
                    PlaceHolder phThreadPost = e.Item.FindControl("phThreadPost") as PlaceHolder;
                    Literal ltlSignature = e.Item.FindControl("ltlSignature") as Literal;
                    Literal ltlFloor = e.Item.FindControl("ltlFloor") as Literal;
                    Literal ltlManage = e.Item.FindControl("ltlManage") as Literal;
                    Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                    if (base.UserUtils.IsOnline(postInfo.UserName))
                    {
                        ltlOnline.Text = @"<img src=""images/status_online.png"" />";
                    }
                    else
                    {
                        ltlOnline.Text = @"<img src=""images/status_offline.png"" />";
                    }
                    hlUserName.Text = postInfo.UserName;
                    hlUserName.NavigateUrl = base.UserUtils.GetUserUrl(postInfo.UserName);

                    imgUserImage.ImageUrl = base.UserUtils.GetUserAvatarMiddleUrl(postInfo.UserName);

                    ltlGroupName.Text = base.UserUtils.GetGroupName(postInfo.UserName);
                    ltlStars.Text = base.UserUtils.GetStars(postInfo.UserName);
                    ltlCredits.Text = base.UserUtils.GetCredits(postInfo.UserName).ToString();
                    ltlPostCount.Text = base.UserUtils.GetPostCount(postInfo.UserName).ToString();
                    ltlPrestige.Text = base.UserUtils.GetPrestige(postInfo.UserName).ToString();
                    ltlContribution.Text = base.UserUtils.GetContribution(postInfo.UserName).ToString();
                    ltlCurrency.Text = base.UserUtils.GetCurrency(postInfo.UserName).ToString();
                    ltlOnlineTotal.Text = base.UserUtils.GetOnlineTotal(postInfo.UserName);
                    ltlCreationDate.Text = DateUtils.GetDateString(UserManager.GetCreateDate(base.PublishmentSystemInfo.GroupSN, postInfo.UserName));
                    
                    ltlAddDate.Text = DateUtils.GetDateAndTimeString(postInfo.AddDate);
                    if (!string.IsNullOrEmpty(postInfo.LastEditUserName))
                    {
                        ltlEditDate.Text = "(于:" + DateUtils.GetDateString(postInfo.LastEditDate);
                        ltlEditUserName.Text ="被"+postInfo.LastEditUserName+"编辑)";
                        
                    }
                    if (!postInfo.IsThread)
                    {
                        ltlTitle.Text = ThreadManager.GetPostTitle(postInfo.Title);
                    }

                    List<AttachmentInfo> attachmentInfoList = null;
                    if (postInfo.IsAttachment)
                    {
                        attachmentInfoList = DataProvider.AttachmentDAO.GetList(base.PublishmentSystemID, postInfo.ThreadID, postInfo.ID);
                    }
                    if (postInfo.ForumID < 0 && postInfo.IsThread)
                    {
                        ltlContent.Text = ThreadManager.GetDeleteContent();
                    }
                    else if (postInfo.ForumID < 0 && !postInfo.IsThread)
                    {
                        postList.Visible = false;
                    }
                     
                    else if (postInfo.IsBanned)
                    {
                        ltlContent.Text = ThreadManager.GetBannedContent();
                    }
                    else
                    {
                        if (!postInfo.IsChecked)
                        {
                            ltlContent.Text = ThreadManager.GetCheckedContent();
                        }
                        else
                        {
                            ltlContent.Text = ThreadManager.GetPostContent(base.PublishmentSystemID, postInfo, attachmentInfoList);
                        }
                        ltlAttachment.Text = AttachManager.GetAttachmentHtml(base.PublishmentSystemID, attachmentInfoList);
                    }

                    if (postInfo.IsThread)
                    {
                        if (this.threadInfo.ThreadType == EThreadType.Poll)
                        {
                            ltlPollTrs.Text = this.GetPoll();
                        }
                        phThreadPost.Visible = true;
                    }

                    if (base.UserUtils.IsSignature(postInfo.IsSignature, postInfo.UserName))
                    {
                        ltlSignature.Text = string.Format(@"<div class=""sign"">{0}</div>", base.UserUtils.GetSignature(postInfo.UserName));
                    }

                    int itemIndex = this.startNum + e.Item.ItemIndex;
                    ltlFloor.Text = StringUtilityBBS.GetFloorLinkHtml(pageNum, postInfo.ID, itemIndex);

                    bool isEditable = this.IsEditable(postInfo.UserName);

                    ltlManage.Text = string.Format(@"<a href=""javascript:;""  onclick=""{0}"">举报</a> <a href=""#"">顶端</a>&nbsp; ", Dialog.Report.GetOpenWindowStringByReportPost(base.PublishmentSystemID, forumID, threadID, postInfo.ID));
                    if (isEditable && !postInfo.IsThread)
                    {
                        ltlManage.Text += string.Format(@"<label for=""postID_{0}""><input id=""postID_{0}"" onclick=""postManage.show('postManagerPop',this.id)"" name=""postIDArray"" value=""{0}"" type=""checkbox"" autocomplete=""off""> 管理</label>", postInfo.ID);
                    }

                    ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"" class=""fastre"">回复</a> <a href=""javascript:;"" onclick=""{1}"" class=""reg"">引用</a>", Dialog.Post.GetOpenWindowStringByReplyPost(base.PublishmentSystemID, forumID, threadID, postInfo.ID), Dialog.Post.GetOpenWindowStringByReferencePost(base.PublishmentSystemID, forumID, threadID, postInfo.ID));
                    if (isEditable)
                    {
                        ltlEditUrl.Text += string.Format(@" <a href=""{0}"" class=""edit"">编辑</a>", PostPage.GetUrl(base.PublishmentSystemID, forumID, threadID, postInfo.ID, string.Empty));
                    }
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }

        public bool IsManageable()
        {
            if (this.isModerator) return true;
            return false;
        }

        public bool IsPostable()
        {
            if (this.threadInfo.IsLocked)
            {
                return false;
            }
            UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
            return  groupInfo.Additional.IsAllowReply;
        }

        public bool IsEditable(string userName)
        {
            if (this.isModerator) return true;
            if (BaiRongDataProvider.UserDAO.CurrentUserName == userName) return true;
            return false;
        }

        public bool IsAllowReply()
        {
            UserGroupInfo userGroupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
            if (userGroupInfo != null)
            {
                return userGroupInfo.Additional.IsAllowReply;
            }
            return false;
        }

        public int GetDownloadCount(int id)
        {
            return DataProvider.AttachmentDAO.GetDownloadCount(id);
        }

        public string GetLocation()
        {
            StringBuilder builder = new StringBuilder();

            if (this.forumInfo != null)
            {
                string parentsPath = this.forumInfo.ParentsPath;
                int parentsCount = this.forumInfo.ParentsCount;
                if (parentsPath.Length != 0)
                {
                    string forumPath = parentsPath + "," + this.forumID;
                    ArrayList forumIDArrayList = TranslateUtils.StringCollectionToArrayList(forumPath);
                    foreach (string forumIDStr in forumIDArrayList)
                    {
                        int currentID = int.Parse(forumIDStr);
                        if (currentID == 0)
                        {
                            builder.AppendFormat(@"<a href=""{0}"">{1}</a>", PageUtilityBBS.GetIndexPageUrl(base.PublishmentSystemID), base.Additional.BBSName);
                        }
                        else if (currentID == this.forumID)
                        {
                            builder.AppendFormat(@"<a href=""{0}"">{1}</a>", PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, this.forumInfo), this.forumInfo.ForumName);
                        }
                        else
                        {
                            ForumInfo currentForumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, currentID);
                            builder.AppendFormat(@"<a href=""{0}"">{1}</a>", PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, currentForumInfo), currentForumInfo.ForumName);
                        }

                        builder.Append("&gt;");
                    }

                    builder.AppendFormat(@"<a href=""{0}"">{1}</a>", PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, this.forumID, this.threadID), StringUtils.MaxLengthText(this.threadInfo.Title, 20));
                }
            }
            return builder.ToString();
        }

        public string GetNavigation(bool isNextThread)
        {
            int taxis = this.threadInfo.Taxis;
            if (isNextThread)
            {
                int siblingThreadID = DataProvider.ThreadDAO.GetThreadID(base.PublishmentSystemID, this.forumID, taxis, true);
                if (siblingThreadID != 0)
                {
                    string url = PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, this.forumID, siblingThreadID);
                    return string.Format(@"<a href=""{0}""><img src=""images/next_ico.gif"" />下一主题</a>", url);
                }
                else
                {
                    return @"<a href=""javascript:;""><img src=""images/next_ico.gif"" />无</a>";
                }
            }
            else
            {
                int siblingThreadID = DataProvider.ThreadDAO.GetThreadID(base.PublishmentSystemID, this.forumID, taxis, false);
                if (siblingThreadID != 0)
                {
                    string url = PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, this.forumID, siblingThreadID);
                    return string.Format(@"<a href=""{0}""><img src=""images/pre_ico.gif"" />上一主题</a>", url);
                }
                else
                {
                    return @"<a href=""javascript:;""><img src=""images/pre_ico.gif"" />无</a>";
                }
            }
        }

        public string GetUrl(string type)
        {
            if (type == "replyUrl")
            {
                return PostPage.GetUrl(base.PublishmentSystemID, this.forumID, this.threadID, 0, string.Empty);
            }
            else if (type == "replyThread")
            {
                return Dialog.Post.GetOpenWindowStringByReplyThread(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            else if (type == "addPostUrl")
            {
                return PostPage.GetAddUrl(base.PublishmentSystemID, this.forumID, false);
            }
            else if (type == "addPollUrl")
            {
                return PostPage.GetAddUrl(base.PublishmentSystemID, this.forumID, true);
            }
            else if (type == "deleteThreadSingle")
            {
                return Dialog.Delete.GetOpenWindowStringDeleteThreadSingle(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            else if (type == "translateSingle")
            {
                return Dialog.Translate.GetOpenWindowString(this.forumID, this.threadID);
            }
            else if (type == "categorySingle")
            {
                return Dialog.Category.GetOpenWindowString(this.forumID, this.threadID);
            }
            else if (StringUtils.StartsWithIgnoreCase(type, "highlightSingle."))
            {
                string action = type.Substring(16);
                return Dialog.Highlight.GetOpenWindowString(base.PublishmentSystemID, action, this.forumID, this.threadID);
            }
            else if (type == "lockSingle")
            {
                return Dialog.Lock.GetOpenWindowString(this.forumID, this.threadID);
            }
            else if (type == "banThreadSingle")
            {
                return Dialog.Ban.GetOpenWindowStringBanThread(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            else if (type == "identifySingle")
            {
                return Dialog.Identify.GetOpenWindowString(this.forumID, this.threadID);
            }
            else if (type == "disableUserSingle")
            {
                return Dialog.DisabledUsers.GetOpenWindowStringDisableUsersSingle(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            else if (type == "deletePost")
            {
                return Dialog.Delete.GetOpenWindowStringDeletePost(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            else if (type == "upDownPost")
            {
                return Dialog.UpDown.GetOpenWindowStringUpDownPost(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            else if (type == "banPost")
            {
                return Dialog.Ban.GetOpenWindowStringBanPost(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            else if (type == "disableUser")
            {
                return Dialog.DisabledUsers.GetOpenWindowStringDisableUsers(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            else if (type == "threadUrl")
            {
                return PageUtils.AddProtocolToUrl(PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, this.forumID, this.threadID));
            }
            else if (type == "floorUrl")
            {
                return RedirectPage.GetRedirectUrlByFloor(base.PublishmentSystemID, this.forumID, this.threadID);
            }
            return "javascript:;";
        }

        public void GetIdentifyImg(int identifyID)
        {
            this.imgIdentify.Visible = false;
            if (identifyID != 0)
            {
                IdentifyInfo identifyInfo = DataProvider.IdentifyDAO.GetIdentifyInfo(identifyID);
                this.imgIdentify.Visible = true;
                this.imgIdentify.ImageUrl ="./"+identifyInfo.StampUrl;
            }
        }
    }
}
