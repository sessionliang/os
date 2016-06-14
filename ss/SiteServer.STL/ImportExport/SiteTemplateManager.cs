using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Model;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
    public class SiteTemplateManager
    {
        private readonly string rootPath;
        private SiteTemplateManager(string rootPath)
        {
            this.rootPath = rootPath;
            DirectoryUtils.CreateDirectoryIfNotExists(this.rootPath);
        }

        public static SiteTemplateManager GetInstance(string rootPath)
        {
            return new SiteTemplateManager(rootPath);
        }

        public static SiteTemplateManager Instance
        {
            get
            {
                return new SiteTemplateManager(PathUtility.GetSiteTemplatesPath(string.Empty));
            }
        }


        public void DeleteSiteTemplate(string siteTemplateDir)
        {
            string directoryPath = PathUtils.Combine(this.rootPath, siteTemplateDir);
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);

            string filePath = PathUtils.Combine(this.rootPath, siteTemplateDir + ".zip");
            FileUtils.DeleteFileIfExists(filePath);
        }

        public bool IsSiteTemplateDirectoryExists(string siteTemplateDir)
        {
            string siteTemplatePath = PathUtils.Combine(this.rootPath, siteTemplateDir);
            return DirectoryUtils.IsDirectoryExists(siteTemplatePath);
        }

        public int GetSiteTemplateCount()
        {
            string[] directorys = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            return directorys.Length;
        }

        public ArrayList GetDirectoryNameLowerArrayList()
        {
            string[] directorys = DirectoryUtils.GetDirectoryNames(this.rootPath);
            ArrayList arraylist = new ArrayList();
            foreach (string directoryName in directorys)
            {
                arraylist.Add(directoryName.ToLower().Trim());
            }
            return arraylist;
        }

        public SortedList GetSiteTemplateSortedList(EPublishmentSystemType publishmentSystemType)
        {
            SortedList sortedlist = new SortedList();
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            foreach (string siteTemplatePath in directoryPaths)
            {
                string metadataXmlFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Metadata);
                if (FileUtils.IsFileExists(metadataXmlFilePath))
                {
                    SiteTemplateInfo siteTemplateInfo = Serializer.ConvertFileToObject(metadataXmlFilePath, typeof(SiteTemplateInfo)) as SiteTemplateInfo;
                    if (siteTemplateInfo != null)
                    {
                        if (publishmentSystemType == EPublishmentSystemTypeUtils.GetEnumType(siteTemplateInfo.PublishmentSystemType))
                        {
                            string directoryName = PathUtils.GetDirectoryName(siteTemplatePath);
                            sortedlist.Add(directoryName, siteTemplateInfo);
                        }
                    }
                }
            }
            return sortedlist;
        }

        public SortedList GetAllSiteTemplateSortedList()
        {
            SortedList sortedlist = new SortedList();
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            foreach (string siteTemplatePath in directoryPaths)
            {
                string metadataXmlFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Metadata);
                if (FileUtils.IsFileExists(metadataXmlFilePath))
                {
                    SiteTemplateInfo siteTemplateInfo = Serializer.ConvertFileToObject(metadataXmlFilePath, typeof(SiteTemplateInfo)) as SiteTemplateInfo;
                    if (siteTemplateInfo != null)
                    {
                        string directoryName = PathUtils.GetDirectoryName(siteTemplatePath);
                        sortedlist.Add(directoryName, siteTemplateInfo);
                    }
                }
            }
            return sortedlist;
        }

        public SortedList GetSiteTemplateSortedListWithoutUserCenter()
        {
            SortedList sortedlist = new SortedList();
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            foreach (string siteTemplatePath in directoryPaths)
            {
                string metadataXmlFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Metadata);
                if (FileUtils.IsFileExists(metadataXmlFilePath))
                {
                    SiteTemplateInfo siteTemplateInfo = Serializer.ConvertFileToObject(metadataXmlFilePath, typeof(SiteTemplateInfo)) as SiteTemplateInfo;
                    if (siteTemplateInfo != null)
                    {
                        string directoryName = PathUtils.GetDirectoryName(siteTemplatePath);
                        if (!EPublishmentSystemTypeUtils.IsUserCenter(EPublishmentSystemTypeUtils.GetEnumType(siteTemplateInfo.PublishmentSystemType)))
                        sortedlist.Add(directoryName, siteTemplateInfo);
                    }
                }
            }
            return sortedlist;
        }

        public void ImportSiteTemplateToEmptyPublishmentSystem(int publishmentSystemID, string siteTemplateDir, bool isUseTables, bool isImportContents, bool isImportTableStyles)
        {
            string siteTemplatePath = PathUtility.GetSiteTemplatesPath(siteTemplateDir);
            if (DirectoryUtils.IsDirectoryExists(siteTemplatePath))
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                string templateFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Template);
                string tableDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.Table);
                string menuDisplayFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_MenuDisplay);
                string tagStyleFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_TagStyle);
                string adFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Ad);
                string seoFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Seo);
                string stlTagPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_StlTag);
                string gatherRuleFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_GatherRule);
                string inputDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.Input);
                string configurationFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Configuration);
                string siteContentDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.SiteContent);
                string contentModelPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath,DirectoryUtils.SiteTemplates.File_ContentModel);

                ImportObject importObject = new ImportObject(publishmentSystemID);

                importObject.ImportFiles(siteTemplatePath, true);

                importObject.ImportTemplates(templateFilePath, true);

                importObject.ImportAuxiliaryTables(tableDirectoryPath, isUseTables);

                importObject.ImportMenuDisplay(menuDisplayFilePath, true);

                importObject.ImportTagStyle(tagStyleFilePath, true);

                importObject.ImportAd(adFilePath, true);

                importObject.ImportSeo(seoFilePath, true);

                importObject.ImportStlTag(stlTagPath, true);

                importObject.ImportGatherRule(gatherRuleFilePath, true);

                importObject.ImportInput(inputDirectoryPath, true);

                importObject.ImportConfiguration(configurationFilePath);

                importObject.ImportContentModel(contentModelPath, true);

                if (EPublishmentSystemTypeUtils.IsB2C(publishmentSystemInfo.PublishmentSystemType))
                {
                    string b2cSettingsDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.B2CSettings);
                    importObject.ImportB2CSettings(b2cSettingsDirectoryPath);
                }
                if (EPublishmentSystemTypeUtils.IsWeixin(publishmentSystemInfo.PublishmentSystemType))
                {
                    string wxDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.WeiXin.DirectoryName);
                    DirectoryUtils.CreateDirectoryIfNotExists(wxDirectoryPath);

                    string fileKeyWordPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.KeyWord);
                    importObject.ImportKeyWord(fileKeyWordPath);

                    string fileMapPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_Map);
                    importObject.ImportMap(fileMapPath);

                    string fileCouponPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Coupon);
                    importObject.ImportCoupon(fileCouponPath);

                    string fileLotteryPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Lottery);
                    importObject.ImportLottery(fileLotteryPath);

                    string fileVotePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Vote);
                    importObject.ImportVote(fileVotePath);

                    string fileMessagePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Message);
                    importObject.ImportMessage(fileMessagePath);

                    string fileConferencePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Conference);
                    importObject.ImportConference(fileConferencePath);

                    string fileAlbumPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Album);
                    importObject.ImportAlbum(fileAlbumPath);

                    string fileSearchPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Search);
                    importObject.ImportSearch(fileSearchPath);

                    string fileAppointmentPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Appointment);
                    importObject.ImportAppointment(fileAppointmentPath);

                    string fileStorePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Store);
                    importObject.ImportStore(fileStorePath);

                    string fileCollectPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Collect);
                    importObject.ImportCollect(fileCollectPath);

                    string fileCardPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Card);
                    importObject.ImportCard(fileCardPath);

                    string fileView360Path = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_View360);
                    importObject.ImportView360(fileView360Path);

                    string fileWifiPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_Wifi);
                    importObject.ImportWifi(fileWifiPath);

                    string fileWifiNodePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_WifiNode);
                    importObject.ImportWifiNode(fileWifiNodePath);

                    string fileAccountPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_Account);
                    importObject.ImportAccount(fileAccountPath);

                    string fileCountPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_Count);
                    importObject.ImportCount(fileCountPath);

                    string fileMenuPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Menu);
                    importObject.ImportMenu(fileMenuPath);

                    string fileWebMenuPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.WebMenu);
                    importObject.ImportWebMenu(fileWebMenuPath);

                }

                ArrayList filePathArrayList = ImportObject.GetSiteContentFilePathArrayList(siteContentDirectoryPath);

                foreach (string filePath in filePathArrayList)
                {
                    importObject.ImportSiteContent(siteContentDirectoryPath, filePath, isImportContents);
                }

                DataProvider.NodeDAO.UpdateContentNum(publishmentSystemInfo);

                if (isImportTableStyles)
                {
                    importObject.ImportTableStyles(tableDirectoryPath);
                }
                importObject.RemoveDbCache();
            }
        }

        public static void ExportPublishmentSystemToSiteTemplate(PublishmentSystemInfo publishmentSystemInfo, string siteTemplateDir)
        {
            ExportObject exportObject = new ExportObject(publishmentSystemInfo.PublishmentSystemID);

            string siteTemplatePath = PathUtility.GetSiteTemplatesPath(siteTemplateDir);

            //导出模板
            string templateFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Template);
            exportObject.ExportTemplates(templateFilePath);
            //导出表
            string tableDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.Table);
            exportObject.ExportTablesAndStyles(tableDirectoryPath);
            //导出下拉菜单
            string menuDisplayFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_MenuDisplay);
            exportObject.ExportMenuDisplay(menuDisplayFilePath);
            //导出模板标签样式
            string tagStyleFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_TagStyle);
            exportObject.ExportTagStyle(tagStyleFilePath);
            //导出广告
            string adFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Ad);
            exportObject.ExportAd(adFilePath);
            //导出采集规则
            string gatherRuleFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_GatherRule);
            exportObject.ExportGatherRule(gatherRuleFilePath);
            //导出提交表单
            string inputDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.Input);
            exportObject.ExportInput(inputDirectoryPath);
            //导出应用属性以及应用属性表单
            string configurationFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Configuration);
            exportObject.ExportConfiguration(configurationFilePath);
            //导出SEO
            string seoFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Seo);
            exportObject.ExportSeo(seoFilePath);
            //导出自定义模板语言
            string stlTagFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_StlTag);
            exportObject.ExportStlTag(stlTagFilePath);
            //导出关联字段
            string relatedFieldDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.RelatedField);
            exportObject.ExportRelatedField(relatedFieldDirectoryPath);
            //导出内容模型（自定义添加的）
            string contentModelDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_ContentModel);
            exportObject.ExportContentModel(contentModelDirectoryPath);

            if (EPublishmentSystemTypeUtils.IsB2C(publishmentSystemInfo.PublishmentSystemType))
            {
                //导出商城设置
                string b2cSettingsDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.B2CSettings);
                exportObject.ExportB2CSettings(b2cSettingsDirectoryPath);
            }
            if (EPublishmentSystemTypeUtils.IsWeixin(publishmentSystemInfo.PublishmentSystemType))
            {
                string wxDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.WeiXin.DirectoryName);
                DirectoryUtils.CreateDirectoryIfNotExists(wxDirectoryPath);

                string fileMapPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_Map);
                exportObject.ExportMap(fileMapPath);

                string fileCouponPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Coupon);
                exportObject.ExportCoupon(fileCouponPath);

                string fileLotteryPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Lottery);
                exportObject.ExportLottery(fileLotteryPath);

                string fileVotePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Vote);
                exportObject.ExportVote(fileVotePath);

                string fileMessagePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Message);
                exportObject.ExportMessage(fileMessagePath);

                string fileConferencePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Conference);
                exportObject.ExportConference(fileConferencePath);

                string fileAlbumPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Album);
                exportObject.ExportAlbum(fileAlbumPath);

                string fileSearchPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Search);
                exportObject.ExportSearch(fileSearchPath);

                string fileAppointmentPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Appointment);
                exportObject.ExportAppointment(fileAppointmentPath);

                string fileStorePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Store);
                exportObject.ExportStore(fileStorePath);

                string fileCollectPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Collect);
                exportObject.ExportCollect(fileCollectPath);

                string fileCardPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Card);
                exportObject.ExportCard(fileCardPath);

                string fileView360Path = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_View360);
                exportObject.ExportView360(fileView360Path);

                string fileWifiPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_Wifi);
                exportObject.ExportWifi(fileWifiPath);

                string fileWifiNodePath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_WifiNode);
                exportObject.ExportWifiNode(fileWifiNodePath);

                string fileAccountPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_Account);
                exportObject.ExportAccount(fileAccountPath);

                string fileCountPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.File_Count);
                exportObject.ExportCount(fileCountPath);

                string fileMenuPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.Menu);
                exportObject.ExportMenu(fileMenuPath);

                string fileWebMenuPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.WebMenu);
                exportObject.ExportWebMenu(fileWebMenuPath);

                string fileKeyWordPath = PathUtils.Combine(wxDirectoryPath, DirectoryUtils.SiteTemplates.WeiXin.KeyWord);
                exportObject.ExportKeyWord(fileKeyWordPath);

            }
        }
    }
}
