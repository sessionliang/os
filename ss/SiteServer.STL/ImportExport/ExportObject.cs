using System.Collections;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Collections.Generic;
using SiteServer.STL.IO;
using SiteServer.CMS.Core;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.STL.ImportExport.B2C;

namespace SiteServer.STL.ImportExport
{
    public class ExportObject
    {
        public FileSystemObject FSO;

        public ExportObject(int publishmentSystemID)
        {
            this.FSO = new FileSystemObject(publishmentSystemID);
        }

        /// <summary>
        /// 将发布系统文件保存到应用模板中
        /// </summary>
        public void ExportFilesToSite(string siteTemplatePath, bool isSaveAll, ArrayList lowerFileSystemArrayList, bool isCreateMetadataDirectory)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(siteTemplatePath);

            ArrayList publishmentSystemDirArrayList = DataProvider.PublishmentSystemDAO.GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters();

            FileSystemInfoExtendCollection fileSystems = FileManager.GetFileSystemInfoExtendCollection(PathUtility.GetPublishmentSystemPath(this.FSO.PublishmentSystemInfo), true);
            foreach (FileSystemInfoExtend fileSystem in fileSystems)
            {
                if (isSaveAll || lowerFileSystemArrayList.Contains(fileSystem.Name.ToLower()))
                {
                    string srcPath = PathUtils.Combine(this.FSO.PublishmentSystemPath, fileSystem.Name);
                    string destPath = PathUtils.Combine(siteTemplatePath, fileSystem.Name);

                    if (fileSystem.IsDirectory)
                    {
                        bool isPublishmentSystemDirectory = false;

                        if (FSO.IsHeadquarters)
                        {
                            foreach (string publishmentSystemDir in publishmentSystemDirArrayList)
                            {
                                if (StringUtils.EqualsIgnoreCase(publishmentSystemDir, fileSystem.Name))
                                {
                                    isPublishmentSystemDirectory = true;
                                }
                            }
                        }
                        if (!isPublishmentSystemDirectory && !DirectoryUtils.IsSystemDirectory(fileSystem.Name))
                        {
                            DirectoryUtils.CreateDirectoryIfNotExists(destPath);
                            DirectoryUtils.MoveDirectory(srcPath, destPath, false);
                        }
                    }
                    else
                    {
                        if (!PathUtility.IsSystemFile(fileSystem.Name))
                        {
                            FileUtils.CopyFile(srcPath, destPath);
                        }
                    }
                }
            }

