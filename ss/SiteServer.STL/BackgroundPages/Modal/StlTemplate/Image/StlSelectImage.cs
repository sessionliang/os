﻿using System;
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
using SiteServer.CMS.BackgroundPages;
using SiteServer.STL.Parser.TemplateDesign;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
	public class StlSelectImage : BackgroundBasePage
	{
		public Literal ltlCurrentDirectory;
        public Literal ltlFileSystems;
		public HyperLink hlUploadLink;

        private string currentRootPath;
        private string rootPath;
        private string directoryPath;
        private string imageUrl;
        private int templateID;
        private string includeUrl;

        protected override bool IsSinglePage { get { return true; } }

		private string GetLinkUrl(string path)
		{
            return PageUtils.GetSTLUrl(string.Format("modal_stlSelectImage.aspx?publishmentSystemID={0}&rootPath={1}&currentRootPath={2}&imageUrl={3}&templateID={4}&includeUrl={5}", base.PublishmentSystemID, this.rootPath, path, this.imageUrl, this.templateID, this.includeUrl));
		}

        private string GetSettingUrl(string selectedImageUrl)
        {
            return PageUtils.GetSTLUrl(string.Format("modal_stlSelectImage.aspx?publishmentSystemID={0}&imageUrl={1}&templateID={2}&includeUrl={3}&selectedImageUrl={4}", base.PublishmentSystemID, this.imageUrl, this.templateID, this.includeUrl, selectedImageUrl));
        }

        public static string GetOpenWindowStringToImageUrl(int publishmentSystemID, string imageUrl, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("imageUrl", imageUrl);
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            return JsUtils.Layer.GetOpenLayerString("选择图片", PageUtils.GetSTLUrl("modal_stlSelectImage.aspx"), arguments, 550, 480);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "imageUrl");

			this.rootPath = base.GetQueryString("RootPath");
            if (!string.IsNullOrEmpty(this.rootPath))
            {
                this.rootPath = rootPath.TrimEnd('/');
            }
            else
            {
                this.rootPath = "@";
            }
            this.currentRootPath = base.GetQueryString("CurrentRootPath");
            if (!string.IsNullOrEmpty(this.currentRootPath))
            {
                this.currentRootPath = this.currentRootPath.TrimEnd('/');
            }

            this.imageUrl = base.GetQueryString("imageUrl");
            this.templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
            this.includeUrl = base.GetQueryString("includeUrl");

            string selectedImageUrl = base.GetQueryString("selectedImageUrl");
            if (!string.IsNullOrEmpty(selectedImageUrl))
            {
                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, this.templateID);

                string templateContent = TemplateDesignUndoRedo.GetTemplateContent(base.PublishmentSystemInfo, templateInfo, this.includeUrl);
                templateContent = templateContent.Replace(this.imageUrl, selectedImageUrl);
                TemplateDesignManager.UpdateTemplateInfo(base.PublishmentSystemInfo, templateInfo, this.includeUrl, templateContent);
                JsUtils.Layer.CloseModalLayer(base.Page);
            }
            else
            {
                if (string.IsNullOrEmpty(this.currentRootPath))
                {
                    this.currentRootPath = base.PublishmentSystemInfo.Additional.Config_SelectImageCurrentUrl.TrimEnd('/');
                }
                else
                {
                    base.PublishmentSystemInfo.Additional.Config_SelectImageCurrentUrl = this.currentRootPath;
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                }
                if (!this.currentRootPath.StartsWith("@"))
                {
                    this.currentRootPath = PageUtils.Combine("@", this.currentRootPath);
                }

                this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.currentRootPath);
                DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

                if (!Page.IsPostBack)
                {
                    this.hlUploadLink.NavigateUrl = "javascript:;";
                    this.hlUploadLink.Attributes.Add("onclick", StlUploadImageSingle.GetOpenWindowStringToList(base.PublishmentSystemID, this.currentRootPath));

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
                if (!PathUtility.IsImageExtenstionAllowed(base.PublishmentSystemInfo, fileInfo.Type))
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
				string imageStyleAttributes = string.Empty;
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
                    }
                }
                catch
                {
                    fileImageUrl = PageUtils.GetClientFileSystemIconUrl(fileSystemType, true);
                }

                //string textBoxUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, linkUrl);
                string settingUrl = this.GetSettingUrl(linkUrl);

				builder.AppendFormat(@"
<td>
	<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
		<tr>
			<td style=""height:100px; width:100px; text-align:center; vertical-align:middle;"">
				<table cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"">
					<tr>
						<td background=""{0}"" style=""background-repeat:no-repeat; background-position:center;height:96px; width:96px; text-align:center; vertical-align:middle;"" align=""center""><a href=""{1}"" title=""点击此项选择此图片""><img src=""{2}"" {3} border=0 /></a></td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td style=""height:20px; width:100%; text-align:center; vertical-align:middle;""><a href=""{4}"" title=""点击此项浏览此图片"" target=""_blank"">{5}</a></td>
		</tr>
	</table>
</td>
", backgroundImageUrl, settingUrl, fileImageUrl, imageStyleAttributes, linkUrl, StringUtils.MaxLengthText(fileInfo.Name, 7));

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
