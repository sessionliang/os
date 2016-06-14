using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;


using BaiRong.BackgroundPages;
using SiteServer.CMS.Core;
using BaiRong.Core.Data.Provider;


namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicDepartment : BackgroundGovPublicBasePage
    {
        public Repeater rptContents;

        public Button AddDepartment;
        public Button Delete;

        private int currentDepartmentID;
        private NameValueCollection additional = new NameValueCollection();

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]) && !string.IsNullOrEmpty(base.Request.QueryString["DepartmentIDCollection"]))
            {
                ArrayList departmentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["DepartmentIDCollection"]);
                foreach (int departmentID in departmentIDArrayList)
                {
                    BaiRongDataProvider.DepartmentDAO.Delete(departmentID);
                }
                base.SuccessMessage("成功删除所选部门");
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["DepartmentID"]) && (!string.IsNullOrEmpty(base.Request.QueryString["Subtract"]) || !string.IsNullOrEmpty(base.Request.QueryString["Add"])))
            {
                int departmentID = int.Parse(base.Request.QueryString["DepartmentID"]);
                bool isSubtract = (!string.IsNullOrEmpty(base.Request.QueryString["Subtract"])) ? true : false;
                BaiRongDataProvider.DepartmentDAO.UpdateTaxis(departmentID, isSubtract);

                PageUtils.Redirect(this.GetRedirectUrl(departmentID));
                return;
            }

            this.additional["PublishmentSystemID"] = base.PublishmentSystemID.ToString();

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContentConfiguration, "机构分类设置", AppManager.CMS.Permission.WebSite.GovPublicContentConfiguration);

                Page.RegisterClientScriptBlock("NodeTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.GovPublicDepartment, this.additional));

                if (!string.IsNullOrEmpty(Request.QueryString["CurrentDepartmentID"]))
                {
                    this.currentDepartmentID = TranslateUtils.ToInt(base.Request.QueryString["CurrentDepartmentID"]);
                    string onLoadScript = this.GetScriptOnLoad(this.currentDepartmentID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        Page.RegisterClientScriptBlock("NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                NameValueCollection arguments = new NameValueCollection();
                string showPopWinString = string.Empty;

                this.AddDepartment.Attributes.Add("onclick", Modal.GovPublicDepartmentAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.GetRedirectUrl(0)));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetWCMUrl(string.Format("background_govPublicDepartment.aspx?PublishmentSystemID={0}&Delete=True", base.PublishmentSystemID)), "DepartmentIDCollection", "DepartmentIDCollection", "请选择需要删除的部门！", "此操作将删除对应部门以及所有下级部门，确认删除吗？"));

                BindGrid();
            }
        }

        public string GetRedirectUrl(int currentDepartmentID)
        {
            if (currentDepartmentID != 0)
            {
                return PageUtils.GetWCMUrl(string.Format("background_govPublicDepartment.aspx?PublishmentSystemID={0}&CurrentDepartmentID={1}", base.PublishmentSystemID, currentDepartmentID));
            }
            return PageUtils.GetWCMUrl(string.Format("background_govPublicDepartment.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
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
                this.rptContents.DataSource = GovPublicManager.GetFirstDepartmentIDArrayList(base.PublishmentSystemInfo);
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

            ltlHtml.Text = BackgroundDepartment.GetDepartmentRowHtml(departmentInfo, EDepartmentLoadingType.GovPublicDepartment, this.additional);
        }
    }
}
