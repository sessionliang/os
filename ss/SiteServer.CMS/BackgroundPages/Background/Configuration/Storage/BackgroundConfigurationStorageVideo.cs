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
	public class BackgroundConfigurationStorageVideo : BackgroundBasePage
	{
        public RadioButtonList rblIsVideoStorage;
        public PlaceHolder phVideoStorage;
        public DropDownList ddlVideoStorageID;
        public TextBox tbVideoStoragePath;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationStorage, "视频存储空间设置", AppManager.CMS.Permission.WebSite.Configration);

                EBooleanUtils.AddListItems(this.rblIsVideoStorage, "采用独立空间存储视频", "与应用存储空间一致");
                ControlUtils.SelectListItems(this.rblIsVideoStorage, base.PublishmentSystemInfo.Additional.IsVideoStorage.ToString());

                ArrayList storageInfoArrayList = BaiRongDataProvider.StorageDAO.GetStorageInfoArrayListEnabled();
                foreach (StorageInfo storageInfo in storageInfoArrayList)
                {
                    this.ddlVideoStorageID.Items.Add(new ListItem(string.Format("{0}（{1}）", storageInfo.StorageName, PageUtils.AddProtocolToUrl(storageInfo.StorageUrl)), storageInfo.StorageID.ToString()));
                }
                ControlUtils.SelectListItems(this.ddlVideoStorageID, base.PublishmentSystemInfo.Additional.VideoStorageID.ToString());

                string storagePath = base.PublishmentSystemInfo.Additional.VideoStoragePath;
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = PageUtils.Combine(base.PublishmentSystemInfo.PublishmentSystemUrl, base.PublishmentSystemInfo.Additional.VideoUploadDirectoryName);
                }
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = "/";
                }
                this.tbVideoStoragePath.Text = storagePath;

                this.rblIsVideoStorage_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void rblIsVideoStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phVideoStorage.Visible = TranslateUtils.ToBool(this.rblIsVideoStorage.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.IsVideoStorage = TranslateUtils.ToBool(this.rblIsVideoStorage.SelectedValue);
                if (base.PublishmentSystemInfo.Additional.IsVideoStorage)
                {
                    base.PublishmentSystemInfo.Additional.VideoStorageID = TranslateUtils.ToInt(this.ddlVideoStorageID.SelectedValue);
                    base.PublishmentSystemInfo.Additional.VideoStoragePath = this.tbVideoStoragePath.Text;
                    if (string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.VideoStoragePath))
                    {
                        base.PublishmentSystemInfo.Additional.VideoStoragePath = "/";
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

                    StringUtility.AddLog(base.PublishmentSystemID, "修改视频存储空间设置");

                    base.SuccessMessage("视频存储空间设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "视频存储空间设置修改失败！");
				}
			}
		}
	}
}
