using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections;
using BaiRong.Controls;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;
using BaiRong.Core.Integration;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundConfiguration : BackgroundBasePage
    {
        public Literal ltlIsDomain;
        public PlaceHolder phDomain;
        public TextBox tbPublishmentSystemUrl;

        public Literal ltlIsPoweredBy;
        public PlaceHolder phPoweredBy;
        public TextBox tbPoweredBy;

        public Literal ltlPublishmentSystemUrl;
        public Literal ltlRedirectJs;

        private bool isDomain = true;
        private DateTime domainExpDate = DateTime.Now.AddYears(1);
        private bool isPoweredBy = true;
        private DateTime poweredByExpDate = DateTime.Now.AddYears(1);

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "站点信息", AppManager.CMS.Permission.WebSite.Configration);

                this.GetStatus();

                if (!this.isDomain)
                {
                    this.phDomain.Visible = false;
                    if (FileConfigManager.Instance.OEMConfig.IsOEM)
                    {
                        ltlIsDomain.Text = @"尚未开通绑定域名服务，需要开通请与我们联系";
                    }
                    else
                    {
                        ltlIsDomain.Text = @"尚未开通绑定域名服务，需要开通请进入<a href=""http://shop.gexia.com/product/6/5.html"" target=""_blank"">阁下商城</a>购买";
                    }
                }
                else
                {
                    this.phDomain.Visible = true;
                    this.tbPublishmentSystemUrl.Text = base.PublishmentSystemInfo.PublishmentSystemUrl;
                }

                if (!this.isPoweredBy)
                {
                    this.phPoweredBy.Visible = false;
                    if (FileConfigManager.Instance.OEMConfig.IsOEM)
                    {
                        ltlIsPoweredBy.Text = @"尚未开通自定义版权服务，需要开通请与我们联系";
                    }
                    else
                    {
                        ltlIsPoweredBy.Text = @"尚未开通自定义版权服务，需要开通请进入<a href=""http://shop.gexia.com/product/5/4.html"" target=""_blank"">阁下商城</a>购买";
                    }
                }
                else
                {
                    this.phPoweredBy.Visible = true;
                    this.tbPoweredBy.Text = base.PublishmentSystemInfo.Additional.WX_PoweredBy;
                }

                string publishmentSystemUrl = PageUtils.AddProtocolToUrl(PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, string.Empty));

                this.ltlPublishmentSystemUrl.Text = string.Format(@"<a href=""{0}"" id=""url"" target=""_blank"">{0}</a>", publishmentSystemUrl);

                this.ltlRedirectJs.Text = string.Format(@"<p class=""well"">&lt;script src=""http://file.gexia.com/static/webappservice/uaredirect.js"" type=""text/javascript""&gt;&lt;/script&gt;&lt;script type=""text/javascript""&gt;uaredirect(""{0}"");&lt;/script&gt;</p>", publishmentSystemUrl);
            }
        }

        private void GetStatus()
        {
            this.isDomain = true;
            this.isPoweredBy = true;

            if (FileConfigManager.Instance.IsSaas)
            {
                this.isDomain = false;
                this.isPoweredBy = false;

                string token = IntegrationManager.GetIntegrationToken(AdminManager.Current.UserName);
                string errorMessage = string.Empty;
                //IntegrationCloudInfo cloudInfo = IntegrationManager.API_GEXIA_COM.GetIntegrationCloudInfo(token, out errorMessage);
                //foreach (AuthPublishmentSystemInfo authPublishmentSystemInfo in cloudInfo.PublishmentSystemInfoList)
                //{
                //    if (base.PublishmentSystemID == authPublishmentSystemInfo.PublishmentSystemID)
                //    {
                //        this.domainExpDate = authPublishmentSystemInfo.DomainExpDate;
                //        this.isDomain = authPublishmentSystemInfo.IsDomain && this.domainExpDate > DateTime.Now;
                //        this.poweredByExpDate = authPublishmentSystemInfo.PoweredByExpDate;
                //        this.isPoweredBy = authPublishmentSystemInfo.IsPoweredBy && this.poweredByExpDate > DateTime.Now;
                //        break;
                //    }
                //}
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.phDomain.Visible)
                {
                    base.PublishmentSystemInfo.PublishmentSystemUrl = this.tbPublishmentSystemUrl.Text;
                }
                if (this.phPoweredBy.Visible)
                {
                    base.PublishmentSystemInfo.Additional.WX_IsPoweredBy = true;
                    base.PublishmentSystemInfo.Additional.WX_PoweredBy = this.tbPoweredBy.Text;
                }

                try
                {
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    base.SuccessMessage("站点信息设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "站点信息设置修改失败！");
                }
            }
        }
    }
}
