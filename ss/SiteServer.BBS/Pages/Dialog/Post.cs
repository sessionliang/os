using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;



using SiteServer.BBS.Model;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.Pages.Dialog
{
    public class Post : BasePage
    {
        protected int publishmentSystemID;
        protected int forumID;
        protected int threadID;
        protected int postID;
        protected PostInfo postInfo;
        protected string postType;

        public Literal ltlFaceLinks;
        public Literal ltlFaceDefaultContents;

        public PlaceHolder phReference;
        public Literal ltlReference;
        public PlaceHolder phVerifyCode;

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public static string GetOpenWindowStringByReplyThread(int publishmentSystemID, int forumID, int threadID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "post.aspx"), arguments, 760, 400, "回复帖子", string.Empty);
        }

        public static string GetOpenWindowStringByReplyPost(int publishmentSystemID, int forumID, int threadID, int postID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("postID", postID.ToString());
            arguments.Add("postType", "Reply");
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "post.aspx"), arguments, 760, 400, "回复帖子", string.Empty);
        }

        public static string GetOpenWindowStringByReferencePost(int publishmentSystemID, int forumID, int threadID, int postID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("threadID", threadID.ToString());
            arguments.Add("postID", postID.ToString());
            arguments.Add("postType", "Reference");
            return DialogUtility.GetOpenWindowString(PageUtilityBBS.GetDialogPageUrl(publishmentSystemID, "post.aspx"), arguments, 760, 420, "引用帖子", string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                this.forumID = TranslateUtils.ToInt(base.Request.QueryString["forumID"]);
                this.threadID = TranslateUtils.ToInt(base.Request.QueryString["threadID"]);
                this.postID = TranslateUtils.ToInt(base.Request.QueryString["postID"]);
                if (this.postID > 0)
                {
                    this.postInfo = DataProvider.PostDAO.GetPostInfo(this.publishmentSystemID, this.postID);
                }
                this.postType = PageUtils.FilterSqlAndXss(base.Request.QueryString["postType"]);

                if (StringUtils.EqualsIgnoreCase(this.postType, "Reference"))
                {
                    this.phReference.Visible = true;
                    this.ltlReference.Text = ThreadManager.GetPostReferenceString(this.postInfo, true);
                }

                this.ltlFaceLinks.Text = StringUtilityBBS.GetFaceLinks(this.publishmentSystemID);
                this.ltlFaceDefaultContents.Text = StringUtilityBBS.GetFaceDefaultContents(this.publishmentSystemID);

                this.phVerifyCode.Visible = UserUtils.GetInstance(this.publishmentSystemID).IsVerifyCodePost;
            }
        }

        protected string GetTitle()
        {
            if (this.postInfo != null)
            {
                if (string.IsNullOrEmpty(this.postType))
                {
                    return DataProvider.ThreadDAO.GetTitle(this.threadID);
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
                return ThreadManager.GetPostContentWithoutReference(this.publishmentSystemID, this.postInfo.Content);
            }
            return string.Empty;
        }

        protected string GetPostPageUrl()
        {
            return PostPage.GetUrl(this.publishmentSystemID, this.forumID, this.threadID, this.postID, this.postType);
        }

        public string GetBBSUrl()
        {
            return PageUtilityBBS.GetBBSUrl(this.publishmentSystemID, string.Empty);
        }
    }
}
