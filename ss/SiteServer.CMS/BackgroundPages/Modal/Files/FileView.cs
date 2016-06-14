using System;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class FileView : BackgroundBasePage
	{
        public Literal ltlFileName;
        public Literal ltlFileType;
        public Literal ltlFilePath;
        public Literal ltlFileSize;
        public Literal ltlCreationTime;
        public Literal ltlLastWriteTime;
        public Literal ltlLastAccessTime;

        public Literal ltlOpen;
        public Literal ltlEdit;
        public Literal ltlChangeName;

		private string relatedPath;
        private string fileName;
        private string filePath;
        private string updateName;
        private string hiddenClientID;

        public static string GetOpenWindowString(int publishmentSystemID, string relatedPath, string fileName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedPath", relatedPath);
            arguments.Add("FileName", fileName);
            return PageUtility.GetOpenWindowString("查看文件属性", "modal_fileView.aspx", arguments, 680, 660);
        }

        public static string GetOpenWindowString(int publishmentSystemID, string fileUrl)
        {
            string relatedPath = "@/";
            string fileName = fileUrl;
            if (!string.IsNullOrEmpty(fileUrl))
            {
                fileUrl = fileUrl.Trim('/');
                int i = fileUrl.LastIndexOf('/');
                if (i != -1)
                {
                    relatedPath = fileUrl.Substring(0, i + 1);
                    fileName = fileUrl.Substring(i + 1, fileUrl.Length - i - 1);
                }
            }
            return GetOpenWindowString(publishmentSystemID, relatedPath, fileName);
        }

        public static string GetOpenWindowStringHidden(int publishmentSystemID, string fileUrl, string hiddenClientID)
        {
            string relatedPath = "@/";
            string fileName = fileUrl;
            if (!string.IsNullOrEmpty(fileUrl))
            {
                fileUrl = fileUrl.Trim('/');
                int i = fileUrl.LastIndexOf('/');
                if (i != -1)
                {
                    relatedPath = fileUrl.Substring(0, i + 1);
                    fileName = fileUrl.Substring(i + 1, fileUrl.Length - i - 1);
                }
            }
            return GetOpenWindowString(publishmentSystemID,hiddenClientID, relatedPath, fileName);
        }

        public static string GetOpenWindowString(int publishmentSystemID,string hiddenClientID, string relatedPath, string fileName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("HiddenClientID", hiddenClientID);
            arguments.Add("RelatedPath", relatedPath);
            arguments.Add("FileName", fileName);
            return PageUtility.GetOpenWindowString("查看文件属性", "modal_fileView.aspx", arguments, 580, 420);
        }

        public static string GetOpenWindowStringWithTextBoxValue(int publishmentSystemID, string textBoxID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("TextBoxID", textBoxID);
            return PageUtility.GetOpenWindowStringWithTextBoxValue("查看文件属性", "modal_fileView.aspx", arguments, textBoxID, 580, 450, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            if (base.GetQueryString("TextBoxID") != null)
            {
                string textBoxID = base.GetQueryString("TextBoxID");
                string virtualUrl = base.GetQueryString(textBoxID);
                this.filePath = PathUtility.MapPath(base.PublishmentSystemInfo, virtualUrl);
            }
            else
            {
                this.relatedPath = base.GetQueryString("RelatedPath").Trim('/');
                this.hiddenClientID = base.GetQueryString("HiddenClientID");
                if (!this.relatedPath.StartsWith("~") && !this.relatedPath.StartsWith("@"))
                {
                    this.relatedPath = "@/" + this.relatedPath;
                }
                this.fileName = base.GetQueryString("FileName");
                this.updateName = base.GetQueryString("UpdateName");
                if (!string.IsNullOrEmpty(this.updateName))
                {
                    this.fileName = this.updateName;
                }
                this.filePath = PathUtility.MapPath(base.PublishmentSystemInfo, PathUtils.Combine(this.relatedPath, this.fileName));
            }

            if (!FileUtils.IsFileExists(this.filePath))
            {
                PageUtils.RedirectToErrorPage("此文件不存在！");
                return;
            }

			if (!IsPostBack)
			{
                FileInfo fileInfo = new FileInfo(this.filePath);
                EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(fileInfo.Extension);
                if (!string.IsNullOrEmpty(base.GetQueryString("UpdateName")))
                {
                    this.ltlFileName.Text = base.GetQueryString("UpdateName");
                }
                else
                {
                    this.ltlFileName.Text = fileInfo.Name;
                }
                this.ltlFileType.Text = EFileSystemTypeUtils.GetText(fileType);
                this.ltlFilePath.Text = Path.GetDirectoryName(this.filePath);
                this.ltlFileSize.Text = TranslateUtils.GetKBSize(fileInfo.Length) + " KB";
                this.ltlCreationTime.Text = fileInfo.CreationTime.ToString("yyyy-MM-dd hh:mm:ss");
                this.ltlLastWriteTime.Text = fileInfo.LastWriteTime.ToString("yyyy-MM-dd hh:mm:ss");
                this.ltlLastAccessTime.Text = fileInfo.LastAccessTime.ToString("yyyy-MM-dd hh:mm:ss");

                this.ltlOpen.Text = string.Format(@"<a href=""{0}"" target=""_blank"">浏 览</a>&nbsp;&nbsp;", PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePath));
                if (EFileSystemTypeUtils.IsTextEditable(fileType))
                {
                    this.ltlEdit.Text = string.Format(@"<a href=""{0}"">修 改</a>&nbsp;&nbsp;", PageUtils.GetCMSUrl(string.Format("modal_fileEdit.aspx?PublishmentSystemID={0}&RelatedPath={1}&FileName={2}&IsCreate=False", base.PublishmentSystemID, this.relatedPath, this.fileName)));
                }
                if (!string.IsNullOrEmpty(this.hiddenClientID))
                {
                    this.ltlChangeName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">改 名</a>", Modal.FileChangeName.GetOpenWindowString(base.PublishmentSystemID, this.relatedPath, fileInfo.Name, this.hiddenClientID));
                }
			}
		}
	}
}
