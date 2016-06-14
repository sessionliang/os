using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Text;
using BaiRong.Core.Diagnostics;
using System.Web.UI.HtmlControls;


namespace BaiRong.BackgroundPages
{
	public class BackgroundLicenseUpload : BackgroundBasePage
	{
        public Literal ltlProductName;
        public Literal ltlComputerID;
        public Literal ltlDomain;
        public HtmlInputFile LicenseFile;

        private string moduleID;
		
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.moduleID = base.GetQueryString("moduleID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Product, "更换许可证", AppManager.Platform.Permission.Platform_Product);

                this.ltlProductName.Text = AppManager.GetAppName(this.moduleID, true);
                this.ltlComputerID.Text = ComputerUtils.GetComputerID();
                this.ltlDomain.Text = PageUtils.GetHost();
			}
		}

        public void ButtonClick(Object sender, EventArgs e)
        {
            if (LicenseFile.PostedFile != null && "" != LicenseFile.PostedFile.FileName)
            {
                try
                {
                    LicenseManager licenseManager = LicenseManager.Instance;

                    string localFilePath = licenseManager.LicenseFilePath;
                    DirectoryUtils.CreateDirectoryIfNotExists(localFilePath);

                    LicenseFile.PostedFile.SaveAs(localFilePath);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "更换许可证", string.Format("功能模块:{0}", this.moduleID));

                    base.SuccessMessage("许可证文件上传成功！");
                    base.AddWaitAndRedirectScript(PageUtils.GetPlatformUrl("background_license.aspx"));
                }
                catch
                {
                    base.FailMessage("许可证文件上传失败！");
                }
            }
            else
            {
                base.FailMessage("请选择需要载入的许可证文件！");
            }
        }
	}
}
