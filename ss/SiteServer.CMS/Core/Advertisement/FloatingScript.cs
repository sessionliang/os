using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core;
using System.Text;

namespace SiteServer.CMS.Core.Advertisement
{
	public class FloatingScript
	{
        private int uniqueID;
        private PublishmentSystemInfo publishmentSystemInfo;
		
		private string floatDivIsCloseableHtml;

        private AdvertisementInfo adInfo;
        private AdvertisementFloatImageInfo adFloatImageInfo;

        public FloatingScript(PublishmentSystemInfo publishmentSystemInfo, int uniqueID, AdvertisementInfo adInfo)
		{
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.adInfo = adInfo;
            this.adFloatImageInfo = new AdvertisementFloatImageInfo(this.adInfo.Settings);
            this.uniqueID = uniqueID;

            this.floatDivIsCloseableHtml = (adFloatImageInfo.IsCloseable) ? string.Format(@"<div style=""text-align:right; line-height:22px;""><a href=""javascript:;"" onclick=""document.getElementById('ad_{0}').style.display='none'"" style=""text-decoration:underline"">¹Ø±Õ<a></div>", this.uniqueID) : string.Empty;
		}

		public string GetScript()
		{
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"<div id=""ad_{0}"">", this.uniqueID);
            if (EFileSystemTypeUtils.IsFlash(PathUtils.GetExtension(adFloatImageInfo.ImageUrl)))
            {
                string height = (adFloatImageInfo.Height > 0) ? adFloatImageInfo.Height.ToString() : "100";
                string width = (adFloatImageInfo.Width > 0) ? adFloatImageInfo.Width.ToString() : "100";

                builder.AppendFormat(@"<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0' width='{0}' height='{1}'><param name='movie' value='{2}' /><param name='wmode' value='window' /><param name='scale' value='exactfit' /><param name='quality' value='high' /><embed src='{2}' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' type='application/x-shockwave-flash' width='{0}' height='{1}'></embed></object>{3}</div>", width, height, PageUtility.ParseNavigationUrl(this.publishmentSystemInfo, adFloatImageInfo.ImageUrl), this.floatDivIsCloseableHtml);
            }
            else
            {
                string floatDivSize = (adFloatImageInfo.Height > 0) ? string.Format(" HEIGHT={0}", adFloatImageInfo.Height) : "";
                floatDivSize += (adFloatImageInfo.Width > 0) ? string.Format(" WIDTH={0} ", adFloatImageInfo.Width) : "";

                if (string.IsNullOrEmpty(this.adFloatImageInfo.NavigationUrl))
                {
                    builder.AppendFormat(@"<img src=""{0}"" {1} border=""0""></a>{2}", PageUtility.ParseNavigationUrl(this.publishmentSystemInfo, adFloatImageInfo.ImageUrl), floatDivSize, this.floatDivIsCloseableHtml);
                }
                else
                {
                    builder.AppendFormat(@"<a href=""{0}"" target = ""_blank""><img src=""{1}"" {2} border=""0""></a>{3}", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(this.publishmentSystemInfo, this.adFloatImageInfo.NavigationUrl)), PageUtility.ParseNavigationUrl(this.publishmentSystemInfo, adFloatImageInfo.ImageUrl), floatDivSize, this.floatDivIsCloseableHtml);
                }
            }
            builder.Append("</div>");
            int type = 1;
            if (adFloatImageInfo.RollingType == ERollingType.FloatingInWindow)
            {
                type = 1;
            }
            else if (adFloatImageInfo.RollingType == ERollingType.FollowingScreen)
            {
                type = 2;
            }
            else if (adFloatImageInfo.RollingType == ERollingType.Static)
            {
                type = 3;
            }

            string positionX = string.Empty;
            string positionY = string.Empty;
            if (adFloatImageInfo.PositionType == EPositionType.LeftTop)
            {
                positionX = adFloatImageInfo.PositionX.ToString();
                positionY = adFloatImageInfo.PositionY.ToString();
            }
            else if (adFloatImageInfo.PositionType == EPositionType.LeftBottom)
            {
                positionX = adFloatImageInfo.PositionX.ToString();
                positionY = string.Format(@"document.body.scrollTop+document.body.offsetHeight-{0}-{1}", adFloatImageInfo.PositionY, adFloatImageInfo.Height);
            }
            else if (adFloatImageInfo.PositionType == EPositionType.RightTop)
            {
                positionX = string.Format(@"document.body.scrollLeft+document.body.offsetWidth-{0}-{1}", adFloatImageInfo.PositionX, adFloatImageInfo.Width);
                positionY = adFloatImageInfo.PositionY.ToString();
            }
            else if (adFloatImageInfo.PositionType == EPositionType.RightBottom)
            {
                positionX = string.Format(@"document.body.scrollLeft+document.body.offsetWidth-{0}-{1}", adFloatImageInfo.PositionX, adFloatImageInfo.Width);
                positionY = string.Format(@"document.body.scrollTop+document.body.offsetHeight-{0}-{1}", adFloatImageInfo.PositionY, adFloatImageInfo.Height);
            }

            string dateLimited = string.Empty;
            if (adInfo.IsDateLimited)
            {
                dateLimited = string.Format(@"
    var sDate{0} = new Date({1}, {2}, {3}, {4}, {5});
    var eDate{0} = new Date({6}, {7}, {8}, {9}, {10});
    ad{0}.SetDate(sDate{0}, eDate{0});
", this.uniqueID, adInfo.StartDate.Year, adInfo.StartDate.Month - 1, adInfo.StartDate.Day, adInfo.StartDate.Hour, adInfo.StartDate.Minute, adInfo.EndDate.Year, adInfo.EndDate.Month - 1, adInfo.EndDate.Day, adInfo.EndDate.Hour, adInfo.EndDate.Minute);
            }

            builder.AppendFormat(@"
<script type=""text/javascript"">
<!--
    var ad{0}=new Ad_Move(""ad_{0}"");
    ad{0}.SetLocation({1}, {2});
    ad{0}.SetType({3});{4}
    ad{0}.Run();
//-->
</script>
", this.uniqueID, positionX, positionY, type, dateLimited);

            return builder.ToString();
		}
	}
}
