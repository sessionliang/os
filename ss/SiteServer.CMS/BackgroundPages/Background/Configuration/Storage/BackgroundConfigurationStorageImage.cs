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
	public class BackgroundConfigurationStorageImage : BackgroundBasePage
	{
        public RadioButtonList rblIsImageStorage;
        public PlaceHolder phImageStorage;
        public DropDownList ddlImageStorageID;
        public TextBox tbImageStoragePath;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationStorage, "图片存储空间设置", AppManager.CMS.Permission.WebSite.Configration);

                EBooleanUtils.AddListItems(this.rblIsImageStorage, "采用独立空间存储图片", "与应用存储空间一致");
                ControlUtils.SelectListItems(this.rblIsImageStorage, base.PublishmentSystemInfo.Additional.IsImageStorage.ToString());

                ArrayList storageInfoArrayList = BaiRongDataProvider.StorageDAO.GetStorageInfoArrayListEnabled();
                foreach (StorageInfo storageInfo in storageInfoArrayList)
                {
                    this.ddlImageStorageID.Items.Add(new ListItem(string.Format("{0}（{1}）", storageInfo.StorageName, PageUtils.AddProtocolToUrl(storageInfo.StorageUrl)), storageInfo.StorageID.ToString()));
                }
                ControlUtils.SelectListItems(this.ddlImageStorageID, base.PublishmentSystemInfo.Additional.ImageStorageID.ToString());

                string storagePath = base.PublishmentSystemInfo.Additional.ImageStoragePath;
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = PageUtils.Combine(base.PublishmentSystemInfo.PublishmentSystemUrl, base.PublishmentSystemInfo.Additional.ImageUploadDirectoryName);
                }
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = "/";
                }
                this.tbImageStoragePath.Text = storagePath;

                this.rblIsImageStorage_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void rblIsImageStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phImageStorage.Visible = TranslateUtils.ToBool(this.rblIsImageStorage.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.IsImageStorage = TranslateUtils.ToBool(this.rblIsImageStorage.SelectedValue);
                if (base.PublishmentSystemInfo.Additional.IsImageStorage)
                {
                    base.PublishmentSystemInfo.Additional.ImageStorageID = TranslateUtils.ToInt(this.ddlImageStorageID.SelectedValue);
                    base.PublishmentSystemInfo.Additional.ImageStoragePath = this.tbImageStoragePath.Text;
                    if (string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.ImageStoragePath))
                    {
                        base.PublishmentSystemInfo.Additional.ImageStoragePath = "/";
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

                    StringUtility.AddLog(base.PublishmentSystemID, "修改图片存储空间设置");

                    base.SuccessMessage("图片存储空间设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "图片存储空间设置修改失败！");
				}
			}
		}
	}
}
