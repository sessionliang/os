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
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Product, "�������֤", AppManager.Platform.Permission.Platform_Product);

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

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�������֤", string.Format("����ģ��:{0}", this.moduleID));

                    base.SuccessMessage("���֤�ļ��ϴ��ɹ���");
                    base.AddWaitAndRedirectScript(PageUtils.GetPlatformUrl("background_license.aspx"));
                }
                catch
                {
                    base.FailMessage("���֤�ļ��ϴ�ʧ�ܣ�");
                }
            }
            else
            {
                base.FailMessage("��ѡ����Ҫ��������֤�ļ���");
            }
        }
	}
}
