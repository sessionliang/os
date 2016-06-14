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

namespace SiteServer.BBS.BackgroundPages {

    public class BackgroundThread : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;
        public Button btnTranslate;
        public Button btnDelete;
        public Button btnTop;
        public Button btnDigest;
        public Button btnSearch;
        public TextBox txtUserName;
        public TextBox txtTitle;
        public DropDownList ddlForum;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_thread.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("threadID") != null)
            {
                // 排序或删除某一主题
                int threadID = base.GetIntQueryString("threadID");
                if (threadID > 0)
                {
                    if (base.GetQueryString("Subtract") != null)
                    {
                        // 排序
                        string strSubtract = base.GetQueryString("Subtract").ToLower();
                        UpdateTaxis(strSubtract, threadID);
                    }
                    if (base.GetQueryString("action") != null)
                    {
                        string strAction = base.GetQueryString("action").ToLower();
                        if (!string.IsNullOrEmpty(strAction) && strAction == "d")
                        {
                            // 删除某一主题
                            try
                            {
                                int forumID = DataProvider.ThreadDAO.GetForumID(threadID);
                                DataProvider.ThreadDAO.Delete(base.PublishmentSystemID, forumID, TranslateUtils.ToIntList(threadID));
                                base.SuccessMessage("成功删除主题");
                            }
                            catch (Exception ex)
                            {
                                base.SuccessMessage(string.Format("删除主题失败，{0}", ex.Message));
                            }
                        }
                    }
                }
                base.AddWaitAndRedirectScript(BackgroundThread.GetRedirectUrl(base.PublishmentSystemID));
                return;
            }
            else if (base.GetQueryString("ThreadIDCollection") != null)
            {
                // 操作多个主题
                string strThreadIDCollection = base.GetQueryString("ThreadIDCollection");
                List<int> threadIDList = TranslateUtils.StringCollectionToIntList(strThreadIDCollection);
                string strAction = base.GetQueryString("action").ToLower();
                if (!string.IsNullOrEmpty(strAction) && strAction == "ds")
                {
                    // 批量删除主题
                    DeleteThreadBThreadIDList(threadIDList);
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = DataProvider.ConnectionString;

            this.spContents.SelectCommand = DataProvider.ThreadDAO.GetSqlString(base.PublishmentSystemID, Request.QueryString["UserName"], Request.QueryString["Title"], Request.QueryString["DateFrom"], Request.QueryString["DateTo"], Request.QueryString["ForumID"]);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Content, "主题管理", AppManager.BBS.Permission.BBS_Content);

                spContents.DataBind();
                ForumBind();
                ButtonPreLoad();
                txtUserName.Text = Request.QueryString["UserName"];
                txtTitle.Text = Request.QueryString["Title"];
                DateFrom.Text = Request.QueryString["DateFrom"];
                DateTo.Text = Request.QueryString["DateTo"];
                ddlForum.SelectedValue = Request.QueryString["ForumID"];
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
                Literal ltlThreadID = e.Item.FindControl("ltlThreadID") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlForumName = e.Item.FindControl("ltlForumName") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlReplies = e.Item.FindControl("ltlReplies") as Literal;
                Literal ltlHits = e.Item.FindControl("ltlHits") as Literal;
                Literal ltlLastDate = e.Item.FindControl("ltlLastDate") as Literal;
                Literal ltlEdit = e.Item.FindControl("ltlEdit") as Literal;
                Literal ltlCheckBox = e.Item.FindControl("ltlCheckBox") as Literal;

                int threadID = (int)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.ID);
                string title = (string)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.Title);
                int forumID = (int)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.ForumID);
                string userName = (string)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.UserName);
                int replies = (int)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.Replies);
                int hits = (int)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.Hits);
                DateTime lastDate = (DateTime)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.LastDate);

                ltlThreadID.Text = threadID.ToString();
                ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtilityBBS.GetThreadUrl(base.PublishmentSystemID, forumID, threadID), title);
                ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, forumID);
                if (forumInfo != null)
                {
                    ltlForumName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, forumInfo), forumInfo.ForumName);
                }
                ltlUserName.Text = userName;
                ltlReplies.Text = replies.ToString();
                ltlHits.Text = hits.ToString();
                if(lastDate != null)
                    ltlLastDate.Text = lastDate.ToString("yyyy-MM-dd hh:mm:ss");
                ltlCheckBox.Text = string.Format("<input type='checkbox' name='ThreadIDCollection' value='{0}' />", threadID);

                ltlEdit.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.ThreadEdit.GetOpenWindowString(base.PublishmentSystemID, threadID, "Edit"));
            }
        }

        private void ButtonPreLoad()
        {
            this.btnTranslate.Attributes.Add("onclick", Modal.ThreadTranslate.GetOpenWindowString(base.PublishmentSystemID));
            this.btnTop.Attributes.Add("onclick", Modal.ThreadTopLevel.GetOpenWindowString(base.PublishmentSystemID));
            this.btnDigest.Attributes.Add("onclick", Modal.ThreadDigest.GetOpenWindowString(base.PublishmentSystemID));
            this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&action=ds", BackgroundThread.GetRedirectUrl(base.PublishmentSystemID)), "ThreadIDCollection", "ThreadIDCollection", "请选择需要删除的主题！", "确定要删除选中的主题吗？"));
        }

        private void UpdateTaxis(string subtract, int threadID) {

            if(!string.IsNullOrEmpty(subtract)) {
                bool isSubtract = TranslateUtils.ToBool(subtract);
                DataProvider.ForumDAO.UpdateTaxis(base.PublishmentSystemID, threadID, isSubtract);
            }
        }

        private void DeleteThreadBThreadIDList(List<int> threadIDList)
        {
            if (threadIDList.Count > 0)
            {
                DataProvider.ThreadDAO.Delete(base.PublishmentSystemID, 0, threadIDList);
                base.SuccessMessage("成功删除主题");
                return;
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
                ListItem listItem = new ListItem(str+forumInfo.ForumName, forumInfo.ForumID.ToString());
                this.ddlForum.Items.Add(listItem);
                BindChild(forumInfo.ForumID);
            }
        }

        public void Search_OnClick(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string title = txtTitle.Text;
            string dateFrom = ConvertHelper.GetString(DateFrom.Text);
            string dateTo = ConvertHelper.GetString(DateTo.Text);
            string forumID = ddlForum.SelectedValue;
            string url = string.Format("{0}&UserName={1}&Title={2}&DateFrom={3}&DateTo={4}&ForumID={5}", BackgroundThread.GetRedirectUrl(base.PublishmentSystemID), userName, title, dateFrom, dateTo, forumID);
            Response.Redirect(url);
        }
    }
}
