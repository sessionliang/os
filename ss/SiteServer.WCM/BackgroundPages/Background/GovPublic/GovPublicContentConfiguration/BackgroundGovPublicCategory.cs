using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;


using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicCategory : BackgroundGovPublicBasePage
    {
        public Repeater rptContents;

        public Button AddChannel;
        public Button Translate;
        public Button Import;
        public Button Export;
        public Button Delete;

        private int currentCategoryID;
        public GovPublicCategoryClassInfo categoryClassInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            string classCode = base.Request.QueryString["ClassCode"];
            this.categoryClassInfo = DataProvider.GovPublicCategoryClassDAO.GetCategoryClassInfo(classCode, base.PublishmentSystemID);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]) && !string.IsNullOrEmpty(base.Request.QueryString["CategoryIDCollection"]))
            {
                ArrayList categoryIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["CategoryIDCollection"]);
                foreach (int categoryID in categoryIDArrayList)
                {
                    DataProvider.GovPublicCategoryDAO.Delete(categoryID);
                }
                base.SuccessMessage("成功删除所选节点");
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["CategoryID"]) && (!string.IsNullOrEmpty(base.Request.QueryString["Subtract"]) || !string.IsNullOrEmpty(base.Request.QueryString["Add"])))
            {
                int categoryID = int.Parse(base.Request.QueryString["CategoryID"]);
                bool isSubtract = (!string.IsNullOrEmpty(base.Request.QueryString["Subtract"])) ? true : false;
                DataProvider.GovPublicCategoryDAO.UpdateTaxis(this.categoryClassInfo.ClassCode, base.PublishmentSystemID, categoryID, isSubtract);

                PageUtils.Redirect(BackgroundGovPublicCategory.GetRedirectUrl(this.categoryClassInfo.ClassCode, base.PublishmentSystemID, categoryID));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContentConfiguration, "分类法管理", this.ClassName + "分类", AppManager.CMS.Permission.WebSite.GovPublicContentConfiguration);

                Page.RegisterClientScriptBlock("NodeTreeScript", GovPublicCategoryTreeItem.GetScript(this.categoryClassInfo.ClassCode, base.PublishmentSystemID, EGovPublicCategoryLoadingType.List, null));

                if (!string.IsNullOrEmpty(Request.QueryString["CurrentCategoryID"]))
                {
                    this.currentCategoryID = TranslateUtils.ToInt(base.Request.QueryString["CurrentCategoryID"]);
                    string onLoadScript = this.GetScriptOnLoad(this.currentCategoryID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        Page.RegisterClientScriptBlock("NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                NameValueCollection arguments = new NameValueCollection();
                string showPopWinString = string.Empty;

                this.AddChannel.Attributes.Add("onclick", Modal.GovPublicCategoryAdd.GetOpenWindowStringToAdd(this.categoryClassInfo.ClassCode, base.PublishmentSystemID, BackgroundGovPublicCategory.GetRedirectUrl(this.categoryClassInfo.ClassCode, base.PublishmentSystemID, 0)));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetWCMUrl(string.Format("background_govPublicCategory.aspx?ClassCode={0}&PublishmentSystemID={1}&Delete=True", this.categoryClassInfo.ClassCode, base.PublishmentSystemID)), "CategoryIDCollection", "CategoryIDCollection", "请选择需要删除的节点！", "此操作将删除对应节点以及所有下级节点，确认删除吗？"));

                BindGrid();
            }
        }

        public string ClassName
        {
            get
            {
                if (this.categoryClassInfo != null)
                {
                    return this.categoryClassInfo.ClassName;
                }
                return string.Empty;
            }
        }

        public static string GetRedirectUrl(string classCode, int publishmentSystemID, int currentCategoryID)
        {
            if (currentCategoryID != 0)
            {
                return PageUtils.GetWCMUrl(string.Format("background_govPublicCategory.aspx?ClassCode={0}&PublishmentSystemID={1}&CurrentCategoryID={2}", classCode, publishmentSystemID, currentCategoryID));
            }
            return PageUtils.GetWCMUrl(string.Format("background_govPublicCategory.aspx?ClassCode={0}&PublishmentSystemID={1}", classCode, publishmentSystemID));
        }

        public string GetScriptOnLoad(int currentCategoryID)
        {
            if (currentCategoryID != 0)
            {
                GovPublicCategoryInfo categoryInfo = DataProvider.GovPublicCategoryDAO.GetCategoryInfo(currentCategoryID);
                if (categoryInfo != null)
                {
                    string path = string.Empty;
                    if (categoryInfo.ParentsCount <= 1)
                    {
                        path = currentCategoryID.ToString();
                    }
                    else
                    {
                        path = categoryInfo.ParentsPath.Substring(categoryInfo.ParentsPath.IndexOf(",") + 1) + "," + currentCategoryID.ToString();
                    }
                    return GovPublicCategoryTreeItem.GetScriptOnLoad(path);
                }
            }
            return string.Empty;
        }

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = DataProvider.GovPublicCategoryDAO.GetCategoryIDArrayListByParentID(this.categoryClassInfo.ClassCode, base.PublishmentSystemID, 0);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int categoryID = (int)e.Item.DataItem;

            GovPublicCategoryInfo categoryInfo = DataProvider.GovPublicCategoryDAO.GetCategoryInfo(categoryID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = BackgroundGovPublicCategory.GetCategoryRowHtml(categoryInfo, true, EGovPublicCategoryLoadingType.List, null);
        }

        public static string GetCategoryRowHtml(GovPublicCategoryInfo categoryInfo, bool enabled, EGovPublicCategoryLoadingType loadingType, NameValueCollection additional)
        {
            GovPublicCategoryTreeItem treeItem = GovPublicCategoryTreeItem.CreateInstance(categoryInfo, enabled);
            string title = treeItem.GetItemHtml(loadingType);

            string rowHtml = string.Empty;

            if (loadingType == EGovPublicCategoryLoadingType.Tree || loadingType == EGovPublicCategoryLoadingType.Select)
            {
                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td nowrap>
		{1}
	</td>
</tr>
", categoryInfo.ParentsCount + 1, title);
            }
            else if (loadingType == EGovPublicCategoryLoadingType.List)
            {
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string checkBoxHtml = string.Empty;

                if (enabled)
                {
                    editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.GovPublicCategoryAdd.GetOpenWindowStringToEdit(categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.CategoryID, BackgroundGovPublicCategory.GetRedirectUrl(categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.CategoryID)));

                    string urlUp = PageUtils.GetWCMUrl(string.Format("background_govPublicCategory.aspx?ClassCode={0}&PublishmentSystemID={1}&Subtract=True&CategoryID={2}", categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.CategoryID));
                    upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                    string urlDown = PageUtils.GetWCMUrl(string.Format("background_govPublicCategory.aspx?ClassCode={0}&PublishmentSystemID={1}&Add=True&CategoryID={2}", categoryInfo.ClassCode, categoryInfo.PublishmentSystemID, categoryInfo.CategoryID));
                    downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                    checkBoxHtml = string.Format("<input type='checkbox' name='CategoryIDCollection' value='{0}' />", categoryInfo.CategoryID);
                }

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td>{2}</td>
    <td class=""center"">{3}</td>
    <td class=""center"">{4}</td>
    <td class=""center"">{5}</td>
    <td class=""center"">{6}</td>
</tr>
", categoryInfo.ParentsCount + 1, title, categoryInfo.CategoryCode, upLink, downLink, editUrl, checkBoxHtml);
            }
            return rowHtml;
        }
    }
}
