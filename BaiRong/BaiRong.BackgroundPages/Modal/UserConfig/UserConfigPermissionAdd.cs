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
	public class UserConfigPermissionAdd : BackgroundBasePage
	{
        public TextBox tbName;
        public TextBox tbText;

        private string productID;
        private string permissionName;

        public static string GetOpenWindowStringToAdd(string productID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProductID", productID);
            return PageUtilityPF.GetOpenWindowString("���Ȩ��", "modal_userConfigPermissionAdd.aspx", arguments, 360, 250);
        }

        public static string GetOpenWindowStringToEdit(string productID, string permissionName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ProductID", productID);
            arguments.Add("PermissionName", permissionName);
            return PageUtilityPF.GetOpenWindowString("�޸�Ȩ��", "modal_userConfigPermissionAdd.aspx", arguments, 360, 250);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.productID = base.GetQueryString("ProductID");
            this.permissionName = base.GetQueryString("permissionName");

            if (!IsPostBack)
            {
                if (this.permissionName != null)
                {
                    ArrayList permissions = UserCenterPermissionConfigManager.GetPermissionsFromFile(this.productID, string.Empty);
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
                    this.tbName.Text = this.productID.ToLower() + "_";
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.permissionName == null)
                {
                    if (!StringUtils.StartsWithIgnoreCase(this.tbName.Text, this.productID.ToLower() + "_"))
                    {
                        base.FailMessage(string.Format("Ȩ�����ʧ�ܣ�Ȩ�ޱ�ʶ������{0}��ʼ��", this.productID.ToLower() + "_"));
                        return;
                    }
                    PermissionConfig permission = new PermissionConfig(this.tbName.Text, this.tbText.Text);
                    ArrayList permissions = UserCenterPermissionConfigManager.GetPermissionsFromFile(this.productID, string.Empty);
                    foreach (PermissionConfig config in permissions)
                    {
                        if (config.Name == permission.Name)
                        {
                            base.FailMessage("Ȩ�����ʧ�ܣ�Ȩ�ޱ�ʶ�Ѵ��ڣ�");
                            return;
                        }
                        else if (config.Text == permission.Text)
                        {
                            base.FailMessage("Ȩ�����ʧ�ܣ�Ȩ�������Ѵ��ڣ�");
                            return;
                        }
                    }

                    permissions.Add(permission);

                    UserCenterPermissionConfigManager.SavePermissionsToFile(this.productID, string.Empty, permissions);
                }
                else
                {
                    ArrayList permissions = UserCenterPermissionConfigManager.GetPermissionsFromFile(this.productID, string.Empty);
                    foreach (PermissionConfig config in permissions)
                    {
                        if (config.Name == this.permissionName)
                        {
                            config.Text = this.tbText.Text;
                        }
                    }

                    UserCenterPermissionConfigManager.SavePermissionsToFile(this.productID, string.Empty, permissions);
                }

                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
	}
}
