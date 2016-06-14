using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

using BaiRong.Model;

using BaiRong.Core.Net;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;

namespace BaiRong.BackgroundPages
{
    public class FrameworkInitialization : BackgroundBasePage
	{
        public Literal ltlContent;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!PageUtils.DetermineRedirectToInstaller())
            {
                if (!BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    PageUtils.RedirectToLoginPage();
                    return;
                }

                if (AdminManager.Current.IsLockedOut)
                {
                    PageUtils.RedirectToLoginPage("对不起，您的账号已被锁定，无法进入系统！");
                    return;
                }

                if (!string.IsNullOrEmpty(ConfigManager.Additional.SiteYun_RedirectUrl) && AdminManager.Current.UserName != AdminManager.SysUserName)
                {
                    //验证跳转地址
                    try
                    {
                        bool isSiteCreated = BaiRongDataProvider.ConfigDAO.GetSiteCount() > 0;
                        string jsonString = WebClientUtils.GetRemoteFileSource(StringUtils.Constants.GetValidateUrl(false, isSiteCreated), ECharset.utf_8);
                        NameValueCollection attributes = TranslateUtils.ParseJsonStringToNameValueCollection(jsonString);
                        if (TranslateUtils.ToBool(attributes["success"]))
                        {
                            if (attributes["redirectUrl"] == ConfigManager.Additional.SiteYun_RedirectUrl)
                            {
                                PageUtils.Redirect(ConfigManager.Additional.SiteYun_RedirectUrl);
                                return;
                            }
                        }
                    }
                    catch { }
                }

                string url = string.Empty;

                List<string> productIDList = RoleManager.GetProductIDList(PermissionsManager.Current.Roles);
                if (!productIDList.Contains(ProductManager.Apps.ProductID) && ProductFileUtils.IsExists(ProductManager.Apps.ProductID))
                {
                    productIDList.Add(ProductManager.Apps.ProductID);
                }

                if (productIDList.Count == 1)
                {
                    string productID = productIDList[0];
                    if (StringUtils.EqualsIgnoreCase(productID, ProductManager.Apps.ProductID))
                    {
                        url = PageUtils.GetCMSUrl("background_initialization.aspx");
                    }
                    else
                    {
                        url = "productMain.aspx?productID=" + productID;
                    }

                    this.ltlContent.Text = string.Format(@"
<img src=""pic/animated_loading.gif"" align=""absmiddle"">
&nbsp;正在加载数据，请稍候...
<script language=""javascript"">
function redirectUrl()
{{
   location.href = ""{0}"";
}}
setTimeout(""redirectUrl()"", 2000);
</script>
", url);
                }
                else
                {
                    string lastProductID = AdminManager.Current.LastProductID;
                    if (!string.IsNullOrEmpty(lastProductID))
                    {
                        if (!ProductManager.IsProductExists(lastProductID))
                        {
                            lastProductID = ProductManager.Apps.ProductID;
                        }
                    }
                    if (string.IsNullOrEmpty(lastProductID) || !productIDList.Contains(lastProductID.ToLower()))
                    {
                        lastProductID = ProductManager.Apps.ProductID;
                    }

                    if (string.IsNullOrEmpty(lastProductID) || StringUtils.EqualsIgnoreCase(lastProductID, ProductManager.Apps.ProductID))
                    {
                        url = PageUtils.GetCMSUrl("background_initialization.aspx");
                    }
                    else
                    {
                        url = "productMain.aspx?productID=" + lastProductID;
                    }
                    this.ltlContent.Text = string.Format(@"
<img src=""pic/animated_loading.gif"" align=""absmiddle"">
&nbsp;正在加载数据，请稍候...
<script language=""javascript"">
function redirectUrl()
{{
   location.href = ""{0}"";
}}
setTimeout(""redirectUrl()"", 2000);
</script>
", url);
                }
            }
        }
	}
}
