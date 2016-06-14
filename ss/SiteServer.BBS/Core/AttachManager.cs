using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Generic;
using SiteServer.BBS.Pages;

namespace SiteServer.BBS.Core
{
    public class AttachManager
    {
        public static string GetFileSize(int fileSize)
        {
            if (fileSize < 1024)
            {
                return fileSize + " Byte";
            }
            else
            {
                return fileSize / 1024 + " K";
            }
        }

        public static string GetIconUrl(int publishmentSystemID, string fileType)
        {
            if ("_,chm,pdf,zip,rar,tar,gz,7z,gif,jpg,jpeg,png,doc,docx,xls,xlsx,ppt,pptx".IndexOf("," +  fileType.ToLower()) != -1)
            {
                return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("images/files/{0}.gif", fileType));
            }
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("images/files/other.gif", fileType));
        }

        public static void ClearCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        private static readonly object lockObject = new object();

        private static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.AnnouncementTypeManager.{0}", publishmentSystemID);
        }

        public static List<AttachmentTypeInfo> GetTypeList(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    List<AttachmentTypeInfo> list = DataProvider.AttachmentTypeDAO.GetList(publishmentSystemID);
                    CacheUtils.Max(cacheKey, list);
                    return list;
                }
                return CacheUtils.Get(cacheKey) as List<AttachmentTypeInfo>;
            }
        }

        public static string GetAttachmentTips(AttachmentInfo attachInfo)
        {
            return string.Format("ID: {0} 上传日期: {1} 文件大小: {2} KB", attachInfo.ID, DateUtils.GetDateAndTimeString(attachInfo.AddDate), attachInfo.FileSize / 1024);
        }

        public static string GetAttachmentHtml(int publishmentSystemID, List<AttachmentInfo> attachmentInfoList)
        {
            if (attachmentInfoList != null && attachmentInfoList.Count > 0)
            {
                StringBuilder builder = new StringBuilder(@"
<br /><br />附件：<br />");
                bool isAttachment = false;
                foreach (AttachmentInfo attachmentInfo in attachmentInfoList)
                {
                    if (!attachmentInfo.IsInContent)
                    {
                        isAttachment = true;
                        builder.Append(@"<div class=""attachment_div"">");
                        builder.Append(AttachManager.GetAttachmentHtml(publishmentSystemID, attachmentInfo));
                        builder.Append("</div>");
                    }
                }
                if (isAttachment)
                {
                    return builder.ToString();
                }
            }
            return string.Empty;
        }

        public static string GetAttachmentHtml(int publishmentSystemID, AttachmentInfo attachmentInfo)
        {
            if (attachmentInfo.IsImage)
            {
                return string.Format(@"<a href=""{0}"" target=""_blank""><img src=""{1}"" border=""0"" /></a>", PageUtilityBBS.GetBBSUrl(publishmentSystemID, attachmentInfo.AttachmentUrl), PageUtilityBBS.GetBBSUrl(publishmentSystemID, attachmentInfo.ImageUrl));
            }
            else
            {
                return string.Format(@"
<span class=""attachment""><img align=""absmiddle"" src=""{0}"" />&nbsp;<a href=""{1}"" target=""_blank"">{2}</a> ({3}, 下载次数:{4})</span>", AttachManager.GetIconUrl(publishmentSystemID, attachmentInfo.FileType), RedirectPage.GetRedirectUrl(publishmentSystemID, attachmentInfo.ID), attachmentInfo.FileName, AttachManager.GetFileSize(attachmentInfo.FileSize), attachmentInfo.Downloads);
            }
        }
    }
}
