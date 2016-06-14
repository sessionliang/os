using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundStoreCategory : BackgroundBasePageWX
    {
        public Repeater rptContents;

        public Button btnAdd;
        public Button btnDelete;

        private int categoryID;

        public static string GetRedirectUrl(int publishmentSystemID, int categoryID)
        {
            return PageUtils.GetWXUrl(string.Format("background_storeCategory.aspx?publishmentSystemID={0}&categoryID={1}", publishmentSystemID, categoryID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.categoryID = TranslateUtils.ToInt(base.Request.QueryString["categoryID"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]) && !string.IsNullOrEmpty(base.Request.QueryString["CategoryIDCollection"]))
            {
                ArrayList categoryIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["CategoryIDCollection"]);
                foreach (int theCategoryID in categoryIDArrayList)
                {
                    DataProviderWX.StoreCategoryDAO.Delete(base.PublishmentSystemID, theCategoryID);
                }
                base.SuccessMessage("成功删除所选区域");
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["Subtract"]) || !string.IsNullOrEmpty(base.Request.QueryString["Add"]))
            {
                bool isSubtract = (!string.IsNullOrEmpty(base.Request.QueryString["Subtract"])) ? true : false;
                DataProviderWX.StoreCategoryDAO.UpdateTaxis(base.PublishmentSystemID, this.categoryID, isSubtract);

                PageUtils.Redirect(BackgroundStoreCategory.GetRedirectUrl(base.PublishmentSystemID, this.categoryID));
                return;
            }

            BindGrid();

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Store, "门店属性管理", AppManager.WeiXin.Permission.WebSite.Store);
                base.RegisterClientScriptBlock("NodeTreeScript", CategoryTreeItem.GetScript(base.PublishmentSystemID, ECategoryLoadingType.Category, null));

                if (this.categoryID > 0)
                {
                    string onLoadScript = this.GetScriptOnLoad();
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        Page.RegisterClientScriptBlock("NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                NameValueCollection arguments = new NameValueCollection();
                string showPopWinString = string.Empty;

                this.btnAdd.Attributes.Add("onclick", Modal.StoreCategoryAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));

                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetWXUrl("background_storeCategory.aspx?Delete=True&publishmentSystemID=" + this.PublishmentSystemID), "CategoryIDCollection", "CategoryIDCollection", "请选择需要删除的门店属性！", "此操作将删除所选门店属性，确认删除吗？"));

            }

        }

        public string GetScriptOnLoad()
        {
            if (this.categoryID > 0)
            {
                StoreCategoryInfo categoryInfo = DataProviderWX.StoreCategoryDAO.GetCategoryInfo(this.categoryID);
                if (categoryInfo != null)
                {
                    string path = string.Empty;
                    if (categoryInfo.ParentsCount <= 1)
                    {
                        path = this.categoryID.ToString();
                    }
                    else
                    {
                        path = categoryInfo.ParentsPath.Substring(categoryInfo.ParentsPath.IndexOf(",") + 1) + "," + this.categoryID.ToString();
                    }
                    return CategoryTreeItem.GetScriptOnLoad(path);
                }
            }
            return string.Empty;
        }

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = DataProviderWX.StoreCategoryDAO.GetCategoryIDListByParentID(base.PublishmentSystemID, 0);
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

            StoreCategoryInfo categoryInfo = DataProviderWX.StoreCategoryDAO.GetCategoryInfo(categoryID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = BackgroundStoreCategory.GetCategoryRowHtml(base.PublishmentSystemID, categoryInfo, ECategoryLoadingType.Category, null);
        }

        public static string GetCategoryRowHtml(int publishmentSystemID, StoreCategoryInfo categoryInfo, ECategoryLoadingType loadingType, NameValueCollection additional)
        {
            CategoryTreeItem treeItem = CategoryTreeItem.CreateInstance(categoryInfo);
            string title = treeItem.GetItemHtml(loadingType, additional, false);

            string rowHtml = string.Empty;

            if (loadingType == ECategoryLoadingType.Category)
            {
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string checkBoxHtml = string.Empty;


                string urlEdit = Modal.StoreCategoryAdd.GetOpenWindowStringToEdit(publishmentSystemID, categoryInfo.ID);
                editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", urlEdit);

                string categoryUrl = BackgroundStoreCategory.GetRedirectUrl(publishmentSystemID, categoryInfo.ID);

                string urlUp = string.Format("{0}&Subtract=True", categoryUrl);
                upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                string urlDown = string.Format("{0}&Add=True", categoryUrl);
                downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                checkBoxHtml = string.Format("<input type='checkbox' name='CategoryIDCollection' value='{0}' />", categoryInfo.ID);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td class=""center"">{2}</td>
    <td class=""center"">{3}</td>
    <td class=""center"">{4}</td>
    <td class=""center"">{5}</td>
</tr>
", categoryInfo.ParentsCount + 1, title, upLink, downLink, editUrl, checkBoxHtml);
            }
            return rowHtml;
        }

    }
}
