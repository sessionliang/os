using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.Configuration;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovInteractPermissions : BackgroundBasePage
	{
        protected CheckBoxList cblPermissions;

        private int nodeID;
        private string userName;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string userName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("UserName", userName);
            return PageUtilityWCM.GetOpenWindowString("权限设置", "modal_govInteractPermissions.aspx", arguments, 450, 320);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);
            this.userName = base.Request.QueryString["UserName"];

			if (!IsPostBack)
			{
                ArrayList permissionArrayList = new ArrayList();
                GovInteractPermissionsInfo permissionsInfo = DataProvider.GovInteractPermissionsDAO.GetPermissionsInfo(this.userName, this.nodeID);
                if (permissionsInfo != null)
                {
                    permissionArrayList = TranslateUtils.StringCollectionToArrayList(permissionsInfo.Permissions);
                }

                foreach (PermissionConfig permission in PermissionConfigManager.Instance.GovInteractPermissions)
                {
                    ListItem listItem = new ListItem(permission.Text, permission.Name);
                    if (permissionArrayList.Contains(permission.Name))
                    {
                        listItem.Selected = true;
                    }
                    this.cblPermissions.Items.Add(listItem);
                }

				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            GovInteractPermissionsInfo permissionsInfo = DataProvider.GovInteractPermissionsDAO.GetPermissionsInfo(this.userName, this.nodeID);

            try
            {
                if (permissionsInfo == null)
                {
                    permissionsInfo = new GovInteractPermissionsInfo(this.userName, this.nodeID, ControlUtils.GetSelectedListControlValueCollection(this.cblPermissions));
                    DataProvider.GovInteractPermissionsDAO.Insert(base.PublishmentSystemID, permissionsInfo);
                }
                else
                {
                    permissionsInfo.Permissions = ControlUtils.GetSelectedListControlValueCollection(this.cblPermissions);
                    DataProvider.GovInteractPermissionsDAO.Update(permissionsInfo);
                }

                StringUtility.AddLog(base.PublishmentSystemID, "设置互动交流管理员权限", string.Format("互动交流类别:{0}", NodeManager.GetNodeName(base.PublishmentSystemID, this.nodeID)));

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "互动交流权限设置失败！");
            }

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetWCMUrl(string.Format("background_govInteractPermissions.aspx?PublishmentSystemID={0}&NodeID={1}", base.PublishmentSystemID, this.nodeID)));
			}
		}
	}
}
