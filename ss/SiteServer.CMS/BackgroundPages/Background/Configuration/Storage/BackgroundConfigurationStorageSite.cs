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
    public class BackgroundConfigurationStorageSite : BackgroundBasePage
    {
        public RadioButtonList rblIsSiteStorage;
        public PlaceHolder phSiteStorage;
        public DropDownList ddlSiteStorageID;
        public TextBox tbSiteStoragePath;

        public RadioButtonList rblIsSonSiteAlone;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, AppManager.CMS.LeftMenu.Configuration.ID_ConfigurationStorage, "Web服务器部署", AppManager.CMS.Permission.WebSite.Configration);

                EBooleanUtils.AddListItems(this.rblIsSiteStorage, "分离部署", "统一部署");
                ControlUtils.SelectListItems(this.rblIsSiteStorage, base.PublishmentSystemInfo.Additional.IsSiteStorage.ToString());

                EBooleanUtils.AddListItems(this.rblIsSonSiteAlone, "子站单独部署", "主子站统一部署");
                ControlUtils.SelectListItems(this.rblIsSonSiteAlone, base.PublishmentSystemInfo.Additional.IsSonSiteAlone.ToString());

                ArrayList storageInfoArrayList = BaiRongDataProvider.StorageDAO.GetStorageInfoArrayListEnabled();
                foreach (StorageInfo storageInfo in storageInfoArrayList)
                {
                    this.ddlSiteStorageID.Items.Add(new ListItem(string.Format("{0}（{1}）", storageInfo.StorageName, PageUtils.AddProtocolToUrl(storageInfo.StorageUrl)), storageInfo.StorageID.ToString()));
                }
                ControlUtils.SelectListItems(this.ddlSiteStorageID, base.PublishmentSystemInfo.Additional.SiteStorageID.ToString());

                string storagePath = base.PublishmentSystemInfo.Additional.SiteStoragePath;
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = base.PublishmentSystemInfo.PublishmentSystemUrl;
                }
                if (string.IsNullOrEmpty(storagePath))
                {
                    storagePath = "/";
                }
                this.tbSiteStoragePath.Text = storagePath;

                this.rblIsSiteStorage_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void rblIsSiteStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phSiteStorage.Visible = TranslateUtils.ToBool(this.rblIsSiteStorage.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.PublishmentSystemInfo.Additional.IsSiteStorage = TranslateUtils.ToBool(this.rblIsSiteStorage.SelectedValue);
                if (base.PublishmentSystemInfo.Additional.IsSiteStorage)
                {
                    if (string.IsNullOrEmpty(this.ddlSiteStorageID.SelectedValue))
                    {
                        base.FailMessage("Web服务器设置修改失败！请先添加存储空间");
                        return;
                    }
                    base.PublishmentSystemInfo.Additional.SiteStorageID = TranslateUtils.ToInt(this.ddlSiteStorageID.SelectedValue);
                    base.PublishmentSystemInfo.Additional.SiteStoragePath = this.tbSiteStoragePath.Text;
                    if (string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.SiteStoragePath))
                    {
                        base.PublishmentSystemInfo.Additional.SiteStoragePath = "/";
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

                    StringUtility.AddLog(base.PublishmentSystemID, "修改Web服务器设置");

                    base.SuccessMessage("Web服务器设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "Web服务器设置修改失败！");
                }
            }
        }
    }
}
