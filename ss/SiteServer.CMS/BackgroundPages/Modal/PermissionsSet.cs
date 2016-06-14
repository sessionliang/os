using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class PermissionsSet : BackgroundBasePage
	{
        protected DropDownList ddlPredefinedRole;

        public PlaceHolder phPublishmentSystemID;
        protected CheckBoxList cblPublishmentSystemID;

        public Control RolesRow;
        public ListBox ListAvailableRoles;
        public ListBox ListAssignedRoles;

        private string userName = string.Empty;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.userName = base.GetQueryString("UserName");

			if (!IsPostBack)
			{
                string[] roles = RoleManager.GetRolesForUser(this.userName);
                if (PermissionsManager.Current.IsConsoleAdministrator)
                {
                    this.ddlPredefinedRole.Items.Add(EPredefinedRoleUtils.GetListItem(EPredefinedRole.ConsoleAdministrator, false));
                }
                if (PermissionsManager.Current.IsSystemAdministrator)
                {
                    this.ddlPredefinedRole.Items.Add(EPredefinedRoleUtils.GetListItem(EPredefinedRole.SystemAdministrator, false));
                }
                this.ddlPredefinedRole.Items.Add(EPredefinedRoleUtils.GetListItem(EPredefinedRole.Administrator, false));

                EPredefinedRole type = EPredefinedRoleUtils.GetEnumTypeByRoles(roles);
                ControlUtils.SelectListItems(this.ddlPredefinedRole, EPredefinedRoleUtils.GetValue(type));

                PublishmentSystemManager.AddListItems(this.cblPublishmentSystemID);
                ControlUtils.SelectListItems(this.cblPublishmentSystemID, BaiRongDataProvider.AdministratorDAO.GetPublishmentSystemIDList(userName));

                this.ListBoxDataBind();

                this.ddlPredefinedRole_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void ddlPredefinedRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EPredefinedRoleUtils.Equals(EPredefinedRole.ConsoleAdministrator, this.ddlPredefinedRole.SelectedValue))
            {
                this.RolesRow.Visible = this.phPublishmentSystemID.Visible = false;
            }
            else if (EPredefinedRoleUtils.Equals(EPredefinedRole.SystemAdministrator, this.ddlPredefinedRole.SelectedValue))
            {
                this.RolesRow.Visible = false;
                this.phPublishmentSystemID.Visible = true;
            }
            else
            {
                this.RolesRow.Visible = true;
                this.phPublishmentSystemID.Visible = false;
            }
        }

        private void ListBoxDataBind()
        {
            this.ListAvailableRoles.Items.Clear();
            this.ListAssignedRoles.Items.Clear();
            string[] allRoles = null;
            if (PermissionsManager.Current.IsConsoleAdministrator)
            {
                allRoles = RoleManager.GetAllRoles();
            }
            else
            {
                allRoles = RoleManager.GetAllRolesByCreatorUserName(AdminManager.Current.UserName);
            }
            string[] userRoles = RoleManager.GetRolesForUser(userName);
            ArrayList userRoleNameArrayList = new ArrayList(userRoles);
            foreach (string roleName in allRoles)
            {
                if (!EPredefinedRoleUtils.IsPredefinedRole(roleName) && !userRoleNameArrayList.Contains(roleName))
                {
                    this.ListAvailableRoles.Items.Add(new ListItem(roleName, roleName));
                }
            }
            foreach (string roleName in userRoles)
            {
                if (!EPredefinedRoleUtils.IsPredefinedRole(roleName))
                {
                    this.ListAssignedRoles.Items.Add(new ListItem(roleName, roleName));
                }
            }
        }

        public void AddRole_OnClick(object sender, EventArgs E)
        {
            if (IsPostBack && IsValid)
            {
                try
                {
                    if (ListAvailableRoles.SelectedIndex != -1)
                    {
                        string[] selectedRoles = ControlUtils.GetSelectedListControlValueArray(ListAvailableRoles);
                        if (selectedRoles.Length > 0)
                        {
                            RoleManager.AddUserToRoles(userName, selectedRoles);
                        }
                    }
                    this.ListBoxDataBind();
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "用户角色分配失败");
                }
            }
        }

        public void AddRoles_OnClick(object sender, EventArgs E)
        {
            if (IsPostBack && IsValid)
            {
                try
                {
                    string[] roles = ControlUtils.GetListControlValues(ListAvailableRoles);
                    if (roles.Length > 0)
                    {
                        RoleManager.AddUserToRoles(userName, roles);
                    }
                    this.ListBoxDataBind();
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "用户角色分配失败");
                }
            }
        }

        public void DeleteRole_OnClick(object sender, EventArgs E)
        {
            if (IsPostBack && IsValid)
            {
                try
                {
                    if (ListAssignedRoles.SelectedIndex != -1)
                    {
                        string[] selectedRoles = ControlUtils.GetSelectedListControlValueArray(ListAssignedRoles);
                        RoleManager.RemoveUserFromRoles(userName, selectedRoles);
                    }
                    this.ListBoxDataBind();
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "用户角色分配失败");
                }
            }
        }

        public void DeleteRoles_OnClick(object sender, EventArgs E)
        {
            if (IsPostBack && IsValid)
            {
                if (IsPostBack && IsValid)
                {
                    try
                    {
                        string[] roles = ControlUtils.GetListControlValues(ListAssignedRoles);
                        if (roles.Length > 0)
                        {
                            RoleManager.RemoveUserFromRoles(userName, roles);
                        }
                        this.ListBoxDataBind();
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "用户角色分配失败");
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                ArrayList allRoles = EPredefinedRoleUtils.GetAllPredefinedRole();
                foreach (EPredefinedRole roleName in allRoles)
                {
                    try
                    {
                        RoleManager.RemoveUserFromRole(userName, roleName.ToString());
                    }
                    catch { }
                }
                RoleManager.AddUserToRole(userName, this.ddlPredefinedRole.SelectedValue);

                if (EPredefinedRoleUtils.Equals(EPredefinedRole.SystemAdministrator, this.ddlPredefinedRole.SelectedValue))
                {
                    BaiRongDataProvider.AdministratorDAO.UpdatePublishmentSystemIDCollection(userName, ControlUtils.SelectedItemsValueToStringCollection(this.cblPublishmentSystemID.Items));
                }
                else
                {
                    BaiRongDataProvider.AdministratorDAO.UpdatePublishmentSystemIDCollection(userName, string.Empty);
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置管理员权限", string.Format("管理员:{0}", userName));

                base.SuccessMessage("权限设置成功！");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "权限设置失败！");
            }

            if (isChanged)
            {
                string redirectUrl = BaiRong.BackgroundPages.BackgroundAdministrator.GetRedirectUrl(0);
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, redirectUrl);
            }
        }
	}
}
