using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.BBS.Model;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundThreadCategory : BackgroundBasePage
    {
        public Repeater rptContents;
        public Button AddCategory;
        public Button Delete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_threadCategory.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("CategoryID") != null)
            { // 排序或删除某一分类
                int categoryID = base.GetIntQueryString("CategoryID");
                if (categoryID > 0)
                {
                    if (base.GetQueryString("Subtract") != null)
                    { // 排序
                        string strSubtract = base.GetQueryString("Subtract").ToLower();
                        UpdateTaxis(strSubtract, categoryID);
                    }
                    if (base.GetQueryString("action") != null)
                    {
                        string strAction = base.GetQueryString("action").ToLower();
                        if (!string.IsNullOrEmpty(strAction) && strAction == "d")
                        {// 删除某一分类
                            DeleteCategory(categoryID);
                        }
                    }
                }
                //base.AddWaitAndRedirectScript(BackgroundThreadCategory.GetRedirectUrl(base.PublishmentSystemID));
                //return;
                //by 20151201 sofuny (修改前原排序代码被注释了，开放回来后排序才正常)
                PageUtils.Redirect(BackgroundThreadCategory.GetRedirectUrl(base.PublishmentSystemID));
            }
            else if (base.GetQueryString("CategoryIDCollection") != null)
            { // 操作多个版块
                string strCategoryIDCollection = base.GetQueryString("CategoryIDCollection");
                ArrayList categoryIDList = TranslateUtils.StringCollectionToArrayList(strCategoryIDCollection, ',');
                string strAction = base.GetQueryString("action").ToLower();
                if (!string.IsNullOrEmpty(strAction) && strAction == "ds") // 批量删除版块
                    this.DeleteCategoryByCategoryIDList(categoryIDList);
            }
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Forum, "分类信息", AppManager.BBS.Permission.BBS_Forum);

                if (!Page.IsStartupScriptRegistered("NodeTreeScript"))
                    Page.RegisterClientScriptBlock("NodeTreeScript", NodeTreeItem.GetScript(null));
                ButtonPreLoad();
                BindGrid();
            }
        }

        private void ButtonPreLoad()
        {
            this.AddCategory.Attributes.Add("onclick", JsUtils.GetRedirectString(BackgroundThreadCategoryAdd.GetRedirectUrl(base.PublishmentSystemID, 0)));
            this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(string.Format("{0}&action=ds", BackgroundThreadCategory.GetRedirectUrl(base.PublishmentSystemID)), "CategoryIDCollection", "CategoryIDCollection", "请选择需要删除的分类！"));
        }

        public void BindGrid()
        {
            this.rptContents.DataSource = DataProvider.ThreadCategoryDAO.GetForumIDArrayList(base.PublishmentSystemID);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.rptContents.DataBind();
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int categoryID = (int)e.Item.DataItem;
            ThreadCategoryInfo info = ThreadCategoryManager.GetThreadCategoryInfo(base.PublishmentSystemID, categoryID);
            Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
            Literal ltlCategoryName = e.Item.FindControl("ltlCategoryName") as Literal;
            Literal ltlForumName = e.Item.FindControl("ltlForumName") as Literal;
            Literal ltlUpLink = e.Item.FindControl("ltlUpLink") as Literal;
            Literal ltlDownLink = e.Item.FindControl("ltlDownLink") as Literal;
            Literal ltlDeleteLink = e.Item.FindControl("ltlDeleteLink") as Literal;
            Literal ltlCheckBox = e.Item.FindControl("ltlCheckBox") as Literal;

            ltlEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", BackgroundThreadCategoryAdd.GetRedirectUrl(base.PublishmentSystemID, info.CategoryID));
            ltlCategoryName.Text = string.Format("<a href=\"{0}\">{1}</a>", BackgroundThreadCategoryAdd.GetRedirectUrl(base.PublishmentSystemID, info.CategoryID), info.CategoryName);
            ForumInfo forum = ForumManager.GetForumInfo(base.PublishmentSystemID, info.ForumID);
            if (forum != null)
                ltlForumName.Text = forum.ForumName;

            string backgroundUrl = BackgroundThreadCategory.GetRedirectUrl(base.PublishmentSystemID);

            ltlUpLink.Text = string.Format(@"<a href=""{0}&Subtract=True&CategoryID={1}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", backgroundUrl, info.CategoryID);
            ltlDownLink.Text = string.Format(@"<a href=""{0}&Subtract=False&CategoryID={1}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", backgroundUrl, info.CategoryID);
            ltlDeleteLink.Text = string.Format(@"<a href=""{0}&action=d&CategoryID={1}"" onclick=""return confirm('此操作将删除所选分类，确定吗？');"">删除</a>", backgroundUrl, info.CategoryID);
            ltlCheckBox.Text = string.Format("<input type='checkbox' name='CategoryIDCollection' value='{0}' />", info.CategoryID);
        }

        private void UpdateTaxis(string subtract, int categoryID)
        {
            if (!string.IsNullOrEmpty(subtract))
            {
                bool isSubtract = TranslateUtils.ToBool(subtract);
                ThreadCategoryManager.UpdateTaxis(base.PublishmentSystemID, categoryID, isSubtract, 1);
            }
        }

        private void DeleteCategory(int categoryID)
        {
            try
            {
                DataProvider.ThreadCategoryDAO.Delete(base.PublishmentSystemID, categoryID);
                base.SuccessMessage("成功删除分类");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, string.Format("删除分类失败，{0}", ex.Message));
            }
            base.AddWaitAndRedirectScript(BackgroundThreadCategory.GetRedirectUrl(base.PublishmentSystemID));
        }

        private void DeleteCategoryByCategoryIDList(ArrayList categoryIDList)
        {
            if (categoryIDList.Count > 0)
            {
                for (int i = 0; i < categoryIDList.Count; i++)
                {
                    try
                    {
                        DataProvider.ThreadCategoryDAO.Delete(base.PublishmentSystemID, TranslateUtils.ToInt(categoryIDList[i].ToString()));
                    }
                    catch
                    {
                    }
                }
                base.SuccessMessage("成功删除分类");
                base.AddWaitAndRedirectScript(BackgroundThreadCategory.GetRedirectUrl(base.PublishmentSystemID));
                return;
            }
        }
    }
}