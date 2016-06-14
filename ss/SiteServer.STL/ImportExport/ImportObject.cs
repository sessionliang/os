using System.Collections;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.IO;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Office;
using BaiRong.Core.Data.Provider;
using SiteServer.STL.IO;
using SiteServer.CMS.Core;
using SiteServer.STL.ImportExport.B2C;

namespace SiteServer.STL.ImportExport
{
    public class ImportObject
    {
        public FileSystemObject FSO;

        public ImportObject(int publishmentSystemID)
        {
            this.FSO = new FileSystemObject(publishmentSystemID);
        }

        //获取保存辅助表名称对应集合数据库缓存键
        private string GetTableNameNameValueCollectionDbCacheKey()
        {
            return "SiteServer.CMS.Core.ImportObject.TableNameNameValueCollection_" + this.FSO.PublishmentSystemID;
        }

        public NameValueCollection GetTableNameCache()
        {
            NameValueCollection nameValueCollection = null;
            string cacheValue = DbCacheManager.Get(this.GetTableNameNameValueCollectionDbCacheKey());
            if (!string.IsNullOrEmpty(cacheValue))
            {
                nameValueCollection = TranslateUtils.ToNameValueCollection(cacheValue);
            }
            return nameValueCollection;
        }

        public void SaveTableNameCache(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection != null && nameValueCollection.Count > 0)
            {
                string cacheKey = this.GetTableNameNameValueCollectionDbCacheKey();
                string cacheValue = TranslateUtils.NameValueCollectionToString(nameValueCollection);
                DbCacheManager.Insert(cacheKey, cacheValue);
            }
        }

        public void RemoveDbCache()
        {
            string cacheKey = this.GetTableNameNameValueCollectionDbCacheKey();
            DbCacheManager.Remove(cacheKey);
        }

        public void ImportFiles(string siteTemplatePath, bool isOverride)
        {
            //if (this.FSO.IsHeadquarters)
            //{
            //    string[] filePaths = DirectoryUtils.GetFilePaths(siteTemplatePath);
            //    foreach (string filePath in filePaths)
            //    {
            //        string fileName = PathUtils.GetFileName(filePath);
            //        if (!PathUtility.IsSystemFile(fileName))
            //        {
            //            string destFilePath = PathUtils.Combine(FSO.PublishmentSystemPath, fileName);
            //            FileUtils.MoveFile(filePath, destFilePath, isOverride);
            //        }
            //    }

            //    ArrayList publishmentSystemDirArrayList = DataProvider.PublishmentSystemDAO.GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters();

            //    string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(siteTemplatePath);
            //    foreach (string subDirectoryPath in directoryPaths)
            //    {
            //        string directoryName = PathUtils.GetDirectoryName(subDirectoryPath);
            //        if (!PathUtility.IsSystemDirectory(directoryName) && !publishmentSystemDirArrayList.Contains(directoryName.ToLower()))
            //        {
            //            string destDirectoryPath = PathUtils.Combine(FSO.PublishmentSystemPath, directoryName);
            //            DirectoryUtils.MoveDirectory(subDirectoryPath, destDirectoryPath, isOverride);
            //        }
            //    }
            //}
            //else
            //{
            //    DirectoryUtils.MoveDirectory(siteTemplatePath, FSO.PublishmentSystemPath, isOverride);
            //}
            //string siteTemplateMetadataPath = PathUtils.Combine(FSO.PublishmentSystemPath, DirectoryUtility.SiteTemplates.SiteTemplateMetadata);
            //DirectoryUtils.DeleteDirectoryIfExists(siteTemplateMetadataPath);
            DirectoryUtility.ImportPublishmentSystemFiles(this.FSO.PublishmentSystemInfo, siteTemplatePath, isOverride);
        }

        public void ImportSiteContent(string siteContentDirectoryPath, string filePath, bool isImportContents)
        {
            SiteContentIE siteContentIE = new SiteContentIE(FSO.PublishmentSystemInfo, siteContentDirectoryPath);
            siteContentIE.ImportChannelsAndContents(filePath, isImportContents, false, 0);
        }


        /// <summary>
        /// 从指定的地址导入网站模板至应用中
        /// </summary>
        /// <param name="filePath">指定的导入地址</param>
        /// <param name="overwrite">是否覆盖原有模板</param>
        public void ImportTemplates(string filePath, bool overwrite)
        {
            TemplateIE templateIE = new TemplateIE(FSO.PublishmentSystemID, filePath);
            templateIE.ImportTemplates(overwrite);
        }



