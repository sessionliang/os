using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSeoSiteMapGoogle : BackgroundBasePage
	{
		public TextBox SiteMapGooglePath;
		public DropDownList SiteMapGoogleChangeFrequency;
		public RadioButtonList SiteMapGoogleIsShowLastModified;
        public TextBox SiteMapGooglePageCount;

        public Literal ltlGoogleSiteMapUrl;
        public Literal ltlYahooSiteMapUrl;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_SEO, "谷歌应用地图", AppManager.CMS.Permission.WebSite.SEO);

                this.SiteMapGooglePath.Text = base.PublishmentSystemInfo.Additional.SiteMapGooglePath;

                this.SiteMapGoogleChangeFrequency.Items.Add(new ListItem("每天", "daily"));
                this.SiteMapGoogleChangeFrequency.Items.Add(new ListItem("每周", "weekly"));
                this.SiteMapGoogleChangeFrequency.Items.Add(new ListItem("每月", "monthly"));
                this.SiteMapGoogleChangeFrequency.Items.Add(new ListItem("每年", "yearly"));
                this.SiteMapGoogleChangeFrequency.Items.Add(new ListItem("永不改变", "never"));
                ControlUtils.SelectListItemsIgnoreCase(this.SiteMapGoogleChangeFrequency, base.PublishmentSystemInfo.Additional.SiteMapGoogleChangeFrequency);

                EBooleanUtils.AddListItems(this.SiteMapGoogleIsShowLastModified, "显示", "不显示");
                ControlUtils.SelectListItemsIgnoreCase(this.SiteMapGoogleIsShowLastModified, base.PublishmentSystemInfo.Additional.SiteMapGoogleIsShowLastModified.ToString());

                this.SiteMapGooglePageCount.Text = base.PublishmentSystemInfo.Additional.SiteMapGooglePageCount.ToString();

                this.ltlGoogleSiteMapUrl.Text = this.ltlYahooSiteMapUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{0}</a>", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.SiteMapGooglePath)));
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.SiteMapGooglePath = this.SiteMapGooglePath.Text;
                base.PublishmentSystemInfo.Additional.SiteMapGoogleChangeFrequency = this.SiteMapGoogleChangeFrequency.SelectedValue;
                base.PublishmentSystemInfo.Additional.SiteMapGoogleIsShowLastModified = TranslateUtils.ToBool(this.SiteMapGoogleIsShowLastModified.SelectedValue);
                base.PublishmentSystemInfo.Additional.SiteMapGooglePageCount = TranslateUtils.ToInt(this.SiteMapGooglePageCount.Text);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    SeoManager.CreateSiteMapGoogle(base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.VisualType);
                    StringUtility.AddLog(base.PublishmentSystemID, "生成谷歌应用地图");
					base.SuccessMessage("谷歌应用地图生成成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "谷歌应用地图生成失败！");
				}
			}
		}
	}
}
