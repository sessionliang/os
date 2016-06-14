using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.BBS.Model;
using SiteServer.BBS.Core;
using System.Collections.Generic;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundPost : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;
        public Button btnShield;
        public Button btnUnShield;
        public Button btnDelete;
        public Button btnSearch;
        public TextBox txtUserName;
        public TextBox txtTitle;
        public DropDownList ddlForum;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_post.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("PostIDCollection") != null)
            {
                string strPostIDCollection = base.GetQueryString("PostIDCollection");
                List<int> postIDList = TranslateUtils.StringCollectionToIntList(strPostIDCollection);
                string strAction = base.GetQueryString("action").ToLower();
                if (!string.IsNullOrEmpty(strAction) && strAction == "del")
                {
                    // 批量删除帖子
                    DeletePostByPostIDList(postIDList);
                }
                if (!string.IsNullOrEmpty(strAction) && strAction == "shield")
                {
                    //批量屏蔽帖子
                    DataProvider.PostDAO.Ban(base.PublishmentSystemID, 0, postIDList, true);
                }
                if (!string.IsNullOrEmpty(strAction) && strAction == "unshield")
                {
                    //批量解除屏蔽帖子
                    DataProvider.PostDAO.Ban(base.PublishmentSystemID, 0, postIDList, false);
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = DataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.PostDAO.GetSqlString(base.PublishmentSystemID, Request.QueryString["UserName"], Request.QueryString["Title"], Request.QueryString["DateFrom"], Request.QueryString["DateTo"], Request.QueryString["ForumID"]);

            if (!this.Page.IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Content, "帖子管理", AppManager.BBS.Permission.BBS_Content);

                spContents.DataBind();
                ButtonPreLoad();
                ForumBind();
                txtUserName.Text = Request.QueryString["UserName"];
                txtTitle.Text = Request.QueryString["Title"];
                DateFrom.Text = Request.QueryString["DateFrom"];
                DateTo.Text = Request.QueryString["DateTo"];
                ddlForum.SelectedValue = Request.QueryString["ForumID"];
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
                    string postUrl = PageUtilityBBS.GetPostUrl(base.PublishmentSystemID, postInfo.ForumID, postInfo.ThreadID, page, postInfo.ID);
                    ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", postUrl,StringUtils.MaxLengthText(postInfo.Title,40));
                    ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, postInfo.ForumID);
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

        public void Search_OnClick(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string title = txtTitle.Text;
            string dateFrom = ConvertHelper.GetString(DateFrom.Text);
            string dateTo = ConvertHelper.GetString(DateTo.Text);
            string forumID = ddlForum.SelectedValue;
            string url = string.Format("{0}&UserName={1}&Title={2}&DateFrom={3}&DateTo={4}&ForumID={5}", BackgroundPost.GetRedirectUrl(base.PublishmentSystemID), userName, title, dateFrom, dateTo, forumID);
            Response.Redirect(url);
        }

        private void ButtonPreLoad()
        {
            string backgroundUrl = BackgroundPost.GetRedirectUrl(base.PublishmentSystemID);

            this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&action=del", backgroundUrl), "PostIDCollection", "PostIDCollection", "请选择需要删除的帖子！", "确定要删除选中的帖子吗？"));
            this.btnShield.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&action=shield", backgroundUrl), "PostIDCollection", "PostIDCollection", "请选择需要屏蔽的帖子！", "确定要屏蔽选中的帖子吗？"));
            this.btnUnShield.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&action=unshield", backgroundUrl), "PostIDCollection", "PostIDCollection", "请选择需要解除屏蔽的帖子！", "确定要解除屏蔽选中的帖子吗？"));
        }

        private void DeletePostByPostIDList(List<int> postIDList)
        {
            if (postIDList.Count > 0)
            {
                string str = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList);
                string[] strs=str.Split(',');
                for (int i = 0; i < strs.Length; i++)
                {
                    int id = ConvertHelper.GetInteger(strs[i]);
                    PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(base.PublishmentSystemID, id);
                    int forumID = postInfo.ForumID;
                    int threadID = postInfo.ThreadID;
                    if (postInfo.IsThread)
                    {
                        List<int> threadArray = new List<int>();
                        threadArray.Add(threadID);
                        DataProvider.ThreadDAO.Delete(base.PublishmentSystemID, forumID, threadArray);
                        threadArray.Clear();
                    }
                    else
                    {
                        List<int> postArray = new List<int>();
                        postArray.Add(id);
                        DataProvider.PostDAO.Delete(base.PublishmentSystemID, forumID, threadID, postArray);
                        postArray.Clear();
                    }
                }
            }
        }

        protected void ForumBind()
        {
            ListItem theListItem = new ListItem("全部", "0");
            theListItem.Selected = true;
            this.ddlForum.Items.Add(theListItem);
            ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayListByParentID(base.PublishmentSystemID, 0);
            foreach (int forumID in forumIDArrayList)
            {
                ForumInfo forumInfo = DataProvider.ForumDAO.GetForumInfo(base.PublishmentSystemID, forumID);
                ListItem listItem = new ListItem(forumInfo.ForumName, forumInfo.ForumID.ToString());
                this.ddlForum.Items.Add(listItem);
                BindChild(forumInfo.ForumID);
            }
        }

        protected void BindChild(int parentID)
        {
            ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayListByParentID(base.PublishmentSystemID, parentID);
            foreach (int forumID in forumIDArrayList)
            {
                ForumInfo forumInfo = DataProvider.ForumDAO.GetForumInfo(base.PublishmentSystemID, forumID);
                string str = string.Empty;
                if (forumInfo.IsLastNode)
                {
                    str = new string('　', TranslateUtils.StringCollectionToArrayList(forumInfo.ParentsPath).Count - 1) + "└";
                }
                else
                {
                    str = new string('　', TranslateUtils.StringCollectionToArrayList(forumInfo.ParentsPath).Count - 1) + "├";
                }
                ListItem listItem = new ListItem(str + forumInfo.ForumName, forumInfo.ForumID.ToString());
                this.ddlForum.Items.Add(listItem);
                BindChild(forumInfo.ForumID);
            }
        }
    }
}