        /// <summary>
        /// 从指定的地址导入网站菜单显示方式至应用中
        /// </summary>
        /// <param name="filePath">指定的导入地址</param>
        /// <param name="overwrite">是否覆盖原有菜单显示方式</param>
        public void ImportMenuDisplay(string filePath, bool overwrite)
        {
            MenuDisplayIE menuDisplayIE = new MenuDisplayIE(FSO.PublishmentSystemID, filePath);
            menuDisplayIE.ImportMenuDisplay(overwrite);
        }

        public void ImportTagStyle(string filePath, bool overwrite)
        {
            TagStyleIE tagStyleIE = new TagStyleIE(FSO.PublishmentSystemID, filePath);
            tagStyleIE.ImportTagStyle(overwrite);
        }


        /// <summary>
        /// 从指定的地址导入固定广告至应用中
        /// </summary>
        /// <param name="filePath">指定的导入地址</param>
        /// <param name="overwrite">是否覆盖原有固定广告</param>
        public void ImportAd(string filePath, bool overwrite)
        {
            AdvIE adIE = new AdvIE(FSO.PublishmentSystemID, filePath);
            adIE.ImportAd(overwrite);
        }

        /// <summary>
        /// 从指定的地址导入搜索引擎应用中
        /// </summary>
        /// <param name="filePath">指定的导入地址</param>
        /// <param name="overwrite">是否覆盖原有搜索引擎</param>
        public void ImportSeo(string filePath, bool overwrite)
        {
            SeoIE seoIE = new SeoIE(FSO.PublishmentSystemID, filePath);
            seoIE.ImportSeo(overwrite);
        }

        /// <summary>
        /// 从指定的地址导入自定义模板语言应用中
        /// </summary>
        /// <param name="filePath">指定的导入地址</param>
        /// <param name="overwrite">是否覆盖原有自定义模板语言</param>
        public void ImportStlTag(string filePath, bool overwrite)
        {
            StlTagIE stlTagIE = new StlTagIE(FSO.PublishmentSystemID, filePath);
            stlTagIE.ImportStlTag(overwrite);
        }

        /// <summary>
        /// 从指定的地址导入采集规则至应用中
        /// </summary>
        /// <param name="filePath">指定的导入地址</param>
        /// <param name="overwrite">是否覆盖原有菜单显示方式</param>
        public void ImportGatherRule(string filePath, bool overwrite)
        {
            GatherRuleIE gatherRuleIE = new GatherRuleIE(FSO.PublishmentSystemID, filePath);
            gatherRuleIE.ImportGatherRule(overwrite);
        }


        public void ImportInput(string inputDirectoryPath, bool overwrite)
        {
            if (DirectoryUtils.IsDirectoryExists(inputDirectoryPath))
            {
                InputIE inputIE = new InputIE(FSO.PublishmentSystemID, inputDirectoryPath);
                inputIE.ImportInput(overwrite);
            }
        }

        public void ImportInputByZipFile(string zipFilePath, bool overwrite)
        {
            string directoryPath = PathUtils.GetTemporaryFilesPath("Input");
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

            ZipUtils.UnpackFiles(zipFilePath, directoryPath);

            InputIE inputIE = new InputIE(FSO.PublishmentSystemID, directoryPath);
            inputIE.ImportInput(overwrite);
        }

        /// <summary>
        /// by 20151029 sofuny
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="overwrite"></param>
        /// <param name="itemID"></param>
        public void ImportInputByZipFile(string zipFilePath, bool overwrite, int itemID)
        {
            string directoryPath = PathUtils.GetTemporaryFilesPath("Input");
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

            ZipUtils.UnpackFiles(zipFilePath, directoryPath);

            InputIE inputIE = new InputIE(FSO.PublishmentSystemID, directoryPath);
            inputIE.ImportInput(overwrite, itemID);
        }

        public void ImportRelatedFieldByZipFile(string zipFilePath, bool overwrite)
        {
            string directoryPath = PathUtils.GetTemporaryFilesPath("RelatedField");
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

            ZipUtils.UnpackFiles(zipFilePath, directoryPath);

            RelatedFieldIE relatedFieldIE = new RelatedFieldIE(FSO.PublishmentSystemID, directoryPath);
            relatedFieldIE.ImportRelatedField(overwrite);
        }