            if (isCreateMetadataDirectory)
            {
                string siteTemplateMetadataPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, string.Empty);
                DirectoryUtils.CreateDirectoryIfNotExists(siteTemplateMetadataPath);
            }
        }

        public void ExportFiles(string filePath)
        {
            string filesDirectoryPath = PathUtils.Combine(DirectoryUtils.GetDirectoryPath(filePath), PathUtils.GetFileNameWithoutExtension(filePath));

            DirectoryUtils.DeleteDirectoryIfExists(filesDirectoryPath);
            FileUtils.DeleteFileIfExists(filePath);

            DirectoryUtils.Copy(FSO.PublishmentSystemPath, filesDirectoryPath);

            ZipUtils.PackFiles(filePath, filesDirectoryPath);

            DirectoryUtils.DeleteDirectoryIfExists(filesDirectoryPath);
        }

        public string ExportSingleTableStyle(ETableStyle tableStyle, string tableName, int relatedIdentity)
        {
            string filePath = PathUtils.GetTemporaryFilesPath("tableStyle.zip");
            string styleDirectoryPath = PathUtils.GetTemporaryFilesPath("TableStyle");
            TableStyleIE.SingleExportTableStyles(tableStyle, tableName, this.FSO.PublishmentSystemID, relatedIdentity, styleDirectoryPath);
            ZipUtils.PackFiles(filePath, styleDirectoryPath);

            DirectoryUtils.DeleteDirectoryIfExists(styleDirectoryPath);

            return PathUtils.GetFileName(filePath);
        }

        public void ExportConfiguration(string configurationFilePath)
        {
            ConfigurationIE configIE = new ConfigurationIE(FSO.PublishmentSystemID, configurationFilePath);
            configIE.Export();
        }

        /// <summary>
        /// 导出网站模板至指定的文件地址
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportTemplates(string filePath)
        {
            TemplateIE templateIE = new TemplateIE(FSO.PublishmentSystemID, filePath);
            templateIE.ExportTemplates();
        }

        public void ExportTemplates(string filePath, List<int> templateIDList)
        {
            TemplateIE templateIE = new TemplateIE(FSO.PublishmentSystemID, filePath);
            templateIE.ExportTemplates(templateIDList);
        }


        /// <summary>
        /// 导出网站菜单显示方式至指定的文件地址
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportMenuDisplay(string filePath)
        {
            MenuDisplayIE menuDisplayIE = new MenuDisplayIE(FSO.PublishmentSystemID, filePath);
            menuDisplayIE.ExportMenuDisplay();
        }

        public void ExportTagStyle(string filePath)
        {
            TagStyleIE tagStyleIE = new TagStyleIE(FSO.PublishmentSystemID, filePath);
            tagStyleIE.ExportTagStyle();
        }

        public string ExportTagStyle(TagStyleInfo styleInfo)
        {
            string filePath = PathUtils.GetTemporaryFilesPath(styleInfo.StyleName + ".xml");

            FileUtils.DeleteFileIfExists(filePath);

            TagStyleIE tagStyleIE = new TagStyleIE(FSO.PublishmentSystemID, filePath);
            tagStyleIE.ExportTagStyle(styleInfo);

            return PathUtils.GetFileName(filePath);
        }

        /// <summary>
        /// 导出固定广告至指定的文件地址
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportAd(string filePath)
        {
            AdvIE adIE = new AdvIE(FSO.PublishmentSystemID, filePath);
            adIE.ExportAd();
        }

        /// <summary>
        /// 导出搜索引擎
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportSeo(string filePath)
        {
            SeoIE seoIE = new SeoIE(FSO.PublishmentSystemID, filePath);
            seoIE.ExportSeo();
        }

        /// <summary>
        /// 导出自定义模板语言
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportStlTag(string filePath)
        {
            StlTagIE stlTagIE = new StlTagIE(FSO.PublishmentSystemID, filePath);
            stlTagIE.ExportStlTag();
        }

        /// <summary>
        /// 导出采集规则至指定的文件地址
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportGatherRule(string filePath)
        {
            ArrayList gatherRuleInfoArrayList = DataProvider.GatherRuleDAO.GetGatherRuleInfoArrayList(FSO.PublishmentSystemID);
            ExportGatherRule(filePath, gatherRuleInfoArrayList);
        }

        public void ExportGatherRule(string filePath, ArrayList gatherRuleInfoArrayList)
        {
            GatherRuleIE gatherRuleIE = new GatherRuleIE(FSO.PublishmentSystemID, filePath);
            gatherRuleIE.ExportGatherRule(gatherRuleInfoArrayList);
        }

        public void ExportInput(string inputDirectoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(inputDirectoryPath);

            InputIE inputIE = new InputIE(FSO.PublishmentSystemID, inputDirectoryPath);
            ArrayList inputIDArrayList = DataProvider.InputDAO.GetInputIDArrayList(FSO.PublishmentSystemID);
            foreach (int inputID in inputIDArrayList)
            {
                inputIE.ExportInput(inputID);
            }
        }

        public string ExportInput(int inputID)
        {
            string directoryPath = PathUtils.GetTemporaryFilesPath("input");
            string filePath = PathUtils.GetTemporaryFilesPath("input.zip");

            FileUtils.DeleteFileIfExists(filePath);
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

            InputIE inputIE = new InputIE(FSO.PublishmentSystemID, directoryPath);
            inputIE.ExportInput(inputID);

            ZipUtils.PackFiles(filePath, directoryPath);

            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);

            return PathUtils.GetFileName(filePath);
        }

        public void ExportRelatedField(string relatedFieldDirectoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(relatedFieldDirectoryPath);

            RelatedFieldIE relatedFieldIE = new RelatedFieldIE(FSO.PublishmentSystemID, relatedFieldDirectoryPath);
            ArrayList relatedFieldInfoArrayList = DataProvider.RelatedFieldDAO.GetRelatedFieldInfoArrayList(FSO.PublishmentSystemID);
            foreach (RelatedFieldInfo relatedFieldInfo in relatedFieldInfoArrayList)
            {
                relatedFieldIE.ExportRelatedField(relatedFieldInfo);
            }
        }

        public string ExportRelatedField(int relatedFieldID)
        {
            string directoryPath = PathUtils.GetTemporaryFilesPath("relatedField");
            string filePath = PathUtils.GetTemporaryFilesPath("relatedField.zip");

            FileUtils.DeleteFileIfExists(filePath);
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

            RelatedFieldInfo relatedFieldInfo = DataProvider.RelatedFieldDAO.GetRelatedFieldInfo(relatedFieldID);

            RelatedFieldIE relatedFieldIE = new RelatedFieldIE(FSO.PublishmentSystemID, directoryPath);
            relatedFieldIE.ExportRelatedField(relatedFieldInfo);

            ZipUtils.PackFiles(filePath, directoryPath);

            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);

            return PathUtils.GetFileName(filePath);
        }


        // 导出网站所有相关辅助表以及除提交表单外的所有表样式
        public void ExportTablesAndStyles(string tableDirectoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(tableDirectoryPath);
            AuxiliaryTableIE tableIE = new AuxiliaryTableIE(tableDirectoryPath);
            TableStyleIE styleIE = new TableStyleIE(tableDirectoryPath);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.FSO.PublishmentSystemID);
            List<string> tableNameList = PublishmentSystemManager.GetAuxiliaryTableNameList(publishmentSystemInfo);

            foreach (string tableName in tableNameList)
            {
                tableIE.ExportAuxiliaryTable(tableName);
                styleIE.ExportTableStyles(publishmentSystemInfo.PublishmentSystemID, tableName);
            }

            styleIE.ExportTableStyles(publishmentSystemInfo.PublishmentSystemID, DataProvider.NodeDAO.TableName);
            styleIE.ExportTableStyles(publishmentSystemInfo.PublishmentSystemID, DataProvider.PublishmentSystemDAO.TableName);
        }


        /// <summary>
        /// 导出网站内容至默认的临时文件地址
        /// </summary>
        public void ExportSiteContent(string siteContentDirectoryPath, bool isSaveContents, bool isSaveAllChannels, ArrayList nodeIDArrayList)
        {
            DirectoryUtils.DeleteDirectoryIfExists(siteContentDirectoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(siteContentDirectoryPath);

            ArrayList allNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(FSO.PublishmentSystemID);

            ArrayList includeNodeIDArrayList = new ArrayList();
            foreach (int nodeID in nodeIDArrayList)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(FSO.PublishmentSystemID, nodeID);
                ArrayList parentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nodeInfo.ParentsPath);
                foreach (int parentID in parentIDArrayList)
                {
                    if (!includeNodeIDArrayList.Contains(parentID))
                    {
                        includeNodeIDArrayList.Add(parentID);
                    }
                }
                if (!includeNodeIDArrayList.Contains(nodeID))
                {
                    includeNodeIDArrayList.Add(nodeID);
                }
            }

            SiteContentIE siteContentIE = new SiteContentIE(FSO.PublishmentSystemInfo, siteContentDirectoryPath);
            foreach (int nodeID in allNodeIDArrayList)
            {
                if (!isSaveAllChannels)
                {
                    if (!includeNodeIDArrayList.Contains(nodeID)) continue;
                }
                siteContentIE.Export(this.FSO.PublishmentSystemID, nodeID, isSaveContents);
            }
        }

        public void ExportMetadata(string siteTemplateName, string webSiteUrl, string description, string samplePicPath, string metadataPath)
        {
            SiteTemplateInfo siteTemplateInfo = new SiteTemplateInfo();
            siteTemplateInfo.SiteTemplateName = siteTemplateName;
            siteTemplateInfo.PicFileName = samplePicPath;
            siteTemplateInfo.WebSiteUrl = webSiteUrl;
            siteTemplateInfo.Description = description;

            string xmlPath = PathUtils.Combine(metadataPath, DirectoryUtils.SiteTemplates.File_Metadata);
            Serializer.SaveAsXML(siteTemplateInfo, xmlPath);
        }


        public string ExportChannels(ArrayList nodeIDArrayList)
        {
            string filePath = PathUtils.GetTemporaryFilesPath(EBackupTypeUtils.GetValue(EBackupType.ChannelsAndContents) + ".zip");
            return ExportChannels(nodeIDArrayList, filePath);
        }

        public string ExportChannels(ArrayList nodeIDArrayList, string filePath)
        {
            string siteContentDirectoryPath = PathUtils.Combine(DirectoryUtils.GetDirectoryPath(filePath), PathUtils.GetFileNameWithoutExtension(filePath));

            DirectoryUtils.DeleteDirectoryIfExists(siteContentDirectoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(siteContentDirectoryPath);

            SiteContentIE siteContentIE = new SiteContentIE(FSO.PublishmentSystemInfo, siteContentDirectoryPath);
            ArrayList allNodeIDArrayList = new ArrayList();
            foreach (int nodeID in nodeIDArrayList)
            {
                if (!allNodeIDArrayList.Contains(nodeID))
                {
                    allNodeIDArrayList.Add(nodeID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(FSO.PublishmentSystemID, nodeID);
                    ArrayList childNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(nodeInfo, EScopeType.Descendant, string.Empty, string.Empty);
                    allNodeIDArrayList.AddRange(childNodeIDArrayList);
                }
            }
            foreach (int nodeID in allNodeIDArrayList)
            {
                siteContentIE.Export(this.FSO.PublishmentSystemID, nodeID, true);
            }

            ZipUtils.PackFiles(filePath, siteContentDirectoryPath);

            DirectoryUtils.DeleteDirectoryIfExists(siteContentDirectoryPath);

            return PathUtils.GetFileName(filePath);
        }

        public bool ExportContents(string filePath, int nodeID, ArrayList contentIDArrayList, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState)
        {
            string siteContentDirectoryPath = PathUtils.Combine(DirectoryUtils.GetDirectoryPath(filePath), PathUtils.GetFileNameWithoutExtension(filePath));

            FileUtils.DeleteFileIfExists(filePath);
            DirectoryUtils.DeleteDirectoryIfExists(siteContentDirectoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(siteContentDirectoryPath);

            SiteContentIE siteContentIE = new SiteContentIE(FSO.PublishmentSystemInfo, siteContentDirectoryPath);
            bool isExport = siteContentIE.ExportContents(this.FSO.PublishmentSystemInfo, nodeID, contentIDArrayList, isPeriods, dateFrom, dateTo, checkedState);
            if (isExport)
            {
                ZipUtils.PackFiles(filePath, siteContentDirectoryPath);
                DirectoryUtils.DeleteDirectoryIfExists(siteContentDirectoryPath);
            }
            return isExport;
        }

        public void ExportB2CSettings(string settingsDirectoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(settingsDirectoryPath);

            B2CSettingsIE settingsIE = new B2CSettingsIE(FSO.PublishmentSystemID, settingsDirectoryPath);
            settingsIE.ExportSettings();
        }

        #region WeiXin

        public void ExportMap(string filePath)
        {
            MapIE mapIE = new MapIE(FSO.PublishmentSystemID, filePath);
            mapIE.Export();
        }

        public void ExportCoupon(string filePath)
        {
            CouponIE couponIE = new CouponIE(FSO.PublishmentSystemID, filePath);
            couponIE.Export();
        }

        public void ExportLottery(string filePath)
        {
            LotteryIE lotteryIE = new LotteryIE(FSO.PublishmentSystemID, filePath);
            lotteryIE.Export();
        }

        public void ExportVote(string filePath)
        {
            VoteIE voteIE = new VoteIE(FSO.PublishmentSystemID, filePath);
            voteIE.Export();
        }

        public void ExportMessage(string filePath)
        {
            MessageIE messageIE = new MessageIE(FSO.PublishmentSystemID, filePath);
            messageIE.Export();
        }

        public void ExportConference(string filePath)
        {
            ConferenceIE conferenceIE = new ConferenceIE(FSO.PublishmentSystemID, filePath);
            conferenceIE.Export();
        }

        public void ExportAlbum(string filePath)
        {
            AlbumIE albumIE = new AlbumIE(FSO.PublishmentSystemID, filePath);
            albumIE.Export();
        }

        public void ExportSearch(string filePath)
        {
            SearchIE searchIE = new SearchIE(FSO.PublishmentSystemID, filePath);
            searchIE.Export();
        }

        public void ExportAppointment(string filePath)
        {
            AppointmentIE appointmentIE = new AppointmentIE(FSO.PublishmentSystemID, filePath);
            appointmentIE.Export();
        }

        public void ExportStore(string filePath)
        {
            StoreIE storeIE = new StoreIE(FSO.PublishmentSystemID, filePath);
            storeIE.Export();
        }

        public void ExportCollect(string filePath)
        {
            CollectIE collectIE = new CollectIE(FSO.PublishmentSystemID, filePath);
            collectIE.Export();
        }

        public void ExportCard(string filePath)
        {
            CardIE cardIE = new CardIE(FSO.PublishmentSystemID, filePath);
            cardIE.Export();
        }

        public void ExportView360(string filePath)
        {
            View360IE view360IE = new View360IE(FSO.PublishmentSystemID, filePath);
            view360IE.Export();
        }
        public void ExportWifi(string filePath)
        {
            WifiIE wifiIE = new WifiIE(FSO.PublishmentSystemID, filePath);
            wifiIE.Export();
        }

        public void ExportWifiNode(string filePath)
        {
            WifiNodeIE wifiNodeIE = new WifiNodeIE(FSO.PublishmentSystemID, filePath);
            wifiNodeIE.Export();
        }
        public void ExportAccount(string filePath)
        {
            AccountIE accountIE = new AccountIE(FSO.PublishmentSystemID, filePath);
            accountIE.Export();
        }

        public void ExportCount(string filePath)
        {
            CountIE countIE = new CountIE(FSO.PublishmentSystemID, filePath);
            countIE.Export();
        }

        public void ExportMenu(string filePath)
        {
            MenuIE menuIE = new MenuIE(FSO.PublishmentSystemID, filePath);
            menuIE.Export();
        }

        public void ExportWebMenu(string filePath)
        {
            WebMenuIE webMenuIE = new WebMenuIE(FSO.PublishmentSystemID, filePath);
            webMenuIE.Export();
        }

        public void ExportKeyWord(string filePath)
        {
            KeyWordIE keyWordIE = new KeyWordIE(FSO.PublishmentSystemID, filePath);
            keyWordIE.Export();
        }

        #endregion

        public void ExportContentModel(string contentModelPath)
        {
            //ArrayList contentModelList = ContentModelManager.GetContentModelArrayList(this.FSO.PublishmentSystemInfo);
            ArrayList contentModelList = BaiRongDataProvider.ContentModelDAO.GetContentModelInfoArrayList(EPublishmentSystemTypeUtils.GetAppID(FSO.PublishmentSystemInfo.PublishmentSystemType), FSO.PublishmentSystemID);
            ExprotContentModel(contentModelPath, contentModelList);
        }

        public void ExprotContentModel(string filePath, ArrayList contentModelList)
        {
            ContentModelIE contentModelIE = new ContentModelIE(FSO.PublishmentSystemID, filePath,FSO.PublishmentSystemInfo.PublishmentSystemType.ToString());
            contentModelIE.ExportContentModel(contentModelList);
        }
    }
}
