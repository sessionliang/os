using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;
using System.Collections;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundThreadCategoryAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public DropDownList ddlForumIDList;
        public TextBox txtCategoryName;
        public TextBox txtSummary;

        private int categoryID = 0;
        private bool[] isLastNodeArray;

        public static string GetRedirectUrl(int publishmentSystemID, int categoryID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_threadCategoryAdd.aspx?publishmentSystemID={0}&categoryID={1}", publishmentSystemID, categoryID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("categoryID") != null)
            {
                this.categoryID = base.GetIntQueryString("categoryID");
            }

            if (!IsPostBack)
            {
                string pageTitle = this.categoryID > 0 ? "编辑分类信息" : "添加分类信息";
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Forum, pageTitle, AppManager.BBS.Permission.BBS_Forum);

                this.ltlPageTitle.Text = pageTitle;

                ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayList(base.PublishmentSystemID);
                if (forumIDArrayList != null)
                {
                    int forumCount = forumIDArrayList.Count;
                    this.isLastNodeArray = new bool[forumCount + 1];
                    ListItem listItem = null;
                    string value = "";
                    ForumInfo forumInfo = null;
                    foreach (int theForumID in forumIDArrayList)
                    {
                        forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, theForumID);
                        value = (forumInfo.ParentID > 0) ? theForumID.ToString() : "";
                        listItem = new ListItem(ForumManager.GetSelectText(forumInfo, isLastNodeArray, true), value);
                        ddlForumIDList.Items.Add(listItem);
                    }
                }
                if (this.categoryID > 0)
                {
                    ThreadCategoryInfo info = ThreadCategoryManager.GetThreadCategoryInfo(base.PublishmentSystemID, this.categoryID);
                    if (info == null)
                    {
                        base.FailMessage("参数出错！");
                        return;
                    }
                    txtCategoryName.Text = info.CategoryName;
                    txtSummary.Text = info.Summary;
                    ControlUtils.SelectListItemsIgnoreCase(ddlForumIDList, info.ForumID.ToString());
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ThreadCategoryInfo info = null;
                int targetForumID = TranslateUtils.ToInt(ddlForumIDList.SelectedValue);
                if (this.categoryID > 0)
                {
                    info = ThreadCategoryManager.GetThreadCategoryInfo(base.PublishmentSystemID, this.categoryID);
                    if (info == null)
                    {
                        base.FailMessage("参数出错！");
                        return;
                    }
                }
                else
                {
                    info = new ThreadCategoryInfo(base.PublishmentSystemID);
                }

                info.Taxis = DataProvider.ThreadCategoryDAO.GetMaxTaxisByForumID(base.PublishmentSystemID, targetForumID) + 1;
                info.CategoryName = txtCategoryName.Text;
                info.Summary = txtSummary.Text;
                info.ForumID = targetForumID;
                if (info.ForumID <= 0)
                {
                    base.FailMessage("请选择所属版块！");
                    return;
                }
                try
                {
                    if (this.categoryID > 0)
                        DataProvider.ThreadCategoryDAO.Update(info);
                    else
                        DataProvider.ThreadCategoryDAO.Insert(info);
                    base.SuccessMessage("操作成功！");
                    base.AddWaitAndRedirectScript(BackgroundThreadCategory.GetRedirectUrl(base.PublishmentSystemID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "操作失败！");
                }
            }
        }
    }
}