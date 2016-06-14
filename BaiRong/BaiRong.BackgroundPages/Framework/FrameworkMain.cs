using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;


namespace BaiRong.BackgroundPages
{
    public class FrameworkMain : BackgroundBasePage
	{
        public Literal ltlTitle;

        public string ProductID
        {
            get
            {
                return productID;
            }
        }

        private string productID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            bool isInstalled = false;
            this.productID = base.GetQueryString("productID");

            if (string.IsNullOrEmpty(this.productID))
            {
                this.productID = ProductManager.Apps.ProductID;
            }

            if (ProductManager.IsProductExists(this.productID))
            {
                ModuleFileInfo moduleFileInfo = ProductFileUtils.GetModuleFileInfo(this.productID);
                if (moduleFileInfo != null)
                {
                    this.ltlTitle.Text = moduleFileInfo.FullName + " " + ProductManager.Version;
                    isInstalled = true;
                }
            }

            bool isRedirect = false;
            string errorMessage = string.Empty;

            if (isInstalled)
            {
                if (!StringUtils.EqualsIgnoreCase(this.productID, AppManager.Platform.AppID))
                {
                    LicenseManager licenseManager = LicenseManager.Instance;
                    if (licenseManager != null && licenseManager.IsExpireDateLimited)
                    {
                        if (licenseManager.ExpireDate <= DateTime.Now)
                        {
                            isRedirect = true;
                            errorMessage = this.ltlTitle.Text + "未经正式授权，请联系官方渠道以便获得使用许可！";
                        }
                    }
                }
            }
            else
            {
                isRedirect = true;
                errorMessage = "指定系统未安装！";
            }

            if (isRedirect)
            {
                BaiRongDataProvider.AdministratorDAO.UpdateLastActivityDate(AdminManager.Current.UserName, string.Empty);
                PageUtils.RedirectToLoginPage(errorMessage);
            }
            else
            {
                BaiRongDataProvider.AdministratorDAO.UpdateLastActivityDate(AdminManager.Current.UserName, this.productID);
            }
        }
	}
}
