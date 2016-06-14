using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundProjectLeft : BackgroundBasePage
    {
        public DataGrid dgContents;

        private string type = string.Empty;

        public void Page_Load(object sender, EventArgs E)
        {
            this.type = base.Request.QueryString["Type"];

            if (!IsPostBack)
            {
                if (StringUtils.EqualsIgnoreCase(this.type, "Payment"))
                {
                    this.dgContents.DataSource = DataProvider.ProjectDAO.GetDataSource(true);
                }
                else
                {
                    this.dgContents.DataSource = DataProvider.ProjectDAO.GetDataSource(false);
                }
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
                this.dgContents.DataBind();
            }
        }

        void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int projectID = TranslateUtils.EvalInt(e.Item.DataItem, "ProjectID");
                string projectName = TranslateUtils.EvalString(e.Item.DataItem, "ProjectName");

                Literal ltlProjectName = e.Item.FindControl("ltlProjectName") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlProjectName.Text = projectName;

                if (StringUtils.EqualsIgnoreCase(this.type, "ApplyType"))
                {
                    ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">办件类型管理</a>", string.Format("background_type.aspx?ProjectID={0}", projectID));
                }
                else if (StringUtils.EqualsIgnoreCase(this.type, "ApplyConfiguration"))
                {
                    ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">项目参数设置</a>", string.Format("background_applyConfiguration.aspx?ProjectID={0}", projectID));
                }
                else if (StringUtils.EqualsIgnoreCase(this.type, "Payment"))
                {
                    ltlEditUrl.Text = string.Format(@"<a target='management' href=""{0}"">项目回款管理</a>", BackgroundPayment.GetRedirectUrl(projectID, string.Empty));
                }
            }
        }
    }
}
