using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;
using SiteServer.CMS.Model;

using System.Collections;
using BaiRong.Model;


namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovInteractPermissions : BackgroundGovInteractBasePage
    {
        public DataGrid dgContents;

        private int nodeID;

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetWCMUrl(string.Format("background_govInteractPermissions.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, AppManager.CMS.LeftMenu.GovInteract.ID_GovInteractConfiguration, "负责人员设置", AppManager.CMS.Permission.WebSite.GovInteractConfiguration);

                GovInteractChannelInfo channelInfo = DataProvider.GovInteractChannelDAO.GetChannelInfo(base.PublishmentSystemID, this.nodeID);
                ArrayList departmentIDArrayList = GovInteractManager.GetFirstDepartmentIDArrayList(channelInfo);
                ArrayList userNameArrayList = new ArrayList();
                foreach (int departmentID in departmentIDArrayList)
                {
                    userNameArrayList.AddRange(BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, true));
                }

                this.dgContents.DataSource = userNameArrayList;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string userName = e.Item.DataItem as string;
                AdministratorInfo administratorInfo = BaiRongDataProvider.AdministratorDAO.GetAdministratorInfo(userName);
                GovInteractPermissionsInfo permissionsInfo = DataProvider.GovInteractPermissionsDAO.GetPermissionsInfo(userName, this.nodeID);

                Literal ltlDepartmentName = e.Item.FindControl("ltlDepartmentName") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlDisplayName = e.Item.FindControl("ltlDisplayName") as Literal;
                Literal ltlPermissions = e.Item.FindControl("ltlPermissions") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlDepartmentName.Text = DepartmentManager.GetDepartmentName(administratorInfo.DepartmentID);
                ltlUserName.Text = userName;
                ltlDisplayName.Text = administratorInfo.DisplayName;

                if (permissionsInfo != null)
                {
                    ArrayList permissionNameArrayList = new ArrayList();
                    ArrayList permissionArrayList = TranslateUtils.StringCollectionToArrayList(permissionsInfo.Permissions);
                    foreach (string permission in permissionArrayList)
                    {
                        permissionNameArrayList.Add(AppManager.CMS.Permission.GovInteract.GetPermissionName(permission));
                    }
                    ltlPermissions.Text = TranslateUtils.ObjectCollectionToString(permissionNameArrayList);
                }

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">设置权限</a>", Modal.GovInteractPermissions.GetOpenWindowString(base.PublishmentSystemID, this.nodeID, userName));
            }
        }
    }
}
