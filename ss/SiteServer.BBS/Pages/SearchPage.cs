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
    public class SearchPage : BasePage
    {
        protected int forumID;

        public Literal ltlText;
        public Repeater rptThreads;
        protected int pageNum;
        protected PagerInfo pagerInfo;

        protected string word;
        protected string orderBy;
        protected string type;
        protected int totalNum;

        public string GetPageUrl(int categoryID, string orderBy, int totalNum)
        {
            string pageUrl = "search.aspx";
            if (categoryID > 0)
            {
                pageUrl = PageUtils.AddQueryString(pageUrl, "categoryID", categoryID.ToString());
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                pageUrl = PageUtils.AddQueryString(pageUrl, "orderBy", orderBy);
            }
            if (totalNum > 0)
            {
                pageUrl = PageUtils.AddQueryString(pageUrl, "totalNum", totalNum.ToString());
            }
            return pageUrl;
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.forumID = base.GetIntQueryString("forumID");
            this.pageNum = TranslateUtils.ToInt(base.Request.QueryString["page"], 1);
            this.word = PageUtils.FilterSqlAndXss(base.Request.QueryString["word"]);
            this.orderBy = PageUtils.FilterSqlAndXss(base.Request.QueryString["orderBy"]);
            this.type = PageUtils.FilterSqlAndXss(base.Request.QueryString["type"]);
            this.totalNum = base.GetIntQueryString("totalNum");

            string allText = "搜 索";
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
            this.ltlText.Text = string.Format(@"<a href=""{0}"">{1}</a>", this.GetPageUrl(0, this.orderBy, this.totalNum), allText);

            UserGroupInfo groupInfo = UserGroupManager.GetCurrent(base.PublishmentSystemInfo.GroupSN);

            ETriState searchType = ETriStateUtils.GetEnumType(groupInfo.Additional.SearchType);
            if (searchType == ETriState.False)
            {
                base.FailureMessage("对不起，您所属的用户组不允许进行搜索");
                return;
            }
            //if (!string.IsNullOrEmpty(this.word) || this.totalNum > 0)
            //{
                string orderByString = string.Empty;
                if (StringUtils.EqualsIgnoreCase(this.orderBy, "addDate"))
                {
                    orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Thread, EBBSTaxisType.OrderByAddDateDesc);
                }
                else
                {
                    orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Thread, EBBSTaxisType.OrderByLastDate);
                }
                int pageCount = 20;
                int startNum = (this.pageNum - 1) * pageCount + 1;

                string sqlWhereString = DataProvider.ThreadDAO.GetParserWhereString(base.PublishmentSystemID, 0, type, false, false, string.Format("Title LIKE '%{0}%'", this.word));
                this.rptThreads.DataSource = DataProvider.ThreadDAO.GetParserDataSourceChecked(base.PublishmentSystemID, forumID, startNum, pageCount, orderByString, sqlWhereString, true);
                int totalCount = DataProvider.ThreadDAO.GetParserDataSourceCheckedCount(base.PublishmentSystemID, forumID, sqlWhereString, true);

                this.rptThreads.ItemDataBound += new RepeaterItemEventHandler(rptThreads_ItemDataBound);
                this.rptThreads.DataBind();

                string urlFormat = string.Format("search.aspx?word={0}&page={1}", this.word, "{0}");
                this.pagerInfo = PagerInfo.GetPagerInfo(totalCount, pageCount, base.Request, urlFormat);
            //}
        }

        private void rptThreads_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ThreadInfo threadInfo = new ThreadInfo(e.Item.DataItem);
                if (threadInfo != null)
                {
                    Literal ltlTr = e.Item.FindControl("ltlTr") as Literal;
                    Literal ltlIcon = e.Item.FindControl("ltlIcon") as Literal;
                    Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                    HyperLink hlUserName = e.Item.FindControl("hlUserName") as HyperLink;
                    Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                    HyperLink hlReplies = e.Item.FindControl("hlReplies") as HyperLink;
                    HyperLink hlLastUserName = e.Item.FindControl("hlLastUserName") as HyperLink;
                    Literal ltlLastDate = e.Item.FindControl("ltlLastDate") as Literal;
                    PlaceHolder phContent = e.Item.FindControl("phContent") as PlaceHolder;
                    Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;

                    string threadUrl = PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, threadInfo.ForumID, threadInfo.ID);
                    if (threadInfo.CategoryID > 0)
                    {
                        ThreadCategoryInfo categoryInfo = ThreadCategoryManager.GetThreadCategoryInfo(base.PublishmentSystemID, threadInfo.CategoryID);
                        if (categoryInfo != null)
                        {
                            ltlTitle.Text = string.Format(@"<em>[<a href=""{0}"" target=""_blank"">{1}</a>]</em>", this.GetPageUrl(threadInfo.CategoryID, this.orderBy, this.totalNum), categoryInfo.CategoryName);
                        }
                    }
                    ltlTitle.Text += string.Format(@"<a class=""xst"" href=""{0}"" style=""{1}"" target=""_blank"">{2}</a>{3}", threadUrl, StringUtilityBBS.GetHighlightStyle(threadInfo.Highlight), threadInfo.Title, ThreadManager.GetThreadRightIcons(base.PublishmentSystemID, threadInfo, string.Empty));

                    string iconUrl = ThreadManager.GetThreadLeftIcon(threadInfo);
                    ltlIcon.Text = string.Format(@"<a href=""{0}"" target=""_blank""><img src=""images/icon_{1}.gif"" /></a>", threadUrl, iconUrl);

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
                    string content = DataProvider.PostDAO.GetThreadValue(base.PublishmentSystemID, threadInfo.ID, PostAttribute.Content);
                    content = StringUtils.StripTags(content);
                    content = content.Replace("&nbsp;", string.Empty);
                    content = content.Trim();

                    if (!string.IsNullOrEmpty(this.word))
                    {
                        content = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", word.Replace(" ", "\\s")), content, string.Format("<span style='color:#cc0000'>{0}</span>", word));
                    }

                    if (string.IsNullOrEmpty(content))
                    {
                        phContent.Visible = false;
                        ltlTr.Text = @"<tr>";
                    }
                    else
                    {
                        ltlContent.Text = content;
                        ltlTr.Text = @"<tr class=""noLine"">";
                    }
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }
    }
}
