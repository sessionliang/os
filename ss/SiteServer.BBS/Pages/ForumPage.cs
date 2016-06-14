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

namespace SiteServer.BBS.Pages
{
    public class ForumPage : BasePage
    {
        protected int forumID;

        public Literal ltlCategories;
        public Repeater rptTopThreads;
        public Repeater rptThreads;
        protected int pageNum;
        protected PagerInfo pagerInfo;
        protected ForumInfo forumInfo;
        protected bool isModerator;
        protected int topForumID;
        public Literal ltlCategorySelect;

        protected int categoryID;
        protected string orderBy;
        protected string type;

        public string GetPageUrl(int categoryID, string orderBy)
        {
            string pageUrl = PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, this.forumInfo);
            if (categoryID > 0)
            {
                pageUrl = PageUtils.AddQueryString(pageUrl, "categoryID", categoryID.ToString());
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                pageUrl = PageUtils.AddQueryString(pageUrl, "orderBy", orderBy);
            }
            return pageUrl;
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.pageNum = TranslateUtils.ToInt(base.GetQueryString("page"), 1);
            this.categoryID = base.GetIntQueryString("categoryID");
            this.orderBy = PageUtils.FilterSqlAndXss(base.Request.QueryString["orderBy"]);
            this.type = PageUtils.FilterSqlAndXss(base.Request.QueryString["type"]);
            this.forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);
            
