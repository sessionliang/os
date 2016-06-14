using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSeoSiteMapBaidu : BackgroundBasePage
    {
        public TextBox SiteMapBaiduPath;
        public TextBox SiteMapBaiduUpdatePeri;
        public TextBox SiteMapBaiduWebMaster;

        public Literal ltlBaiduSiteMapUrl;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_SEO, "百度新闻地图", AppManager.CMS.Permission.WebSite.SEO);

                this.SiteMapBaiduPath.Text = base.PublishmentSystemInfo.Additional.SiteMapBaiduPath;
                this.SiteMapBaiduUpdatePeri.Text = base.PublishmentSystemInfo.Additional.SiteMapBaiduUpdatePeri;
                this.SiteMapBaiduWebMaster.Text = base.PublishmentSystemInfo.Additional.SiteMapBaiduWebMaster;

                this.ltlBaiduSiteMapUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.SiteMapBaiduPath)));
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                base.PublishmentSystemInfo.Additional.SiteMapBaiduPath = this.SiteMapBaiduPath.Text;
                base.PublishmentSystemInfo.Additional.SiteMapBaiduUpdatePeri = this.SiteMapBaiduUpdatePeri.Text;
                base.PublishmentSystemInfo.Additional.SiteMapBaiduWebMaster = this.SiteMapBaiduWebMaster.Text;

                try
                {
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    SeoManager.CreateSiteMapBaidu(base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.VisualType);
                    StringUtility.AddLog(base.PublishmentSystemID, "生成百度新闻地图");
                    base.SuccessMessage("百度新闻地图生成成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "百度新闻地图生成失败！");
                }
            }
        }
    }
}
