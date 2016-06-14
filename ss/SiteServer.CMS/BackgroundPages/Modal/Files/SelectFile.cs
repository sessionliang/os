using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class SelectFile : BackgroundBasePage
	{
		public Literal ltlCurrentDirectory;
        public Literal ltlFileSystems;
		public HyperLink hlUploadLink;
		public DropDownList ddlListType;

		string currentRootPath;
		string rootPath;
		string directoryPath;
        string hiddenClientID;

        //限制限制文件夹路径最高路径
        private const string topPath = "@/upload/files";

		private string GetLinkUrl(string path)
		{
            //here, limit top path
            if (!DirectoryUtils.IsInDirectory(topPath, path))
                path = topPath;
            return PageUtils.GetCMSUrl(string.Format("modal_selectFile.aspx?PublishmentSystemID={0}&RootPath={1}&CurrentRootPath={2}&HiddenClientID={3}", base.PublishmentSystemID, this.rootPath, path, this.hiddenClientID));
		}

		private string GetLinkUrlWithType(string path, string listTypeStr)
		{
            //here, limit top path
            if (!DirectoryUtils.IsInDirectory(topPath, path))
                path = topPath;
            return PageUtils.GetCMSUrl(string.Format("modal_selectFile.aspx?PublishmentSystemID={0}&RootPath={1}&CurrentRootPath={2}&ListType={3}&HiddenClientID={4}", base.PublishmentSystemID, this.rootPath, path, listTypeStr, this.hiddenClientID));
		}

        public static string GetOpenWindowString(int publishmentSystemID, string hiddenClientID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            string currentRootPath = string.Empty;
            return GetOpenWindowString(publishmentSystemID, hiddenClientID, currentRootPath);
        }

        public static string GetOpenWindowString(int publishmentSystemID, string hiddenClientID, string currentRootPath)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RootPath", "@");
            arguments.Add("CurrentRootPath", currentRootPath);
            arguments.Add("HiddenClientID", hiddenClientID);
            return PageUtility.GetOpenWindowString("选择文件", "modal_selectFile.aspx", arguments, 550, 480, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "RootPath", "CurrentRootPath", "HiddenClientID");
			
			this.rootPath = base.GetQueryString("RootPath").TrimEnd('/');
			this.currentRootPath = base.GetQueryString("CurrentRootPath");
            this.hiddenClientID = base.GetQueryString("HiddenClientID");

            if (string.IsNullOrEmpty(this.currentRootPath))
            {
                this.currentRootPath = base.PublishmentSystemInfo.Additional.Config_SelectFileCurrentUrl.TrimEnd('/');
            }
            else
            {
                base.PublishmentSystemInfo.Additional.Config_SelectFileCurrentUrl = this.currentRootPath;
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
            }
            this.currentRootPath = this.currentRootPath.TrimEnd('/');

			this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.currentRootPath);
            DirectoryUtils.CreateDirectoryIfNotExists(this.directoryPath);
			if (!DirectoryUtils.IsDirectoryExists(directoryPath))
			{
                PageUtils.RedirectToErrorPage("文件夹不存在！");
                return;
			}

			if (!Page.IsPostBack)
			{
                this.hlUploadLink.NavigateUrl = "javascript:;";
                this.hlUploadLink.Attributes.Add("onclick", Modal.UploadFile.GetOpenWindowStringToList(base.PublishmentSystemID, EUploadType.File, this.currentRootPath));

				this.ddlListType.Items.Add(new ListItem("显示缩略图", "Image"));
				this.ddlListType.Items.Add(new ListItem("显示详细信息", "List"));
				if (!string.IsNullOrEmpty(base.GetQueryString("ListType")))
				{
                    ControlUtils.SelectListItems(this.ddlListType, base.GetQueryString("ListType"));
				}

				ArrayList previousUrls = this.Session["PreviousUrls"] as ArrayList;
				if (previousUrls == null)
				{
					previousUrls = new ArrayList();
				}
				string currentUrl = this.GetLinkUrl(this.currentRootPath);
				if (previousUrls.Count > 0)
				{
					string url = previousUrls[previousUrls.Count - 1] as string;
					if (!string.Equals(url, currentUrl))
					{
						previousUrls.Add(currentUrl);
						this.Session["PreviousUrls"] = previousUrls;
					}
				}
				else
				{
					previousUrls.Add(currentUrl);
					this.Session["PreviousUrls"] = previousUrls;
				}

				StringBuilder navigationBuilder = new StringBuilder();
				string[] directoryNames = this.currentRootPath.Split('/');
				string linkCurrentRootPath = this.rootPath;
				foreach (string directoryName in directoryNames)
				{
					if (!string.IsNullOrEmpty(directoryName))
					{
						if (directoryName.Equals("~"))
						{
							navigationBuilder.AppendFormat("<a href='{0}'>根目录</a>", this.GetLinkUrl(this.rootPath));
						}
						else if (directoryName.Equals("@"))
						{
							navigationBuilder.AppendFormat("<a href='{0}'>{1}</a>", this.GetLinkUrl(this.rootPath), PublishmentSystemManager.GetPublishmentSystemInfo(base.PublishmentSystemID).PublishmentSystemDir);
						}
						else
						{
							linkCurrentRootPath += "/" + directoryName;
							navigationBuilder.AppendFormat("<a href='{0}'>{1}</a>", this.GetLinkUrl(linkCurrentRootPath), directoryName);
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
				if (e.CommandArgument.Equals("Back"))
				{
					ArrayList previousUrls = this.Session["PreviousUrls"] as ArrayList;
					if (previousUrls != null && previousUrls.Count > 1)
					{
						previousUrls.RemoveAt(previousUrls.Count - 1);
						this.Session["PreviousUrls"] = previousUrls;

						navigationUrl = previousUrls[previousUrls.Count - 1] as string;
					}
				}
				else if (e.CommandArgument.Equals("Up"))
				{
					if (this.currentRootPath.StartsWith(this.rootPath) && this.currentRootPath.Length > this.rootPath.Length)
					{
						int index = this.currentRootPath.LastIndexOf("/");
						if (index != -1)
						{
							this.currentRootPath = this.currentRootPath.Substring(0, index);
							navigationUrl = this.GetLinkUrl(this.currentRootPath);
						}
					}
				}
			}

			if (string.IsNullOrEmpty(navigationUrl))
			{
				navigationUrl = this.GetLinkUrl(this.currentRootPath);
			}
			PageUtils.Redirect(navigationUrl);
		}

		public void ddlListType_SelectedIndexChanged(object sender, EventArgs e)
		{
			string navigationUrl = this.GetLinkUrlWithType(this.currentRootPath, this.ddlListType.SelectedValue);
			PageUtils.Redirect(navigationUrl);
		}

		#region Helper
		private void FillFileSystems(bool isReload)
		{
			string cookieName = "SiteServer.CMS.BackgroundPages.Modal.SelectAttachment";
			bool isSetCookie = !string.IsNullOrEmpty(base.GetQueryString("ListType"));
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
                CookieUtils.SetCookie(cookieName, base.GetQueryString("ListType"), DateTime.MaxValue);
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
			builder.Append(@"<table class=""table table-noborder table-hover"">");
			
			string directoryUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, this.directoryPath);
            string backgroundImageUrl = PageUtils.GetIconUrl("filesystem/management/background.gif");
			string directoryImageUrl = PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Directory, true);

			FileSystemInfoExtendCollection fileSystemInfoExtendCollection = FileManager.GetFileSystemInfoExtendCollection(this.directoryPath, isReload);

			int mod = 0;
			foreach (FileSystemInfoExtend subDirectoryInfo in fileSystemInfoExtendCollection.Folders)
			{
				if (mod % 4 == 0)
				{
					builder.Append("<tr>");
				}
				string linkUrl = this.GetLinkUrl(PageUtils.Combine(this.currentRootPath, subDirectoryInfo.Name));

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
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{1}"">{3}</a></td>
			</tr>
		</table>
	</td>
", backgroundImageUrl, linkUrl, directoryImageUrl, StringUtils.MaxLengthText(subDirectoryInfo.Name, 8));

				if (mod % 4 == 3)
				{
					builder.Append("</tr>");
				}
				mod++;
			}

			foreach (FileSystemInfoExtend fileInfo in fileSystemInfoExtendCollection.Files)
			{
				if (mod % 4 == 0)
				{
					builder.Append("<tr>");
				}
				EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileInfo.Type);
				string linkUrl = PageUtils.Combine(directoryUrl, fileInfo.Name);
				string fileImageUrl;
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
					fileImageUrl = PathUtility.GetFileSystemIconUrl(base.PublishmentSystemInfo, fileInfo, true);
				}

                string attachmentUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, linkUrl);
                //string fileViewUrl = Modal.FileView.GetOpenWindowString(base.PublishmentSystemID, attachmentUrl);
                string fileViewUrl = Modal.FileView.GetOpenWindowStringHidden(base.PublishmentSystemID, attachmentUrl,this.hiddenClientID);

				builder.AppendFormat(@"
<td>
		<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
			<tr>
				<td style=""height:100px; width:100px; text-align:center; vertical-align:middle;"">
					<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
						<tr>
							<td background=""{0}"" style=""background-repeat:no-repeat; background-position:center;height:96px; width:96px; text-align:center; vertical-align:middle;"" align=""center""><a href=""javascript:;"" onClick=""window.parent.SelectAttachment('{1}', '{2}', '{3}');{4}"" title=""{5}""><img src=""{6}"" {7} border=0 /></a></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{8}"" title=""点击此项浏览此附件"" target=""_blank"">{9}</a></td>
			</tr>
		</table>
	</td>
", backgroundImageUrl, this.hiddenClientID, attachmentUrl.Replace("'", "\\'"), fileViewUrl.Replace("'", "\\'"), JsUtils.OpenWindow.HIDE_POP_WIN, fileInfo.Name, fileImageUrl, imageStyleAttributes, linkUrl, StringUtils.MaxLengthText(fileInfo.Name, 8));

				if (mod % 4 == 3)
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
			builder.Append(@"<table class=""table table-bordered table-hover""><tr class=""info thead""><td>名称</td><td width=""80"">大小</td><td width=""120"">类型</td><td width=""120"">修改日期</td></tr>");
			string directoryUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, this.directoryPath);

			FileSystemInfoExtendCollection fileSystemInfoExtendCollection = FileManager.GetFileSystemInfoExtendCollection(this.directoryPath, isReload);

			foreach (FileSystemInfoExtend subDirectoryInfo in fileSystemInfoExtendCollection.Folders)
			{
				string fileNameString = string.Format("<img src={0} border=0 /> {1}", PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Directory, false), subDirectoryInfo.Name);
				string fileSystemTypeString = "文件夹";
				DateTime fileModifyDateTime = subDirectoryInfo.LastWriteTime;
				string linkUrl = this.GetLinkUrl(PageUtils.Combine(this.currentRootPath, subDirectoryInfo.Name));
				string trHtml = string.Format("<tr><td><nobr><a href=\"{0}\">{1}</a></nobr></td><td align=\"right\">&nbsp;</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td></tr>", linkUrl, fileNameString, fileSystemTypeString, DateUtils.GetDateString(fileModifyDateTime, EDateFormatType.Day));
				builder.Append(trHtml);
			}

			foreach (FileSystemInfoExtend fileInfo in fileSystemInfoExtendCollection.Files)
			{
				string fileNameString = string.Format("<img src={0} border=0 /> {1}", PathUtility.GetFileSystemIconUrl(base.PublishmentSystemInfo, fileInfo, false), fileInfo.Name);
                EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileInfo.Type);
                string fileSystemTypeString = (fileSystemType == EFileSystemType.Unknown) ? string.Format("{0} 文件", fileInfo.Type.TrimStart('.').ToUpper()) : EFileSystemTypeUtils.GetText(fileSystemType);
				long fileKBSize = fileInfo.Size / 1024;
				if (fileKBSize == 0)
				{
					fileKBSize = 1;
				}
				DateTime fileModifyDateTime = fileInfo.LastWriteTime;
				string linkUrl = PageUtils.Combine(directoryUrl, fileInfo.Name);
				string attachmentUrl = linkUrl.Replace(base.PublishmentSystemInfo.PublishmentSystemUrl, "@");
                //string fileViewUrl = Modal.FileView.GetOpenWindowString(base.PublishmentSystemID, attachmentUrl);
                string fileViewUrl = Modal.FileView.GetOpenWindowStringHidden(base.PublishmentSystemID, attachmentUrl,this.hiddenClientID);
                string trHtml = string.Format("<tr><td><nobr><a href=\"javascript:;\" onClick=\"window.parent.SelectAttachment('{0}', '{1}', '{2}');{3}\" title=\"点击此项选择此附件\">{4}</a></nobr></td><td align=\"right\">{5} KB</td><td align=\"center\">{6}</td><td align=\"center\">{7}</td></tr>"
                    , this.hiddenClientID, attachmentUrl.Replace("'", "\\'"), fileViewUrl.Replace("'", "\\'"), JsUtils.OpenWindow.HIDE_POP_WIN, fileNameString, fileKBSize, fileSystemTypeString, DateUtils.GetDateString(fileModifyDateTime, EDateFormatType.Day));
				builder.Append(trHtml);
			}

			builder.Append("</table>");
			this.ltlFileSystems.Text = builder.ToString();
		}

		#endregion
	}
}
