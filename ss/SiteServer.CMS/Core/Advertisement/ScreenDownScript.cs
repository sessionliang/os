using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Core.Advertisement
{
	public class ScreenDownScript
	{
        private PublishmentSystemInfo publishmentSystemInfo;
        private int uniqueID;

        private readonly AdvertisementScreenDownInfo adScreenDownInfo;

        public ScreenDownScript(PublishmentSystemInfo publishmentSystemInfo, int uniqueID, AdvertisementInfo adInfo)
		{
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.uniqueID = uniqueID;

            this.adScreenDownInfo = new AdvertisementScreenDownInfo(adInfo.Settings);
		}

		public string GetScript()
		{
            string sizeString = (this.adScreenDownInfo.Width > 0) ? string.Format("width={0} ", this.adScreenDownInfo.Width) : string.Empty;
            sizeString += (this.adScreenDownInfo.Height > 0) ? string.Format("height={0}", this.adScreenDownInfo.Height) : string.Empty;

            return string.Format(@"
<script language=""javascript"" type=""text/javascript"">
function ad_changediv(){{
    jQuery('#ad_hiddenLayer_{0}').slideDown();
    setTimeout(""ad_hidediv()"",{1}000);
}}
function ad_hidediv(){{
    jQuery('#ad_hiddenLayer_{0}').slideUp();
}}
jQuery(document).ready(function(){{
    jQuery('body').prepend('<div id=""ad_hiddenLayer_{0}"" style=""display: none;""><center><a href=""{2}"" target=""_blank""><img src=""{3}"" {4} border=""0"" /></a></center></div>');
    setTimeout(""ad_changediv()"",2000);
}});
</script>
", this.uniqueID, this.adScreenDownInfo.Delay, PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(this.publishmentSystemInfo, this.adScreenDownInfo.NavigationUrl)), PageUtility.ParseNavigationUrl(this.publishmentSystemInfo, this.adScreenDownInfo.ImageUrl), sizeString);
		}
	}
}
