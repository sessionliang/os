using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;
using System.Web.UI.WebControls;
using SiteServer.BBS.Core;
using BaiRong.Core;

using BaiRong.Model;
using System.Collections.Specialized;
using System.Collections;


namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class ThreadEdit : BackgroundBasePage
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

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public static string GetOpenWindowString(int publishmentSystemID, int threadID, string type)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ID", threadID.ToString());
            arguments.Add("Type", type);
            return JsUtils.OpenWindow.GetOpenWindowString("编辑帖子", PageUtils.GetBBSUrl("modal_threadEdit.aspx"), arguments, 800, 650, true);
        }

        public static string GetUrl(int publishmentSystemID, int forumID, int threadID, int postID, string postType)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("post.aspx?forumID={0}&threadID={1}&postID={2}&postType={3}", forumID, threadID, postID, postType));
        }

        static string GetAddUrl(int publishmentSystemID, int forumID, bool isPoll)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("post.aspx?forumID={0}{1}", forumID, isPoll ? "&addType=poll" : string.Empty));
        }

        public void InitMessage()
        {
            threadID = base.GetIntQueryString("ID");
            ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(base.PublishmentSystemID, threadID);
            forumID = ConvertHelper.GetInteger(threadInfo.ForumID);
            postType = string.Empty;

            postID =DataProvider.PostDAO.GetPostIDByTaxis(base.PublishmentSystemID, forumID, threadID, 0);
            postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, postID);

        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            InitMessage();
            this.addType = base.GetQueryString("addType");

            if (!IsPostBack)
            {
                this.isPoll = false;
                if (this.postID > 0)
                {
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
                }
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
                return string.Empty;
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

        #region poll

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

        #endregion
    }
}
