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
using System.Web.UI.HtmlControls;
using System.IO;
using BaiRong.Model;
using BaiRong.Core.Drawing;
using System.Text;

namespace SiteServer.BBS.Pages
{
    public class PermissionPage : BasePage
    {
        public Literal ltlPermissions;

        public Literal ltlIsAllowVisit;
        public Literal ltlIsAllowHide;
        public Literal IsAllowSignature;
        public Literal ltlSearchType;
        public Literal ltlSearchInterval;

        public Literal ltlIsAllowRead;
        public Literal ltlIsAllowPost;
        public Literal ltlIsAllowReply;
        public Literal ltlIsAllowPoll;
        public Literal ltlMaxPostPerDay;
        public Literal ltlPostInterval;

        public Literal ltlUploadType;
        public Literal ltlDownloadType;
        public Literal ltlIsAllowSetAttachmentPermissions;
        public Literal ltlMaxSize;
        public Literal ltlMaxSizePerDay;
        public Literal ltlMaxNumPerDay;
        public Literal ltlAttachmentExtensions;

        private int forumID;

        protected override bool IsAccessable
        {
            get
            {
                return false;
            }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.forumID = base.GetIntQueryString("forumID");

            if (!IsPostBack)
            {
                UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);;

                if (this.forumID > 0)
                {
                    ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);
                    this.ltlPermissions.Text = string.Format(@"版块权限：<a href=""{0}"">{1}</a>&nbsp;&nbsp;", PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, forumInfo), forumInfo.ForumName);
                }
                this.ltlPermissions.Text += string.Format(@"用户权限：<a href=""javascript:;"">{0}</a>", groupInfo.GroupName);

                this.ltlIsAllowVisit.Text = this.GetTrueFalseString(groupInfo.Additional.IsAllowVisit);
                this.ltlIsAllowHide.Text = this.GetTrueFalseString(groupInfo.Additional.IsAllowHide);
                this.IsAllowSignature.Text = this.GetTrueFalseString(groupInfo.Additional.IsAllowSignature);
                ETriState searchType = ETriStateUtils.GetEnumType(groupInfo.Additional.SearchType);
                this.ltlSearchType.Text = ETriStateUtils.GetText(searchType, "允许搜索主题标题、内容", "允许搜索主题标题", "不允许");
                this.ltlSearchInterval.Text = this.GetZeroString(groupInfo.Additional.SearchInterval, "秒");

                this.ltlIsAllowRead.Text = this.GetTrueFalseString(groupInfo.Additional.IsAllowRead);
                this.ltlIsAllowPost.Text = this.GetTrueFalseString(groupInfo.Additional.IsAllowPost);
                this.ltlIsAllowReply.Text = this.GetTrueFalseString(groupInfo.Additional.IsAllowReply);
                this.ltlIsAllowPoll.Text = this.GetTrueFalseString(groupInfo.Additional.IsAllowPoll);
                this.ltlMaxPostPerDay.Text = this.GetZeroString(groupInfo.Additional.MaxPostPerDay, "篇");
                this.ltlPostInterval.Text = this.GetZeroString(groupInfo.Additional.PostInterval, "秒");                

                ETriState uploadType = ETriStateUtils.GetEnumType(groupInfo.Additional.UploadType);
                this.ltlUploadType.Text = ETriStateUtils.GetText(uploadType, "允许上传附件，不奖励或扣除积分", "允许上传附件，按照版块设置奖励或扣除积分", "不允许上传附件");

                ETriState downloadType = ETriStateUtils.GetEnumType(groupInfo.Additional.DownloadType);
                this.ltlDownloadType.Text = ETriStateUtils.GetText(downloadType, "允许下载附件，不奖励或扣除积分", "允许下载附件，按照版块设置奖励或扣除积分", "不允许下载附件");
                this.ltlIsAllowSetAttachmentPermissions.Text = this.GetTrueFalseString(groupInfo.Additional.IsAllowSetAttachmentPermissions);
                this.ltlMaxSize.Text = this.GetZeroString(groupInfo.Additional.MaxSize, "");
                this.ltlMaxSizePerDay.Text = this.GetZeroString(groupInfo.Additional.MaxSizePerDay, "");
                this.ltlMaxNumPerDay.Text = this.GetZeroString(groupInfo.Additional.MaxNumPerDay, ""); 
                this.ltlAttachmentExtensions.Text = groupInfo.Additional.AttachmentExtensions;
                if (string.IsNullOrEmpty(this.ltlAttachmentExtensions.Text))
                {
                    this.ltlAttachmentExtensions.Text = "系统默认类型";
                }
            }
        }

        public string GetZeroString(int num, string unit)
        {
            if (num == 0)
            {
                return "不限制";
            }
            else
            {
                return num.ToString() + unit;
            }
        }

        private string GetTrueFalseString(bool isTrue)
        {
            if (isTrue)
            {
                return @"<font color=""green"" size=""+"">&#8730;</font>";
            }
            else
            {
                return @"<font color=""red"" size=""+"">&#215;</font>";
            }
        }
    }
}
