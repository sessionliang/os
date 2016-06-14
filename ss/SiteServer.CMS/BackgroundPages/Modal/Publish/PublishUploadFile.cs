using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Model.Service;
using System.Web.UI.WebControls;
using BaiRong.Core.Service;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class PublishUploadFile : BackgroundBasePage
	{
        public HtmlInputFile hifUpload;

        private EStorageClassify classify;

        public static string GetOpenWindowString(int publishmentSystemID, EStorageClassify classify)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("classify", EStorageClassifyUtils.GetValue(classify));
            return PageUtility.GetOpenWindowString("上传附件", "modal_publishUploadFile.aspx", arguments, 480, 220);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.classify = EStorageClassifyUtils.GetEnumType(base.GetQueryString("classify"));
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isUpload = false;

            if (this.hifUpload.PostedFile != null && "" != this.hifUpload.PostedFile.FileName)
            {
                string filePath = this.hifUpload.PostedFile.FileName;

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
                        StorageManager storageManager = new StorageManager(storageInfo, PathUtils.Combine(storagePath, StorageManager.GetCurrentPath(AdminManager.Current.UserName, true)));
                        if (storageManager.IsEnabled)
                        {
                            try
                            {
                                string fileExtName = PathUtils.GetExtension(filePath).ToLower();
                                string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

                                if (!PathUtility.IsUploadExtenstionAllowed(this.classify, base.PublishmentSystemInfo, fileExtName))
                                {
                                    base.FailMessage("此格式不允许上传，请选择有效的文件");
                                    return;
                                }
                                if (!PathUtility.IsUploadSizeAllowed(this.classify, base.PublishmentSystemInfo, this.hifUpload.PostedFile.ContentLength))
                                {
                                    base.FailMessage("上传失败，上传文件超出规定文件大小！");
                                    return;
                                }

                                this.hifUpload.PostedFile.SaveAs(localFilePath);

                                FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);

                                storageManager.Manager.UploadFile(localFilePath);

                                isUpload = true;
                            }
                            catch (Exception ex)
                            {
                                base.FailMessage(ex, "文件上传失败");
                            }
                        }
                    }
                }
			}

            if (isUpload)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
		}

	}
}
