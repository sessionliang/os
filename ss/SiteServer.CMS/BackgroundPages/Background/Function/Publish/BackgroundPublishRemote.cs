using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;


using BaiRong.Model.Service;
using BaiRong.Core.Service;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundPublishRemote : BackgroundBasePage
	{
        public Literal ltlCurrentDirectory;
        public Literal ltlFileSystems;

        public ImageButton ibPublish;
        public HyperLink hlUp;
        public HyperLink hlReload;
        public HyperLink hlUpload;
		public ImageButton ibCreate;
        public HyperLink hlDelete;
		public DropDownList ddlListType;

        private string virtualPath = string.Empty;
        private string listType = string.Empty;
        private EStorageClassify classify;
        private StorageManager storageManager;

		public static string GetRedirectUrl(int publishmentSystemID, string virtualPath, EStorageClassify classify, string listType)
        {
            return string.Format("background_publishRemote.aspx?publishmentSystemID={0}&virtualPath={1}&classify={2}&listType={3}", publishmentSystemID, virtualPath, EStorageClassifyUtils.GetValue(classify), listType);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.classify = EStorageClassifyUtils.GetEnumType(base.GetQueryString("classify"));
            this.virtualPath = base.GetQueryString("virtualPath").Trim('/');
            this.listType = base.GetQueryString("listType");
            StorageManager.SetCurrentPath(AdminManager.Current.UserName, this.virtualPath, true);

            if (!IsPostBack)
            {
                this.ddlListType.Items.Add(new ListItem("显示缩略图", "Image"));
                this.ddlListType.Items.Add(new ListItem("显示详细信息", "List"));
                if (!string.IsNullOrEmpty(this.listType))
                {
                    ControlUtils.SelectListItems(this.ddlListType, this.listType);
                }

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
                        string path = PathUtils.Combine(storagePath, this.virtualPath);
                        this.storageManager = new StorageManager(storageInfo, path);
                        if (this.storageManager.IsEnabled)
                        {
                            this.ltlCurrentDirectory.Text = path;

                            if (base.GetQueryString("Delete") != null)
                            {
                                string nameCollection = this.Request["NameCollection"];
                                if (!string.IsNullOrEmpty(nameCollection))
                                {
                                    ArrayList nameArrayList = TranslateUtils.StringCollectionToArrayList(nameCollection);
                                    if (nameArrayList != null && nameArrayList.Count > 0)
                                    {
                                        foreach (string name in nameArrayList)
                                        {
                                            string type =  name.Split('_')[0];
                                            string fsName = name.Split('_')[1];
                                            if (type == "file")
                                            {
                                                this.storageManager.Manager.RemoveFile(fsName);
                                            }
                                            else if (type == "dir")
                                            {
                                                this.storageManager.Manager.RemoveDirs(fsName);
                                            }
                                        }
                                    }
                                }

                                try
                                {
                                    this.ltlCurrentDirectory.Text = this.storageManager.Manager.GetWorkingDirectory();

                                    this.FillFileSystems();
                                }
                                catch (Exception ex)
                                {
                                    PageUtils.RedirectToErrorPage(ex.Message);
                                    return;
                                }
                            }

                            this.FillFileSystems();
                        }
                        else
                        {
                            base.FailMessage(this.storageManager.ErrorMessage);
                            return;
                        }
                    }
                }

                this.ibPublish.Attributes.Add("onclick", Modal.PublishFile.GetOpenWindowString(base.PublishmentSystemID, this.classify, false));

                if (!string.IsNullOrEmpty(this.virtualPath))
                {
                    string path = string.Empty;
                    int index = this.virtualPath.LastIndexOf("/");
                    if (index != -1)
                    {
                        path = this.virtualPath.Substring(0, index);
                    }
                    else
                    {
                        path = string.Empty;
                    }
                    this.hlUp.NavigateUrl = BackgroundPublishRemote.GetRedirectUrl(base.PublishmentSystemID, path, this.classify, this.listType);
                }
                else
                {
                    this.hlUp.Visible = false;
                }

                this.hlReload.NavigateUrl = PageUtils.GetLoadingUrl("cms/" + BackgroundPublishRemote.GetRedirectUrl(base.PublishmentSystemID, this.virtualPath, this.classify, this.listType));

                this.hlUpload.Attributes.Add("onclick", Modal.PublishUploadFile.GetOpenWindowString(base.PublishmentSystemID, this.classify));

                this.ibCreate.Attributes.Add("onclick", Modal.PublishCreateDirectory.GetOpenWindowString(base.PublishmentSystemID, this.classify));

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.AddQueryString(BackgroundPublishRemote.GetRedirectUrl(base.PublishmentSystemID, this.virtualPath, this.classify, this.listType), "Delete", "True"), "NameCollection", "NameCollection", "请选择需要删除的项目", "此操作将删除所选文件夹及文件，确定吗？"));
            }
		}

		public void ddlListType_SelectedIndexChanged(object sender, EventArgs e)
		{
            string navigationUrl = BackgroundPublishRemote.GetRedirectUrl(base.PublishmentSystemID, this.virtualPath, this.classify, this.ddlListType.SelectedValue);
			PageUtils.Redirect(navigationUrl);
		}

		#region Helper
        private void FillFileSystems()
		{
			string cookieName = "SiteServer.CMS.BackgroundPages.BackgroundPublishRemote";
			bool isSetCookie = !string.IsNullOrEmpty(this.listType);
			if (!isSetCookie)
			{
				bool cookieExists = false;
				if (CookieUtils.IsExists(cookieName))
				{
					string cookieValue = CookieUtils.GetCookie(cookieName);
                    foreach (ListItem item in this.ddlListType.Items)
					{
						if (string.Equals(item.Value, cookieValue))
						{
							cookieExists = true;
							item.Selected = true;
						}
					}
				}
				if (!cookieExists)
				{
                    CookieUtils.SetCookie(cookieName, this.ddlListType.SelectedValue, DateTime.MaxValue);
				}
			}
			else
			{
				CookieUtils.SetCookie(cookieName, this.listType, DateTime.MaxValue);
			}
            if (this.ddlListType.SelectedValue == "List")
			{
				this.FillFileSystemsToList();
			}
            else if (this.ddlListType.SelectedValue == "Image")
			{
                this.FillFileSystemsToImage();
			}
		}

        private void FillFileSystemsToImage()
		{
			StringBuilder builder = new StringBuilder();
            builder.Append("<table class=\"table table-noborder table-hover\">");

            string backgroundImageUrl = PageUtils.GetIconUrl("filesystem/management/background.gif");
			string directoryImageUrl = PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Directory, true);

			int mod = 0;
            foreach (string d in this.storageManager.Manager.ListDirectories())
			{
                string directoryName = string.Empty;
                if (this.storageManager.StorageType == EStorageType.Ftp)
                {
                    FTPFileSystemInfo subDirectoryInfo = new FTPFileSystemInfo(d);
                    directoryName = subDirectoryInfo.Name;
                }
                else if (this.storageManager.StorageType == EStorageType.Local)
                {
                    directoryName = PathUtils.GetDirectoryName(d);
                }

                if (directoryName == "." || directoryName == "..") continue;
				if (mod % 3 == 0)
				{
					builder.Append("<tr>");
				}
                string linkUrl = BackgroundPublishRemote.GetRedirectUrl(base.PublishmentSystemID, PageUtils.Combine(this.virtualPath, directoryName), this.classify, this.listType);

				builder.AppendFormat(@"
<td>
		<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
			<tr>
				<td style=""height:100px; width:100px; text-align:center; vertical-align:middle;"">
					<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
						<tr>
							<td background=""{0}"" style=""background-repeat:no-repeat; background-position:center;height:96px; width:96px; text-align:center; vertical-align:middle;"" align=""center""><a href=""{1}""><img src=""{2}"" border=0 /></a></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{1}"">{3}</a> <input type=""checkbox"" name=""NameCollection"" value=""dir_{4}"" /></td>
			</tr>
		</table>
	</td>
", backgroundImageUrl, linkUrl, directoryImageUrl, StringUtils.MaxLengthText(directoryName, 7), directoryName);

				if (mod % 3 == 2)
				{
					builder.Append("</tr>");
				}
				mod++;
			}

            foreach (string f in this.storageManager.Manager.ListFiles())
            {
                string fileName = string.Empty;
                string fileType = string.Empty;
                if (this.storageManager.StorageType == EStorageType.Ftp)
                {
                    FTPFileSystemInfo fileInfo = new FTPFileSystemInfo(f);
                    fileName = fileInfo.Name;
                    fileType = fileInfo.Type;
                }
                else if (this.storageManager.StorageType == EStorageType.Local)
                {
                    fileName = PathUtils.GetFileName(f);
                    fileType = PathUtils.GetExtension(f);
                }
                
				if (mod % 3 == 0)
				{
					builder.Append("<tr>");
				}
				EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileType);
                string fileImageUrl = PageUtils.GetClientFileSystemIconUrl(fileSystemType, true);
				
				builder.AppendFormat(@"
<td>
		<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
			<tr>
				<td style=""height:100px; width:100px; text-align:center; vertical-align:middle;"">
					<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
						<tr>
							<td background=""{0}"" style=""background-repeat:no-repeat; background-position:center;height:96px; width:96px; text-align:center; vertical-align:middle;"" align=""center""><img src=""{1}"" border=0 /></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;"">{2} <input type=""checkbox"" name=""NameCollection"" value=""file_{3}"" /></td>
			</tr>
		</table>
	</td>
", backgroundImageUrl, fileImageUrl, StringUtils.MaxLengthText(fileName, 7), fileName);

				if (mod % 3 == 2)
				{
					builder.Append("</tr>");
				}
				mod++;
			}

			builder.Append("</table>");
			this.ltlFileSystems.Text = builder.ToString();
		}

        private void FillFileSystemsToList()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<table class=\"table table-noborder table-hover\"><tr class=\"info thead\"><td>名称</td><td width=\"80\">大小</td><td width=\"120\">修改日期</td><td width=\"40\" class=\"center\"><input type=\"checkbox\" onclick=\"_checkFormAll(this.checked)\" /></td></tr>");

            try
            {
                foreach (string d in this.storageManager.Manager.ListDirectories())
                {
                    string directoryName = string.Empty;
                    DateTime fileModifyDateTime = DateTime.Now;
                    if (this.storageManager.StorageType == EStorageType.Ftp)
                    {
                        FTPFileSystemInfo subDirectoryInfo = new FTPFileSystemInfo(d);
                        directoryName = subDirectoryInfo.Name;
                        fileModifyDateTime = subDirectoryInfo.LastWriteTime;
                    }
                    else if (this.storageManager.StorageType == EStorageType.Local)
                    {
                        directoryName = PathUtils.GetDirectoryName(d);
                        fileModifyDateTime = System.IO.Directory.GetLastWriteTime(d);
                    }

                    if (directoryName == "." || directoryName == "..") continue;
                    string fileNameString = string.Format("<img src={0} border=0 /> {1}", PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Directory, false), directoryName);
                    string linkUrl = BackgroundPublishRemote.GetRedirectUrl(base.PublishmentSystemID, PageUtils.Combine(this.virtualPath, directoryName), this.classify, this.listType);
                    string trHtml = string.Format("<tr><td><nobr><a href=\"{0}\">{1}</a></nobr></td><td align=\"right\">&nbsp;</td><td class=\"center\">{2}</td><td class=\"center\"><input type=\"checkbox\" name=\"NameCollection\" value=\"dir_{3}\" /></td></tr>"
                        , linkUrl, fileNameString, DateUtils.GetDateAndTimeString(fileModifyDateTime, EDateFormatType.Day, ETimeFormatType.ShortTime), directoryName);
                    builder.Append(trHtml);
                }

                foreach (string f in this.storageManager.Manager.ListFiles())
                {
                    string fileName = string.Empty;
                    string fileType = string.Empty;
                    long fileSize = 0;
                    DateTime fileModifyDateTime = DateTime.Now;
                    if (this.storageManager.StorageType == EStorageType.Ftp)
                    {
                        FTPFileSystemInfo fileInfo = new FTPFileSystemInfo(f);
                        fileName = fileInfo.Name;
                        fileType = fileInfo.Type;
                        fileSize = fileInfo.Size;
                        fileModifyDateTime = fileInfo.LastWriteTime;
                    }
                    else if (this.storageManager.StorageType == EStorageType.Local)
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(f);
                        fileName = fileInfo.Name;
                        fileType = fileInfo.Extension;
                        fileSize = fileInfo.Length;
                        fileModifyDateTime = fileInfo.LastWriteTime;
                    }

                    EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileType);
                    string fileNameString = string.Format("<img src={0} border=0 /> {1}", PageUtils.GetClientFileSystemIconUrl(fileSystemType, false), fileName);
                    long fileKBSize = TranslateUtils.GetKBSize(fileSize);
                    string trHtml = string.Format("<tr><td><nobr>{0}</nobr></td><td align=\"right\">{1} KB</td><td class=\"center\">{2}</td><td class=\"center\"><input type=\"checkbox\" name=\"NameCollection\" value=\"file_{3}\" /></td></tr>"
                        , fileNameString, fileKBSize, DateUtils.GetDateAndTimeString(fileModifyDateTime, EDateFormatType.Day, ETimeFormatType.ShortTime), fileName);
                    builder.Append(trHtml);
                }
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
                return;
            }

            builder.Append("</table>");
            this.ltlFileSystems.Text = builder.ToString();
        }

		#endregion
	}
}
