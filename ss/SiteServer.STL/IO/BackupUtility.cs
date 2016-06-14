using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.STL.ImportExport;
using SiteServer.CMS.Core;

namespace SiteServer.STL.IO
{
	public class BackupUtility
	{
        #region 外部调用

        public const string CACHE_TOTAL_COUNT = "_TotalCount";
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";
        public const string CACHE_MESSAGE = "_Message";

        public static void BackupTemplates(int publishmentSystemID, string filePath)
        {
            ExportObject exportObject = new ExportObject(publishmentSystemID);
            exportObject.ExportTemplates(filePath);
        }

        public static void BackupChannelsAndContents(int publishmentSystemID, string filePath)
        {
            ExportObject exportObject = new ExportObject(publishmentSystemID);

            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, publishmentSystemID);
            exportObject.ExportChannels(nodeIDArrayList, filePath);
        }

        public static void BackupFiles(int publishmentSystemID, string filePath)
        {
            ExportObject exportObject = new ExportObject(publishmentSystemID);

            exportObject.ExportFiles(filePath);
        }


        public static void BackupSite(int publishmentSystemID, string filePath)
        {
            ExportObject exportObject = new ExportObject(publishmentSystemID);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            string siteTemplateDir = PathUtils.GetFileNameWithoutExtension(filePath);
            string siteTemplatePath = PathUtils.Combine(DirectoryUtils.GetDirectoryPath(filePath), siteTemplateDir);
            DirectoryUtils.DeleteDirectoryIfExists(siteTemplatePath);
            FileUtils.DeleteFileIfExists(filePath);
            string metadataPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, string.Empty);

            exportObject.ExportFilesToSite(siteTemplatePath, true, new ArrayList(), true);

            string siteContentDirectoryPath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.SiteContent);
            exportObject.ExportSiteContent(siteContentDirectoryPath, true, true, new ArrayList());

            string templateFilePath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.File_Template);
            exportObject.ExportTemplates(templateFilePath);
            string tableDirectoryPath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.Table);
            exportObject.ExportTablesAndStyles(tableDirectoryPath);
            string menuDisplayFilePath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.File_MenuDisplay);
            exportObject.ExportMenuDisplay(menuDisplayFilePath);
            string tagStyleFilePath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.File_TagStyle);
            exportObject.ExportTagStyle(tagStyleFilePath);
            string adFilePath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.File_Ad);
            exportObject.ExportAd(adFilePath);
            string gatherRuleFilePath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.File_GatherRule);
            exportObject.ExportGatherRule(gatherRuleFilePath);
            string inputDirectoryPath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.Input);
            exportObject.ExportInput(inputDirectoryPath);
            string configurationFilePath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.File_Configuration);
            exportObject.ExportConfiguration(configurationFilePath);
            string contentModelFilePath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.File_ContentModel);
            exportObject.ExportContentModel(contentModelFilePath);
            exportObject.ExportMetadata(publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemUrl, string.Empty, string.Empty, metadataPath);
            if (EPublishmentSystemTypeUtils.IsB2C(publishmentSystemInfo.PublishmentSystemType))
            {
                string b2cSettingsDirectoryPath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.B2CSettings);
                exportObject.ExportB2CSettings(b2cSettingsDirectoryPath);
            }

            ZipUtils.PackFiles(filePath, siteTemplatePath);
            DirectoryUtils.DeleteDirectoryIfExists(siteTemplatePath);
        }

        public static void RecoverySite(int publishmentSystemID, bool isDeleteChannels, bool isDeleteTemplates, bool isDeleteFiles, bool isZip, string path, bool isOverride, bool isUseTable)
        {
            ImportObject importObject = new ImportObject(publishmentSystemID);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            string siteTemplatePath = path;
            if (isZip)
            {
                //解压文件
                siteTemplatePath = PathUtils.GetTemporaryFilesPath(EBackupTypeUtils.GetValue(EBackupType.Site));
                DirectoryUtils.DeleteDirectoryIfExists(siteTemplatePath);
                DirectoryUtils.CreateDirectoryIfNotExists(siteTemplatePath);

                ZipUtils.UnpackFiles(path, siteTemplatePath);
            }
            string siteTemplateMetadataPath = PathUtils.Combine(siteTemplatePath, DirectoryUtils.SiteTemplates.SiteTemplateMetadata);

            if (isDeleteChannels)
            {
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, publishmentSystemID);
                foreach (int nodeID in nodeIDArrayList)
                {
                    DataProvider.NodeDAO.Delete(nodeID);
                }
            }
            if (isDeleteTemplates)
            {
                ArrayList templateInfoArrayList =
                    DataProvider.TemplateDAO.GetTemplateInfoArrayListByPublishmentSystemID(publishmentSystemID);
                foreach (TemplateInfo templateInfo in templateInfoArrayList)
                {
                    if (templateInfo.IsDefault == false)
                    {
                        DataProvider.TemplateDAO.Delete(publishmentSystemID, templateInfo.TemplateID);
                    }
                }
            }
            if (isDeleteFiles)
            {
                DirectoryUtility.DeletePublishmentSystemFiles(publishmentSystemInfo);
            }

            //导入文件
            importObject.ImportFiles(siteTemplatePath, isOverride);

            //导入模板
            string templateFilePath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.File_Template);
            importObject.ImportTemplates(templateFilePath, isOverride);

            //导入辅助表
            string tableDirectoryPath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.Table);
            importObject.ImportAuxiliaryTables(tableDirectoryPath, isUseTable);

            //导入菜单
            string menuDisplayFilePath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.File_MenuDisplay);
            importObject.ImportMenuDisplay(menuDisplayFilePath, isOverride);

            //导入标签样式
            string tagStyleFilePath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.File_TagStyle);
            importObject.ImportTagStyle(tagStyleFilePath, isOverride);

            //导入固定广告
            string adFilePath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.File_Ad);
            importObject.ImportAd(adFilePath, isOverride);

            //导入采集规则
            string gatherRuleFilePath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.File_GatherRule);
            importObject.ImportGatherRule(gatherRuleFilePath, isOverride);

            //导入提交表单
            string inputDirectoryPath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.Input);
            importObject.ImportInput(inputDirectoryPath, isOverride);

            //导入应用设置
            string configurationFilePath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.File_Configuration);
            importObject.ImportConfiguration(configurationFilePath);

            //导入内容模型
            string contentModelFilePath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.File_ContentModel);
            importObject.ImportContentModel(contentModelFilePath, true);

            if (EPublishmentSystemTypeUtils.IsB2C(publishmentSystemInfo.PublishmentSystemType))
            {
                //导入商城设置
                string b2cSettingsDirectoryPath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.B2CSettings);
                importObject.ImportB2CSettings(b2cSettingsDirectoryPath);
            }

            //导入栏目及内容
            string siteContentDirectoryPath = PathUtils.Combine(siteTemplateMetadataPath, DirectoryUtils.SiteTemplates.SiteContent);
            importObject.ImportChannelsAndContents(0, siteContentDirectoryPath, isOverride);

            DataProvider.NodeDAO.UpdateContentNum(publishmentSystemInfo);

            //导入表样式及清除缓存
            if (isUseTable)
            {
                importObject.ImportTableStyles(tableDirectoryPath);
            }
            importObject.RemoveDbCache();

			CacheUtils.Clear();
        }

        #endregion

    }
}
