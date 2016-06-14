using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;



using SiteServer.BBS.Model;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.BBS.Core;
using BaiRong.Text.LitJson;
using BaiRong.Model;
using BaiRong.Core.Drawing;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.BBS.Pages.Ajax
{
    public class Upload : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            if (!BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                UserGroupInfo groupInfo = UserGroupManager.GetCurrent(publishmentSystemInfo.GroupSN);
                ETriState uploadType = ETriStateUtils.GetEnumType(groupInfo.Additional.UploadType);
                if (uploadType != ETriState.False && !string.IsNullOrEmpty(groupInfo.Additional.AttachmentExtensions))
                {
                    NameValueCollection jsonAttributes = UploadFile(publishmentSystemID, groupInfo);

                    string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
                    jsonString = StringUtils.ToJsString(jsonString);

                    base.Response.Write(jsonString);
                    base.Response.End();
                }
            }
        }

        private NameValueCollection UploadFile(int publishmentSystemID, UserGroupInfo groupInfo)
        {
            string message = string.Empty;
            bool isImage = false;
            string attachmentUrl = string.Empty;
            string imageUrl = string.Empty;
            ArrayList extArrayList = TranslateUtils.StringCollectionToArrayList(groupInfo.Additional.AttachmentExtensions.ToLower());

            AttachmentInfo attachInfo = null;

            if (base.Request.Files != null && base.Request.Files["Filedata"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["Filedata"];

                try
                {
                    string fileNameOriginal = PathUtils.GetFileName(postedFile.FileName);
                    string fileExtName = PathUtils.GetExtension(fileNameOriginal).TrimStart('.').ToLower();
                    if (extArrayList.Contains(fileExtName))
                    {
                        string directoryPath = PathUtilityBBS.GetUploadDirectoryPath(publishmentSystemID, DateTime.Now);
                        string fileName = PathUtilityBBS.GetUploadFileName(publishmentSystemID, postedFile.FileName);
                        string filePath = PathUtils.Combine(directoryPath, fileName);

                        int maxSize = groupInfo.Additional.MaxSize == 0 ? 102400 : groupInfo.Additional.MaxSize;
                        List<AttachmentTypeInfo> typeList = AttachManager.GetTypeList(publishmentSystemID);
                        if (typeList.Count > 0)
                        {
                            foreach (AttachmentTypeInfo typeInfo in typeList)
                            {
                                if (typeInfo.FileExtName.ToLower() == fileExtName && typeInfo.MaxSize < maxSize)
                                {
                                    maxSize = typeInfo.MaxSize;
                                }
                            }
                        }
                        if (postedFile.ContentLength <= maxSize * 1024)
                        {
                            postedFile.SaveAs(filePath);
                            attachmentUrl = PageUtilityBBS.GetRelatedUrlByPhysicalPath(publishmentSystemID, filePath);

                            isImage = EFileSystemTypeUtils.IsImage(fileExtName);
                            if (isImage)
                            {
                                string fileNameThumb = "t_" + fileName;
                                string filePathThumb = PathUtils.Combine(directoryPath, fileNameThumb);

                                if (ImageUtils.MakeThumbnailIfExceedWidth(filePath, filePathThumb, 740))
                                {
                                    imageUrl = PageUtilityBBS.GetRelatedUrlByPhysicalPath(publishmentSystemID, filePathThumb);
                                }
                                else
                                {
                                    imageUrl = attachmentUrl;
                                }
                            }

                            attachInfo = new AttachmentInfo(0, publishmentSystemID, 0, 0, BaiRongDataProvider.UserDAO.CurrentUserName, false, isImage, 0, fileNameOriginal, fileExtName, postedFile.ContentLength, attachmentUrl, imageUrl, 0, DateTime.Now, string.Empty);
                            attachInfo.ID = DataProvider.AttachmentDAO.Insert(attachInfo);
                        }
                        else
                        {
                            message = "上传失败，上传文件超出规定的文件大小";
                        }
                    }
                    else
                    {
                        message = "系统不允许上传指定的格式";
                    }                    
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }

            NameValueCollection jsonAttributes = new NameValueCollection();
            if (attachInfo != null)
            {
                jsonAttributes.Add("success", "true");
                jsonAttributes.Add("id", attachInfo.ID.ToString());
                jsonAttributes.Add("fileName", attachInfo.FileName);
                jsonAttributes.Add("tips", AttachManager.GetAttachmentTips(attachInfo));
                jsonAttributes.Add("description", attachInfo.Description);
                jsonAttributes.Add("price", attachInfo.Price.ToString());
            }
            else
            {
                jsonAttributes.Add("success", "false");
                jsonAttributes.Add("message", message);
            }
            
            return jsonAttributes;
        }
    }
}
