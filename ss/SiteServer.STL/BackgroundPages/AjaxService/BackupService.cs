using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Web.UI;

using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Collections.Specialized;
using SiteServer.STL.IO;
using SiteServer.STL.ImportExport;

namespace SiteServer.STL.BackgroundPages.Service
{
    public class BackupService : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request.QueryString["type"];
            string userKeyPrefix = base.Request["userKeyPrefix"];
            NameValueCollection retval = new NameValueCollection();

            if (type == "Backup")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                string backupTypeString = base.Request.Form["backupTypeString"];
                retval = Backup(publishmentSystemID, backupTypeString, userKeyPrefix);
            }
            else if (type == "Recovery")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                bool isDeleteChannels = TranslateUtils.ToBool(base.Request.Form["isDeleteChannels"]);
                bool isDeleteTemplates = TranslateUtils.ToBool(base.Request.Form["isDeleteTemplates"]);
                bool isDeleteFiles = TranslateUtils.ToBool(base.Request.Form["isDeleteFiles"]);
                bool isZip = TranslateUtils.ToBool(base.Request.Form["isZip"]);
                string path = base.Request.Form["path"];
                bool isOverride = TranslateUtils.ToBool(base.Request.Form["isOverride"]);
                bool isUseTable = TranslateUtils.ToBool(base.Request.Form["isUseTable"]);
                retval = Recovery(publishmentSystemID, isDeleteChannels, isDeleteTemplates, isDeleteFiles, isZip, path, isOverride, isUseTable, userKeyPrefix);
            }
            else if (type == "IndependentTemplateReplace")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                string directoryName = base.Request.Form["directoryName"];
                retval = IndependentTemplateReplace(publishmentSystemID, directoryName, userKeyPrefix);
            }

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
            Page.Response.Write(jsonString);
            Page.Response.End();
        }

        public NameValueCollection Backup(int publishmentSystemID, string backupTypeString, string userKeyPrefix)
        {
            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            try
            {
                EBackupType backupType = EBackupTypeUtils.GetEnumType(backupTypeString);

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                string filePath = PathUtility.GetBackupFilePath(publishmentSystemInfo, backupType);
                DirectoryUtils.CreateDirectoryIfNotExists(filePath);
                FileUtils.DeleteFileIfExists(filePath);

                if (backupType == EBackupType.Templates)
                {
                    BackupUtility.BackupTemplates(publishmentSystemID, filePath);
                }
                else if (backupType == EBackupType.ChannelsAndContents)
                {
                    BackupUtility.BackupChannelsAndContents(publishmentSystemID, filePath);
                }
                else if (backupType == EBackupType.Files)
                {
                    BackupUtility.BackupFiles(publishmentSystemID, filePath);
                }
                else if (backupType == EBackupType.Site)
                {
                    BackupUtility.BackupSite(publishmentSystemID, filePath);
                }

                string resultString = string.Format("任务完成，备份地址：<br /><strong> {0} </strong>&nbsp;<a href='{1}'><img src='{2}' />下载</a>。", filePath, PageUtility.ServiceSTL.Utils.GetDownloadUrlByFilePath(filePath), PageUtils.GetIconUrl("download.gif"));

                retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(resultString, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Empty, ex.Message, string.Empty);
                LogUtils.AddErrorLog(ex);
            }

            return retval;
        }

        public NameValueCollection Recovery(int publishmentSystemID, bool isDeleteChannels, bool isDeleteTemplates, bool isDeleteFiles, bool isZip, string path, bool isOverride, bool isUseTable, string userKeyPrefix)
        {
            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            try
            {
                BackupUtility.RecoverySite(publishmentSystemID, isDeleteChannels, isDeleteTemplates, isDeleteFiles, isZip, PageUtils.UrlDecode(path), isOverride, isUseTable);

                StringUtility.AddLog(publishmentSystemID, "恢复备份数据");

                retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection("数据恢复成功!", string.Empty, string.Empty);

                //retval = new string[] { "数据恢复成功!", string.Empty, string.Empty };
            }
            catch (Exception ex)
            {
                //retval = new string[] { string.Empty, ex.Message, string.Empty };
                retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Empty, ex.Message, string.Empty);
                LogUtils.AddErrorLog(ex);
            }

            return retval;
        }

        public NameValueCollection IndependentTemplateReplace(int publishmentSystemID, string directoryName, string userKeyPrefix)
        {
            //返回“运行结果”和“错误信息”的字符串数组
            NameValueCollection retval;

            try
            {
                IndependentTemplateManager.Instance.LoadIndependentTemplateToPublishmentSystem(publishmentSystemID, directoryName);

                retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection("恭喜，模板已替换成功！", string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Empty, ex.Message, string.Empty);
                LogUtils.AddErrorLog(ex);
            }

            return retval;
        }

        //public NameValueCollection IndependentTemplateReplace(int publishmentSystemID, string directoryName, string userKeyPrefix)
        //{
        //    //返回“运行结果”和“错误信息”的字符串数组
        //    NameValueCollection retval;

        //    try
        //    {
        //        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

        //        string siteTemplatePath = ConfigurationManager.Additional.OuterSitePath;
        //        if (string.IsNullOrEmpty(siteTemplatePath) || DirectoryUtils.IsDirectoryExists(siteTemplatePath))
        //        {
        //            siteTemplatePath = PathUtility.GetSiteTemplatesPath(string.Empty);
        //        }
        //        siteTemplatePath = PathUtils.Combine(siteTemplatePath, siteDir);

        //        DirectoryUtility.ImportPublishmentSystemFiles(publishmentSystemInfo, siteTemplatePath, true);

        //        string sourceDataFilePath = PathUtils.Combine(siteTemplatePath, DirectoryUtils.SiteTemplates.SiteTemplateMetadata, "Data.asax");
        //        string destDataFilePath = PathUtils.GetSiteFilesPath("Data.asax");
        //        FileUtils.DeleteFileIfExists(destDataFilePath);
        //        FileUtils.CopyFile(sourceDataFilePath, destDataFilePath);

        //        //retval = new string[] { "导入应用成功!", string.Empty, string.Empty };
        //        retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection("导入应用成功!", string.Empty, string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        //retval = new string[] { string.Empty, ex.Message, string.Empty };
        //        retval = JsManager.AjaxService.GetWaitingTaskNameValueCollection(string.Empty, ex.Message, string.Empty);
        //        LogUtils.AddErrorLog(ex);
        //    }

        //    return retval;
        //}
    }
}
