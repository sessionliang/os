using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core;
using BaiRong.Core;
using System.Collections;
using BaiRong.Controls;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundThreadTrash : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Button btnRestore;
        public Button btnAllRestore;
        public Button btnDelete;
        public Button btnDeleteAll;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_threadTrash.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("IsRestore")))
            {
                string strThreadIDCollection = base.GetQueryString("ThreadIDCollection");
                List<int> threadIDList = TranslateUtils.StringCollectionToIntList(strThreadIDCollection);
                RestoreThread(threadIDList);
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("IsAllRestore")))
            {
                DataProvider.ThreadDAO.AllRestore(base.PublishmentSystemID);
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("IsDelete")))
            {
                string strThreadIDCollection = base.GetQueryString("ThreadIDCollection");
                List<int> threadIDList = TranslateUtils.StringCollectionToIntList(strThreadIDCollection);
                DeleteThreadBThreadIDList(threadIDList);
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("IsAllDelete")))
            {
                DataProvider.ThreadDAO.DeleteAllByTrash(base.PublishmentSystemID);
            }
            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = DataProvider.ConnectionString;

            this.spContents.SelectCommand = DataProvider.ThreadDAO.GetSqlTrashString(base.PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Content, "主题回收站", AppManager.BBS.Permission.BBS_Content);

                spContents.DataBind();
                ButtonPreLoad();
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlThreadID = e.Item.FindControl("ltlThreadID") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlForumName = e.Item.FindControl("ltlForumName") as Literal;
                Literal ltlLastDate = e.Item.FindControl("ltlLastDate") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlEdit = e.Item.FindControl("ltlEdit") as Literal;
                Literal ltlCheckBox = e.Item.FindControl("ltlCheckBox") as Literal;
                int threadID = (int)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.ID);
                string title = (string)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.Title);
                int forumID = (int)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.ForumID);
                string userName = (string)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.UserName);
                DateTime lastDate = (DateTime)DataBinder.Eval(e.Item.DataItem, ThreadAttribute.LastDate);

                ltlThreadID.Text = threadID.ToString();
                ltlTitle.Text = title;
                ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, -forumID);
                if (forumInfo != null)
                {
                    ltlForumName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtilityBBS.GetForumUrl(base.PublishmentSystemID, forumInfo), forumInfo.ForumName);
                }
                ltlUserName.Text = userName;
                if (lastDate != null)
                    ltlLastDate.Text = lastDate.ToString("yyyy-MM-dd hh:mm:ss");
                ltlCheckBox.Text = string.Format("<input type='checkbox' name='ThreadIDCollection' value='{0}' />", threadID);

                ltlEdit.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.ThreadEdit.GetOpenWindowString(base.PublishmentSystemID, threadID, "Edit"));
            }
        }

        private void ButtonPreLoad()
        {
            string backgroundUrl = BackgroundThreadTrash.GetRedirectUrl(base.PublishmentSystemID);

            this.btnRestore.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.AddQueryString(backgroundUrl, "IsRestore", "True"), "ThreadIDCollection", "ThreadIDCollection", "请选择需要还原的主题！", "确定要还原选中的主题吗？"));
            this.btnAllRestore.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.AddQueryString(backgroundUrl, "IsAllRestore", "True"), "确定要还原所有的主题吗？"));
            this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.AddQueryString(backgroundUrl, "IsDelete", "True"), "ThreadIDCollection", "ThreadIDCollection", "请选择需要删除的主题！", "删除此主题,主题包含的帖子也会同时被删除。确定要删除选中的主题吗？"));
            this.btnDeleteAll.Attributes.Add("onclick", JsUtils.GetRedirectStringWithConfirm(PageUtils.AddQueryString(backgroundUrl, "IsAllDelete", "True"), "确定要删除所有的主题吗？"));
        }

        private void RestoreThread(List<int> threadIDList)
        {
            if (threadIDList.Count > 0)
            {
                try
                {
                    DataProvider.ThreadDAO.Restore(base.PublishmentSystemID, threadIDList);
                    base.SuccessMessage("成功还原主题");
                }
                catch
                {
                    base.FailMessage("还原失败");
                }
                
                return;
            }
        }

        private void DeleteThreadBThreadIDList(List<int> threadIDList)
        {

            if (threadIDList.Count > 0)
            {
                try
                {
                    DataProvider.ThreadDAO.Delete(base.PublishmentSystemID, 0, threadIDList);
                    base.SuccessMessage("成功删除主题");
                }
                catch
                {
                    base.FailMessage("删除失败");
                }
               
                return;
            }
        }
    }
}
