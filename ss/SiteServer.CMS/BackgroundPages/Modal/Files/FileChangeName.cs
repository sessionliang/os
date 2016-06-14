using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class FileChangeName : BackgroundBasePage
	{
        protected Literal ltlFileName;
        protected TextBox FileName;
		protected RegularExpressionValidator FileNameValidator;

		private string rootPath;
		private string directoryPath;

        public static string GetOpenWindowString(int publishmentSystemID, string rootPath, string fileName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RootPath", rootPath);
            arguments.Add("FileName", fileName);
            return PageUtility.GetOpenWindowString("修改文件名", "modal_fileChangeName.aspx", arguments, 400, 250);
        }

        public static string GetOpenWindowString(int publishmentSystemID, string rootPath, string fileName, string hiddenClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RootPath", rootPath);
            arguments.Add("FileName", fileName);
            arguments.Add("HiddenClientID", hiddenClientID);
            return PageUtility.GetOpenWindowString("修改文件名", "modal_fileChangeName.aspx", arguments, 400, 250);
        }

		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "RootPath");

            this.rootPath = base.GetQueryString("RootPath").TrimEnd('/');
            this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.rootPath);

			if (!Page.IsPostBack)
			{
                this.ltlFileName.Text = base.GetQueryString("FileName");
			}
		}

        private string RedirectURL()
        {
            return PageUtils.GetCMSUrl(string.Format("modal_fileView.aspx?PublishmentSystemID={0}&RelatedPath={1}&FileName={2}&UpdateName={3}&random={4}&HiddenClientID={5}", base.PublishmentSystemID, base.GetQueryString("rootPath"), base.GetQueryString("FileName"), this.FileName.Text, DateTime.Now.Millisecond, base.GetQueryString("HiddenClientID")));
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChange = false;

            if (!DirectoryUtils.IsDirectoryNameCompliant(this.FileName.Text))
            {
                this.FileNameValidator.IsValid = false;
                this.FileNameValidator.ErrorMessage = "文件名称不符合要求";
                return;
            }

            string path = PathUtils.Combine(this.directoryPath, this.FileName.Text);
            if (FileUtils.IsFileExists(path))
            {
                this.FileNameValidator.IsValid = false;
                this.FileNameValidator.ErrorMessage = "文件已经存在";
            }
            else
            {
                string pathSource = PathUtils.Combine(this.directoryPath, this.ltlFileName.Text);
                FileUtils.MoveFile(pathSource, path, true);
                FileUtils.DeleteFileIfExists(pathSource);
                isChange = true;
            }

            if (isChange)
			{
                StringUtility.AddLog(base.PublishmentSystemID, "修改文件名", string.Format("文件名:{0}", this.FileName.Text));
                //JsUtils.SubModal.CloseModalPageWithoutRefresh(Page);
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.RedirectURL());
			}
		}
	}
}
