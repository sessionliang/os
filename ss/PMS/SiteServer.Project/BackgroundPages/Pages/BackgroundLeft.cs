using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

using SiteServer.Project.Controls;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundLeft : BackgroundBasePage
	{
        public Literal ltlTitle;
        public Literal ltlScript;
        public SiteServer.Project.Controls.NavigationTree navigationTree;

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                string menuID = base.Request.QueryString["menuID"];
                if (StringUtils.EqualsIgnoreCase(menuID, "Projects"))
                {
                    this.ltlTitle.Text = "项目管理";
                }
                else if (StringUtils.EqualsIgnoreCase(menuID, "Documents"))
                {
                    this.ltlTitle.Text = "文档管理";
                }
                navigationTree.FileName = string.Format("~/SiteFiles/Module/{0}", PathUtils.Combine("Project", DirectoryUtils.Module.Menu.DirectoryName, menuID + ".config"));
                navigationTree.PermissionArrayList = PermissionsManager.Current.PermissionArrayList;
                ltlScript.Text = NavigationTreeItem.GetNavigationBarScript();
            }
        }
	}
}
