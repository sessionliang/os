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

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundFileManagement : BackgroundBasePage
	{
		public Literal ltlCurrentDirectory;
        public Literal ltlFileSystems;
		public ImageButton DeleteButton;
		public HyperLink UploadLink;
		public DropDownList ListType;

        private string directoryPath;
        private string relatedPath;

        public static string GetRedirectUrl(int publishmentSystemID, string relatedPath)
		{
            return PageUtils.GetCMSUrl(string.Format("background_fileManagement.aspx?PublishmentSystemID={0}&relatedPath={1}", publishmentSystemID, relatedPath));
		}

        public static string GetRedirectUrlWithType(int publishmentSystemID, string relatedPath, string listTypeStr)
		{
            return PageUtils.GetCMSUrl(string.Format("background_fileManagement.aspx?PublishmentSystemID={0}&relatedPath={1}&ListType={2}", publishmentSystemID, relatedPath, listTypeStr));
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.relatedPath = base.GetQueryString("relatedPath");

            PageUtils.CheckRequestParameter("PublishmentSystemID", "relatedPath");

            if (ProductPermissionsManager.Current.PublishmentSystemIDList.Contains(base.PublishmentSystemID))
            {
                this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.relatedPath);

                if (!DirectoryUtils.IsDirectoryExists(directoryPath))
                {
                    PageUtils.RedirectToErrorPage("文件夹不存在！");
                    return;
                }

                if (!IsPostBack)
                {
                    base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, "应用文件管理", AppManager.CMS.Permission.WebSite.FileManagement);

                    this.ListType.Items.Add(new ListItem("显示缩略图", "Image"));
                    this.ListType.Items.Add(new ListItem("显示详细信息", "List"));
                    if (!string.IsNullOrEmpty(base.GetQueryString("ListType")))
                    {
                        ControlUtils.SelectListItems(this.ListType, base.GetQueryString("ListType"));
                    }

                    this.ltlCurrentDirectory.Text = PageUtils.Combine(base.PublishmentSystemInfo.PublishmentSystemDir, this.relatedPath);

                    this.FillFileSystems(false);

                    this.DeleteButton.Attributes.Add("onclick", "return confirm(\"此操作将删除所选文件夹及文件，确定吗？\");");

                    string showPopWinString = Modal.UploadFile.GetOpenWindowStringToList(base.PublishmentSystemID, EUploadType.File, this.relatedPath);
                    this.UploadLink.Attributes.Add("onclick", showPopWinString);
                }
            }
		}

		public void LinkButton_Command(object sender, CommandEventArgs e)
		{
			if (e.CommandName.Equals("NavigationBar"))
			{
				if (e.CommandArgument.Equals("Delete"))
				{
					string directoryNameCollection = this.Request["DirectoryNameCollection"];
					if (!string.IsNullOrEmpty(directoryNameCollection))
					{
						ArrayList directoryNameArrayList = TranslateUtils.StringCollectionToArrayList(directoryNameCollection);
						if (directoryNameArrayList != null && directoryNameArrayList.Count > 0)
						{
							foreach (string directoryName in directoryNameArrayList)
							{
								string path = PathUtils.Combine(this.directoryPath, directoryName);
								DirectoryUtils.DeleteDirectoryIfExists(path);
							}
						}
					}
					string fileNameCollection = this.Request["FileNameCollection"];
					if (!string.IsNullOrEmpty(fileNameCollection))
					{
						ArrayList fileNameArrayList = TranslateUtils.StringCollectionToArrayList(fileNameCollection);
						if (fileNameArrayList != null && fileNameArrayList.Count > 0)
						{
							FileUtils.DeleteFilesIfExists(this.directoryPath, fileNameArrayList);
						}
					}
				}
			}

			PageUtils.Redirect(BackgroundFileManagement.GetRedirectUrl(base.PublishmentSystemID, this.relatedPath));
		}

		public void ListType_SelectedIndexChanged(object sender, EventArgs e)
		{
			string navigationUrl = BackgroundFileManagement.GetRedirectUrlWithType(base.PublishmentSystemID, this.relatedPath, this.ListType.SelectedValue);
			PageUtils.Redirect(navigationUrl);
		}


		#region Helper
		private void FillFileSystems(bool isReload)
		{
			string cookieName = "SiteServer.CMS.BackgroundPages.BackgroundFileManagement";
			bool isSetCookie = !string.IsNullOrEmpty(base.GetQueryString("ListType"));
			if (!isSetCookie)
			{
				bool cookieExists = false;
				if (CookieUtils.IsExists(cookieName))
				{
					string cookieValue = CookieUtils.GetCookie(cookieName);
					foreach (ListItem item in this.ListType.Items)
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
					CookieUtils.SetCookie(cookieName, this.ListType.SelectedValue, DateTime.MaxValue);
				}
			}
			else
			{
                CookieUtils.SetCookie(cookieName, base.GetQueryString("ListType"), DateTime.MaxValue);
			}
			if (this.ListType.SelectedValue == "List")
			{
				this.FillFileSystemsToList(isReload);
			}
			else if (this.ListType.SelectedValue == "Image")
			{
				this.FillFileSystemsToImage(isReload);
			}
		}

		private void FillFileSystemsToImage(bool isReload)
		{
			StringBuilder builder = new StringBuilder();
            builder.Append("<table class=\"table table-noborder table-hover\">");

            string directoryUrl = PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, this.relatedPath);

            string backgroundImageUrl = PageUtils.GetIconUrl("filesystem/management/background.gif");
			string directoryImageUrl = PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Directory, true);

			FileSystemInfoExtendCollection fileSystemInfoExtendCollection = FileManager.GetFileSystemInfoExtendCollection(this.directoryPath, isReload);

			int mod = 0;
			foreach (FileSystemInfoExtend subDirectoryInfo in fileSystemInfoExtendCollection.Folders)
			{
                if (string.IsNullOrEmpty(this.relatedPath))
                {
                    if (StringUtils.EqualsIgnoreCase(subDirectoryInfo.Name, "api"))
                    {
                        continue;
                    }
                }
				if (mod % 5 == 0)
				{
					builder.Append("<tr>");
				}
                string linkUrl = BackgroundFileManagement.GetRedirectUrl(base.PublishmentSystemID, PageUtils.Combine(this.relatedPath, subDirectoryInfo.Name));

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
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{1}"">{3}</a> <input type=""checkbox"" name=""DirectoryNameCollection"" value=""{4}"" /></td>
			</tr>
		</table>
	</td>
", backgroundImageUrl, linkUrl, directoryImageUrl, StringUtils.MaxLengthText(subDirectoryInfo.Name, 7), subDirectoryInfo.Name);

				if (mod % 5 == 4)
				{
					builder.Append("</tr>");
				}
				mod++;
			}

            foreach (FileSystemInfoExtend fileInfo in fileSystemInfoExtendCollection.Files)
            {
                if (mod % 5 == 0)
                {
                    builder.Append("<tr>");
                }
                EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileInfo.Type);
                string showPopWinString = Modal.FileView.GetOpenWindowString(base.PublishmentSystemID, this.relatedPath, fileInfo.Name);
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
							<td background=""{0}"" style=""background-repeat:no-repeat; background-position:center;height:96px; width:96px; text-align:center; vertical-align:middle;"" align=""center""><a href=""javascript:;"" onclick=""{1}"" target=""_blank""><img src=""{2}"" {3} border=0 /></a></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{4}"" target=""_blank"">{5}</a> <input type=""checkbox"" name=""FileNameCollection"" value=""{6}"" /></td>
			</tr>
		</table>
	</td>
