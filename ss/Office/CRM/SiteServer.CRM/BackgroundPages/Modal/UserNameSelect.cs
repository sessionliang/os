using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using SiteServer.CRM.Model;
using SiteServer.CRM.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class UserNameSelect : BackgroundBasePage
	{
        public Repeater rptDepartment;
        public Literal ltlDepartmentName;
        public Repeater rptUser;

        private NameValueCollection additional = new NameValueCollection();
        private string scriptName;


        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString(int departmentID, string scriptName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("DepartmentID", departmentID.ToString());
            arguments.Add("ScriptName", scriptName);
            return JsUtils.OpenWindow.GetOpenWindowString("用户选择", "modal_userNameSelect.aspx", arguments, 460, 400, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.scriptName = base.Request.QueryString["ScriptName"];
            this.additional.Add("UrlFormatString", string.Format("modal_userNameSelect.aspx?ScriptName={0}", this.scriptName) + "&DepartmentID={0}");

			if (!IsPostBack)
			{
                this.ltlDepartmentName.Text = "人员列表";
                if (!string.IsNullOrEmpty(base.Request.QueryString["UserName"]))
                {
                    string userName = base.Request.QueryString["UserName"];
                    string displayName = AdminManager.GetDisplayName(userName, true);
                    string scripts = string.Format("window.parent.{0}('{1}', '{2}');", this.scriptName, displayName, userName);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else if (!string.IsNullOrEmpty(base.Request.QueryString["DepartmentID"]))
                {
                    int departmentID = TranslateUtils.ToInt(base.Request.QueryString["DepartmentID"]);
                    if (departmentID > 0)
                    {
                        this.ltlDepartmentName.Text = DepartmentManager.GetDepartmentName(departmentID);
                        this.rptUser.DataSource = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
                        this.rptUser.ItemDataBound += new RepeaterItemEventHandler(rptUser_ItemDataBound);
                        this.rptUser.DataBind();
                    }
                }
                else
                {
                    ProjectManager.RegisterClientScriptBlock(Page, "NodeTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.DepartmentSelect, this.additional));
                }
			}

            this.BindGrid();
		}

        public void BindGrid()
        {
            try
            {
                this.rptDepartment.DataSource = ProjectManager.GetFirstDepartmentIDArrayList();
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

            ltlUrl.Text = string.Format("<a href='modal_userNameSelect.aspx?ScriptName={0}&UserName={1}'>{2}</a>", this.scriptName, userName, AdminManager.GetDisplayName(userName, false));
        }
	}
}
