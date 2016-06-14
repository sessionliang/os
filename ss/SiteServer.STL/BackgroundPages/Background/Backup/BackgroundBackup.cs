using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Model;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundBackup : BackgroundBasePage
	{
		public DropDownList BackupType;
		public Button BackupButton;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!string.IsNullOrEmpty(base.GetQueryString("isRedirectToFiles")))
            {
                string redirectUrl = PageUtils.GetCMSUrl(string.Format("background_fileMain.aspx?RootPath=~/SiteFiles/BackupFiles/{0}", base.PublishmentSystemInfo.PublishmentSystemDir));
                PageUtils.Redirect(redirectUrl);
                return;
            }

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Backup, "数据备份", AppManager.CMS.Permission.WebSite.Backup);

                EBackupTypeUtils.AddListItems(this.BackupType);
			}
		}


		public void BackupButton_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                string url = BackgroundProgressBar.GetBackupUrl(base.PublishmentSystemID, this.BackupType.SelectedValue, StringUtils.GUID());
                PageUtils.Redirect(url);
			}
		}
	}
}
