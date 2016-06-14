using System.Text;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using BaiRong.Core.Data.Provider;

using System;
using BaiRong.Model;
using System.Reflection;

namespace SiteServer.CMS.Core
{
    public class StringUtility
    {
        private StringUtility()
        {
        }

        //        public static string GetRedirectPageHtml(string href)
        //        {
        //            return string.Format(@"
        //<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
        //<html xmlns=""http://www.w3.org/1999/xhtml"">
        //<head>
        //  <META HTTP-EQUIV=""Pragma"" CONTENT=""no-cache"">   
        //  <META HTTP-EQUIV=""Cache-Control"" CONTENT=""no-cache"">   
        //  <META HTTP-EQUIV=""Expires"" CONTENT=""0"">   
        //  <title></title>
        //  <script type=""text/javascript"" language=""javascript"">
        //	location.href = ""{0}"";
        //  </script>
        //</head>
        //<body>
        //</body>
        //</html>
        //", href);
        //        }

        public static string GetHelpHtml(string text, string helpText)
        {
            if (string.IsNullOrEmpty(helpText)) helpText = text;
            string html = string.Format(@"{0}：", text);
            return html;
        }

        /// <summary>
        /// 得到系统图片的路径
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static string GetSystemImageSrc(string imageName, string fileType)
        {
            return string.Format("../pic/System/{0}{1}", imageName, fileType);
        }

        /// <summary>
        /// 得到GIF格式的系统图片路径
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public static string GetSystemImageSrc(string imageName)
        {
            return GetSystemImageSrc(imageName, ".gif");
        }

        public static string TextEditorContentEncode(string content, PublishmentSystemInfo publishmentSystemInfo)
        {
            return TextEditorContentEncode(content, publishmentSystemInfo, publishmentSystemInfo.Additional.IsSaveImageInTextEditor);
        }

        public static string TextEditorContentEncode(string content, PublishmentSystemInfo publishmentSystemInfo, bool isSaveImage)
        {
            string publishmentSystemUrl = string.Empty;
            if (publishmentSystemInfo != null)
            {
                publishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl;
                if (isSaveImage && !string.IsNullOrEmpty(content))
                {
                    content = PathUtility.SaveImage(publishmentSystemInfo, content);
                }
            }

            StringBuilder builder = new StringBuilder(content);

            //内容保存之前，把EditorUploadFilePre替换为@符号
            string editorUploadFilePre = publishmentSystemInfo.Additional.EditorUploadFilePre;
            if (!string.IsNullOrEmpty(editorUploadFilePre))
            {
                builder.Replace("href=\"" + editorUploadFilePre + "/upload", "href=\"@/upload");
                builder.Replace("href='" + editorUploadFilePre + "/upload", "href='@/upload");
                builder.Replace("href=" + editorUploadFilePre + "/upload", "href=@/upload");
                builder.Replace("href=&quot;" + editorUploadFilePre + "/upload", "href=&quot;@/upload");
                builder.Replace("src=\"" + editorUploadFilePre + "/upload", "src=\"@/upload");
                builder.Replace("src='" + editorUploadFilePre + "/upload", "src='@/upload");
                builder.Replace("src=" + editorUploadFilePre + "/upload", "src=@/upload");
                builder.Replace("src=&quot;" + editorUploadFilePre + "/upload", "src=&quot;@/upload");
            }

            if (publishmentSystemUrl == "/")
            {
                publishmentSystemUrl = string.Empty;
            }

            builder.Replace("href=\"" + publishmentSystemUrl, "href=\"@");
            builder.Replace("href='" + publishmentSystemUrl, "href='@");
            builder.Replace("href=" + publishmentSystemUrl, "href=@");
            builder.Replace("href=&quot;" + publishmentSystemUrl, "href=&quot;@");
            builder.Replace("src=\"" + publishmentSystemUrl, "src=\"@");
            builder.Replace("src='" + publishmentSystemUrl, "src='@");
            builder.Replace("src=" + publishmentSystemUrl, "src=@");
            builder.Replace("src=&quot;" + publishmentSystemUrl, "src=&quot;@");

            builder.Replace("@'@", "'@");
            builder.Replace("@\"@", "\"@");

            return builder.ToString();
        }

        /// <summary>
        /// 获取编辑器中内容，解析@符号，添加了远程路径处理 20151103
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="isFromBack">是否是后台请求</param>
        /// <returns></returns>
        public static string TextEditorContentDecode(string content, PublishmentSystemInfo publishmentSystemInfo, bool isFromBack)
        {
            string publishmentSystemUrl;
            if (publishmentSystemInfo != null)
            {
                publishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl;
            }
            else
            {
                publishmentSystemUrl = ConfigUtils.Instance.ApplicationPath;
            }
            return TextEditorContentDecode(content, publishmentSystemUrl, publishmentSystemInfo.Additional.EditorUploadFilePre, isFromBack);
        }

        /// <summary>
        /// 获取编辑器中内容，解析@符号，添加了远程路径处理 20151103
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publishmentSystemInfo"></param>
        /// <returns></returns>
        public static string TextEditorContentDecode(string content, PublishmentSystemInfo publishmentSystemInfo)
        {
            return TextEditorContentDecode(content, publishmentSystemInfo, false);
        }

