using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SiteServer.BBS.Model;
using BaiRong.Core;
using SiteServer.BBS.Core;
using BaiRong.Controls;
namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundReport : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlDel;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_report.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

       protected void Page_Load(object sender, EventArgs e)
       {
           if (base.IsForbidden) return;

           if (base.GetQueryString("ReportID") != null)
           {
               int ReprotID = base.GetIntQueryString("ReportID");
               DataProvider.ReportDAO.DeleteReport(ReprotID);
           }
           this.spContents.ControlToPaginate = this.rptContents;
           this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

           this.spContents.ItemsPerPage = 20;
           this.spContents.ConnectionString = DataProvider.ConnectionString;

           this.spContents.SelectCommand = DataProvider.ReportDAO.GetSqlString(base.PublishmentSystemID);

           if (!IsPostBack)
           {
               base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Content, "用户举报", AppManager.BBS.Permission.BBS_Content);

               spContents.DataBind();
           }
       }

       public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
       {
           if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
           {
               Literal ltlReportID = e.Item.FindControl("ltlReportID") as Literal;
               Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;
               Literal ltlForumName = e.Item.FindControl("ltlForumName") as Literal;

               Literal ltlPost = e.Item.FindControl("ltlPost") as Literal;
               Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
               Literal ltlDatetime = e.Item.FindControl("ltlDatetime") as Literal;
               Literal ltlDel = e.Item.FindControl("ltlDel") as Literal;

               int reportID = (int)DataBinder.Eval(e.Item.DataItem, ReportAttribute.ID);
               string Content = (string)DataBinder.Eval(e.Item.DataItem, ReportAttribute.Content);
               int forumID = (int)DataBinder.Eval(e.Item.DataItem, ReportAttribute.ForumID);
               int postID = (int)DataBinder.Eval(e.Item.DataItem, ReportAttribute.PostID);
               string userName = (string)DataBinder.Eval(e.Item.DataItem, ReportAttribute.UserName);
               int threadID = (int)DataBinder.Eval(e.Item.DataItem, ReportAttribute.ThreadID);

               DateTime dateTime = (DateTime)DataBinder.Eval(e.Item.DataItem, ReportAttribute.AddDate);

               ltlReportID.Text = reportID.ToString();
               ltlContent.Text = Content.ToString();
               ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, forumID);
               if (forumInfo != null)
               {
                   ltlForumName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, forumInfo), forumInfo.ForumName);
               }

               PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, postID);
               if (postInfo != null)
               {
                   int page = ThreadManager.GetPostPage(base.PublishmentSystemID, postInfo.Taxis);
                   ltlPost.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtilityBBS.GetPostUrl(base.PublishmentSystemID, forumID, threadID, page, postID), StringUtils.MaxLengthText(StringUtils.NoHTML(postInfo.Content), 40));

               }

               ltlUserName.Text = userName.ToString();
               ltlDatetime.Text = dateTime.ToString();

               ltlDel.Text = string.Format(@"<a href=""{0}&ReportID={1}"">删除</a>", BackgroundReport.GetRedirectUrl(base.PublishmentSystemID), reportID);
           }
       }
    }
}
