using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Core.Advertisement
{
	public class OpenWindowScript
	{
        private PublishmentSystemInfo publishmentSystemInfo;
        private int uniqueID;
        private AdvertisementOpenWindowInfo adOpenWindowInfo;

        public OpenWindowScript(PublishmentSystemInfo publishmentSystemInfo, int uniqueID, AdvertisementInfo adInfo)
		{
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.uniqueID = uniqueID;
            this.adOpenWindowInfo = new AdvertisementOpenWindowInfo(adInfo.Settings);
		}

		public string GetScript()
		{
            string sizeString = (this.adOpenWindowInfo.Width > 0) ? string.Format(",width={0}", this.adOpenWindowInfo.Width) : string.Empty;
            sizeString += (this.adOpenWindowInfo.Height > 0) ? string.Format(",height={0} ", this.adOpenWindowInfo.Height) : string.Empty;

            return string.Format(@"
<script language=""javascript"" type=""text/javascript"">
function ad_open_win_{0}() {{
	var popUpWin{0} = open(""{1}"", (window.name!=""popUpWin{0}"")?""popUpWin{0}"":"""", ""toolbar=no,location=no,directories=no,resizable=no,copyhistory=yes{2}"");
}}
try{{
	setTimeout(""ad_open_win_{0}();"",50);
}}catch(e){{}}
</script>
", this.uniqueID, PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(this.publishmentSystemInfo, this.adOpenWindowInfo.FileUrl)), sizeString);
		}
	}
}
