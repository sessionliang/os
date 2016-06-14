using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Model.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Service;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class PublishFile : BackgroundBasePage
	{
        private bool isToRemote;
        private string nameCollection;
        private EStorageClassify classify;

        public static string GetOpenWindowString(int publishmentSystemID, EStorageClassify classify, bool isToRemote)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("classify", EStorageClassifyUtils.GetValue(classify));
            arguments.Add("isToRemote", isToRemote.ToString());
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("传送文件", "modal_publishFile.aspx", arguments, "NameCollection", "请选择需要传送的内容！", 350, 200);
        }
	
		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "isToRemote");

            this.isToRemote = TranslateUtils.ToBool(base.GetQueryString("isToRemote"));
            this.classify = EStorageClassifyUtils.GetEnumType(base.GetQueryString("classify"));
            this.nameCollection = this.Request["NameCollection"];

			if (!Page.IsPostBack)
			{
                string message = string.Empty;

                int directoryCount = 0;
                int fileCount = 0;
                foreach (string name in nameCollection.Split(','))
                {
                    string type = name.Split('_')[0];
                    if (type == "file")
                    {
                        fileCount++;
                    }
                    else if (type == "dir")
                    {
                        directoryCount++;
                    }
                }

                if (directoryCount > 0 && fileCount > 0)
                {
                    message = string.Format("您共选择传送<strong>{0}</strong>个文件夹及<strong>{1}</strong>个文件，确定吗？", directoryCount, fileCount);
                }
                else if (directoryCount > 0)
                {
                    message = string.Format("您共选择传送<strong>{0}</strong>个文件夹，确定吗？", directoryCount);
                }
                else if (fileCount > 0)
                {
                    message = string.Format("您共选择传送<strong>{0}</strong>个文件，确定吗？", fileCount);
                }
                base.InfoMessage(message);				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            string currentLocalPath = StorageManager.GetCurrentPath(AdminManager.Current.UserName, false);
            string currentRemotePath = StorageManager.GetCurrentPath(AdminManager.Current.UserName, true);

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
                    try
                    {
                        string currentDirectoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, currentLocalPath);

                        StorageManager storageManager = new StorageManager(storageInfo, PathUtils.Combine(storagePath, currentRemotePath));

                        if (storageManager.IsEnabled)
                        {
                            ArrayList fileArrayList = new ArrayList();
                            ArrayList directoryArrayList = new ArrayList();
                            foreach (string name in nameCollection.Split(','))
                            {
                                string type = name.Split('_')[0];
                                string fsName = name.Split('_')[1];
                                if (type == "file")
                                {
                                    fileArrayList.Add(fsName);
                                }
                                else if (type == "dir")
                                {
                                    directoryArrayList.Add(fsName);
                                }
                            }

                            if (this.isToRemote)
                            {
                                if (fileArrayList.Count > 0)
                                {
                                    string[] fileNames = TranslateUtils.ArrayListToStringArray(fileArrayList);
                                    storageManager.Manager.UploadFiles(fileNames, currentDirectoryPath);
                                }

                                if (directoryArrayList.Count > 0)
                                {
                                    foreach (string directoryName in directoryArrayList)
                                    {
                                        storageManager.Manager.UploadDirectory(currentDirectoryPath, directoryName);
                                    }
                                }
                            }
                            else
                            {
                                if (fileArrayList.Count > 0)
                                {
                                    string[] fileNames = TranslateUtils.ArrayListToStringArray(fileArrayList);
                                    storageManager.Manager.DownloadFiles(fileNames, currentDirectoryPath);
                                }

                                if (directoryArrayList.Count > 0)
                                {
                                    foreach (string directoryName in directoryArrayList)
                                    {
                                        storageManager.Manager.DownloadDirectory(currentDirectoryPath, directoryName);
                                    }
                                }
                            }
                            isSuccess = true;
                        }
                        else
                        {
                            PageUtils.RedirectToErrorPage(storageManager.ErrorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        PageUtils.RedirectToErrorPage(ex.Message);
                        isSuccess = false;
                    }
                }
            }

            if (isSuccess)
            {
                if (this.isToRemote)
                {
                    string remoteUrl = PageUtils.GetLoadingUrl("cms/" + BackgroundPublishRemote.GetRedirectUrl(base.PublishmentSystemID, currentRemotePath, this.classify, string.Empty));
                    string scripts = string.Format(@"
window.parent.parent.frames[""remote""].location.href=""{0}"";
", remoteUrl);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, scripts);
                }
                else
                {
                    string localUrl = PageUtils.GetLoadingUrl("cms/" + BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, currentLocalPath, this.classify, string.Empty));
                    string scripts = string.Format(@"
window.parent.parent.frames[""local""].location.href=""{0}"";
", localUrl);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, scripts);
                }
            }
		}
	}
}
