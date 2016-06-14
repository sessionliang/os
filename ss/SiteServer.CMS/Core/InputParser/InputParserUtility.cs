using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CMS.Core
{
    public class InputParserUtility
    {
        private InputParserUtility()
        {
        }

        public static string GetImageOrFlashHtml(PublishmentSystemInfo publishmentSystemInfo, string imageUrl, StringDictionary attributes, bool isStlEntity)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrl);
                if (isStlEntity)
                {
                    retval = imageUrl;
                }
                else
                {
                    if (!imageUrl.ToUpper().Trim().EndsWith(".SWF"))
                    {
                        HtmlImage htmlImage = new HtmlImage();
                        ControlUtils.AddAttributesIfNotExists(htmlImage, attributes);
                        htmlImage.Src = imageUrl;
                        retval = ControlUtils.GetControlRenderHtml(htmlImage);
                    }
                    else
                    {
                        int width = 100;
                        int height = 100;
                        if (attributes != null)
                        {
                            if (!string.IsNullOrEmpty(attributes["width"]))
                            {
                                try
                                {
                                    width = int.Parse(attributes["width"]);
                                }
                                catch { }
                            }
                            if (!string.IsNullOrEmpty(attributes["height"]))
                            {
                                try
                                {
                                    height = int.Parse(attributes["height"]);
                                }
                                catch { }
                            }
                        }
                        retval = string.Format(@"
<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0"" width=""{0}"" height=""{1}"">
                <param name=""movie"" value=""{2}"">
                <param name=""quality"" value=""high"">
                <param name=""wmode"" value=""transparent"">
                <embed src=""{2}"" width=""{0}"" height=""{1}"" quality=""high"" pluginspage=""http://www.macromedia.com/go/getflashplayer"" type=""application/x-shockwave-flash"" wmode=""transparent""></embed></object>
", width, height, imageUrl);
                    }
                }
            }
            return retval;
        }

        public static string GetVideoHtml(PublishmentSystemInfo publishmentSystemInfo, string videoUrl, StringDictionary attributes, bool isStlEntity)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(videoUrl))
            {
                videoUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, videoUrl);
                if (isStlEntity)
                {
                    retval = videoUrl;
                }
                else
                {
                    retval = string.Format(@"
<embed src=""{0}"" allowfullscreen=""true"" flashvars=""controlbar=over&autostart={1}&image={2}&file={3}"" width=""{4}"" height=""{5}""/>
", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.BRPlayer.Swf), true.ToString().ToLower(), string.Empty, videoUrl, 450, 350);
                }
            }
            return retval;
        }

        public static string GetFileHtmlWithCount(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, string fileUrl, StringDictionary attributes, string innerXml, bool isStlEntity)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(fileUrl))
            {
                if (isStlEntity)
                {
                    retval = PageUtility.ServiceSTL.Utils.GetDownloadUrl(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, fileUrl);
                }
                else
                {
                    HtmlAnchor stlAnchor = new HtmlAnchor();
                    ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);
                    stlAnchor.HRef = PageUtility.ServiceSTL.Utils.GetDownloadUrl(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, fileUrl);
                    if (string.IsNullOrEmpty(innerXml))
                    {
                        stlAnchor.InnerHtml = PageUtils.GetFileNameFromUrl(fileUrl);
                    }
                    else
                    {
                        stlAnchor.InnerHtml = innerXml;
                    }

                    retval = ControlUtils.GetControlRenderHtml(stlAnchor);
                }
            }
            return retval;
        }

        public static string GetFileHtmlWithoutCount(PublishmentSystemInfo publishmentSystemInfo, string fileUrl, StringDictionary attributes, string innerXml, bool isStlEntity)
        {
            if (publishmentSystemInfo != null)
            {
                string retval = string.Empty;
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    if (isStlEntity)
                    {
                        retval = PageUtility.ServiceSTL.Utils.GetDownloadUrl(publishmentSystemInfo.PublishmentSystemID, fileUrl);
                    }
                    else
                    {
                        HtmlAnchor stlAnchor = new HtmlAnchor();
                        ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);
                        stlAnchor.HRef = PageUtility.ServiceSTL.Utils.GetDownloadUrl(publishmentSystemInfo.PublishmentSystemID, fileUrl);
                        if (string.IsNullOrEmpty(innerXml))
                        {
                            stlAnchor.InnerHtml = PageUtils.GetFileNameFromUrl(fileUrl);
                        }
                        else
                        {
                            stlAnchor.InnerHtml = innerXml;
                        }

                        retval = ControlUtils.GetControlRenderHtml(stlAnchor);
                    }
                }
                return retval;
            }
            return string.Empty;
        }
    }
}
