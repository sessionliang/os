using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class SelectVideo : BackgroundBasePage
	{
		public Literal ltlCurrentDirectory;
        public Literal ltlFileSystems;
		public HyperLink hlUploadLink;

		string currentRootPath;
		string textBoxClientID;
		string rootPath;
		string directoryPath;

        //限制限制文件夹路径最高路径
        private const string topPath = "@/upload/videos";

		private string GetLinkUrl(string path)
		{
            //here, limit top path
            if (!DirectoryUtils.IsInDirectory(topPath, path))
                path = topPath;
            return PageUtils.GetCMSUrl(string.Format("modal_selectVideo.aspx?PublishmentSystemID={0}&RootPath={1}&CurrentRootPath={2}&TextBoxClientID={3}", base.PublishmentSystemID, this.rootPath, path, this.textBoxClientID));
		}

        public string PublishmentSystemUrl
        {
            get
            {
                return base.PublishmentSystemInfo.PublishmentSystemUrl;
            }
        }

        public string RootUrl
        {
            get
            {
                return ConfigUtils.Instance.ApplicationPath;
            }
        }

        public static string GetOpenWindowString(PublishmentSystemInfo publishmentSystemInfo, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemInfo.PublishmentSystemID.ToString());
            arguments.Add("RootPath", "@");
            arguments.Add("CurrentRootPath", string.Empty);
            arguments.Add("TextBoxClientID", textBoxClientID);
            return PageUtility.GetOpenWindowString("选择视频", "modal_selectVideo.aspx", arguments, 550, 480, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "RootPath", "CurrentRootPath", "TextBoxClientID");

			this.rootPath = base.GetQueryString("RootPath").TrimEnd('/');
            this.currentRootPath = base.GetQueryString("CurrentRootPath");
            this.textBoxClientID = base.GetQueryString("TextBoxClientID");

            if (string.IsNullOrEmpty(this.currentRootPath))
            {
                this.currentRootPath = base.PublishmentSystemInfo.Additional.Config_SelectVideoCurrentUrl.TrimEnd('/');
            }
            else
            {
                base.PublishmentSystemInfo.Additional.Config_SelectVideoCurrentUrl = this.currentRootPath;
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
            }
            this.currentRootPath = this.currentRootPath.TrimEnd('/');

			this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.currentRootPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

			if (!Page.IsPostBack)
			{
                this.hlUploadLink.NavigateUrl = "javascript:;";
                this.hlUploadLink.Attributes.Add("onclick", Modal.UploadVideo.GetOpenWindowStringToList(base.PublishmentSystemID, this.currentRootPath));

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
							navigationBuilder.AppendFormat("<a href='{0}'>{1}</a>", this.GetLinkUrl(this.rootPath), base.PublishmentSystemInfo.PublishmentSystemDir);
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

				this.FillFileSystemsToImage(false);
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


		#region Helper

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
", backgroundImageUrl, linkUrl, directoryImageUrl, StringUtils.MaxLengthText(subDirectoryInfo.Name, 7));

				if (mod % 4 == 3)
				{
					builder.Append("</tr>");
				}
				mod++;
			}

			foreach (FileSystemInfoExtend fileInfo in fileSystemInfoExtendCollection.Files)
			{
                if (!PathUtility.IsVideoExtenstionAllowed(base.PublishmentSystemInfo, fileInfo.Type))
				{
					continue;
				}
				if (mod % 4 == 0)
				{
					builder.Append("<tr>");
				}
				
				EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileInfo.Type);
				string linkUrl = PageUtils.Combine(directoryUrl, fileInfo.Name);
				string fileImageUrl;
				string imageAttributes = string.Empty;
                string imagePath = PathUtils.Combine(this.directoryPath, fileInfo.Name);

                fileImageUrl = PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Video, true);

                string textBoxUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, linkUrl);

				builder.AppendFormat(@"
<td onmouseover=""this.className='tdbg-dark';"" onmouseout=""this.className='';"">
		<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
			<tr>
				<td style=""height:100px; width:100px; text-align:center; vertical-align:middle;"">
					<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
						<tr>
							<td background=""{0}"" style=""background-repeat:no-repeat; background-position:center;height:96px; width:96px; text-align:center; vertical-align:middle;"" align=""center""><a href=""javascript:;"" onClick=""selectVideo('{1}', '{2}');"" title=""点击选择视频""><img src=""{3}"" {4} border=0 /></a></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{2}"" title=""点击浏览视频"" target=""_blank"">{5}</a></td>
			</tr>
		</table>
	</td>
", backgroundImageUrl, textBoxUrl, linkUrl, fileImageUrl, imageAttributes, StringUtils.MaxLengthText(fileInfo.Name, 7));

				if (mod % 4 == 3)
				{
					builder.Append("</tr>");
				}
				mod++;
			}

			builder.Append("</table>");
			this.ltlFileSystems.Text = builder.ToString();
		}

		#endregion
	}
}
