using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using BaiRong.Model;

namespace SiteServer.BBS.Pages
{
    public class PostPage : BasePage
    {
        public Literal ltlOperate;

        protected int forumID;
        protected int threadID;
        protected int postID;
        protected string postType;
        protected int fileCount;
        protected string addType;

        protected PostInfo postInfo;
        private string addToListScript;
        protected bool isPoll;
        protected PollInfo pollInfo;

        public Literal ltlPollScript;
        public PlaceHolder phReference;
        public Literal ltlReference;
        public HyperLink bbslost;

        protected bool isVerifyCode;

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public static string GetUrl(int publishmentSystemID, int forumID, int threadID, int postID, string postType)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("post.aspx?forumID={0}&threadID={1}&postID={2}&postType={3}", forumID, threadID, postID, postType));
        }

        public static string GetAddUrl(int publishmentSystemID, int forumID, bool isPoll)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("post.aspx?forumID={0}{1}", forumID, isPoll ? "&addType=poll" : string.Empty));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.forumID = TranslateUtils.ToInt(base.Request.QueryString["forumID"]);
            this.threadID = TranslateUtils.ToInt(base.Request.QueryString["threadID"]);
            this.postID = TranslateUtils.ToInt(base.Request.QueryString["postID"]);
            this.postType = PageUtils.FilterSqlAndXss(base.Request.QueryString["postType"]);
            this.addType = PageUtils.FilterSqlAndXss(base.Request.QueryString["addType"]);

            if (!IsPostBack)
            {
                this.isPoll = false;

                if (this.postID == 0 && this.threadID == 0)
                {
                    this.isVerifyCode = base.Additional.IsVerifyCodeThread;
                }
                else
                {
                    this.isVerifyCode = base.Additional.IsVerifyCodePost;
                }

                if (this.postID > 0)
                {
                    this.postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, this.postID);

                    bool isEditable = false;

                    if (UserManager.IsAdministrator)
                    {
                        isEditable = true;
                    }
                    else if (this.postInfo.UserName == UserManager.Current.UserName)
                    {
                        isEditable = true;
                    }

                    if (!isEditable)
                    {
                        PageUtils.RedirectToErrorPage("对不起，权限不足无法进行编辑");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.postType))
                    {
                        this.ltlOperate.Text = "编辑帖子";
                        if (postInfo.IsAttachment)
                        {
                            IList<AttachmentInfo> lists = DataProvider.AttachmentDAO.GetList(base.PublishmentSystemID, postInfo.ThreadID, postInfo.ID);
                            if (lists.Count > 0)
                            {
                                StringBuilder builder = new StringBuilder();
                                foreach (AttachmentInfo attachInfo in lists)
                                {
                                    builder.AppendFormat(@"addToList({0}, '{1}', '{2}', '{3}', '{4}');", attachInfo.ID, attachInfo.FileName, AttachManager.GetAttachmentTips(attachInfo), attachInfo.Description, attachInfo.Price.ToString()).AppendLine();
                                }
                                this.fileCount = 0;
                                this.addToListScript = builder.ToString();
                            }
                        }
                        if (postInfo.IsThread)
                        {
                            EThreadType threadType = EThreadTypeUtils.GetEnumType(DataProvider.ThreadDAO.GetValue(this.threadID, ThreadAttribute.ThreadType));
                            if (threadType == EThreadType.Poll)
                            {
                                this.isPoll = true;
                                this.pollInfo = DataProvider.PollDAO.GetPollInfo(this.threadID);

                                StringBuilder builder = new StringBuilder();
                                builder.Append(@"
<script type=""text/javascript"">
$(document).ready(function(){");
                                builder.AppendFormat(@"$(""input[name='IsVoteFirst'][value='{0}']"").attr(""checked"",true);    $('#MaxNum').val( '{1}' );", this.pollInfo.IsVoteFirst.ToString(), this.pollInfo.MaxNum);

                                ArrayList arraylist = DataProvider.PollDAO.GetPollItemInfoArrayList(this.pollInfo.ID);

                                if (arraylist.Count > 5)
                                {
                                    builder.Append(@"addItems();");
                                }
                                if (arraylist.Count > 10)
                                {
                                    builder.Append(@"addItems();");
                                }
                                if (arraylist.Count > 15)
                                {
                                    builder.Append(@"addItems();");
                                }
                                int index = 0;
                                foreach (PollItemInfo pollItemInfo in arraylist)
                                {
                                    builder.AppendFormat(@"$('#PollItems{0}').val( '{1}' );", ++index, pollItemInfo.Title);
                                }

                                builder.Append(@"});
</script>");

                                this.ltlPollScript.Text = builder.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (StringUtils.EqualsIgnoreCase(this.postType, "Reference"))
                        {
                            this.ltlOperate.Text = "引用帖子";
                            this.phReference.Visible = true;
                            this.ltlReference.Text = ThreadManager.GetPostReferenceString(this.postInfo, true);
                        }
                        else if (StringUtils.EqualsIgnoreCase(this.postType, "Reply"))
                        {
                            this.ltlOperate.Text = "回复帖子";
                        }

                        string redirectUrl = string.Empty;
                        UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
                        bool isAddable = AccessManager.IsPostAddable(base.PublishmentSystemID, groupInfo, this.forumID, base.Request.RawUrl, out redirectUrl);
                        if (!isAddable)
                        {
                            PageUtils.Redirect(redirectUrl);
                        }
                    }
                }
                else if (this.threadID > 0)
                {
                    this.ltlOperate.Text = "回复帖子";
                    string redirectUrl = string.Empty;
                    UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
                    bool isAddable = AccessManager.IsPostAddable(base.PublishmentSystemID, groupInfo, this.forumID, base.Request.RawUrl, out redirectUrl);
                    if (!isAddable)
                    {
                        PageUtils.Redirect(redirectUrl);
                    }
                }
                else
                {
                    this.ltlOperate.Text = "发表帖子";
                    if (this.addType == "poll")
                    {
                        this.isPoll = true;
                        this.ltlOperate.Text = "发表投票";
                    }

                    string redirectUrl = string.Empty;
                    UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
                    bool isAddable = false;
                    if (!isPoll)
                    {
                        isAddable = AccessManager.IsThreadAddable(base.PublishmentSystemID, groupInfo, this.forumID, base.Request.RawUrl, out redirectUrl);
                    }
                    else
                    {
                        isAddable = AccessManager.IsPollAddable(base.PublishmentSystemID, groupInfo, this.forumID, base.Request.RawUrl, out redirectUrl);
                    }
                    if (!isAddable)
                    {
                        PageUtils.Redirect(redirectUrl);
                    }
                }
                this.bbslost.NavigateUrl = "/bbs";
                //by 20151202 sofuny
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.PublishmentSystemUrl))
                    this.bbslost.NavigateUrl = base.PublishmentSystemInfo.PublishmentSystemUrl;
            }
        }

        protected string GetTitle()
        {
            if (this.postInfo != null)
            {
                if (string.IsNullOrEmpty(this.postType))
                {
                    return this.postInfo.Title;
                }
                else if (StringUtils.EqualsIgnoreCase(this.postType, "Reply"))
                {
                    return string.Format("回复 {0}({1}) 的帖子", StringUtilityBBS.GetFloorByTaxis(this.postInfo.Taxis), this.postInfo.UserName);
                }
                else
                {
                    return "Re:" + DataProvider.ThreadDAO.GetTitle(this.threadID);
                }
            }
            else if (this.threadID > 0)//回复帖子
            {
                return "Re:" + DataProvider.ThreadDAO.GetTitle(this.threadID);
            }
            return string.Empty;
        }

        protected string GetContent()
        {
            if (this.postInfo != null && string.IsNullOrEmpty(this.postType))
            {
                return ThreadManager.GetPostContentWithoutReference(base.PublishmentSystemID, this.postInfo.Content);
            }
            return string.Empty;
        }

        protected string GetCategorySelectHtml()
        {
            if ((this.threadID == 0 && this.postID == 0) || (this.postInfo != null && this.postInfo.IsThread))
            {
                int categoryID = 0;
                if (this.threadID > 0 && this.postInfo != null && this.postInfo.IsThread)
                {
                    categoryID = DataProvider.ThreadDAO.GetCategoryID(this.threadID);
                }
                return ThreadCategoryManager.GetCategorySelectHtml(base.PublishmentSystemID, this.forumID, categoryID);
            }
            return string.Empty;
        }

        protected string GetUploadTips()
        {
            if (!BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
                ETriState uploadType = ETriStateUtils.GetEnumType(groupInfo.Additional.UploadType);
                if (uploadType != ETriState.False && !string.IsNullOrEmpty(groupInfo.Additional.AttachmentExtensions))
                {
                    return string.Format("在此上传附件 ({0})", groupInfo.Additional.AttachmentExtensions);
                }
            }
            return string.Empty;
        }

        protected string GetUploadTypes(bool isSingle)
        {
            if (!BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);
                ETriState uploadType = ETriStateUtils.GetEnumType(groupInfo.Additional.UploadType);
                if (uploadType != ETriState.False && !string.IsNullOrEmpty(groupInfo.Additional.AttachmentExtensions))
                {
                    if (isSingle)
                    {
                        return groupInfo.Additional.AttachmentExtensions.Replace(',', '|');
                    }
                    else
                    {
                        StringBuilder builder = new StringBuilder();
                        string[] exts = groupInfo.Additional.AttachmentExtensions.Split(',');
                        foreach (string ext in exts)
                        {
                            builder.AppendFormat("*.{0};", ext);
                        }
                        if (builder.Length > 0)
                        {
                            builder.Length -= 1;
                        }
                        return builder.ToString();
                    }
                }
            }
            return string.Empty;
        }

        protected string GetAddToListScript()
        {
            return this.addToListScript;
        }

        protected string GetPollDeadline()
        {
            if (this.pollInfo == null)
            {
                return DateTime.Now.AddDays(30).ToString("yyyy-MM-dd hh:mm");
            }
            else
            {
                return pollInfo.Deadline.ToString("yyyy-MM-dd hh:mm");
            }
        }
    }
}
