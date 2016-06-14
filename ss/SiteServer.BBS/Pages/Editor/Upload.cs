using System;
using System.Text;
using System.Web;
using System.IO;
using System.Collections;
using System.Globalization;
using BaiRong.Text.LitJson;
using SiteServer.BBS.Core;
using BaiRong.Core.Drawing;
using BaiRong.Core;

using SiteServer.BBS.Model;
using BaiRong.Model;
using System.Collections.Generic;

using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Pages.Editor
{
    public class Upload : IHttpHandler
    {
        private HttpContext context;
        private PublishmentSystemInfo publishmentSystemInfo;

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;
            this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(TranslateUtils.ToInt(this.context.Request.QueryString["publishmentSystemID"]));

            bool isSuccess = false;
            if (!BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                UserGroupInfo groupInfo = UserGroupManager.GetCurrent(this.publishmentSystemInfo.GroupSN);
                ETriState uploadType = ETriStateUtils.GetEnumType(groupInfo.Additional.UploadType);
                if (uploadType != ETriState.False && !string.IsNullOrEmpty(groupInfo.Additional.AttachmentExtensions))
                {
                    Hashtable hash = this.UploadFile(groupInfo);
                    isSuccess = true;
                    context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                    context.Response.Write(JsonMapper.ToJson(hash));
                    context.Response.End();
                }
            }
            if (!isSuccess)
            {
                Hashtable hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "上传图片出错";
                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(JsonMapper.ToJson(hash));
                context.Response.End();
            }
        }

        private Hashtable UploadFile(UserGroupInfo groupInfo)
        {
            Hashtable hash = new Hashtable();

            ArrayList extArrayList = TranslateUtils.StringCollectionToArrayList(groupInfo.Additional.AttachmentExtensions.ToLower());

            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                hash["error"] = 1;
                hash["message"] = "请选择文件";
                return hash;
            }

            string directoryPath = PathUtilityBBS.GetUploadDirectoryPath(this.publishmentSystemInfo.PublishmentSystemID, DateTime.Now);
            string fileName = PathUtilityBBS.GetUploadFileName(this.publishmentSystemInfo.PublishmentSystemID, imgFile.FileName);
            string filePath = PathUtils.Combine(directoryPath, fileName);
            string fileExt = Path.GetExtension(fileName).ToLower();

            int maxSize = groupInfo.Additional.MaxSize == 0 ? 102400 : groupInfo.Additional.MaxSize;
            IList<AttachmentTypeInfo> typeList = AttachManager.GetTypeList(this.publishmentSystemInfo.PublishmentSystemID);
            if (typeList.Count > 0)
            {
                foreach (AttachmentTypeInfo typeInfo in typeList)
                {
                    if (typeInfo.FileExtName.ToLower() == fileExt.TrimStart('.') && typeInfo.MaxSize < maxSize)
                    {
                        maxSize = typeInfo.MaxSize;
                    }
                }
            }

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize * 1024)
            {
                hash["error"] = 1;
                hash["message"] = "上传文件大小超过限制";
                return hash;
            }

            if (string.IsNullOrEmpty(fileExt) || !extArrayList.Contains(fileExt.Substring(1).ToLower()) || !EFileSystemTypeUtils.IsImage(fileExt))
            {
                hash["error"] = 1;
                hash["message"] = "上传文件扩展名是不允许的扩展名";
                return hash;
            }

            string filePathThumbnail = PathUtils.Combine(directoryPath, "t_" + fileName);

            imgFile.SaveAs(filePath);

            bool isThumbnail = ImageUtils.MakeThumbnailIfExceedWidth(filePath, filePathThumbnail, 740);

            string fileUrl = string.Empty;
            if (isThumbnail)
            {
                FileUtils.DeleteFileIfExists(filePath);
                fileUrl = PageUtilityBBS.GetBBSUrlByPhysicalPath(this.publishmentSystemInfo.PublishmentSystemID, filePathThumbnail);
            }
            else
            {
                fileUrl = PageUtilityBBS.GetBBSUrlByPhysicalPath(this.publishmentSystemInfo.PublishmentSystemID, filePath);
            }

            hash["error"] = 0;
            hash["url"] = fileUrl;

            return hash;
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
