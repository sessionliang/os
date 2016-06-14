using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Diagnostics;
using BaiRong.Model;

using System;
using System.Web.UI.WebControls;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundPreview : BackgroundBasePage
	{
        public Literal ltlContent;

        private string pageUrl;
        private string deviceWidth;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetRedirectUrlToMobile(string pageUrl)
        {
            return GetRedirectUrl(pageUrl, "320px");
        }

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_preview.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        private static string GetRedirectUrl(string pageUrl, string deviceWidth)
        {
            return PageUtils.GetCMSUrl(string.Format("background_preview.aspx?deviceWidth={0}&pageUrl={1}", deviceWidth, StringUtils.ValueToUrl(pageUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.deviceWidth = base.GetQueryString("deviceWidth");
            this.pageUrl = StringUtils.ValueFromUrl(base.GetQueryString("pageUrl"));

            if (string.IsNullOrEmpty(this.pageUrl))
            {
                if (base.PublishmentSystemID > 0)
                {
                    if (EPublishmentSystemTypeUtils.IsMobile(base.PublishmentSystemInfo.PublishmentSystemType))
                    {
                        this.deviceWidth = "320px";
                    }
                    this.pageUrl = PageUtility.GetIndexPageUrl(base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.VisualType);
                }
            }
            
            if (!IsPostBack)
            {
                this.ltlContent.Text = string.Format(@"
<iframe style=""width:100%;height:100%;background-color:#ffffff;margin-bottom:15px;"" scrolling=""auto"" frameborder=""0"" width=""100%"" height=""100%"" src=""{0}""></iframe>
<script type=""text/javascript"" language=""javascript"">
$(function(){{
    $('#qrCode').qrcode({{width: 200, height:200, text: ""{0}""}});
    $(""a[device-width='{1}']"").click();
}});
</script>
", PageUtils.AddProtocolToUrl(PageUtils.AddQueryString(this.pageUrl, "_r", StringUtils.GetRandomInt(1000, 10000).ToString())), this.deviceWidth);
            }
        }
	}
}
