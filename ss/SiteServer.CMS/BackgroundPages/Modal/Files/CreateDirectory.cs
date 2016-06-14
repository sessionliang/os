using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Model.Service;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class CreateDirectory : BackgroundBasePage
	{
		protected TextBox DirectoryName;
		protected RegularExpressionValidator DirectoryNameValidator;

		private string currentRootPath;
		private string directoryPath;

        public static string GetOpenWindowString(int publishmentSystemID, string currentRootPath)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("CurrentRootPath", currentRootPath);
            return PageUtility.GetOpenWindowString("创建文件夹", "modal_createDirectory.aspx", arguments, 400, 250);
        }
	
		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "CurrentRootPath");

			this.currentRootPath = base.GetQueryString("CurrentRootPath").TrimEnd('/');
			this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.currentRootPath);
		}

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
			bool isCreated = false;

            if (!DirectoryUtils.IsDirectoryNameCompliant(this.DirectoryName.Text))
            {
                DirectoryNameValidator.IsValid = false;
                DirectoryNameValidator.ErrorMessage = "文件夹名称不符合要求";
                return;
            }

            string path = PathUtils.Combine(this.directoryPath, this.DirectoryName.Text);
            if (DirectoryUtils.IsDirectoryExists(path))
            {
                DirectoryNameValidator.IsValid = false;
                DirectoryNameValidator.ErrorMessage = "文件夹已经存在";
            }
            else
            {
                DirectoryUtils.CreateDirectoryIfNotExists(path);
                isCreated = true;
            }

			if (isCreated)
			{
                StringUtility.AddLog(base.PublishmentSystemID, "新建文件夹", string.Format("文件夹:{0}", this.DirectoryName.Text));
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
