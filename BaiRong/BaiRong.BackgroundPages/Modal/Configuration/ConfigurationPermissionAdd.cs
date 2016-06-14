using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;



namespace BaiRong.BackgroundPages.Modal
{
	public class ConfigurationPermissionAdd : BackgroundBasePage
	{
        public TextBox tbName;
        public TextBox tbText;

        private string appID;
        private string permissionName;

        public static string GetOpenWindowStringToAdd(string appID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("AppID", appID);
            return PageUtilityPF.GetOpenWindowString("添加权限", "modal_configurationPermissionAdd.aspx", arguments, 360, 250);
        }

        public static string GetOpenWindowStringToEdit(string appID, string permissionName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("AppID", appID);
            arguments.Add("PermissionName", permissionName);
            return PageUtilityPF.GetOpenWindowString("修改权限", "modal_configurationPermissionAdd.aspx", arguments, 360, 250);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.appID = base.GetQueryString("appID");
            this.permissionName = base.GetQueryString("permissionName");

            if (!IsPostBack)
            {
                if (this.permissionName != null)
                {
                    ArrayList permissions = PermissionConfigManager.GetGeneralPermissionsOfApp(this.appID);
                    foreach (PermissionConfig config in permissions)
                    {
                        if (config.Name == this.permissionName)
                        {
                            this.tbName.Text = config.Name;
                            this.tbName.Enabled = false;
                            this.tbText.Text = config.Text;
                            break;
                        }
                    }
                }
                else
                {
                    this.tbName.Text = this.appID.ToLower() + "_";
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.permissionName == null)
                {
                    if (!StringUtils.StartsWithIgnoreCase(this.tbName.Text, this.appID.ToLower() + "_"))
                    {
                        base.FailMessage(string.Format("权限添加失败，权限标识必须以{0}开始！", this.appID.ToLower() + "_"));
                        return;
                    }
                    PermissionConfig permission = new PermissionConfig(this.tbName.Text, this.tbText.Text);
                    ArrayList permissions = PermissionConfigManager.GetGeneralPermissionsOfApp(this.appID);
                    foreach (PermissionConfig config in permissions)
                    {
                        if (config.Name == permission.Name)
                        {
                            base.FailMessage("权限添加失败，权限标识已存在！");
                            return;
                        }
                        else if (config.Text == permission.Text)
                        {
                            base.FailMessage("权限添加失败，权限名称已存在！");
                            return;
                        }
                    }

                    permissions.Add(permission);

                    PermissionConfigManager.SaveGeneralPermissionsOfApp(this.appID, permissions);
                }
                else
                {
                    ArrayList permissions = PermissionConfigManager.GetGeneralPermissionsOfApp(this.appID);
                    foreach (PermissionConfig config in permissions)
                    {
                        if (config.Name == this.permissionName)
                        {
                            config.Text = this.tbText.Text;
                        }
                    }

                    PermissionConfigManager.SaveGeneralPermissionsOfApp(this.appID, permissions);
                }

                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
	}
}
