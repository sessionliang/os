using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;



namespace BaiRong.BackgroundPages.Modal
{
	public class AdminSelect : BackgroundBasePage
	{
        public Repeater rptDepartment;
        public Literal ltlDepartment;
        public Repeater rptUser;

        private NameValueCollection additional = new NameValueCollection();
        private int departmentID;
        private string scriptName;

        public static string GetShowPopWinString(int departmentID, string scriptName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("departmentID", departmentID.ToString());
            arguments.Add("scriptName", scriptName);
            return PageUtilityPF.GetOpenWindowString("管理员选择", "modal_adminSelect.aspx", arguments, 460, 400, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.departmentID = base.GetIntQueryString("departmentID");
            this.scriptName = base.GetQueryString("ScriptName");
            this.additional.Add("UrlFormatString", PageUtils.GetPlatformUrl(string.Format("modal_adminSelect.aspx?scriptName={0}", this.scriptName) + "&departmentID={0}"));

			if (!IsPostBack)
			{
                this.ltlDepartment.Text = "管理员列表";
                if (!string.IsNullOrEmpty(base.GetQueryString("UserName")))
                {
                    string userName = base.GetQueryString("UserName");
                    string displayName = AdminManager.GetDisplayName(userName, true);
                    string scripts = string.Format("window.parent.{0}('{1}', '{2}');", this.scriptName, displayName, userName);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else if (!string.IsNullOrEmpty(base.GetQueryString("departmentID")))
                {
                    if (this.departmentID > 0)
                    {
                        this.ltlDepartment.Text = DepartmentManager.GetDepartmentName(this.departmentID);
                        this.rptUser.DataSource = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(this.departmentID, false);
                        this.rptUser.ItemDataBound += new RepeaterItemEventHandler(rptUser_ItemDataBound);
                        this.rptUser.DataBind();
                    }
                }
                else
                {
                    Page.RegisterClientScriptBlock("NodeTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.DepartmentSelect, this.additional));
                }
			}

            this.BindGrid();
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

            ltlHtml.Text = BaiRong.BackgroundPages.BackgroundDepartment.GetDepartmentRowHtml(departmentInfo, EDepartmentLoadingType.DepartmentSelect, this.additional);
        }

        private void rptUser_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string userName = (string)e.Item.DataItem;

            Literal ltlUrl = e.Item.FindControl("ltlUrl") as Literal;

            string urlSelect = PageUtils.GetPlatformUrl(string.Format("modal_adminSelect.aspx?scriptName={0}&UserName={1}", this.scriptName, userName));
            ltlUrl.Text = string.Format("<a href='{0}'>{1}</a>", urlSelect, AdminManager.GetDisplayName(userName, false));
        }
	}
}
