using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using BaiRong.Core;
using System.Collections;
using BaiRong.Controls;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundPostTrash : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;
        public Button btnRestore;
        public Button btnAllRestore;
        public Button btnDelete;
        public Button btnAllDelete;
        public TextBox txtUserName;
        public TextBox txtTitle;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_postTrash.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("IsRestore")))
            {
                string postIDCollection = base.GetQueryString("PostIDCollection");

                List<int> postIDList = TranslateUtils.StringCollectionToIntList(postIDCollection);
                RestorePost(postIDList);
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("IsAllRestore")))
            {
                DataProvider.PostDAO.AllRestore(base.PublishmentSystemID);
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("IsDelete")))
            {
                string strThreadIDCollection = base.GetQueryString("PostIDCollection");
                List<int> threadIDList = TranslateUtils.StringCollectionToIntList(strThreadIDCollection);
                DeleteThreadBPostIDList(threadIDList);
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("IsAllDelete")))
            {

                DataProvider.PostDAO.DeleteAll(base.PublishmentSystemID); ;
            }
            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = DataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.PostDAO.GetSqlStringTrash(base.PublishmentSystemID, Request.QueryString["UserName"], Request.QueryString["Title"], Request.QueryString["DateFrom"], Request.QueryString["DateTo"]);

            if (!this.Page.IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Content, "帖子回收站", AppManager.BBS.Permission.BBS_Content);

                spContents.DataBind();
                ButtonPreLoad();

                txtUserName.Text = Request.QueryString["UserName"];
                txtTitle.Text = Request.QueryString["Title"];
                DateFrom.Text = Request.QueryString["DateFrom"];
                DateTo.Text = Request.QueryString["DateTo"];
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PostInfo postInfo = new PostInfo(e.Item.DataItem);
                if (postInfo != null)
                {
                    Literal ltlPostID = e.Item.FindControl("ltlPostID") as Literal;
                    Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                    Literal ltlForumName = e.Item.FindControl("ltlForumName") as Literal;
                    Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                    Literal ltlIsThread = e.Item.FindControl("ltlIsThread") as Literal;
                    Literal ltlIsBanned = e.Item.FindControl("ltlIsBanned") as Literal;
                    Literal ltlIP = e.Item.FindControl("ltlIP") as Literal;
                    Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                    Literal ltlCheckBox = e.Item.FindControl("ltlCheckBox") as Literal;

                    ltlPostID.Text = postInfo.ID.ToString();
                    int page = ThreadManager.GetPostPage(base.PublishmentSystemID, postInfo.Taxis);
                    string postUrl = PageUtilityBBS.GetPostUrl(base.PublishmentSystemID, -postInfo.ForumID, postInfo.ThreadID, page, postInfo.ID);
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", postUrl, StringUtils.MaxLengthText(postInfo.Title, 40));
                    ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, -postInfo.ForumID);
                    if (forumInfo != null)
                    {
                        ltlForumName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, forumInfo), forumInfo.ForumName);
                    }
                    ltlUserName.Text = postInfo.UserName;
                    ltlIsThread.Text = postInfo.IsThread ? "是" : "否";
                    ltlIsBanned.Text = postInfo.IsBanned ? "是" : "否";
                    ltlIP.Text = postInfo.IPAddress;
                    ltlAddDate.Text = postInfo.AddDate.ToString("yyyy-MM-dd hh:mm:ss");
                    ltlCheckBox.Text = string.Format("<input type='checkbox' name='PostIDCollection' value='{0}' />", postInfo.ID);
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }

        private void ButtonPreLoad()
        {
            string backgroundUrl = BackgroundPostTrash.GetRedirectUrl(base.PublishmentSystemID);

            this.btnRestore.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.AddQueryString(backgroundUrl, "IsRestore", "True"), "PostIDCollection", "PostIDCollection", "请选择需要还原的帖子！", "确定要还原选中的帖子吗？"));
            this.btnAllRestore.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.AddQueryString(backgroundUrl, "IsAllRestore", "True"), "确定要还原所有的帖子吗？"));
            this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.AddQueryString(backgroundUrl, "IsDelete", "True"), "PostIDCollection", "PostIDCollection", "请选择需要删除的帖子！", "确定要删除选中的帖子吗？"));
            this.btnAllDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.AddQueryString(backgroundUrl, "IsAllDelete", "True"), "确定要删除所有的帖子吗？"));
        }
        private void RestorePost(List<int> postIDList)
        {
            if (postIDList.Count > 0)
            {
                try
                {
                    DataProvider.PostDAO.Restore(base.PublishmentSystemID, postIDList);
                    base.SuccessMessage("成功还原主题");
                }
                catch
                {
                    base.FailMessage("还原失败");
                }

                return;
            }
        }
        private void DeleteThreadBPostIDList(List<int> postIDList)
        {
            if (postIDList.Count > 0)
            {
                string str = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList);
                string[] strs = str.Split(',');
                for (int i = 0; i < strs.Length; i++)
                {
                    int id = ConvertHelper.GetInteger(strs[i]);
                    PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, id);
                    int forumID = postInfo.ForumID;
                    int threadID = postInfo.ThreadID;

                    List<int> postArray = new List<int>();
                    postArray.Add(id);
                    DataProvider.PostDAO.Delete(base.PublishmentSystemID, forumID, threadID, postArray);
                    postArray.Clear();

                }
            }
        }
    }
}
