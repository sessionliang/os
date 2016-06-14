using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;

using BaiRong.Core.Data.Provider;
using BaiRong.Model.Service;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationStorageBackup : BackgroundBasePage
	{
        public RadioButtonList rblIsBackupStorage;
        public PlaceHolder phBackupStorage;
        public DropDownList ddlBackupStorageID;
        public TextBox tbBackupStoragePath;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationStorage, "备份存储空间设置", AppManager.CMS.Permission.WebSite.Configration);

                EBooleanUtils.AddListItems(this.rblIsBackupStorage, "采用独立空间存储备份文件", "存储在当前空间");
                ControlUtils.SelectListItems(this.rblIsBackupStorage, base.PublishmentSystemInfo.Additional.IsBackupStorage.ToString());

                ArrayList storageInfoArrayList = BaiRongDataProvider.StorageDAO.GetStorageInfoArrayListEnabled();
                foreach (StorageInfo storageInfo in storageInfoArrayList)
                {
                    this.ddlBackupStorageID.Items.Add(new ListItem(string.Format("{0}（{1}）", storageInfo.StorageName, PageUtils.AddProtocolToUrl(storageInfo.StorageUrl)), storageInfo.StorageID.ToString()));
                }
                ControlUtils.SelectListItems(this.ddlBackupStorageID, base.PublishmentSystemInfo.Additional.BackupStorageID.ToString());

                string storagePath = base.PublishmentSystemInfo.Additional.BackupStoragePath;
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = PageUtils.Combine("BackupFiles", base.PublishmentSystemInfo.PublishmentSystemUrl);
                }
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = "/";
                }
                this.tbBackupStoragePath.Text = storagePath;

                this.rblIsBackupStorage_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void rblIsBackupStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phBackupStorage.Visible = TranslateUtils.ToBool(this.rblIsBackupStorage.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.IsBackupStorage = TranslateUtils.ToBool(this.rblIsBackupStorage.SelectedValue);
                if (base.PublishmentSystemInfo.Additional.IsBackupStorage)
                {
                    base.PublishmentSystemInfo.Additional.BackupStorageID = TranslateUtils.ToInt(this.ddlBackupStorageID.SelectedValue);
                    base.PublishmentSystemInfo.Additional.BackupStoragePath = this.tbBackupStoragePath.Text;
                    if (string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.BackupStoragePath))
                    {
                        base.PublishmentSystemInfo.Additional.BackupStoragePath = "/";
                    }
                }
                
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改备份文件存储空间设置");

                    base.SuccessMessage("备份文件存储空间设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "备份文件存储空间设置修改失败！");
				}
			}
		}
	}
}
