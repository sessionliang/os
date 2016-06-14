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
using BaiRong.Model;
using System.Collections.Generic;

namespace SiteServer.BBS.Pages
{
    public class HandlePage : BasePage
    {
        public Literal ltlText;
        public Literal ltlCount;
        public Repeater rptPosts;
        protected int pageNum;
        protected PagerInfo pagerInfo;
        private string type;

        public string GetPageUrl(string type)
        {
            string pageUrl = "handle.aspx";
            if (!string.IsNullOrEmpty(type))
            {
                pageUrl = PageUtils.AddQueryString(pageUrl, "type", type);
            }
            return pageUrl;
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.pageNum = TranslateUtils.ToInt(base.GetQueryString("page"), 1);
            this.type = PageUtils.FilterSqlAndXss(base.Request.QueryString["type"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Handle"]))
            {
                List<int> idList = TranslateUtils.StringCollectionToIntList(base.Request.QueryString["IDCollection"]);
                DataProvider.PostDAO.Handle(base.PublishmentSystemID, idList);
            }

            string allText = "今 日";
            int days = 1;
            if (this.type == "3day")
            {
                allText = "3日内";
                days = 3;
            }
            else if (this.type == "7day")
            {
                allText = "7日内";
                days = 7;
            }
            this.ltlText.Text = string.Format(@"<a href=""{0}"">{1}</a>", this.GetPageUrl(this.type), allText);

            if (!base.UserUtils.IsModerator)
            {
                base.FailureMessage("对不起，您所属的用户组不允许访问本页面");
                return;
            }

            int count = 30;
            int startNum = (this.pageNum - 1) * count + 1;

            this.rptPosts.DataSource = DataProvider.PostDAO.GetDataSourceByIsHandled(base.PublishmentSystemID, startNum, count, days);
            int totalCount = DataProvider.PostDAO.GetDataSourceCountByIsHandled(base.PublishmentSystemID, days);
            this.ltlCount.Text = totalCount.ToString();

            this.rptPosts.ItemDataBound += new RepeaterItemEventHandler(rptPosts_ItemDataBound);
            this.rptPosts.DataBind();

            this.pagerInfo = PagerInfo.GetPagerInfo(totalCount, count, base.Request, string.Empty);
        }

        private void rptPosts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PostInfo postInfo = new PostInfo(e.Item.DataItem);
                if (postInfo != null && postInfo.ForumID > 0)
                {
                    Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                    Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                    Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                    Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                    Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;

                    int page = ThreadManager.GetPostPage(base.PublishmentSystemID, postInfo.Taxis);
                    string postUrl = PageUtilityBBS.GetPostUrl(base.PublishmentSystemID, postInfo.ForumID, postInfo.ThreadID, page, postInfo.ID);
                    ltlTitle.Text = string.Format(@"<a style=""color:red"" href=""{0}"" target=""_blank"">{1}</a>", postUrl, postInfo.Title);

                    ltlUserName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", base.UserUtils.GetUserUrl(postInfo.UserName), postInfo.UserName);

                    ltlChannel.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, postInfo.ForumID), ForumManager.GetForumName(base.PublishmentSystemID, postInfo.ForumID));

                    ltlAddDate.Text = postInfo.AddDate.ToString("MM-dd hh:mm");

                    string content = postInfo.Content;
                    content = StringUtils.StripTags(content);
                    content = content.Replace("&nbsp;", string.Empty);
                    content = content.Trim();
                    ltlContent.Text = content;
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }

        protected string GetClickString()
        {
            return JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.AddQueryString(this.GetPageUrl(this.type), "Handle", true.ToString()), "IDCollection", "IDCollection", "请选择需要设置的帖子");
        }
    }
}
