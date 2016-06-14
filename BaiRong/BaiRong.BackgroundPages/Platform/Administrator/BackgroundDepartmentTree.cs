using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;

using System.Collections.Generic;
using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages
{
    public class BackgroundDepartmentTree : BackgroundBasePage
    {
        public Repeater rptDepartment;

        private NameValueCollection additional = new NameValueCollection();

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.additional.Add("module", base.GetQueryString("module"));

            if (!IsPostBack)
            {
                Page.RegisterClientScriptBlock("NodeTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.AdministratorTree, null));

                this.BindGrid();
            }
        }

        public void BindGrid()
        {
            try
            {
                this.rptDepartment.DataSource = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(0);
                this.rptDepartment.ItemDataBound += new RepeaterItemEventHandler(rptDepartment_ItemDataBound);
                this.rptDepartment.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        private void rptDepartment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int departmentID = (int)e.Item.DataItem;
            DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = BackgroundDepartment.GetDepartmentRowHtml(departmentInfo, EDepartmentLoadingType.AdministratorTree, this.additional);
        }
    }
}
