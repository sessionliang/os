using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Specialized;

using SiteServer.B2C.Core;
using System.Web;
using BaiRong.Core.Drawing;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundAjaxUpload : BackgroundBasePage
    {
        public static string GetContentPhotoUploadMultipleUrl(int publishmentSystemID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_ajaxUpload.aspx?PublishmentSystemID={0}&isContentPhotoSwfUpload=True", publishmentSystemID));
        }

        public static string GetContentPhotoUploadSingleUrl(int publishmentSystemID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_ajaxUpload.aspx?publishmentSystemID={0}&isContentPhoto=True", publishmentSystemID));
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            NameValueCollection jsonAttributes = new NameValueCollection();

            if (base.GetBoolQueryString("isContentPhoto"))
            {
                string message = string.Empty;
                string url = string.Empty;
                string smallUrl = string.Empty;
                string middleUrl = string.Empty;
                string largeUrl = string.Empty;
                bool success = UploadContentPhotoImage(out message, out url, out smallUrl, out middleUrl, out largeUrl);
                jsonAttributes.Add("success", success.ToString().ToLower());
                jsonAttributes.Add("message", message);
                jsonAttributes.Add("url", url);
                jsonAttributes.Add("smallUrl", smallUrl);
                jsonAttributes.Add("middleUrl", middleUrl);
                jsonAttributes.Add("largeUrl", largeUrl);
            }
            else if (base.GetBoolQueryString("isContentPhotoSwfUpload"))
            {
                string message = string.Empty;
                string url = string.Empty;
                string smallUrl = string.Empty;
                string middleUrl = string.Empty;
                string largeUrl = string.Empty;
                bool success = UploadContentPhotoSwfUpload(out message, out url, out smallUrl, out middleUrl, out largeUrl);
                jsonAttributes.Add("success", success.ToString().ToLower());
                jsonAttributes.Add("message", message);
                jsonAttributes.Add("url", url);
                jsonAttributes.Add("smallUrl", smallUrl);
                jsonAttributes.Add("middleUrl", middleUrl);
                jsonAttributes.Add("largeUrl", largeUrl);
            }

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            base.Response.Write(jsonString);
            base.Response.End();
        }

        public bool UploadContentPhotoImage(out string message, out string url, out string smallUrl, out string middleUrl, out string largeUrl)
        {
            message = url = smallUrl = middleUrl = largeUrl = string.Empty;

            if (base.Request.Files != null && base.Request.Files["ImageUrl"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["ImageUrl"];

                try
                {
                    string fileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, postedFile.FileName);
                    string fileExtName = PathUtils.GetExtension(fileName).ToLower();
                    string directoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                    string fileNameSmall = "small_" + fileName;
                    string fileNameMiddle = "middle_" + fileName;
                    string filePath = PathUtils.Combine(directoryPath, fileName);
                    string filePathSamll = PathUtils.Combine(directoryPath, fileNameSmall);
                    string filePathMiddle = PathUtils.Combine(directoryPath, fileNameMiddle);

                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        if (!PathUtility.IsImageSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                        {
                            base.FailMessage("上传失败，上传图片超出规定文件大小！");
                            return false;
                        }

                        postedFile.SaveAs(filePath);

                        FileUtility.AddWaterMark(base.PublishmentSystemInfo, filePath);

                        int widthSmall = base.PublishmentSystemInfo.Additional.PhotoSmallWidth;
                        int heightSamll = base.PublishmentSystemInfo.Additional.PhotoSmallHeight;
                        ImageUtils.MakeThumbnail(filePath, filePathSamll, widthSmall, heightSamll, true);

                        int widthMiddle = base.PublishmentSystemInfo.Additional.PhotoMiddleWidth;
                        int heightMiddle = base.PublishmentSystemInfo.Additional.PhotoMiddleHeight;
                        ImageUtils.MakeThumbnail(filePath, filePathMiddle, widthMiddle, heightMiddle, true);

                        url = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePathSamll);

                        smallUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, url);
                        middleUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePathMiddle));
                        largeUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePath));
                        return true;
                    }
                    else
                    {
                        message = "您必须上传图片文件！";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool UploadContentPhotoSwfUpload(out string message, out string url, out string smallUrl, out string middleUrl, out string largeUrl)
        {
            message = url = smallUrl = middleUrl = largeUrl = string.Empty;

            if (base.Request.Files != null && base.Request.Files["Filedata"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["Filedata"];

                try
                {
                    string fileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, postedFile.FileName);
                    string fileExtName = PathUtils.GetExtension(fileName).ToLower();
                    string directoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                    string fileNameSmall = "small_" + fileName;
                    string fileNameMiddle = "middle_" + fileName;
                    string filePath = PathUtils.Combine(directoryPath, fileName);
                    string filePathSmall = PathUtils.Combine(directoryPath, fileNameSmall);
                    string filePathMiddle = PathUtils.Combine(directoryPath, fileNameMiddle);

                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        if (!PathUtility.IsImageSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                        {
                            base.FailMessage("上传失败，上传图片超出规定文件大小！");
                            return false;
                        }

                        postedFile.SaveAs(filePath);

                        FileUtility.AddWaterMark(base.PublishmentSystemInfo, filePath);

                        int widthSmall = base.PublishmentSystemInfo.Additional.PhotoSmallWidth;
                        int heightSmall = base.PublishmentSystemInfo.Additional.PhotoSmallHeight;
                        ImageUtils.MakeThumbnail(filePath, filePathSmall, widthSmall, heightSmall, true);

                        int widthMiddle = base.PublishmentSystemInfo.Additional.PhotoMiddleWidth;
                        int heightMiddle = base.PublishmentSystemInfo.Additional.PhotoMiddleHeight;
                        ImageUtils.MakeThumbnail(filePath, filePathMiddle, widthMiddle, heightMiddle, true);

                        url = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePathSmall);

                        smallUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, url);
                        middleUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePathMiddle));
                        largeUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePath));
                        return true;
                    }
                    else
                    {
                        message = "您必须上传图片文件！";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }
    }
}
