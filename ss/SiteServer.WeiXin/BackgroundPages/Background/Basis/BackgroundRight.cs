using System.Web.UI.WebControls;
using BaiRong.Core;
using System;


using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Text;
using BaiRong.Core.WebService;
using BaiRong.Core.Net;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundRight : BackgroundBasePageWX
    {
        public Literal ltlWelcome;
        public Literal ltlBinding;
        public Literal ltlDelete;

        public Literal ltlURL;
        public Literal ltlToken;

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Accounts, AppManager.WeiXin.LeftMenu.Function.ID_Info, string.Empty, AppManager.WeiXin.Permission.WebSite.Info);
                if (FileConfigManager.Instance.IsSaas)
                {
                    if (FileConfigManager.Instance.OEMConfig.IsOEM)
                    {
                        this.ltlWelcome.Text = "欢迎使用 微信公众号管理平台";
                    }
                    else
                    {
                        this.ltlWelcome.Text = "欢迎使用 GEXIA 阁下微信平台";
                    }
                }
                else
                {
                    this.ltlWelcome.Text = "欢迎使用 SiteServer WeiXin 微信平台";
                }

                string bindingUrl = ConsoleAccountBinding.GetRedirectUrl(base.PublishmentSystemID, PageUtils.GetWXUrl(string.Format("background_right.aspx?publishmentSystemID={0}", base.PublishmentSystemID)));

                AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                bool isBinding = WeiXinManager.IsBinding(accountInfo);
                if (isBinding)
                {
                    ltlBinding.Text = string.Format(@"<a href=""{0}"" class=""btn btn-success"">已绑定微信</a>", bindingUrl);
                }
                else
                {
                    ltlBinding.Text = string.Format(@"<a href=""{0}"" class=""btn btn-danger"">未绑定微信</a>", bindingUrl);
                }

                this.ltlURL.Text = PageUtilityWX.API.GetMPUrl(base.PublishmentSystemID);

                this.ltlToken.Text = accountInfo.Token;

                if (!FileConfigManager.Instance.IsSaasQCloud)
                {
                    string deleteUrl = PageUtils.GetSTLUrl(string.Format("console_publishmentSystemDelete.aspx?nodeID={0}&isBackgroundDelete=true", base.PublishmentSystemID));
                    this.ltlDelete.Text = string.Format(@"<a href=""{0}"" class=""btn btn-danger"">删除当前应用</a>", deleteUrl);
                }
            }
        }
    }
}