        public static string TextEditorContentDecode(string content, string publishmentSystemUrl, string editorUploadFilePre, bool isFromBack)
        {
            StringBuilder builder = new StringBuilder(content);

            if (publishmentSystemUrl == "/")
            {
                publishmentSystemUrl = string.Empty;
            }

            if (!isFromBack && !string.IsNullOrEmpty(editorUploadFilePre))
            {
                builder.Replace("href=\"@/upload", "href=\"" + editorUploadFilePre + "/upload");
                builder.Replace("href='@/upload", "href='" + editorUploadFilePre + "/upload");
                builder.Replace("href=@/upload", "href=" + editorUploadFilePre + "/upload");
                builder.Replace("href=&quot;@/upload", "href=&quot;" + editorUploadFilePre + "/upload");
                builder.Replace("src=\"@/upload", "src=\"" + editorUploadFilePre + "/upload");
                builder.Replace("src='@/upload", "src='" + editorUploadFilePre + "/upload");
                builder.Replace("src=@/upload", "src=" + editorUploadFilePre + "/upload");
                builder.Replace("src=&quot;@/upload", "src=&quot;" + editorUploadFilePre + "/upload");
            }

            builder.Replace("href=\"@", "href=\"" + publishmentSystemUrl);
            builder.Replace("href='@", "href='" + publishmentSystemUrl);
            builder.Replace("href=@", "href=" + publishmentSystemUrl);
            builder.Replace("href=&quot;@", "href=&quot;" + publishmentSystemUrl);
            builder.Replace("src=\"@", "src=\"" + publishmentSystemUrl);
            builder.Replace("src='@", "src='" + publishmentSystemUrl);
            builder.Replace("src=@", "src=" + publishmentSystemUrl);
            builder.Replace("src=&quot;@", "src=&quot;" + publishmentSystemUrl);
            return builder.ToString();
        }

        public static string GetApplicationName(int publishmentSystemID)
        {
            return AppManager.CMS.AppID + "_" + publishmentSystemID;
        }

        public static string GetProgressCacheKey(int publishmentSystemID, EProgressType progressType, string cacheType)
        {
            return string.Format("{0}_{1}_{2}_{3}", EProgressTypeUtils.GetValue(progressType), cacheType, publishmentSystemID, AdminManager.Current.UserName);
        }

        public static void AddLog(int publishmentSystemID, string action)
        {
            StringUtility.AddLog(publishmentSystemID, 0, 0, action, string.Empty);
        }

        public static void AddLog(int publishmentSystemID, string action, string summary)
        {
            StringUtility.AddLog(publishmentSystemID, 0, 0, action, summary);
        }

        public static void AddLog(int publishmentSystemID, int channelID, int contentID, string action, string summary)
        {
            if (publishmentSystemID <= 0)
            {
                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, action, summary);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        action = StringUtils.MaxLengthText(action, 250);
                    }
                    if (!string.IsNullOrEmpty(summary))
                    {
                        summary = StringUtils.MaxLengthText(summary, 250);
                    }
                    if (channelID < 0)
                    {
                        channelID = -channelID;
                    }
                    SiteServer.CMS.Model.LogInfo logInfo = new SiteServer.CMS.Model.LogInfo(0, publishmentSystemID, channelID, contentID, BaiRongDataProvider.AdministratorDAO.UserName, PageUtils.GetIPAddress(), DateTime.Now, action, summary);
                    DataProvider.LogDAO.Insert(logInfo);
                }
                catch { }
            }
        }

        public class Template
        {
            public static string GetSearchTemplateContent(ECharset charset)
            {
                return string.Format(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset={0}"" />
<title>搜索</title>
<style>
*{{font-size:12px}}
</style>
</head>

<body>

<stl:searchInput openwin=""false"" isLoadValues=""true""></stl:searchInput>

<br />

<stl:searchOutput isHighlight=""true""></stl:searchOutput>

</body>
</html>", ECharsetUtils.GetValue(charset));
            }

            public static string GetCommentsTemplateContent(ECharset charset)
            {
                return string.Format(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset={0}"" />
<title>评论</title>
<style>
*{{font-size:12px}}
</style>
</head>

<body>

<stl:comments isPage=""true"" pageNum=""20"" isLinkToAll=""false""></stl:comments>

<br />

<stl:commentInput isDynamic=""true""></stl:commentInput>

</body>
</html>", ECharsetUtils.GetValue(charset));
            }
        }

        public class Controls
        {
            public static string GetImageUrlButtonGroupHtml(PublishmentSystemInfo publishmentSystemInfo, string textBoxID)
            {
                string selectImageClick = SiteServer.CMS.BackgroundPages.Modal.SelectImage.GetOpenWindowString(publishmentSystemInfo, textBoxID);
                string uploadImageClick = SiteServer.CMS.BackgroundPages.Modal.UploadImageSingle.GetOpenWindowStringToTextBox(publishmentSystemInfo.PublishmentSystemID, textBoxID);
                string cuttingImageClick = SiteServer.CMS.BackgroundPages.Modal.CuttingImage.GetOpenWindowStringWithTextBox(publishmentSystemInfo.PublishmentSystemID, textBoxID);
                string previewImageClick = SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewImage(publishmentSystemInfo.PublishmentSystemID, textBoxID);

                return string.Format(@"
<div class=""btn-group"">
    <a class=""btn"" href=""javascript:;"" onclick=""{0};return false;"" title=""选择""><i class=""icon-th""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{1};return false;"" title=""上传""><i class=""icon-arrow-up""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{2};return false;"" title=""裁切""><i class=""icon-crop""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{3};return false;"" title=""预览""><i class=""icon-eye-open""></i></a>
</div>
", selectImageClick, uploadImageClick, cuttingImageClick, previewImageClick);
            }
        }
    }
}
