using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Office;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class Message : BackgroundBasePage
    {
        public Literal ltlHtml;

        private string type;

        public const string TYPE_PreviewImage = "PreviewImage";
        public const string TYPE_PreviewVideo = "PreviewVideo";
        public const string TYPE_PreviewVideoByUrl = "PreviewVideoByUrl";

        protected override bool IsSinglePage { get { return true; } }

        public static string GetRedirectUrlString(string html)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_message.aspx?html={0}", RuntimeUtils.EncryptStringByTranslate(html)));
        }

        public static string GetOpenWindowString(string title, string html, int width, int height)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("html", RuntimeUtils.EncryptStringByTranslate(html));
            return PageUtility.GetOpenWindowString(title, "modal_message.aspx", arguments, width, height, true);
        }

        public static string GetOpenWindowStringToPreviewImage(int publishmentSystemID, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("type", TYPE_PreviewImage);
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("textBoxClientID", textBoxClientID);
            return PageUtility.GetOpenWindowString("‘§¿¿Õº∆¨", "modal_message.aspx", arguments, 500, 500, true);
        }

        public static string GetOpenWindowStringToPreviewVideo(int publishmentSystemID, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("type", TYPE_PreviewVideo);
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("textBoxClientID", textBoxClientID);
            return PageUtility.GetOpenWindowString("‘§¿¿ ”∆µ", "modal_message.aspx", arguments, 500, 500, true);
        }

        public static string GetOpenWindowStringToPreviewVideoByUrl(int publishmentSystemID, string videoUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("type", TYPE_PreviewVideoByUrl);
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("videoUrl", videoUrl);
            return PageUtility.GetOpenWindowString("‘§¿¿ ”∆µ", "modal_message.aspx", arguments, 500, 500, true);
        }

        public static string GetRedirectStringToPreviewVideoByUrl(int publishmentSystemID, string videoUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("type", TYPE_PreviewVideoByUrl);
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("videoUrl", videoUrl);
            return PageUtils.AddQueryString("modal_message.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.type = base.Request.QueryString["type"];

            if (!IsPostBack)
            {
                if (StringUtils.EqualsIgnoreCase(this.type, TYPE_PreviewImage))
                {
                    int publishmentSystemID = TranslateUtils.ToInt(base.GetQueryString("publishmentSystemID"));
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    string textBoxClientID = base.GetQueryString("textBoxClientID");
                    this.ltlHtml.Text = string.Format(@"
<span id=""previewImage""></span>
<script>
var rootUrl = '{0}';
var publishmentSystemUrl = '{1}';
var imageUrl = window.parent.document.getElementById('{2}').value;
if(imageUrl && imageUrl.search(/\.bmp|\.jpg|\.jpeg|\.gif|\.png$/i) != -1){{
	if (imageUrl.charAt(0) == '~'){{
		imageUrl = imageUrl.replace('~', rootUrl);
	}}else if (imageUrl.charAt(0) == '@'){{
		imageUrl = imageUrl.replace('@', publishmentSystemUrl);
	}}
	if(imageUrl.substr(0,2)=='//'){{
		imageUrl = imageUrl.replace('//', '/');
	}}
    $('#previewImage').html('<img src=""' + imageUrl + '"" class=""img-polaroid"" />');
}}
</script>
", PageUtils.GetRootUrl(string.Empty), PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, string.Empty, true), textBoxClientID);
                }
                else if (StringUtils.EqualsIgnoreCase(this.type, TYPE_PreviewVideo))
                {
                    int publishmentSystemID = TranslateUtils.ToInt(base.GetQueryString("publishmentSystemID"));
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    string textBoxClientID = base.GetQueryString("textBoxClientID");

                    this.ltlHtml.Text = string.Format(@"
<span id=""previewVideo""></span>
<script>
var rootUrl = '{0}';
var publishmentSystemUrl = '{1}';
var videoUrl = window.parent.document.getElementById('{2}').value;
if (videoUrl.charAt(0) == '~'){{
	videoUrl = videoUrl.replace('~', rootUrl);
}}else if (videoUrl.charAt(0) == '@'){{
	videoUrl = videoUrl.replace('@', publishmentSystemUrl);
}}
if(videoUrl.substr(0,2)=='//'){{
	videoUrl = videoUrl.replace('//', '/');
}}
if (videoUrl){{
    $('#previewVideo').html('<embed src=""{3}"" allowfullscreen=""true"" flashvars=""controlbar=over&autostart=true&file='+videoUrl+'"" width=""{4}"" height=""{5}""/>');
}}
</script>
", PageUtils.GetRootUrl(string.Empty), PageUtility.GetPublishmentSystemUrl(publishmentSystemInfo, string.Empty, true), textBoxClientID, PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.BRPlayer.Swf), 450, 350);
                }
                else if (StringUtils.EqualsIgnoreCase(this.type, TYPE_PreviewVideoByUrl))
                {
                    int publishmentSystemID = TranslateUtils.ToInt(base.GetQueryString("publishmentSystemID"));
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    string videoUrl = base.GetQueryString("videoUrl");

                    this.ltlHtml.Text = string.Format(@"
<embed src=""{0}"" allowfullscreen=""true"" flashvars=""controlbar=over&autostart=true&file={1}"" width=""{2}"" height=""{3}""/>
", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.BRPlayer.Swf), PageUtility.ParseNavigationUrl(publishmentSystemInfo, videoUrl), 450, 350);
                }
                else
                {
                    this.ltlHtml.Text = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["html"]);
                }
            }
        }
    }
}
