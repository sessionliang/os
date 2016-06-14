using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;


using SiteServer.CMS.Model;
using BaiRong.Core.Service;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundPublishLocal : BackgroundBasePage
	{
		public Literal ltlCurrentDirectory;
        public Literal ltlFileSystems;
		public ImageButton DeleteButton;
		public ImageButton CreateButton;
        public ImageButton SendButton;
		public HyperLink UploadLink;
		public DropDownList ddlListType;

        private string virtualPath = string.Empty;
        private string listType = string.Empty;
        private string directoryPath = string.Empty;
        private EStorageClassify classify;

        public static string GetRedirectUrl(int publishmentSystemID, string virtualPath, EStorageClassify classify, string listType)
        {
            return PageUtils.GetCMSUrl(string.Format("background_publishLocal.aspx?publishmentSystemID={0}&virtualPath={1}&classify={2}&listType={3}", publishmentSystemID, virtualPath, EStorageClassifyUtils.GetValue(classify), listType));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.classify = EStorageClassifyUtils.GetEnumType(base.GetQueryString("classify"));
            this.virtualPath = base.GetQueryString("virtualPath").Trim('/');
            if (string.IsNullOrEmpty(this.virtualPath))
            {
                this.virtualPath = "@";
                if (this.classify == EStorageClassify.Image)
                {
                    this.virtualPath = "@/" + base.PublishmentSystemInfo.Additional.ImageUploadDirectoryName;
                }
                else if (this.classify == EStorageClassify.Video)
                {
                    this.virtualPath = "@/" + base.PublishmentSystemInfo.Additional.VideoUploadDirectoryName;
                }
                else if (this.classify == EStorageClassify.File)
                {
                    this.virtualPath = "@/" + base.PublishmentSystemInfo.Additional.FileUploadDirectoryName;
                }
            }
            this.listType = base.GetQueryString("listType");
            StorageManager.SetCurrentPath(AdminManager.Current.UserName, this.virtualPath, false);

            this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.virtualPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

			if (!IsPostBack)
            {
				this.DeleteButton.Attributes.Add("onclick", "return confirm(\"此操作将删除所选文件夹及文件，确定吗？\");");

                this.SendButton.Attributes.Add("onclick", Modal.PublishFile.GetOpenWindowString(base.PublishmentSystemID, this.classify, true));
                this.CreateButton.Attributes.Add("onclick", Modal.CreateDirectory.GetOpenWindowString(base.PublishmentSystemID, this.virtualPath));
                this.UploadLink.Attributes.Add("onclick", Modal.UploadFile.GetOpenWindowStringToList(base.PublishmentSystemID, EUploadType.File, this.virtualPath));

				this.ddlListType.Items.Add(new ListItem("显示缩略图", "Image"));
                this.ddlListType.Items.Add(new ListItem("显示详细信息", "List"));
				if (!string.IsNullOrEmpty(this.listType))
				{
                    ControlUtils.SelectListItems(this.ddlListType, this.listType);
				}

				StringBuilder navigationBuilder = new StringBuilder();
				string[] directoryNames = this.virtualPath.Split('/');
				string linkCurrentPath = "@";
				foreach (string directoryName in directoryNames)
				{
					if (!string.IsNullOrEmpty(directoryName))
					{
						if (directoryName.Equals("~"))
						{
							navigationBuilder.AppendFormat("<a href='{0}'>根目录</a>", BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, string.Empty, this.classify, this.listType));
						}
						else if (directoryName.Equals("@"))
						{
                            navigationBuilder.AppendFormat("<a href='{0}'>{1}</a>", BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, string.Empty, this.classify, this.listType), base.PublishmentSystemInfo.PublishmentSystemDir);
						}
						else
						{
							linkCurrentPath += "/" + directoryName;
                            navigationBuilder.AppendFormat("<a href='{0}'>{1}</a>", BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, linkCurrentPath, this.classify, this.listType), directoryName);
						}
						navigationBuilder.Append("\\");
					}
				}
                this.ltlCurrentDirectory.Text = navigationBuilder.ToString();

				this.FillFileSystems(false);
			}
		}

		public void LinkButton_Command(object sender, CommandEventArgs e)
		{
			string navigationUrl = string.Empty;
			if (e.CommandName.Equals("NavigationBar"))
			{
				if (e.CommandArgument.Equals("Up"))
				{
                    if (this.virtualPath != "@")
					{
                        int index = this.virtualPath.LastIndexOf("/");
						if (index != -1)
						{
                            this.virtualPath = this.virtualPath.Substring(0, index);
                            navigationUrl = BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, this.virtualPath, this.classify, this.listType);
						}
					}
				}
				else if (e.CommandArgument.Equals("Delete"))
				{
					string nameCollection = this.Request["NameCollection"];
                    if (!string.IsNullOrEmpty(nameCollection))
					{
                        ArrayList nameArrayList = TranslateUtils.StringCollectionToArrayList(nameCollection);
                        if (nameArrayList != null && nameArrayList.Count > 0)
						{
                            foreach (string name in nameArrayList)
							{
                                string type = name.Split('_')[0];
                                string fsName = name.Split('_')[1];
                                if (type == "file")
                                {
                                    FileUtils.DeleteFileIfExists(PathUtils.Combine(this.directoryPath, fsName));
                                }
                                else if (type == "dir")
                                {
                                    string path = PathUtils.Combine(this.directoryPath, fsName);
                                    DirectoryUtils.DeleteDirectoryIfExists(path);
                                }
							}
						}
					}

                    this.FillFileSystems(true);
                }
			}

			if (string.IsNullOrEmpty(navigationUrl))
			{
                navigationUrl = BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, this.virtualPath, this.classify, this.listType);
			}
			PageUtils.Redirect(navigationUrl);
		}

		public void ddlListType_SelectedIndexChanged(object sender, EventArgs e)
		{
            string navigationUrl = BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, this.virtualPath, this.classify, this.ddlListType.SelectedValue);
			PageUtils.Redirect(navigationUrl);
		}


		#region Helper
		private void FillFileSystems(bool isReload)
		{
			string cookieName = "SiteServer.CMS.BackgroundPages.BackgroundPublishLocal";
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
				this.FillFileSystemsToList(isReload);
			}
            else if (this.ddlListType.SelectedValue == "Image")
			{
				this.FillFileSystemsToImage(isReload);
			}
		}

		private void FillFileSystemsToImage(bool isReload)
		{
			StringBuilder builder = new StringBuilder();
            builder.Append("<table class=\"table table-noborder table-hover\">");
            string directoryUrl = PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, string.Empty);
            string backgroundImageUrl = PageUtils.GetIconUrl("filesystem/management/background.gif");
			string directoryImageUrl = PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Directory, true);

			FileSystemInfoExtendCollection fileSystemInfoExtendCollection = FileManager.GetFileSystemInfoExtendCollection(this.directoryPath, isReload);

            ArrayList lowerSystemDirArrayList = new ArrayList();
            if (this.virtualPath == "@")
            {
                lowerSystemDirArrayList = DataProvider.NodeDAO.GetLowerSystemDirArrayList(base.PublishmentSystemID);
                if (base.PublishmentSystemInfo.IsHeadquarters)
                {
                    lowerSystemDirArrayList.AddRange(DataProvider.NodeDAO.GetLowerSystemDirArrayList(0));
                    ArrayList systemArrayList = DirectoryUtils.GetLowerSystemDirectoryNames();
                    lowerSystemDirArrayList.AddRange(systemArrayList);
                }
            }

			int mod = 0;
			foreach (FileSystemInfoExtend subDirectoryInfo in fileSystemInfoExtendCollection.Folders)
			{
                if (lowerSystemDirArrayList.Contains(subDirectoryInfo.Name.ToLower())) continue;
				if (mod % 3 == 0)
				{
					builder.Append("<tr>");
				}
                string linkUrl = BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, PageUtils.Combine(this.virtualPath, subDirectoryInfo.Name), this.classify, this.listType);

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
", backgroundImageUrl, linkUrl, directoryImageUrl, StringUtils.MaxLengthText(subDirectoryInfo.Name, 7), subDirectoryInfo.Name);

				if (mod % 3 == 2)
				{
					builder.Append("</tr>");
				}
				mod++;
			}

			foreach (FileSystemInfoExtend fileInfo in fileSystemInfoExtendCollection.Files)
			{
				if (mod % 3 == 0)
				{
					builder.Append("<tr>");
				}
				EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileInfo.Type);
				string linkUrl = PageUtils.Combine(directoryUrl, fileInfo.Name);
				string fileImageUrl = string.Empty;
                string imageStyleAttributes = string.Empty;
                if (EFileSystemTypeUtils.IsImage(fileInfo.Type))
				{
					string imagePath = PathUtils.Combine(this.directoryPath, fileInfo.Name);
					try
					{
						System.Drawing.Image image = ImageUtils.GetImage(imagePath);
						if (image.Height > image.Width)
						{
							if (image.Height > 94)
							{
                                imageStyleAttributes = @"style=""height:94px;""";
							}
						}
						else
						{
							if (image.Width > 94)
							{
                                imageStyleAttributes = @"style=""width:94px;""";
							}
						}
						fileImageUrl = PageUtils.Combine(directoryUrl, fileInfo.Name);
						image.Dispose();
					}
					catch
					{
						fileImageUrl = PageUtils.GetClientFileSystemIconUrl(fileSystemType, true);
					}
				}
				else
				{
					fileImageUrl = PageUtils.GetClientFileSystemIconUrl(fileSystemType, true);
				}

				builder.AppendFormat(@"
<td>
		<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
			<tr>
				<td style=""height:100px; width:100px; text-align:center; vertical-align:middle;"">
					<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
						<tr>
							<td background=""{0}"" style=""background-repeat:no-repeat; background-position:center;height:96px; width:96px; text-align:center; vertical-align:middle;"" align=""center""><a href=""{1}"" target=""_blank""><img src=""{2}"" {3} border=0 /></a></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{1}"" target=""_blank"">{4}</a> <input type=""checkbox"" name=""NameCollection"" value=""file_{5}"" /></td>
			</tr>
		</table>
	</td>
", backgroundImageUrl, linkUrl, fileImageUrl, imageStyleAttributes, StringUtils.MaxLengthText(fileInfo.Name, 7), fileInfo.Name);

				if (mod % 3 == 2)
				{
					builder.Append("</tr>");
				}
				mod++;
			}

			builder.Append("</table>");
            this.ltlFileSystems.Text = builder.ToString();
		}

		private void FillFileSystemsToList(bool isReload)
		{
			StringBuilder builder = new StringBuilder();
            builder.Append("<table class=\"table table-noborder table-hover\"><tr class=\"info thead\"><td>名称</td><td width=\"80\">大小</td><td width=\"120\">修改日期</td><td width=\"40\" class=\"center\"><input type=\"checkbox\" onclick=\"_checkFormAll(this.checked)\" /></td></tr>");

            string directoryUrl = PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, string.Empty);

			FileSystemInfoExtendCollection fileSystemInfoExtendCollection = FileManager.GetFileSystemInfoExtendCollection(this.directoryPath, isReload);

            ArrayList lowerSystemDirArrayList = new ArrayList();
            if (this.virtualPath == "@")
            {
                lowerSystemDirArrayList = DataProvider.NodeDAO.GetLowerSystemDirArrayList(base.PublishmentSystemID);
                if (base.PublishmentSystemInfo.IsHeadquarters)
                {
                    lowerSystemDirArrayList.AddRange(DataProvider.NodeDAO.GetLowerSystemDirArrayList(0));
                    ArrayList systemArrayList = DirectoryUtils.GetLowerSystemDirectoryNames();
                    lowerSystemDirArrayList.AddRange(systemArrayList);
                }
            }

			foreach (FileSystemInfoExtend subDirectoryInfo in fileSystemInfoExtendCollection.Folders)
			{
                if (lowerSystemDirArrayList.Contains(subDirectoryInfo.Name.ToLower())) continue;
				string fileNameString = string.Format("<img src={0} border=0 /> {1}", PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Directory, false), subDirectoryInfo.Name);
				DateTime fileModifyDateTime = subDirectoryInfo.LastWriteTime;
                string linkUrl = BackgroundPublishLocal.GetRedirectUrl(base.PublishmentSystemID, PageUtils.Combine(this.virtualPath, subDirectoryInfo.Name), this.classify, this.listType);
                string trHtml = string.Format("<tr><td><nobr><a href=\"{0}\">{1}</a></nobr></td><td align=\"right\">&nbsp;</td><td class=\"center\">{2}</td><td class=\"center\"><input type=\"checkbox\" name=\"NameCollection\" value=\"dir_{3}\" /></td></tr>"
					, linkUrl, fileNameString, DateUtils.GetDateAndTimeString(fileModifyDateTime, EDateFormatType.Day, ETimeFormatType.ShortTime), subDirectoryInfo.Name);
				builder.Append(trHtml);
			}

			foreach (FileSystemInfoExtend fileInfo in fileSystemInfoExtendCollection.Files)
			{
				string fileExt = fileInfo.Type;
				EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileExt);
				string fileNameString = string.Format("<img src={0} border=0 /> {1}", PageUtils.GetClientFileSystemIconUrl(fileSystemType, false), fileInfo.Name);
				long fileKBSize = TranslateUtils.GetKBSize(fileInfo.Size);
				DateTime fileModifyDateTime = fileInfo.LastWriteTime;
				string linkUrl = PageUtils.Combine(directoryUrl, fileInfo.Name);
                string trHtml = string.Format("<tr><td><nobr><a href=\"{0}\" target=\"_blank\">{1}</a></nobr></td><td align=\"right\">{2} KB</td><td class=\"center\">{3}</td><td class=\"center\"><input type=\"checkbox\" name=\"NameCollection\" value=\"file_{4}\" /></td></tr>"
					, linkUrl, fileNameString, fileKBSize, DateUtils.GetDateAndTimeString(fileModifyDateTime, EDateFormatType.Day, ETimeFormatType.ShortTime), fileInfo.Name);
				builder.Append(trHtml);
			}

			builder.Append("</table>");
            this.ltlFileSystems.Text = builder.ToString();
		}

		#endregion
	}
}
