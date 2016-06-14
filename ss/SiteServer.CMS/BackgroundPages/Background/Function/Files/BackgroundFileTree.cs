using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundFileTree : BackgroundBasePage
	{
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!ProductPermissionsManager.Current.PublishmentSystemIDList.Contains(base.PublishmentSystemID))
            {
                PageUtils.RedirectToErrorPage("权限不足");
            }

            if (!IsPostBack)
            {
                AdminUtility.VerifyWebsitePermissions(this.PublishmentSystemID, AppManager.CMS.Permission.WebSite.FileManagement);
            }
		}
	}
}
