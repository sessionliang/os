using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages
{
    public class BackgroundDepartment : BackgroundBasePage
    {
        public Repeater rptContents;

        public Button AddChannel;
        public Button Translate;
        public Button Import;
        public Button Export;
        public Button Delete;

        private int currentDepartmentID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("Delete")) && !string.IsNullOrEmpty(base.GetQueryString("DepartmentIDCollection")))
            {
                ArrayList departmentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("DepartmentIDCollection"));
                foreach (int departmentID in departmentIDArrayList)
                {
                    BaiRongDataProvider.DepartmentDAO.Delete(departmentID);
                }
                base.SuccessMessage("成功删除所选部门");
            }
            else if (!string.IsNullOrEmpty(base.GetQueryString("DepartmentID")) && (!string.IsNullOrEmpty(base.GetQueryString("Subtract")) || !string.IsNullOrEmpty(base.GetQueryString("Add"))))
            {
                int departmentID = base.GetIntQueryString("DepartmentID");
                bool isSubtract = (!string.IsNullOrEmpty(base.GetQueryString("Subtract"))) ? true : false;
                BaiRongDataProvider.DepartmentDAO.UpdateTaxis(departmentID, isSubtract);

                PageUtils.Redirect(BackgroundDepartment.GetRedirectUrl(departmentID));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_AdminAttributes, "所属部门管理", AppManager.Platform.Permission.Platform_AdminAttributes);

                Page.RegisterClientScriptBlock("NodeTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.ContentList, null));

                if (!string.IsNullOrEmpty(base.GetQueryString("CurrentDepartmentID")))
                {
                    this.currentDepartmentID = base.GetIntQueryString("CurrentDepartmentID");
                    string onLoadScript = this.GetScriptOnLoad(this.currentDepartmentID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        Page.RegisterClientScriptBlock("NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                NameValueCollection arguments = new NameValueCollection();
                string showPopWinString = string.Empty;

                this.AddChannel.Attributes.Add("onclick", Modal.DepartmentAdd.GetOpenWindowStringToAdd(BackgroundDepartment.GetRedirectUrl(0)));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetPlatformUrl("background_department.aspx?Delete=True"), "DepartmentIDCollection", "DepartmentIDCollection", "请选择需要删除的部门！", "此操作将删除对应部门以及所有下级部门，确认删除吗？"));

                BindGrid();
            }
        }

        public static string GetRedirectUrl(int currentDepartmentID)
        {
            if (currentDepartmentID != 0)
            {
                return PageUtils.GetPlatformUrl(string.Format("background_department.aspx?CurrentDepartmentID={0}", currentDepartmentID));
            }
            return PageUtils.GetPlatformUrl("background_department.aspx");
        }

        public string GetScriptOnLoad(int currentDepartmentID)
        {
            if (currentDepartmentID != 0)
            {
                DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(currentDepartmentID);
                if (departmentInfo != null)
                {
                    string path = string.Empty;
                    if (departmentInfo.ParentsCount <= 1)
                    {
                        path = currentDepartmentID.ToString();
                    }
                    else
                    {
                        path = departmentInfo.ParentsPath.Substring(departmentInfo.ParentsPath.IndexOf(",") + 1) + "," + currentDepartmentID.ToString();
                    }
                    return DepartmentTreeItem.GetScriptOnLoad(path);
                }
            }
            return string.Empty;
        }

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(0);
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
            int departmentID = (int)e.Item.DataItem;

            DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = BackgroundDepartment.GetDepartmentRowHtml(departmentInfo, EDepartmentLoadingType.ContentList, null);
        }

        public static string GetDepartmentRowHtml(DepartmentInfo departmentInfo, EDepartmentLoadingType loadingType, NameValueCollection additional)
        {
            DepartmentTreeItem treeItem = DepartmentTreeItem.CreateInstance(departmentInfo);
            string title = treeItem.GetItemHtml(loadingType, additional, false);

            string rowHtml = string.Empty;

            if (loadingType == EDepartmentLoadingType.AdministratorTree || loadingType == EDepartmentLoadingType.DepartmentSelect || loadingType == EDepartmentLoadingType.ContentTree)
            {
                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td nowrap>{1}</td>
</tr>
", departmentInfo.ParentsCount + 1, title);
            }
            else if (loadingType == EDepartmentLoadingType.ContentList)
            {
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string checkBoxHtml = string.Empty;

                editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.DepartmentAdd.GetOpenWindowStringToEdit(departmentInfo.DepartmentID, BackgroundDepartment.GetRedirectUrl(departmentInfo.DepartmentID)));

                string urlUp = PageUtils.GetPlatformUrl(string.Format("background_department.aspx?Subtract=True&DepartmentID={0}", departmentInfo.DepartmentID));
                upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                string urlDown = PageUtils.GetPlatformUrl(string.Format("background_department.aspx?Add=True&DepartmentID={0}", departmentInfo.DepartmentID));
                downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                checkBoxHtml = string.Format("<input type='checkbox' name='DepartmentIDCollection' value='{0}' />", departmentInfo.DepartmentID);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td>&nbsp;{2}</td>
    <td class=""center"">{3}</td>
    <td class=""center"">{4}</td>
    <td class=""center"">{5}</td>
    <td class=""center"">{6}</td>
    <td class=""center"">{7}</td>
</tr>
", departmentInfo.ParentsCount + 1, title, departmentInfo.Code, departmentInfo.CountOfAdmin, upLink, downLink, editUrl, checkBoxHtml);
            }
            else if (loadingType == EDepartmentLoadingType.GovPublicDepartment)
            {
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string checkBoxHtml = string.Empty;

                int publishmentSystemID = TranslateUtils.ToInt(additional["PublishmentSystemID"]);

                string returnUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicDepartment.aspx?PublishmentSystemID={0}&CurrentDepartmentID={1}", publishmentSystemID, departmentInfo.DepartmentID));

                editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.DepartmentAdd.GetOpenWindowStringToEdit(departmentInfo.DepartmentID, returnUrl));
                if (departmentInfo.ParentID > 0)
                {
                    string urlUp = PageUtils.GetWCMUrl(string.Format("background_govPublicDepartment.aspx?PublishmentSystemID={0}&Subtract=True&DepartmentID={1}", publishmentSystemID, departmentInfo.DepartmentID));
                    upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                    string urlDown = PageUtils.GetWCMUrl(string.Format("background_govPublicDepartment.aspx?PublishmentSystemID={0}&Add=True&DepartmentID={1}", publishmentSystemID, departmentInfo.DepartmentID));
                    downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                    checkBoxHtml = string.Format("<input type='checkbox' name='DepartmentIDCollection' value='{0}' />", departmentInfo.DepartmentID);
                }

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td>&nbsp;{2}</td>
    <td class=""center"">{3}</td>
    <td class=""center"">{4}</td>
    <td class=""center"">{5}</td>
    <td class=""center"">{6}</td>
</tr>
", departmentInfo.ParentsCount + 1, title, departmentInfo.Code, upLink, downLink, editUrl, checkBoxHtml);
            }
            return rowHtml;
        }
    }
}