            if (this.forumInfo != null)
            {
                string redirectUrl = string.Empty;
                bool isViewable = AccessManager.IsViewable(base.PublishmentSystemID, this.forumInfo, this.Page.Request.RawUrl, out redirectUrl);
                if (isViewable)
                {
                    this.isModerator = BBSUserManager.IsModerator(this.forumInfo);

                    if (this.ltlCategories != null)
                    {
                        string allText = "全 部";
                        if (!string.IsNullOrEmpty(this.type))
                        {
                            if (StringUtils.EqualsIgnoreCase(type, "digest"))
                            {
                                allText = "精华帖";
                            }
                            else if (StringUtils.EqualsIgnoreCase(type, "image"))
                            {
                                allText = "图片贴";
                            }
                        }
                        if (this.categoryID == 0)
                        {
                            this.ltlCategories.Text = string.Format(@"<li class=""thr_current""><a href=""{0}"">{1}</a></li>", this.GetPageUrl(0, this.orderBy), allText);
                        }
                        else
                        {
                            this.ltlCategories.Text = string.Format(@"<li><a href=""{0}"">{1}</a></li>", this.GetPageUrl(0, this.orderBy), allText);
                        }
                        ArrayList categoryInfoArrayList = ThreadCategoryManager.GetThreadCategoryInfoArrayList(base.PublishmentSystemID, this.forumID);
                        foreach (ThreadCategoryInfo categoryInfo in categoryInfoArrayList)
                        {
                            if (categoryInfo.CategoryID != this.categoryID)
                            {
                                this.ltlCategories.Text += string.Format(@"<li><a href=""{0}"">{1}</a></li>", this.GetPageUrl(categoryInfo.CategoryID, this.orderBy), categoryInfo.CategoryName);
                            }
                            else
                            {
                                this.ltlCategories.Text += string.Format(@"<li class=""thr_current""><a href=""{0}"">{1}</a></li>", this.GetPageUrl(categoryInfo.CategoryID, this.orderBy), categoryInfo.CategoryName);
                            }
                        }
                    }
                    if (this.rptTopThreads != null)
                    {
                        int areaID = ForumManager.GetAreaID(base.PublishmentSystemID, this.forumID);
                        this.rptTopThreads.DataSource = ThreadManager.GetTopLevelThradInfoArrayList(base.PublishmentSystemID, areaID, this.forumID);
                        this.rptTopThreads.ItemDataBound += new RepeaterItemEventHandler(rptThreads_ItemDataBound);
                        this.rptTopThreads.DataBind();
                    }
                    if (this.rptThreads != null)
                    {
                        string orderByString = string.Empty;
                        if (StringUtils.EqualsIgnoreCase(this.orderBy, "addDate"))
                        {
                            orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Thread, EBBSTaxisType.OrderByAddDateDesc);
                        }
                        else
                        {
                            orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Thread, EBBSTaxisType.OrderByLastDate);
                        }
                        int startNum = (this.pageNum - 1) * base.Additional.ThreadPageNum + 1;
                        this.rptThreads.DataSource = DataUtility.GetThreadsDataSource(base.PublishmentSystemID, this.forumID, this.categoryID, this.type, startNum, base.Additional.ThreadPageNum, orderByString);
                        this.rptThreads.ItemDataBound += new RepeaterItemEventHandler(rptThreads_ItemDataBound);
                        this.rptThreads.DataBind();

                        this.pagerInfo = PagerInfo.GetPagerInfo(forumInfo.ThreadCount, base.Additional.ThreadPageNum, base.Request, string.Empty);
                    }
                }
                else
                {
                    PageUtils.Redirect(redirectUrl);
                }
            }
        }

        private void rptThreads_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ThreadInfo threadInfo = new ThreadInfo(e.Item.DataItem);
                if (threadInfo != null)
                {
                    Literal ltlIcon = e.Item.FindControl("ltlIcon") as Literal;
                    Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                    Literal ltlPages = e.Item.FindControl("ltlPages") as Literal;
                    HyperLink hlUserName = e.Item.FindControl("hlUserName") as HyperLink;
                    Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                    HyperLink hlReplies = e.Item.FindControl("hlReplies") as HyperLink;
                    HyperLink hlLastUserName = e.Item.FindControl("hlLastUserName") as HyperLink;
                    Literal ltlLastDate = e.Item.FindControl("ltlLastDate") as Literal;
                   
                    string threadUrl = PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, threadInfo.ForumID, threadInfo.ID);
                    if (threadInfo.CategoryID > 0)
                    {
                        ThreadCategoryInfo categoryInfo = ThreadCategoryManager.GetThreadCategoryInfo(base.PublishmentSystemID, threadInfo.CategoryID);
                        if (categoryInfo != null)
                        {
                            ltlTitle.Text = string.Format(@"<em>[<a href=""{0}"">{1}</a>]</em>", this.GetPageUrl(threadInfo.CategoryID, this.orderBy), categoryInfo.CategoryName);
                        }
                    }
                    if (threadInfo.ForumID != this.forumID)
                    {
                        this.topForumID = threadInfo.ForumID;
                    }
                  
                    ltlTitle.Text += string.Format(@"<a class=""xst"" href=""{0}"" style=""{1}"">{2}</a>{3}", threadUrl, StringUtilityBBS.GetHighlightStyle(threadInfo.Highlight), threadInfo.Title, ThreadManager.GetThreadRightIcons(base.PublishmentSystemID, threadInfo, "../"));
                    int pageCount = ThreadManager.GetPageCount(base.PublishmentSystemID, threadInfo.Replies);
                    if (pageCount > 1)
                    {
                        StringBuilder pageBuilder = new StringBuilder();
                        if (pageCount <= 6)
                        {
                            for (int page = 1; page <= pageCount; page++)
                            {
                                pageBuilder.AppendFormat(@"<a href=""{0}"">{1}</a>", PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, threadInfo.ForumID, threadInfo.ID, page), page);
                            }
                        }
                        else
                        {
                            for (int page = 1; page <= 5; page++)
                            {
                                pageBuilder.AppendFormat(@"<a href=""{0}"">{1}</a>", PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, threadInfo.ForumID, threadInfo.ID, page), page);
                            }
                            pageBuilder.AppendFormat(@"..<a href=""{0}"">{1}</a>", PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, threadInfo.ForumID, threadInfo.ID, pageCount), pageCount);
                        }
                        ltlPages.Text = pageBuilder.ToString();
                    }

                    string iconUrl = ThreadManager.GetThreadLeftIcon(threadInfo);
                    ltlIcon.Text = string.Format(@"<a href=""{0}"" target=""_blank""><img src=""../images/icon_{1}.gif"" /></a>", threadUrl, iconUrl);

                    hlUserName.Text = threadInfo.UserName;
                    hlUserName.NavigateUrl = base.UserUtils.GetUserUrl(threadInfo.UserName);

                    ltlAddDate.Text = DateUtils.GetDateString(threadInfo.AddDate);

                    hlReplies.Text = threadInfo.Replies.ToString();
                    hlReplies.NavigateUrl = threadUrl;

                    if (!string.IsNullOrEmpty(threadInfo.LastUserName))
                    {
                        hlLastUserName.Text = threadInfo.LastUserName;
                        hlLastUserName.NavigateUrl = base.UserUtils.GetUserUrl(threadInfo.LastUserName);

                        ltlLastDate.Text = DateUtils.ParseThisMoment(threadInfo.LastDate, DateTime.Now);
                    }
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }

        protected string GetCategorySelectHtml()
        {
            return ThreadCategoryManager.GetCategorySelectHtml(base.PublishmentSystemID, this.forumID, 0);
        }
    }
}
