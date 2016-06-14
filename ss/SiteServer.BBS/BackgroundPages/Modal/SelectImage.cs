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
using SiteServer.BBS.BackgroundPages;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core;


namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class SelectImage : BackgroundBasePage
	{
        public NoTagText CurrentDirectory;
        public Literal ltlFileSystems;
        public HyperLink UploadLink;

        int forumID;
		string currentRootPath;
		string textBoxClientID;
		string rootPath;
		string directoryPath;

		private string GetRedirectUrl(string path)
		{
            return PageUtils.GetBBSUrl(string.Format("modal_selectImage.aspx?publishmentSystemID={0}&ForumID={1}&RootPath={2}&CurrentRootPath={3}&TextBoxClientID={4}", base.PublishmentSystemID, this.forumID, this.rootPath, path, this.textBoxClientID));
		}

        public string RootUrl
        {
            get
            {
                return ConfigUtils.Instance.ApplicationPath;
            }
        }

        public static string GetOpenWindowString(int publishmentSystemID, ForumInfo forumInfo, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ForumID", forumInfo.ForumID.ToString());
            arguments.Add("RootPath", "@");
            arguments.Add("CurrentRootPath", "@/" + "upload");
            arguments.Add("TextBoxClientID", textBoxClientID);
            return JsUtils.OpenWindow.GetOpenWindowString("选择图片", PageUtils.GetBBSUrl("modal_selectImage.aspx"), arguments, 550, 580, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("ForumID", "RootPath", "CurrentRootPath", "TextBoxClientID");

			this.rootPath = base.GetQueryString("RootPath").TrimEnd('/');
			this.currentRootPath = base.GetQueryString("CurrentRootPath").TrimEnd('/');
			this.textBoxClientID = base.GetQueryString("TextBoxClientID");
            this.forumID = base.GetIntQueryString("ForumID");
			this.directoryPath = PathUtilityBBS.MapPath(this.currentRootPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

			if (!Page.IsPostBack)
			{
                this.UploadLink.NavigateUrl = Modal.UploadImageSingle.GetRedirectUrl(base.PublishmentSystemID, this.forumID, this.currentRootPath, this.textBoxClientID);

				ArrayList previousUrls = this.Session["PreviousUrls"] as ArrayList;
				if (previousUrls == null)
				{
					previousUrls = new ArrayList();
				}
                string currentUrl = this.GetRedirectUrl(this.currentRootPath);
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
                            navigationBuilder.AppendFormat("<a href='{0}'>根目录</a>", this.GetRedirectUrl(this.rootPath));
						}
						else if (directoryName.Equals("@"))
						{
                            navigationBuilder.AppendFormat("<a href='{0}'>{1}</a>", this.GetRedirectUrl(this.rootPath), "bbs");
						}
						else
						{
							linkCurrentRootPath += "/" + directoryName;
                            navigationBuilder.AppendFormat("<a href='{0}'>{1}</a>", this.GetRedirectUrl(linkCurrentRootPath), directoryName);
						}
						navigationBuilder.Append("\\");
					}
				}
				this.CurrentDirectory.Text = navigationBuilder.ToString();

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
                            navigationUrl = this.GetRedirectUrl(this.currentRootPath);
						}
					}
				}
			}

			if (string.IsNullOrEmpty(navigationUrl))
			{
                navigationUrl = this.GetRedirectUrl(this.currentRootPath);
			}
			PageUtils.Redirect(navigationUrl);
		}


		#region Helper

		private void FillFileSystemsToImage(bool isReload)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("<table class=\"table table-noborder table-hover\">");

            string directoryUrl = PageUtilityBBS.GetBBSUrlByPhysicalPath(base.PublishmentSystemID, this.directoryPath);

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
                string linkUrl = this.GetRedirectUrl(PageUtils.Combine(this.currentRootPath, subDirectoryInfo.Name));

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
                if (!EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileInfo.Type))
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

                try
                {
                    if (fileSystemType == EFileSystemType.Swf)
                    {
                        fileImageUrl = PageUtils.GetClientFileSystemIconUrl(EFileSystemType.Swf, true);
                    }
                    else
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
                        if (image.Height > image.Width)
                        {
                            if (image.Height > 94)
                            {
                                imageAttributes = string.Format("height=\"{0}\"", 94);
                            }
                        }
                        else
                        {
                            if (image.Width > 94)
                            {
                                imageAttributes = string.Format("width=\"{0}\"", 94);
                            }
                        }
                        fileImageUrl = PageUtils.Combine(directoryUrl, fileInfo.Name);
                    }
                }
                catch
                {
                    fileImageUrl = PageUtils.GetClientFileSystemIconUrl(fileSystemType, true);
                }

                string textBoxUrl = PageUtilityBBS.GetVirtualUrl(linkUrl);

                builder.AppendFormat(@"
<td>
		<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
			<tr>
				<td style=""height:100px; width:100px; text-align:center; vertical-align:middle;"">
					<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
						<tr>
							<td background=""{0}"" style=""background-repeat:no-repeat; background-position:center;height:96px; width:96px; text-align:center; vertical-align:middle;"" align=""center""><a href=""javascript:;"" onClick=""selectImage('{1}', '{2}');"" title=""点击此项选择此图片""><img src=""{3}"" {4} border=0 /></a></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{2}"" title=""点击此项浏览此图片"" target=""_blank"">{5}</a></td>
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
