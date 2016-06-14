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
	public class BackgroundConfigurationStorageFile : BackgroundBasePage
	{
        public RadioButtonList rblIsFileStorage;
        public PlaceHolder phFileStorage;
        public DropDownList ddlFileStorageID;
        public TextBox tbFileStoragePath;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationStorage, "附件存储空间设置", AppManager.CMS.Permission.WebSite.Configration);

                EBooleanUtils.AddListItems(this.rblIsFileStorage, "采用独立空间存储附件", "与应用存储空间一致");
                ControlUtils.SelectListItems(this.rblIsFileStorage, base.PublishmentSystemInfo.Additional.IsFileStorage.ToString());

                ArrayList storageInfoArrayList = BaiRongDataProvider.StorageDAO.GetStorageInfoArrayListEnabled();
                foreach (StorageInfo storageInfo in storageInfoArrayList)
                {
                    this.ddlFileStorageID.Items.Add(new ListItem(string.Format("{0}（{1}）", storageInfo.StorageName, PageUtils.AddProtocolToUrl(storageInfo.StorageUrl)), storageInfo.StorageID.ToString()));
                }
                ControlUtils.SelectListItems(this.ddlFileStorageID, base.PublishmentSystemInfo.Additional.FileStorageID.ToString());

                string storagePath = base.PublishmentSystemInfo.Additional.FileStoragePath;
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = PageUtils.Combine(base.PublishmentSystemInfo.PublishmentSystemUrl, base.PublishmentSystemInfo.Additional.FileUploadDirectoryName);
                }
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = "/";
                }
                this.tbFileStoragePath.Text = storagePath;

                this.rblIsFileStorage_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void rblIsFileStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phFileStorage.Visible = TranslateUtils.ToBool(this.rblIsFileStorage.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.IsFileStorage = TranslateUtils.ToBool(this.rblIsFileStorage.SelectedValue);
                if (base.PublishmentSystemInfo.Additional.IsFileStorage)
                {
                    base.PublishmentSystemInfo.Additional.FileStorageID = TranslateUtils.ToInt(this.ddlFileStorageID.SelectedValue);
                    base.PublishmentSystemInfo.Additional.FileStoragePath = this.tbFileStoragePath.Text;
                    if (string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.FileStoragePath))
                    {
                        base.PublishmentSystemInfo.Additional.FileStoragePath = "/";
                    }

                    PublishmentSystemManager.AddSystemPublishTaskIfNotExists(base.PublishmentSystemID);
                }
                else
                {
                    PublishmentSystemManager.RemoveSystemPublishTaskIfNecessary(base.PublishmentSystemInfo);
                }
                
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改附件存储空间设置");

                    base.SuccessMessage("附件存储空间设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "附件存储空间设置修改失败！");
				}
			}
		}
	}
}
