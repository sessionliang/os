using BaiRong.Core;
using BaiRong.Core.Cryptography;


namespace BaiRong.BackgroundPages
{
    public class FrameworkLogout : BackgroundBasePage
    {
        protected override bool IsAccessable
        {
            get { return true; }
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            string redirectUrl = PageUtils.GetAdminDirectoryUrl("login.aspx");

            if (FileConfigManager.Instance.IsSaas)
            {
                if (FileConfigManager.Instance.SSOConfig.IntegrationType == Model.EIntegrationType.GeXia)
                {
                    if (!FileConfigManager.Instance.OEMConfig.IsOEM)
                    {
                        redirectUrl = "http://www.gexia.com/home/login.html";
                    }
                }
                else if (FileConfigManager.Instance.SSOConfig.IntegrationType == Model.EIntegrationType.QCloud)
                {
                    redirectUrl = PageUtils.GetAdminDirectoryUrl("qcloud.aspx");
                }
            }

            //∑¿÷πcsrf£¨update by sessionliang at 20151214
            string token = base.Request.Form["t"];
            DESEncryptor encryptor = new DESEncryptor();
            encryptor.InputString = token;
            encryptor.DecryptKey = "csrf_filter_xxxx";
            encryptor.DesDecrypt();
            string outToken = encryptor.OutString;
            if (string.Equals(outToken, AdminManager.Current.UserName) && Request.HttpMethod.Equals("POST"))
            {
                AdminManager.Logout();
                PageUtils.Redirect(PageUtils.ParseNavigationUrl(redirectUrl));
            }
        }
    }
}