        public void ImportAuxiliaryTables(string tableDirectoryPath, bool isUseTables)
        {
            NameValueCollection nameValueCollection = null;
            if (DirectoryUtils.IsDirectoryExists(tableDirectoryPath))
            {
                AuxiliaryTableIE tableIE = new AuxiliaryTableIE(tableDirectoryPath);
                nameValueCollection = tableIE.ImportAuxiliaryTables(this.FSO.PublishmentSystemID, isUseTables);
            }
            this.SaveTableNameCache(nameValueCollection);
        }


        public void ImportTableStyles(string tableDirectoryPath)
        {
            if (DirectoryUtils.IsDirectoryExists(tableDirectoryPath))
            {
                TableStyleIE tableStyleIE = new TableStyleIE(tableDirectoryPath);
                tableStyleIE.ImportTableStyles(this.FSO.PublishmentSystemID);
            }
        }

        public void ImportTableStyleByZipFile(ETableStyle tableStyle, string tableName, int nodeID, string zipFilePath)
        {
            string styleDirectoryPath = PathUtils.GetTemporaryFilesPath("TableStyle");
            DirectoryUtils.DeleteDirectoryIfExists(styleDirectoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(styleDirectoryPath);

            ZipUtils.UnpackFiles(zipFilePath, styleDirectoryPath);

            TableStyleIE.SingleImportTableStyle(tableStyle, tableName, styleDirectoryPath, this.FSO.PublishmentSystemID, nodeID);
        }

        public void ImportConfiguration(string configurationFilePath)
        {
            ConfigurationIE configIE = new ConfigurationIE(FSO.PublishmentSystemID, configurationFilePath);
            configIE.Import();
        }


        public void ImportChannelsAndContentsByZipFile(int parentID, string zipFilePath, bool isOverride)
        {
            string siteContentDirectoryPath = PathUtils.GetTemporaryFilesPath(EBackupTypeUtils.GetValue(EBackupType.ChannelsAndContents));
            DirectoryUtils.DeleteDirectoryIfExists(siteContentDirectoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(siteContentDirectoryPath);

            ZipUtils.UnpackFiles(zipFilePath, siteContentDirectoryPath);

            this.ImportChannelsAndContentsFromZip(parentID, siteContentDirectoryPath, isOverride);

            DataProvider.NodeDAO.UpdateContentNum(this.FSO.PublishmentSystemInfo);
        }

        public void ImportChannelsAndContentsFromZip(int parentID, string siteContentDirectoryPath, bool isOverride)
        {
            ArrayList filePathArrayList = ImportObject.GetSiteContentFilePathArrayList(siteContentDirectoryPath);

            SiteContentIE siteContentIE = new SiteContentIE(FSO.PublishmentSystemInfo, siteContentDirectoryPath);

            Hashtable levelHashtable = null;
            foreach (string filePath in filePathArrayList)
            {
                int firstIndex = filePath.LastIndexOf(PathUtils.SeparatorChar) + 1;
                int lastIndex = filePath.LastIndexOf(".");
                string orderString = filePath.Substring(firstIndex, lastIndex - firstIndex);

                int level = StringUtils.GetCount("_", orderString);

                if (levelHashtable == null)
                {
                    levelHashtable = new Hashtable();
                    levelHashtable[level] = parentID;
                }

                int insertNodeID = siteContentIE.ImportChannelsAndContents(filePath, true, isOverride, (int)levelHashtable[level]);
                levelHashtable[level + 1] = insertNodeID;
            }
        }

        public void ImportChannelsAndContents(int parentID, string siteContentDirectoryPath, bool isOverride)
        {
            ArrayList filePathArrayList = ImportObject.GetSiteContentFilePathArrayList(siteContentDirectoryPath);

            SiteContentIE siteContentIE = new SiteContentIE(FSO.PublishmentSystemInfo, siteContentDirectoryPath);

            string parentOrderString = "none";
            //int parentID = 0;
            foreach (string filePath in filePathArrayList)
            {
                int firstIndex = filePath.LastIndexOf(PathUtils.SeparatorChar) + 1;
                int lastIndex = filePath.LastIndexOf(".");
                string orderString = filePath.Substring(firstIndex, lastIndex - firstIndex);

                if (StringUtils.StartsWithIgnoreCase(orderString, parentOrderString))
                {
                    parentID = siteContentIE.ImportChannelsAndContents(filePath, true, isOverride, parentID);
                    parentOrderString = orderString;
                }
                else
                {
                    siteContentIE.ImportChannelsAndContents(filePath, true, isOverride, parentID);
                }
            }
        }

        public void ImportContentsByZipFile(NodeInfo nodeInfo, string zipFilePath, bool isOverride, int importStart, int importCount, bool isChecked, int checkedLevel)
        {
            string siteContentDirectoryPath = PathUtils.GetTemporaryFilesPath("contents");
            DirectoryUtils.DeleteDirectoryIfExists(siteContentDirectoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(siteContentDirectoryPath);

            ZipUtils.UnpackFiles(zipFilePath, siteContentDirectoryPath);

            string tableName = NodeManager.GetTableName(this.FSO.PublishmentSystemInfo, nodeInfo);

            int taxis = BaiRongDataProvider.ContentDAO.GetMaxTaxis(tableName, nodeInfo.NodeID, false);

            this.ImportContents(nodeInfo, siteContentDirectoryPath, isOverride, taxis, importStart, importCount, isChecked, checkedLevel);

            DataProvider.NodeDAO.UpdateContentNum(this.FSO.PublishmentSystemInfo);
        }

        public void ImportContentsByAccessFile(int nodeID, string excelFilePath, bool isOverride, int importStart, int importCount, bool isChecked, int checkedLevel)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.FSO.PublishmentSystemID, nodeID);
            ArrayList contentInfoArrayList = AccessObject.GetContentsByAccessFile(excelFilePath, this.FSO.PublishmentSystemInfo, nodeInfo);

            if (importStart > 1 || importCount > 0)
            {
                ArrayList theArrayList = new ArrayList();

                if (importStart == 0)
                {
                    importStart = 1;
                }
                if (importCount == 0)
                {
                    importCount = contentInfoArrayList.Count;
                }

                int firstIndex = contentInfoArrayList.Count - importStart - importCount + 1;
                if (firstIndex <= 0)
                {
                    firstIndex = 0;
                }

                int addCount = 0;
                for (int i = 0; i < contentInfoArrayList.Count; i++)
                {
                    if (addCount >= importCount) break;
                    if (i >= firstIndex)
                    {
                        theArrayList.Add(contentInfoArrayList[i]);
                        addCount++;
                    }
                }

                contentInfoArrayList = theArrayList;
            }

            string tableName = NodeManager.GetTableName(this.FSO.PublishmentSystemInfo, nodeInfo);

            foreach (BackgroundContentInfo contentInfo in contentInfoArrayList)
            {
                contentInfo.IsChecked = isChecked;
                contentInfo.CheckedLevel = checkedLevel;

                //contentInfo.ID = DataProvider.ContentDAO.Insert(tableName, this.FSO.PublishmentSystemInfo, contentInfo);
                if (isOverride)
                {
                    if (contentInfo.ID > 0)
                    {
                        DataProvider.ContentDAO.Update(tableName, FSO.PublishmentSystemInfo, contentInfo);
                    }
                    else
                    {
                        ArrayList existsIDs = new ArrayList();
                        existsIDs = DataProvider.ContentDAO.GetIDListBySameTitleInOneNode(tableName, contentInfo.NodeID, contentInfo.Title);
                        if (existsIDs.Count > 0)
                        {
                            foreach (int id in existsIDs)
                            {
                                contentInfo.ID = id;
                                DataProvider.ContentDAO.Update(tableName, FSO.PublishmentSystemInfo, contentInfo);
                            }
                        }
                        else
                        {
                            contentInfo.ID = DataProvider.ContentDAO.Insert(tableName, FSO.PublishmentSystemInfo, contentInfo);
                        }
                    }
                }
                else
                {
                    contentInfo.ID = DataProvider.ContentDAO.Insert(tableName, FSO.PublishmentSystemInfo, contentInfo);
                }
            }

            try
            {
                //批量导入内容，不做ajaxUrl处理
                //foreach (BackgroundContentInfo contentInfo in contentInfoArrayList)
                //{
                //    this.FSO.AddContentToWaitingCreate(contentInfo.NodeID, contentInfo.ID);
                //}

                //this.FSO.AddChannelToWaitingCreate(nodeInfo.NodeID);
            }
            catch { }
        }

        public void ImportContentsByExcelFile(int nodeID, string excelFilePath, bool isOverride, int importStart, int importCount, bool isChecked, int checkedLevel)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.FSO.PublishmentSystemID, nodeID);
            ArrayList contentInfoArrayList = ExcelObject.GetContentsByExcelFile(excelFilePath, this.FSO.PublishmentSystemInfo, nodeInfo);

