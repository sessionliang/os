using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using BaiRong.Core.Net;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class ConsoleAccountBinding : BackgroundBasePage
    {
        public PlaceHolder phStep1;
        public RadioButtonList rblWXAccountType;
        public TextBox tbWhchatID;

        public PlaceHolder phStep2;
        public Literal ltlURL;
        public Literal ltlToken;

        public PlaceHolder phStep3;
        public TextBox tbAppID;
        public TextBox tbAppSecret;

        private string returnUrl;

        public static string GetRedirectUrl(int publishmentSystemID, string returnUrl)
        {
            return PageUtils.GetWXUrl(string.Format("console_accountBinding.aspx?publishmentSystemID={0}&returnUrl={1}", publishmentSystemID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("returnUrl"));

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "绑定微信公众帐号", AppManager.Platform.Permission.Platform_Site);


                #region 确认绑定接口没有问题
                if (!IsNet45OrNewer())
                {
                    base.FailMessage("请检查是否安装了.NET Framework 4.5以上版本");
                    return;
                }

                string hostUrl = PageUtils.GetHost();
                if (hostUrl.IndexOf(":") >= 0)
                {
                    string port = hostUrl.Split(new char[] { ':' })[1];
                    if (port != "80")
                    {
                        base.FailMessage("请检查站点是否设置为80端口");
                        return;
                    }
                }
                
                string testUrl = PageUtils.AddProtocolToUrl("/api/mp/url?id=1");
                string result = string.Empty;
                WebClientUtils.Post(testUrl, string.Empty, out result);
                if (!StringUtils.EqualsIgnoreCase(result, "failed:id=1") && !StringUtils.EqualsIgnoreCase(result, "参数错误"))
                {
                    base.FailMessage("绑定微信公众账号需要的api有问题! 详细错误信息：" + result);
                    return;
                }


                #endregion

                AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                EWXAccountTypeUtils.AddListItems(this.rblWXAccountType);
                ControlUtils.SelectListItems(this.rblWXAccountType, EWXAccountTypeUtils.GetValue(EWXAccountTypeUtils.GetEnumType(accountInfo.AccountType)));

                this.tbWhchatID.Text = accountInfo.WeChatID;

                this.ltlURL.Text = PageUtilityWX.API.GetMPUrl(base.PublishmentSystemID);

                this.ltlToken.Text = accountInfo.Token;

                this.tbAppID.Text = accountInfo.AppID;
                this.tbAppSecret.Text = accountInfo.AppSecret;
            }
        }

        public static bool IsNet45OrNewer()
        {
            // Class "ReflectionContext" exists from .NET 4.5 onwards.
            return Type.GetType("System.Reflection.ReflectionContext", false) != null;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.phStep1.Visible)
                {
                    this.phStep2.Visible = true;
                    this.phStep3.Visible = this.phStep1.Visible = false;
                }
                else if (this.phStep2.Visible)
                {
                    EWXAccountType accountType = EWXAccountTypeUtils.GetEnumType(this.rblWXAccountType.SelectedValue);
                    if (accountType == EWXAccountType.Subscribe)
                    {
                        AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                        accountInfo.AccountType = this.rblWXAccountType.SelectedValue;
                        accountInfo.WeChatID = this.tbWhchatID.Text;

                        try
                        {
                            DataProviderWX.AccountDAO.Update(accountInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "绑定微信公众帐号", string.Format("应用:{0}", base.PublishmentSystemInfo.PublishmentSystemName));

                            base.SuccessMessage("绑定微信公众帐号成功！");
                            base.AddWaitAndRedirectScript(this.returnUrl);
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "绑定微信公众帐号失败！");
                        }
                    }
                    else
                    {
                        this.phStep3.Visible = true;
                        this.phStep1.Visible = this.phStep2.Visible = false;
                    }
                }
                else
                {
                    AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                    accountInfo.AccountType = this.rblWXAccountType.SelectedValue;
                    accountInfo.WeChatID = this.tbWhchatID.Text;
                    accountInfo.AppID = this.tbAppID.Text;
                    accountInfo.AppSecret = this.tbAppSecret.Text;

                    try
                    {
                        DataProviderWX.AccountDAO.Update(accountInfo);

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "绑定微信公众帐号", string.Format("应用:{0}", base.PublishmentSystemInfo.PublishmentSystemName));

                        base.SuccessMessage("绑定微信公众帐号成功！");
                        base.AddWaitAndRedirectScript(this.returnUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "绑定微信公众帐号失败！");
                    }
                }
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }
    }
}
