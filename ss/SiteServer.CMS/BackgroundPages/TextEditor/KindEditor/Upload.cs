using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using System.Collections;
using BaiRong.Text.LitJson;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.IO;
using BaiRong.Core.Drawing;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.TextEditorKindEditor
{
    public class Upload : IHttpHandler
    {
        private HttpContext context;

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;

            Hashtable hash = new Hashtable();
            bool isSuccess = this.UploadFile(hash);

            if (isSuccess)
            {
                hash["error"] = 0;
                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(JsonMapper.ToJson(hash));
                context.Response.End();
            }
            else
            {
                hash["error"] = 1;
                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(JsonMapper.ToJson(hash));
                context.Response.End();
            }
        }

        private bool UploadFile(Hashtable hash)
        {
            bool isUser = TranslateUtils.ToBool(this.context.Request.QueryString["isUser"]);
            int publishmentSystemID = TranslateUtils.ToInt(this.context.Request.QueryString["publishmentSystemID"]);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            if (isUser && BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                hash["message"] = "权限不足";
                return false;
            }
            else if (!isUser && !BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
            {
                hash["message"] = "权限不足";
                return false;
            }

            try
            {
                string localDirectoryPath = string.Empty;
                string localFileName = string.Empty;

                HttpPostedFile myFile = context.Request.Files["imgFile"];
                if (myFile == null)
                {
                    hash["message"] = "请选择文件";
                    return false;
                }
                string filePath = myFile.FileName;
                string fileExtName = PathUtils.GetExtension(filePath);

                if (isUser)
                {
                    localDirectoryPath = PathUtils.GetUserUploadDirectoryPath(BaiRongDataProvider.UserDAO.CurrentUserName);
                    localFileName = PathUtils.GetUserUploadFileName(filePath);
                }
                else
                {
                    localDirectoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtName);
                    localFileName = PathUtility.GetUploadFileName(publishmentSystemInfo, filePath);
                }

                string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                if (!PathUtils.IsFileExtenstionAllowed("gif|jpg|jpeg|bmp|png", fileExtName))
                {
                    hash["message"] = "此文件格式不正确，请更换文件上传！";
                    return false;
                }

                if (isUser)
                {
                    if (PathUtils.IsFileExtenstionAllowed(UserConfigManager.Additional.UploadImageTypeCollection, fileExtName))
                    {
                        if (myFile.ContentLength > UserConfigManager.Additional.UploadImageTypeMaxSize * 1024)
                        {
                            hash["message"] = "上传失败，上传文件超出规定文件大小！";
                            return false;
                        }
                        myFile.SaveAs(localFilePath);

                        string imageUrl = PageUtils.GetUserUploadFileUrl(BaiRongDataProvider.UserDAO.CurrentUserName, localFileName);
                        hash["url"] = imageUrl;
                    }
                    else
                    {
                        hash["message"] = "此文件格式被管理员禁止，请更换文件上传！";
                        return false;
                    }
                }
                else
                {
                    if (!PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileExtName))
                    {
                        hash["message"] = "此文件格式被管理员禁止，请更换文件上传！";
                        return false;
                    }
                    if (!PathUtility.IsImageSizeAllowed(publishmentSystemInfo, myFile.ContentLength))
                    {
                        hash["message"] = "上传失败，上传文件超出规定文件大小！";
                        return false;
                    }

                    myFile.SaveAs(localFilePath);
                    string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, localFilePath);
                    hash["url"] = imageUrl;
                }
            }
            catch(Exception ex)
            {
                hash["message"] = ex.Message;
                return false;
            }

            return true;
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
