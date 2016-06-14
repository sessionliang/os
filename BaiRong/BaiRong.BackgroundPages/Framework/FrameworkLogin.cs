using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI;

using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

using BaiRong.Core.Diagnostics;

namespace BaiRong.BackgroundPages
{
    public class FrameworkLogin : BackgroundBasePage
	{
        //protected Literal ltlHeadLinks;
        public Literal ltlLogo;
        protected Literal ltlMessage;
		protected TextBox UserName;
		protected TextBox Password;
        protected PlaceHolder phValidateCode;
        protected TextBox ValidateCode;
        protected Literal ValidateCodeImage;
        protected CheckBox RememberMe;
        //protected Literal Version;
        //protected Literal NetVersion;
        //protected Literal Database;
        //protected Literal CompanyName;
        protected Literal ltlScript;

        VCManager vcManager;

        protected override bool IsAccessable
        {
            get { return true; }
        }
	
		public void Page_Load(object sender, System.EventArgs e)
		{
            if (base.IsForbidden) return;

            if (FileConfigManager.Instance.IsSaas && FileConfigManager.Instance.OEMConfig.IsOEM)
            {
                PageUtils.Redirect("http://" + base.Request.Url.Host);
                return;
            }

            try
            {
                this.vcManager = VCManager.GetInstanceOfAdminLogin();
                if (!Page.IsPostBack)
                {
                    if (!string.IsNullOrEmpty(base.GetQueryString("error")))
                    {
                        this.ltlMessage.Text = this.GetMessageHtml(base.GetQueryString("error"));
                    }
                    if (RestrictionManager.IsVisitAllowed(ConfigManager.Additional.RestrictionType, ConfigManager.Instance.RestrictionBlackList, ConfigManager.Instance.RestrictionWhiteList))
                    {
                        PageUtils.DetermineRedirectToInstaller();

                        //if (FileConfigManager.Instance.OEMConfig.IsOEM)
                        //{
                        //    this.ltlHeadLinks.Text = string.Format(@"<a href=""{0}"" target=""_blank"">公司网站</a>", FileConfigManager.Instance.OEMConfig.CompanyUrl);
                        //}
                        //else
                        //{
                        //    this.ltlHeadLinks.Text = @"<a href=""http://www.siteserver.cn"" target=""_blank"">产品网站</a>&nbsp;-&nbsp;<a href=""http://help.siteserver.cn"" target=""_blank"">系统帮助</a>";
                        //}

                        if (FileConfigManager.Instance.IsValidateCode)
                        {
                            this.ValidateCodeImage.Text = string.Format(@"<img id=""imgVerify"" name=""imgVerify"" src=""{0}"" align=""absmiddle"" />", this.vcManager.GetImageUrl(false));
                        }
                        else
                        {
                            this.phValidateCode.Visible = false;
                        }
                        //this.Version.Text = ProductManager.GetFullVersion();
                        //this.NetVersion.Text = string.Format("{0}.{1}", System.Environment.Version.Major, System.Environment.Version.Minor);
                        //this.Database.Text = EDatabaseTypeUtils.GetText(BaiRongDataProvider.DatabaseType);
                        //this.CompanyName.Text = FileConfigManager.Instance.OEMConfig.CompanyName;

                        if (FileConfigManager.Instance.IsSaas)
                        {
                            //this.ltlLogo.Text = string.Format(@"<a class=""yunLogo"" href=""http://www.siteyun.com""><img src=""pic/login/siteyun.png"" /></a>");
                            this.ltlLogo.Text = string.Format(@"<a class=""yunLogo"" href=""http://www.gexia.com""><img src=""pic/login/gexia.png"" /></a>");
                        }
                        else
                        {
                            this.ltlLogo.Text = string.Format(@"<a class=""yunLogo"" href=""http://www.siteserver.cn""><img src=""pic/login/siteserver.png"" /></a>");
                        }                        

                        this.ltlScript.Text = StringUtils.Constants.GetUpdateSiteYunStateScript();

                        //if (ProductManager.IsNeedUpgrade())
                        //{
                        //    this.ltlUpdate.Text += string.Format(@"<div class=""input_fpwd"" style=""color:red"">系统检测出数据库需要升级，请 <a href=""upgrade/default.aspx""> 点击这里 </a> 进行升级。</div>");
                        //}
                    }
                    else
                    {
                        Page.Response.Write("<h1>此页面禁止访问.</h1>");
                        Page.Response.Write(string.Format("<p>IP地址：{0}<br />需要访问此页面请与网站管理员联系开通相关权限.</p>", PageUtils.GetIPAddress()));
                        Page.Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ProductManager.IsNeedInstall())
                {
                    PageUtils.Redirect("installer/default.aspx");
                    return;
                }
                else if (ProductManager.IsNeedUpgrade())
                {
                    PageUtils.Redirect("upgrade/default.aspx");
                    return;
                }
                else
                {
                    throw ex;
                }
            }
		}

        //protected string GetProductName()
        //{
        //    return FileConfigManager.Instance.OEMConfig.ProductName;
        //}

        //protected string GetProductUrl()
        //{
        //    return FileConfigManager.Instance.OEMConfig.ProductUrl;
        //}
		
		public override void Submit_OnClick(object sender, System.EventArgs e)
		{
            string userName = UserName.Text;
            string password = Password.Text;

            if (FileConfigManager.Instance.IsValidateCode)
            {
                if (!this.vcManager.IsCodeValid(this.ValidateCode.Text))
                {
                    this.ltlMessage.Text = this.GetMessageHtml("验证码不正确，请重新输入！");
                    return;
                }
            }

            string errorMessage = string.Empty;
            if (!AdminManager.Authticate(userName, password, out errorMessage))
            {
                this.ltlMessage.Text = this.GetMessageHtml(errorMessage);
            }
            else
            {
                BaiRongDataProvider.AdministratorDAO.RedirectFromLoginPage(userName, this.RememberMe.Checked);
            }
		}

        private string GetMessageHtml(string message)
        {
            return string.Format(@"<div class=""alert"">{0}</div>", message);
        }

        protected void ibSubmit_OnClick(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.Submit_OnClick(sender, EventArgs.Empty);
        }
	}
}