            if (importStart > 1 || importCount > 0)
            {
                ArrayList theArrayList = new ArrayList();

                if (importStart == 0)
                {
                    importStart = 1;
                }
                if (importCount == 0)
                {
                    importCount = contentInfoArrayList.Count;
                }

                int firstIndex = contentInfoArrayList.Count - importStart - importCount + 1;
                if (firstIndex <= 0)
                {
                    firstIndex = 0;
                }

                int addCount = 0;
                for (int i = 0; i < contentInfoArrayList.Count; i++)
                {
                    if (addCount >= importCount) break;
                    if (i >= firstIndex)
                    {
                        theArrayList.Add(contentInfoArrayList[i]);
                        addCount++;
                    }
                }

                contentInfoArrayList = theArrayList;
            }

            string tableName = NodeManager.GetTableName(this.FSO.PublishmentSystemInfo, nodeInfo);

            foreach (BackgroundContentInfo contentInfo in contentInfoArrayList)
            {
                contentInfo.IsChecked = isChecked;
                contentInfo.CheckedLevel = checkedLevel;
                if (isOverride)
                {
                    if (contentInfo.ID > 0)
                    {
                        DataProvider.ContentDAO.Update(tableName, FSO.PublishmentSystemInfo, contentInfo);
                    }
                    else
                    {
                        ArrayList existsIDs = new ArrayList();
                        existsIDs = DataProvider.ContentDAO.GetIDListBySameTitleInOneNode(tableName, contentInfo.NodeID, contentInfo.Title);
                        if (existsIDs.Count > 0)
                        {
                            foreach (int id in existsIDs)
                            {
                                contentInfo.ID = id;
                                DataProvider.ContentDAO.Update(tableName, FSO.PublishmentSystemInfo, contentInfo);
                            }
                        }
                        else
                        {
                            contentInfo.ID = DataProvider.ContentDAO.Insert(tableName, FSO.PublishmentSystemInfo, contentInfo);
                        }
                    }
                }
                else
                {
                    contentInfo.ID = DataProvider.ContentDAO.Insert(tableName, FSO.PublishmentSystemInfo, contentInfo);
                }
                //this.FSO.AddContentToWaitingCreate(contentInfo.NodeID, contentID);
            }
        }

        public void ImportContentsByTxtZipFile(int nodeID, string zipFilePath, bool isOverride, int importStart, int importCount, bool isChecked, int checkedLevel)
        {
            string directoryPath = PathUtils.GetTemporaryFilesPath("contents");
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

            ZipUtils.UnpackFiles(zipFilePath, directoryPath);

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.FSO.PublishmentSystemID, nodeID);

            ArrayList contentInfoArrayList = TxtObject.GetContentsByTxtFile(directoryPath, this.FSO.PublishmentSystemInfo, nodeInfo);

            if (importStart > 1 || importCount > 0)
            {
                ArrayList theArrayList = new ArrayList();

                if (importStart == 0)
                {
                    importStart = 1;
                }
                if (importCount == 0)
                {
                    importCount = contentInfoArrayList.Count;
                }

                int firstIndex = contentInfoArrayList.Count - importStart - importCount + 1;
                if (firstIndex <= 0)
                {
                    firstIndex = 0;
                }

                int addCount = 0;
                for (int i = 0; i < contentInfoArrayList.Count; i++)
                {
                    if (addCount >= importCount) break;
                    if (i >= firstIndex)
                    {
                        theArrayList.Add(contentInfoArrayList[i]);
                        addCount++;
                    }
                }

                contentInfoArrayList = theArrayList;
            }

            string tableName = NodeManager.GetTableName(this.FSO.PublishmentSystemInfo, nodeInfo);

            foreach (BackgroundContentInfo contentInfo in contentInfoArrayList)
            {
                contentInfo.IsChecked = isChecked;
                contentInfo.CheckedLevel = checkedLevel;

                //int contentID = DataProvider.ContentDAO.Insert(tableName, this.FSO.PublishmentSystemInfo, contentInfo);

                if (isOverride)
                {
                    if (contentInfo.ID > 0)
                    {
                        DataProvider.ContentDAO.Update(tableName, FSO.PublishmentSystemInfo, contentInfo);
                    }
                    else
                    {
                        ArrayList existsIDs = new ArrayList();
                        existsIDs = DataProvider.ContentDAO.GetIDListBySameTitleInOneNode(tableName, contentInfo.NodeID, contentInfo.Title);
                        if (existsIDs.Count > 0)
                        {
                            foreach (int id in existsIDs)
                            {
                                contentInfo.ID = id;
                                DataProvider.ContentDAO.Update(tableName, FSO.PublishmentSystemInfo, contentInfo);
                            }
                        }
                        else
                        {
                            contentInfo.ID = DataProvider.ContentDAO.Insert(tableName, FSO.PublishmentSystemInfo, contentInfo);
                        }
                    }
                }
                else
                {
                    contentInfo.ID = DataProvider.ContentDAO.Insert(tableName, FSO.PublishmentSystemInfo, contentInfo);
                }

                //this.FSO.AddContentToWaitingCreate(contentInfo.NodeID, contentID);
            }
        }

        public void ImportContents(NodeInfo nodeInfo, string siteContentDirectoryPath, bool isOverride, int taxis, int importStart, int importCount, bool isChecked, int checkedLevel)
        {
            string filePath = PathUtils.Combine(siteContentDirectoryPath, "contents.xml");

            SiteContentIE siteContentIE = new SiteContentIE(FSO.PublishmentSystemInfo, siteContentDirectoryPath);

            siteContentIE.ImportContents(filePath, isOverride, nodeInfo, taxis, importStart, importCount, isChecked, checkedLevel);

            FileUtils.DeleteFileIfExists(filePath);

            DirectoryUtils.MoveDirectory(siteContentDirectoryPath, this.FSO.PublishmentSystemPath, isOverride);
        }

        public void ImportInputContentsByExcelFile(InputInfo inputInfo, string excelFilePath, int importStart, int importCount, bool isChecked)
        {
            ArrayList contentInfoArrayList = ExcelObject.GetInputContentsByExcelFile(excelFilePath, this.FSO.PublishmentSystemInfo, inputInfo);

            if (importStart > 1 || importCount > 0)
            {
                ArrayList theArrayList = new ArrayList();

                if (importStart == 0)
                {
                    importStart = 1;
                }
                if (importCount == 0)
                {
                    importCount = contentInfoArrayList.Count;
                }

                int firstIndex = contentInfoArrayList.Count - importStart - importCount + 1;
                if (firstIndex <= 0)
                {
                    firstIndex = 0;
                }

                int addCount = 0;
                for (int i = 0; i < contentInfoArrayList.Count; i++)
                {
                    if (addCount >= importCount) break;
                    if (i >= firstIndex)
                    {
                        theArrayList.Add(contentInfoArrayList[i]);
                        addCount++;
                    }
                }

                contentInfoArrayList = theArrayList;
            }

            foreach (InputContentInfo contentInfo in contentInfoArrayList)
            {
                contentInfo.IsChecked = isChecked;
                DataProvider.InputContentDAO.Insert(contentInfo);
            }
        }

        public static ArrayList GetSiteContentFilePathArrayList(string siteContentDirectoryPath)
        {
            string[] filePaths = DirectoryUtils.GetFilePaths(siteContentDirectoryPath);
            SortedList filePathSortedList = new SortedList();
            foreach (string filePath in filePaths)
            {
                StringBuilder keyBuilder = new StringBuilder();
                string fileName = PathUtils.GetFileName(filePath).ToLower().Replace(".xml", "");
                string[] nums = fileName.Split('_');
                foreach (string numStr in nums)
                {
                    int count = 7 - numStr.Length;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            keyBuilder.Append("0");
                        }
                    }
                    keyBuilder.Append(numStr);
                    keyBuilder.Append("_");
                }
                if (keyBuilder.Length > 0) keyBuilder.Remove(keyBuilder.Length - 1, 1);
                filePathSortedList.Add(keyBuilder.ToString(), filePath);
            }
            ArrayList filePathArrayList = new ArrayList(filePathSortedList.Values);
            return filePathArrayList;
        }

        public void ImportB2CSettings(string settingsDirectoryPath)
        {
            if (DirectoryUtils.IsDirectoryExists(settingsDirectoryPath))
            {
                B2CSettingsIE settingsIE = new B2CSettingsIE(FSO.PublishmentSystemID, settingsDirectoryPath);
                settingsIE.ImportSettings();
            }
        }

        #region

        public void ImportMap(string filePath)
        {
            MapIE mapIE = new MapIE(FSO.PublishmentSystemID, filePath);
            mapIE.Import();
        }

        public void ImportCoupon(string filePath)
        {
            CouponIE couponIE = new CouponIE(FSO.PublishmentSystemID, filePath);
            couponIE.Import();
        }

        public void ImportLottery(string filePath)
        {
            LotteryIE lotteryIE = new LotteryIE(FSO.PublishmentSystemID, filePath);
            lotteryIE.Import();
        }

        public void ImportVote(string filePath)
        {
            VoteIE voteIE = new VoteIE(FSO.PublishmentSystemID, filePath);
            voteIE.Import();
        }

        public void ImportMessage(string filePath)
        {
            MessageIE messageIE = new MessageIE(FSO.PublishmentSystemID, filePath);
            messageIE.Import();
        }

        public void ImportConference(string filePath)
        {
            ConferenceIE conferenceIE = new ConferenceIE(FSO.PublishmentSystemID, filePath);
            conferenceIE.Import();
        }

        public void ImportAlbum(string filePath)
        {
            AlbumIE albumIE = new AlbumIE(FSO.PublishmentSystemID, filePath);
            albumIE.Import();
        }

        public void ImportSearch(string filePath)
        {
            SearchIE searchIE = new SearchIE(FSO.PublishmentSystemID, filePath);
            searchIE.Import();
        }

        public void ImportAppointment(string filePath)
        {
            AppointmentIE appointmentIE = new AppointmentIE(FSO.PublishmentSystemID, filePath);
            appointmentIE.Import();
        }

        public void ImportStore(string filePath)
        {
            StoreIE storeIE = new StoreIE(FSO.PublishmentSystemID, filePath);
            storeIE.Import();
        }

        public void ImportCollect(string filePath)
        {
            CollectIE collectIE = new CollectIE(FSO.PublishmentSystemID, filePath);
            collectIE.Import();
        }

        public void ImportCard(string filePath)
        {
            CardIE cardIE = new CardIE(FSO.PublishmentSystemID, filePath);
            cardIE.Import();
        }

        public void ImportView360(string filePath)
        {
            View360IE view360IE = new View360IE(FSO.PublishmentSystemID, filePath);
            view360IE.Import();
        }
        public void ImportWifi(string filePath)
        {
            WifiIE wifiIE = new WifiIE(FSO.PublishmentSystemID, filePath);
            wifiIE.Import();
        }

        public void ImportWifiNode(string filePath)
        {
            WifiNodeIE wifiNodeIE = new WifiNodeIE(FSO.PublishmentSystemID, filePath);
            wifiNodeIE.Import();
        }

        public void ImportAccount(string filePath)
        {
            AccountIE accountIE = new AccountIE(FSO.PublishmentSystemID, filePath);
            accountIE.Import();
        }

        public void ImportCount(string filePath)
        {
            CountIE countIE = new CountIE(FSO.PublishmentSystemID, filePath);
            countIE.Import();
        }

        public void ImportMenu(string filePath)
        {
            MenuIE menuIE = new MenuIE(FSO.PublishmentSystemID, filePath);
            menuIE.Import();
        }

        public void ImportWebMenu(string filePath)
        {
            WebMenuIE webMenuIE = new WebMenuIE(FSO.PublishmentSystemID, filePath);
            webMenuIE.Import();
        }

        public void ImportKeyWord(string filePath)
        {
            KeyWordIE keyWordIE = new KeyWordIE(FSO.PublishmentSystemID, filePath);
            keyWordIE.Import();
        }

        #endregion

        /// <summary>
        /// 从指定的地址导入内容模型至应用中
        /// </summary>
        /// <param name="filePath">指定的导入地址</param>
        /// <param name="overwrite">是否覆盖原有内容模型</param>
        public void ImportContentModel(string filePath, bool overwrite)
        {
            ContentModelIE contentModelIE = new ContentModelIE(FSO.PublishmentSystemID, filePath, EPublishmentSystemTypeUtils.GetAppID(FSO.PublishmentSystemInfo.PublishmentSystemType));
            contentModelIE.ImportContentModelInfo(overwrite);
        }

    }
}
