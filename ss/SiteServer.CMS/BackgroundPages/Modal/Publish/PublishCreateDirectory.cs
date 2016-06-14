using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Service;
using System;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class PublishCreateDirectory : BackgroundBasePage
	{
		protected TextBox DirectoryName;
		protected RegularExpressionValidator DirectoryNameValidator;

		private EStorageClassify classify;

        public static string GetOpenWindowString(int publishmentSystemID, EStorageClassify classify)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("classify", EStorageClassifyUtils.GetValue(classify));
            return PageUtility.GetOpenWindowString("创建文件夹", "modal_publishCreateDirectory.aspx", arguments, 400, 250);
        }
	
		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "classify");

            this.classify = EStorageClassifyUtils.GetEnumType(base.GetQueryString("classify"));
		}

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
			bool isCreated = false;

            if (!DirectoryUtils.IsDirectoryNameCompliant(this.DirectoryName.Text))
            {
                DirectoryNameValidator.IsValid = false;
                DirectoryNameValidator.ErrorMessage = "文件夹名称不符合要求";
                return;
            }

            try
            {
                int storageID = 0;
                string storagePath = string.Empty;
                if (this.classify == EStorageClassify.Site)
                {
                    storageID = base.PublishmentSystemInfo.Additional.SiteStorageID;
                    storagePath = base.PublishmentSystemInfo.Additional.SiteStoragePath;
                }
                else if (this.classify == EStorageClassify.Image)
                {
                    storageID = base.PublishmentSystemInfo.Additional.ImageStorageID;
                    storagePath = base.PublishmentSystemInfo.Additional.ImageStoragePath;
                }
                else if (this.classify == EStorageClassify.Video)
                {
                    storageID = base.PublishmentSystemInfo.Additional.VideoStorageID;
                    storagePath = base.PublishmentSystemInfo.Additional.VideoStoragePath;
                }
                else if (this.classify == EStorageClassify.File)
                {
                    storageID = base.PublishmentSystemInfo.Additional.FileStorageID;
                    storagePath = base.PublishmentSystemInfo.Additional.FileStoragePath;
                }

                if (storageID > 0)
                {
                    StorageInfo storageInfo = BaiRongDataProvider.StorageDAO.GetStorageInfo(storageID);
                    if (storageInfo != null)
                    {
                        string path = PathUtils.Combine(storagePath, StorageManager.GetCurrentPath(AdminManager.Current.UserName, true));
                        StorageManager storageManager = new StorageManager(storageInfo, path);
                        if (storageManager.IsEnabled)
                        {
                            storageManager.Manager.CreateDirectory(this.DirectoryName.Text);
                            isCreated = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                DirectoryNameValidator.IsValid = false;
                DirectoryNameValidator.ErrorMessage = ex.Message;
            }

			if (isCreated)
			{
                StringUtility.AddLog(base.PublishmentSystemID, "新建文件夹", string.Format("文件夹:{0}", this.DirectoryName.Text));
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
