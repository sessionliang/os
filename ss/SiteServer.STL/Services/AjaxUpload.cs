using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.CMS.Core.Security;

using SiteServer.CMS.Core;
using System.Web;
using BaiRong.Core.Drawing;
using SiteServer.CMS.Core.Office;

namespace SiteServer.CMS.Services
{
    public class AjaxUpload : BasePage
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();

            if (TranslateUtils.ToBool(base.Request.QueryString["isContentPhoto"]))
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
            else if (TranslateUtils.ToBool(base.Request.QueryString["isContentPhotoSwfUpload"]))
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
            else if (TranslateUtils.ToBool(base.Request.QueryString["isContentTeleplay"]))
            {
                string message = string.Empty;
                string url = string.Empty;
                string file = string.Empty;
                bool success = UploadContentTeleplayVideo(out message, out url, out file);
                jsonAttributes.Add("success", success.ToString().ToLower());
                jsonAttributes.Add("message", message);
                jsonAttributes.Add("url", url);
                jsonAttributes.Add("file", file);
            }
            else if (TranslateUtils.ToBool(base.Request.QueryString["isContentTeleplaySwfUpload"]))
            {
                string message = string.Empty;
                string url = string.Empty;
                string file = string.Empty;
                bool success = UploadContentTeleplaySwfUpload(out message, out url, out file);
                jsonAttributes.Add("success", success.ToString().ToLower());
                jsonAttributes.Add("message", message);
                jsonAttributes.Add("url", url);
                jsonAttributes.Add("file", file);
            }
            else if (TranslateUtils.ToBool(base.Request.QueryString["isWordSwfUpload"]))
            {
                string message = string.Empty;
                string fileName = string.Empty;
                bool success = this.UploadWordSwfUpload(out message, out fileName);
                jsonAttributes.Add("success", success.ToString().ToLower());
                jsonAttributes.Add("message", message);
                jsonAttributes.Add("fileName", fileName);
            }
            else if (TranslateUtils.ToBool(base.Request.QueryString["isResume"]))
            {
                string message = string.Empty;
                string url = string.Empty;
                string value = string.Empty;
                bool success = UploadResumeImage(out message, out url, out value);
                jsonAttributes.Add("success", success.ToString().ToLower());
                jsonAttributes.Add("message", message);
                jsonAttributes.Add("url", url);
                jsonAttributes.Add("value", value);
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
                            message = "上传失败，上传图片超出规定文件大小！";
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
                            message = "上传失败，上传图片超出规定文件大小！";
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

        public bool UploadContentTeleplayVideo(out string message, out string url, out string file)
        {
            message = url = file = string.Empty;

            if (base.Request.Files != null && base.Request.Files["VideoUrl"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["VideoUrl"];

                try
                {
                    string fileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, postedFile.FileName);
                    string fileExtName = PathUtils.GetExtension(fileName).ToLower();
                    string directoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);

                    string filePath = PathUtils.Combine(directoryPath, fileName);

                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        if (!PathUtility.IsVideoSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                        {
                            message = "上传失败，上传视频超出规定文件大小！";
                            return false;
                        }

                        postedFile.SaveAs(filePath);

                        FileUtility.AddWaterMark(base.PublishmentSystemInfo, filePath);

                        url = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePath);
                        file = fileName;
                        return true;
                    }
                    else
                    {
                        message = "您必须上传视频文件！";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool UploadContentTeleplaySwfUpload(out string message, out string url, out string file)
        {
            message = url = file = string.Empty;

            if (base.Request.Files != null && base.Request.Files["Filedata"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["Filedata"];

                try
                {
                    string fileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, postedFile.FileName);
                    string fileExtName = PathUtils.GetExtension(fileName).ToLower();
                    string directoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);

                    string filePath = PathUtils.Combine(directoryPath, fileName);

                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        if (!PathUtility.IsVideoSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                        {
                            message = "上传失败，上传视频超出规定文件大小！";
                            return false;
                        }

                        postedFile.SaveAs(filePath);

                        FileUtility.AddWaterMark(base.PublishmentSystemInfo, filePath);

                        url = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, filePath);
                        file = fileName;
                        return true;
                    }
                    else
                    {
                        message = "您必须上传视频文件！";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool UploadWordSwfUpload(out string message, out string fileName)
        {
            message = fileName = string.Empty;

            if (base.Request.Files != null && base.Request.Files["Filedata"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["Filedata"];

                try
                {
                    fileName = postedFile.FileName;
                    string extendName = fileName.Substring(fileName.LastIndexOf(".")).ToLower();
                    if (extendName == ".doc" || extendName == ".docx")
                    {
                        string filePath = WordUtils.GetWordFilePath(fileName);
                        postedFile.SaveAs(filePath);
                        return true;
                    }
                    else
                    {
                        message = "请选择Word文件上传！";
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return false;
        }

        public bool UploadResumeImage(out string message, out string url, out string value)
        {
            message = url = value = string.Empty;

            if (base.Request.Files != null && base.Request.Files["ImageUrl"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["ImageUrl"];

                string filePath = postedFile.FileName;
                try
                {
                    string fileExtName = PathUtils.GetExtension(filePath).ToLower();
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                    string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);
                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                    if (!PathUtility.IsImageExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                    {
                        message = "上传失败，上传图片格式不正确！";
                        return false;
                    }
                    if (!PathUtility.IsImageSizeAllowed(base.PublishmentSystemInfo, postedFile.ContentLength))
                    {
                        message = "上传失败，上传图片超出规定文件大小！";
                        return false;
                    }

                    postedFile.SaveAs(localFilePath);

                    url = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                    value = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, url);
                    return true;
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