", backgroundImageUrl, showPopWinString, fileImageUrl, imageStyleAttributes, linkUrl, StringUtils.MaxLengthText(fileInfo.Name, 7), fileInfo.Name);

                if (mod % 5 == 4)
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

            builder.Append("<table class=\"table table-noborder table-hover\"><tr class=\"info thead\"><td>名称</td><td width=\"80\">大小</td><td width=\"120\">类型</td><td width=\"120\">修改日期</td><td width=\"40\"><input type=\"checkbox\" onclick=\"_checkFormAll(this.checked)\" /></td></tr>");
            string directoryUrl = PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, this.relatedPath);

			FileSystemInfoExtendCollection fileSystemInfoExtendCollection = FileManager.GetFileSystemInfoExtendCollection(this.directoryPath, isReload);

			foreach (FileSystemInfoExtend subDirectoryInfo in fileSystemInfoExtendCollection.Folders)
			{
                if (string.IsNullOrEmpty(this.relatedPath))
                {
                    if (StringUtils.EqualsIgnoreCase(subDirectoryInfo.Name, "api"))
                    {
                        continue;
                    }
                }
				string fileNameString = string.Format("<img src={0} border=0 /> {1}", PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Directory, false), subDirectoryInfo.Name);
				string fileSystemTypeString = "文件夹";
				DateTime fileModifyDateTime = subDirectoryInfo.LastWriteTime;
                string linkUrl = BackgroundFileManagement.GetRedirectUrl(base.PublishmentSystemID, PageUtils.Combine(this.relatedPath, subDirectoryInfo.Name));
                string trHtml = string.Format("<tr><td><nobr><a href=\"{0}\">{1}</a></nobr></td><td class=\"center\">&nbsp;</td><td class=\"center\">{2}</td><td class=\"center\">{3}</td><td class=\"center\"><input type=\"checkbox\" name=\"DirectoryNameCollection\" value=\"{4}\" /></td></tr>"
					, linkUrl, fileNameString, fileSystemTypeString, DateUtils.GetDateAndTimeString(fileModifyDateTime, EDateFormatType.Day, ETimeFormatType.ShortTime), subDirectoryInfo.Name);
				builder.Append(trHtml);
			}

            foreach (FileSystemInfoExtend fileInfo in fileSystemInfoExtendCollection.Files)
            {
                string fileExt = fileInfo.Type;
                EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileExt);
                string fileNameString = string.Format("<img src={0} border=0 /> {1}", PageUtils.GetClientFileSystemIconUrl(fileSystemType, false), fileInfo.Name);
                string fileSystemTypeString = (fileSystemType == EFileSystemType.Unknown) ? string.Format("{0} 文件", fileExt.TrimStart('.').ToUpper()) : EFileSystemTypeUtils.GetText(fileSystemType);
                long fileKBSize = TranslateUtils.GetKBSize(fileInfo.Size);
                DateTime fileModifyDateTime = fileInfo.LastWriteTime;
                string linkUrl = PageUtils.Combine(directoryUrl, fileInfo.Name);
                string trHtml = string.Format("<tr><td><nobr><a href=\"{0}\" target=\"_blank\">{1}</a></nobr></td><td class=\"center\">{2} KB</td><td class=\"center\">{3}</td><td class=\"center\">{4}</td><td class=\"center\"><input type=\"checkbox\" name=\"FileNameCollection\" value=\"{5}\" /></td></tr>"
                    , linkUrl, fileNameString, fileKBSize, fileSystemTypeString, DateUtils.GetDateAndTimeString(fileModifyDateTime, EDateFormatType.Day, ETimeFormatType.ShortTime), fileInfo.Name);
                builder.Append(trHtml);
            }

			builder.Append("</table>");
			this.ltlFileSystems.Text = builder.ToString();
		}

		#endregion
	}
}
