using System;
using System.Data;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundRole : BackgroundBasePage
	{
		public DataGrid dgContents;
		public LinkButton pageFirst;
		public LinkButton pageLast;
		public LinkButton pageNext;
		public LinkButton pagePrevious;
		public Label currentPage;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
				string roleName = base.GetQueryString("RoleName");
				try
				{
                    BaiRongDataProvider.PermissionsInRolesDAO.Delete(roleName);
                    BaiRongDataProvider.RoleDAO.DeleteRole(roleName);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除管理员角色", string.Format("角色名称:{0}", roleName));

					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
					base.FailDeleteMessage(ex);
				}
			}

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Administrator, "角色管理", AppManager.Platform.Permission.Platform_Administrator);

				BindGrid();
			}
		}

        public void dgContents_Page(object sender, DataGridPageChangedEventArgs e)
		{
            this.dgContents.CurrentPageIndex = e.NewPageIndex;
			BindGrid();
		}

		public DataSet GetDataSetByRoles(string[] roles)
		{
			DataSet dataset = new DataSet();

			DataTable dataTable = new DataTable("AllRoles");

			DataColumn column = new  DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "RoleName";
			dataTable.Columns.Add(column);

			column = new  DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "Description";
			dataTable.Columns.Add(column);

			foreach (string roleName in roles)
			{
                DataRow dataRow = dataTable.NewRow();

                dataRow["RoleName"] = roleName;
                dataRow["Description"] = RoleManager.GetRoleDescription(roleName);

                dataTable.Rows.Add(dataRow);
			}

			dataset.Tables.Add(dataTable);
			return dataset;
		}

		public void BindGrid()
		{
			try
			{
                this.dgContents.PageSize = StringUtils.Constants.PageSize;
                if (PermissionsManager.Current.IsConsoleAdministrator)
				{
                    this.dgContents.DataSource = GetDataSetByRoles(RoleManager.GetAllRoles());
				}
				else
				{
                    this.dgContents.DataSource = GetDataSetByRoles(RoleManager.GetAllRolesByCreatorUserName(AdminManager.Current.UserName));
				}
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                if (this.dgContents.CurrentPageIndex > 0)
				{
					pageFirst.Enabled = true;
					pagePrevious.Enabled = true;
				}
				else
				{
					pageFirst.Enabled = false;
					pagePrevious.Enabled = false;
				}

                if (this.dgContents.CurrentPageIndex + 1 == this.dgContents.PageCount)
				{
					pageLast.Enabled = false;
					pageNext.Enabled = false;
				}
				else
				{
					pageLast.Enabled = true;
					pageNext.Enabled = true;
				}

                currentPage.Text = string.Format("{0}/{1}", this.dgContents.CurrentPageIndex + 1, this.dgContents.PageCount);
			}
			catch(Exception ex)
			{
                PageUtils.RedirectToErrorPage(ex.Message);
			}
				
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string roleName = (string)this.dgContents.DataKeys[e.Item.ItemIndex];
                e.Item.Visible = !EPredefinedRoleUtils.IsPredefinedRole(roleName);
            }
        }

		protected void NavigationButtonClick(object sender, System.EventArgs e)
		{
			LinkButton button = (LinkButton)sender;
			string direction = button.CommandName;

			switch (direction.ToUpper())
			{
				case "FIRST" :
                    this.dgContents.CurrentPageIndex = 0;
					break;
				case "PREVIOUS" :
                    this.dgContents.CurrentPageIndex =
                        Math.Max(this.dgContents.CurrentPageIndex - 1, 0);
					break;
				case "NEXT" :
                    this.dgContents.CurrentPageIndex =
                        Math.Min(this.dgContents.CurrentPageIndex + 1,
                        this.dgContents.PageCount - 1);
					break;
				case "LAST" :
                    this.dgContents.CurrentPageIndex = this.dgContents.PageCount - 1;
					break;
				default :
					break;
			}
			BindGrid();
		}


	}
}
