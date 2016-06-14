using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;


using BaiRong.Model;

namespace BaiRong.BackgroundPages
{
    public class BackgroundConfigurationPermission : BackgroundBasePage
	{
        public Repeater rptModules;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null && base.GetQueryString("AppID") != null && base.GetQueryString("PermissionName") != null)
            {
                string appID = base.GetQueryString("AppID");
                string permissionName = base.GetQueryString("PermissionName");
                try
                {
                    ArrayList permissions = new ArrayList();
                    ArrayList permissionsAll = PermissionConfigManager.GetGeneralPermissionsOfApp(appID);
                    foreach (PermissionConfig config in permissionsAll)
                    {
                        if (config.Name != permissionName)
                        {
                            permissions.Add(config);
                        }
                    }

                    PermissionConfigManager.SaveGeneralPermissionsOfApp(appID, permissions);

                    base.SuccessMessage("�ɹ�ɾ��Ȩ��");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("ɾ��Ȩ��ʧ�ܣ�{0}", ex.Message));
                }
            }

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Configuration, AppManager.Platform.LeftMenu.Configuration.ID_ConfigurationMenu, "Ȩ������", AppManager.Platform.Permission.Platform_Configuration);

                this.rptModules.DataSource = AppManager.GetAppIDList(true);
                this.rptModules.ItemDataBound += new RepeaterItemEventHandler(rptModules_ItemDataBound);
                this.rptModules.DataBind();
			}
		}

        private string appID;
        private void rptModules_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                this.appID = (string)e.Item.DataItem;
                Literal ltlModuleName = e.Item.FindControl("ltlModuleName") as Literal;
                Literal ltlTips = e.Item.FindControl("ltlTips") as Literal;
                Repeater rptPermissions = e.Item.FindControl("rptPermissions") as Repeater;
                Button btnSubmit = e.Item.FindControl("btnSubmit") as Button;

                ArrayList permissions = PermissionConfigManager.GetGeneralPermissionsOfApp(this.appID);
                if (permissions.Count > 0)
                {
                    ModuleFileInfo fileInfo = ProductFileUtils.GetModuleFileInfoOfApp(this.appID);
                    ltlModuleName.Text = fileInfo.ModuleName;

                    ltlTips.Text = string.Format("{0}Ȩ�޴洢��/SiteFiles/Products/Apps/{1}/Menu/Permissions.config�ļ��У������ֹ��޸Ĵ��ļ�", fileInfo.FullName, this.appID);

                    rptPermissions.DataSource = permissions;
                    rptPermissions.ItemDataBound += new RepeaterItemEventHandler(rptPermissions_ItemDataBound);
                    rptPermissions.DataBind();

                    btnSubmit.Attributes.Add("onclick", Modal.ConfigurationPermissionAdd.GetOpenWindowStringToAdd(this.appID));
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }

        void rptPermissions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PermissionConfig config = e.Item.DataItem as PermissionConfig;

                Literal ltlName = e.Item.FindControl("ltlName") as Literal;
                Literal ltlText = e.Item.FindControl("ltlText") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlName.Text = config.Name;
                ltlText.Text = config.Text;

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">�༭</a>", Modal.ConfigurationPermissionAdd.GetOpenWindowStringToEdit(this.appID, config.Name));

                string urlDelete = PageUtils.GetPlatformUrl(string.Format("background_configurationPermission.aspx?Delete=True&AppID={0}&PermissionName={1}", this.appID, config.Name));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('�˲�����ɾ��Ȩ�ޡ�{1}����ȷ����');"">ɾ��</a>", urlDelete, config.Text);
            }
        }
	}
}
