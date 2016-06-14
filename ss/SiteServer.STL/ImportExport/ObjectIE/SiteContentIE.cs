using System;
using System.Collections;
using System.Collections.Specialized;
using Atom.AdditionalElements;
using Atom.Core;
using Atom.Core.Collections;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using SiteServer.STL.IO;
using SiteServer.CMS.Core;
using SiteServer.B2C.Core;
using System.Collections.Generic;
using SiteServer.B2C.Model;
using SiteServer.STL.ImportExport.B2C;

namespace SiteServer.STL.ImportExport
{
    internal class SiteContentIE
    {
        readonly PublishmentSystemInfo publishmentSystemInfo;
        readonly FileSystemObject FSO;

        //保存除内容表本身字段外的属性
        private const string CHANNEL_TEMPLATE_NAME = "ChannelTemplateName";
        private const string CONTENT_TEMPLATE_NAME = "ContentTemplateName";

        private readonly string siteContentDirectoryPath;
        private PhotoIE photoIE;
        private SpecIE specIE;
        private FilterIE filterIE;
        private GoodsIE goodsIE;
        private Dictionary<int, int> specIDDictionary;
        private Dictionary<int, int> itemIDDictionary;

        public SiteContentIE(PublishmentSystemInfo publishmentSystemInfo, string siteContentDirectoryPath)
        {
            this.siteContentDirectoryPath = siteContentDirectoryPath;
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.FSO = new FileSystemObject(this.publishmentSystemInfo.PublishmentSystemID);

            string photoDirectoryPath = PathUtils.Combine(siteContentDirectoryPath, DirectoryUtils.SiteTemplates.Photo);
            DirectoryUtils.CreateDirectoryIfNotExists(photoDirectoryPath);
            this.photoIE = new PhotoIE(this.publishmentSystemInfo, photoDirectoryPath);

            string specDirectoryPath = PathUtils.Combine(siteContentDirectoryPath, DirectoryUtils.SiteTemplates.Spec);
            DirectoryUtils.CreateDirectoryIfNotExists(specDirectoryPath);
            this.specIE = new SpecIE(this.publishmentSystemInfo.PublishmentSystemID, specDirectoryPath);

            string filterDirectoryPath = PathUtils.Combine(siteContentDirectoryPath, DirectoryUtils.SiteTemplates.Filter);
            DirectoryUtils.CreateDirectoryIfNotExists(filterDirectoryPath);
            this.filterIE = new FilterIE(this.publishmentSystemInfo, filterDirectoryPath);

            string goodsDirectoryPath = PathUtils.Combine(siteContentDirectoryPath, DirectoryUtils.SiteTemplates.Goods);
            DirectoryUtils.CreateDirectoryIfNotExists(goodsDirectoryPath);
            this.goodsIE = new GoodsIE(this.publishmentSystemInfo, goodsDirectoryPath);
        }

        public Dictionary<int, int> SpecIDDictionary
        {
            get
            {
                if (this.specIDDictionary == null)
                {
                    this.specIDDictionary = IECacheUtils.GetSpecIDDictionary();
                }
                return this.specIDDictionary;
            }
        }

        public Dictionary<int, int> ItemIDDictionary
        {
            get
            {
                if (this.itemIDDictionary == null)
                {
                    this.itemIDDictionary = IECacheUtils.GetItemIDDictionary();
                }
                return this.itemIDDictionary;
            }
        }

        public int ImportChannelsAndContents(string filePath, bool isImportContents, bool isOverride, int theParentID)
        {
            int psChildCount = DataProvider.NodeDAO.GetNodeCount(publishmentSystemInfo.PublishmentSystemID);
            ArrayList nodeIndexNameArrayList = DataProvider.NodeDAO.GetNodeIndexNameArrayList(publishmentSystemInfo.PublishmentSystemID);

            if (!FileUtils.IsFileExists(filePath)) return 0;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            int firstIndex = filePath.LastIndexOf(PathUtils.SeparatorChar) + 1;
            int lastIndex = filePath.LastIndexOf(".");
            string orderString = filePath.Substring(firstIndex, lastIndex - firstIndex);

            int idx = orderString.IndexOf("_");
            if (idx != -1)
            {
                int secondOrder = TranslateUtils.ToInt(orderString.Split(new char[] { '_' })[1]);
                secondOrder = secondOrder + psChildCount;
                orderString = orderString.Substring(idx + 1);
                idx = orderString.IndexOf("_");
                if (idx != -1)
                {
                    orderString = orderString.Substring(idx);
                    orderString = "1_" + secondOrder + orderString;
                }
                else
                {
                    orderString = "1_" + secondOrder;
                }

                orderString = orderString.Substring(0, orderString.LastIndexOf("_"));
            }

            int parentID = DataProvider.NodeDAO.GetNodeID(publishmentSystemInfo.PublishmentSystemID, orderString);
            if (theParentID != 0)
            {
                parentID = theParentID;
            }

            int nodeIDFromFile = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, NodeAttribute.NodeID));
            ENodeType nodeType = ENodeTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(feed.AdditionalElements, NodeAttribute.NodeType));
            int nodeID = 0;
            if (nodeType == ENodeType.BackgroundPublishNode)
            {
                nodeID = publishmentSystemInfo.PublishmentSystemID;
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemID);
                this.ImportNodeInfo(nodeInfo, feed.AdditionalElements, parentID, nodeIndexNameArrayList);

                DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                if (EContentModelTypeUtils.Equals(EContentModelType.Goods, nodeInfo.ContentModelID))
                {
                    if (nodeInfo.Additional.SpecCount > 0)
                    {
                        this.specIE.ImportSpec(nodeIDFromFile, nodeID, true);
                    }

                    if (nodeInfo.Additional.FilterCount > 0)
                    {
                        this.filterIE.ImportFilter(nodeIDFromFile, nodeID);
                    }
                }

                if (isImportContents)
                {
                    this.ImportContents(feed.Entries, nodeInfo, 0, isOverride);
                }
            }
            else
            {
                NodeInfo nodeInfo = new NodeInfo();
                this.ImportNodeInfo(nodeInfo, feed.AdditionalElements, parentID, nodeIndexNameArrayList);
                if (string.IsNullOrEmpty(nodeInfo.NodeName)) return 0;

                bool isUpdate = false;
                int theSameNameNodeID = 0;
                if (isOverride)
                {
                    theSameNameNodeID = DataProvider.NodeDAO.GetNodeIDByParentIDAndNodeName(publishmentSystemInfo.PublishmentSystemID, parentID, nodeInfo.NodeName, false);
                    if (theSameNameNodeID != 0)
                    {
                        isUpdate = true;
                    }
                }
                if (!isUpdate)
                {
                    //BackgroundNodeInfo backgroundNodeInfo = new BackgroundNodeInfo();
                    //this.ImportBackgroundNodeInfo(backgroundNodeInfo, feed.AdditionalElements);

                    nodeID = DataProvider.NodeDAO.InsertNodeInfo(nodeInfo);
                }
                else
                {
                    nodeID = theSameNameNodeID;
                    nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, theSameNameNodeID);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                    this.ImportNodeInfo(nodeInfo, feed.AdditionalElements, parentID, nodeIndexNameArrayList);

                    DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                    DataProvider.ContentDAO.DeleteContentsByNodeID(publishmentSystemInfo.PublishmentSystemID, tableName, theSameNameNodeID);
                }

                if (EContentModelTypeUtils.Equals(EContentModelType.Goods, nodeInfo.ContentModelID))
                {
                    if (nodeInfo.Additional.SpecCount > 0)
                    {
                        this.specIE.ImportSpec(nodeIDFromFile, nodeID, true);
                    }

                    if (nodeInfo.Additional.FilterCount > 0)
                    {
                        this.filterIE.ImportFilter(nodeIDFromFile, nodeID);
                    }
                }

                if (isImportContents)
                {
                    this.ImportContents(feed.Entries, nodeInfo, 0, isOverride);
                }
            }

            //this.FSO.CreateRedirectChannel(nodeID);
            //this.FSO.AddChannelToWaitingCreate(nodeID);
            return nodeID;
        }

        public void ImportContents(string filePath, bool isOverride, NodeInfo nodeInfo, int taxis, int importStart, int importCount, bool isChecked, int checkedLevel)
        {
            if (!FileUtils.IsFileExists(filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            this.ImportContents(feed.Entries, nodeInfo, taxis, importStart, importCount, false, isChecked, checkedLevel, isOverride);

            //this.FSO.CreateRedirectChannel(nodeID);
            //this.FSO.AddChannelToWaitingCreate(nodeInfo.NodeID);
        }

        private void ImportContents(AtomEntryCollection entries, NodeInfo nodeInfo, int taxis, bool isOverride)
        {
            this.ImportContents(entries, nodeInfo, taxis, 0, 0, true, true, 0, isOverride);
        }

        // 内部消化掉错误
        private void ImportContents(AtomEntryCollection entries, NodeInfo nodeInfo, int taxis, int importStart, int importCount, bool isCheckedBySettings, bool isChecked, int checkedLevel, bool isOverride)
        {
            if (importStart > 1 || importCount > 0)
            {
                AtomEntryCollection theEntries = new AtomEntryCollection();

                if (importStart == 0)
                {
                    importStart = 1;
                }
                if (importCount == 0)
                {
                    importCount = entries.Count;
                }

                int firstIndex = entries.Count - importStart - importCount + 1;
                if (firstIndex <= 0)
                {
                    firstIndex = 0;
                }

                int addCount = 0;
                for (int i = 0; i < entries.Count; i++)
                {
                    if (addCount >= importCount) break;
                    if (i >= firstIndex)
                    {
                        theEntries.Add(entries[i]);
                        addCount++;
                    }
                }

                entries = theEntries;
            }

            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

            foreach (AtomEntry entry in entries)
            {
                try
                {
                    taxis++;
                    int contentIDFromFile = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.ID));
                    string lastEditDate = AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.LastEditDate);
                    string contentGroupNameCollection = AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.ContentGroupNameCollection);
                    if (isCheckedBySettings)
                    {
                        isChecked = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.IsChecked));
                        checkedLevel = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.CheckedLevel));
                    }
                    int comments = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.Comments));
                    int photos = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.Photos));
                    int hits = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.Hits));
                    int hitsByDay = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.HitsByDay));
                    int hitsByWeek = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.HitsByWeek));
                    int hitsByMonth = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.HitsByMonth));
                    string lastHitsDate = AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.LastHitsDate);
                    string title = AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.Title);
                    bool isTop = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.IsTop));
                    string addDate = AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.AddDate);

                    int topTaxis = 0;
                    if (isTop)
                    {
                        topTaxis = taxis - 1;
                        taxis = BaiRongDataProvider.ContentDAO.GetMaxTaxis(tableName, nodeInfo.NodeID, true) + 1;
                    }
                    string tags = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, ContentAttribute.Tags));

                    string starSetting = AtomUtility.GetDcElementContent(entry.AdditionalElements, BackgroundContentAttribute.StarSetting);

                    ContentInfo contentInfo = new ContentInfo();
                    contentInfo.NodeID = nodeInfo.NodeID;
                    contentInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                    contentInfo.AddUserName = AdminManager.Current.UserName;
                    contentInfo.AddDate = TranslateUtils.ToDateTime(addDate);
                    contentInfo.LastEditUserName = contentInfo.AddUserName;
                    contentInfo.LastEditDate = TranslateUtils.ToDateTime(lastEditDate);
                    contentInfo.ContentGroupNameCollection = contentGroupNameCollection;
                    contentInfo.Tags = tags;
                    contentInfo.IsChecked = isChecked;
                    contentInfo.CheckedLevel = checkedLevel;
                    contentInfo.Comments = comments;
                    contentInfo.Photos = photos;
                    contentInfo.Hits = hits;
                    contentInfo.HitsByDay = hitsByDay;
                    contentInfo.HitsByWeek = hitsByWeek;
                    contentInfo.HitsByMonth = hitsByMonth;
                    contentInfo.LastHitsDate = TranslateUtils.ToDateTime(lastHitsDate);
                    contentInfo.Title = AtomUtility.Decrypt(title);
                    contentInfo.IsTop = isTop;

                    NameValueCollection attributes = AtomUtility.GetDcElementNameValueCollection(entry.AdditionalElements);
                    foreach (string attributeName in attributes.Keys)
                    {
                        if (!contentInfo.ContainsKey(attributeName.ToLower()))
                        {
                            contentInfo.Attributes[attributeName] = AtomUtility.Decrypt(attributes[attributeName]);
                        }
                    }

                    int skuCount = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(GoodsContentAttribute.SKUCount));
                    if (skuCount > 0)
                    {
                        string specIDCollection = contentInfo.GetExtendedAttribute(GoodsContentAttribute.SpecIDCollection);
                        if (!string.IsNullOrEmpty(specIDCollection))
                        {
                            List<int> specIDListFromFile = TranslateUtils.StringCollectionToIntList(specIDCollection);
                            List<int> specIDList = new List<int>();

                            foreach (int specIDFromFile in specIDListFromFile)
                            {
                                if (this.SpecIDDictionary.ContainsKey(specIDFromFile))
                                {
                                    specIDList.Add(this.SpecIDDictionary[specIDFromFile]);
                                }
                            }

                            contentInfo.SetExtendedAttribute(GoodsContentAttribute.SpecIDCollection, TranslateUtils.ObjectCollectionToString(specIDList));
                        }

                        string itemIDCollection = contentInfo.GetExtendedAttribute(GoodsContentAttribute.SpecItemIDCollection);
                        if (!string.IsNullOrEmpty(itemIDCollection))
                        {
                            List<int> itemIDListFromFile = TranslateUtils.StringCollectionToIntList(itemIDCollection);
                            List<int> itemIDList = new List<int>();

                            foreach (int itemIDFromFile in itemIDListFromFile)
                            {
                                if (this.ItemIDDictionary.ContainsKey(itemIDFromFile))
                                {
                                    itemIDList.Add(this.ItemIDDictionary[itemIDFromFile]);
                                }
                            }

                            contentInfo.SetExtendedAttribute(GoodsContentAttribute.SpecItemIDCollection, TranslateUtils.ObjectCollectionToString(itemIDList));
                        }
                    }
                    bool isInsert = false;
                    if (isOverride)
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
                            isInsert = true;
                        }
                    }
                    else
                    {
                        isInsert = true;
                    }

                    if (isInsert)
                    {
                        int contentID = DataProvider.ContentDAO.Insert(tableName, this.publishmentSystemInfo, contentInfo, false, taxis);
                        if (photos > 0)
                        {
                            this.photoIE.ImportPhoto(contentIDFromFile, contentID);
                        }

                        if (skuCount > 0)
                        {
                            this.goodsIE.ImportGoods(contentIDFromFile, contentID, this.SpecIDDictionary, this.ItemIDDictionary);
                        }

                        if (this.publishmentSystemInfo.Additional.IsRelatedByTags && !string.IsNullOrEmpty(tags))
                        {
                            StringCollection tagCollection = TagUtils.ParseTagsString(tags);
                            TagUtils.AddTags(tagCollection, AppManager.CMS.AppID, this.publishmentSystemInfo.PublishmentSystemID, contentID);
                        }

                        if (!string.IsNullOrEmpty(starSetting))
                        {
                            string[] settings = starSetting.Split('_');
                            if (settings != null && settings.Length == 2)
                            {
                                int totalCount = TranslateUtils.ToInt(settings[0]);
                                decimal pointAverage = TranslateUtils.ToDecimal(settings[1]);
                                StarsManager.SetStarSetting(this.publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentID, totalCount, pointAverage);
                            }
                        }
                    }
                    //this.FSO.AddContentToWaitingCreate(contentInfo.NodeID, contentID);

                    if (isTop)
                    {
                        taxis = topTaxis;
                    }
                }
                catch { }
            }
        }


        private void ImportNodeInfo(NodeInfo nodeInfo, ScopedElementCollection additionalElements, int parentID, IList nodeIndexNameArrayList)
        {
            nodeInfo.NodeName = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.NodeName);
            nodeInfo.NodeType = ENodeTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.NodeType));
            nodeInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
            string contentModelID = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.ContentModelID);
            if (!string.IsNullOrEmpty(contentModelID))
            {
                nodeInfo.ContentModelID = contentModelID;
            }
            nodeInfo.ParentID = parentID;
            string nodeIndexName = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.NodeIndexName);
            if (!string.IsNullOrEmpty(nodeIndexName) && nodeIndexNameArrayList.IndexOf(nodeIndexName) == -1)
            {
                nodeInfo.NodeIndexName = nodeIndexName;
                nodeIndexNameArrayList.Add(nodeIndexName);
            }
            nodeInfo.NodeGroupNameCollection = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.NodeGroupNameCollection);
            nodeInfo.AddDate = DateTime.Now;
            nodeInfo.ImageUrl = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.ImageUrl);
            nodeInfo.Content = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.Content));
            nodeInfo.FilePath = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.FilePath);
            nodeInfo.ChannelFilePathRule = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.ChannelFilePathRule);
            nodeInfo.ContentFilePathRule = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.ContentFilePathRule);

            nodeInfo.LinkUrl = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.LinkUrl);
            nodeInfo.LinkType = ELinkTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.LinkType));

            string channelTemplateName = AtomUtility.GetDcElementContent(additionalElements, SiteContentIE.CHANNEL_TEMPLATE_NAME);
            if (!string.IsNullOrEmpty(channelTemplateName))
            {
                nodeInfo.ChannelTemplateID = TemplateManager.GetTemplateIDByTemplateName(this.publishmentSystemInfo.PublishmentSystemID, ETemplateType.ChannelTemplate, channelTemplateName);
            }
            string contentTemplateName = AtomUtility.GetDcElementContent(additionalElements, SiteContentIE.CONTENT_TEMPLATE_NAME);
            if (!string.IsNullOrEmpty(contentTemplateName))
            {
                nodeInfo.ContentTemplateID = TemplateManager.GetTemplateIDByTemplateName(this.publishmentSystemInfo.PublishmentSystemID, ETemplateType.ContentTemplate, contentTemplateName);
            }

            nodeInfo.Keywords = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.Keywords);
            nodeInfo.Description = AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.Description);

            nodeInfo.SetExtendValues(AtomUtility.GetDcElementContent(additionalElements, NodeAttribute.ExtendValues));
        }


        /// <summary>
        /// 导出栏目及栏目下内容至XML文件
        /// </summary>
        /// <returns></returns>
        public void Export(int publishmentSystemID, int nodeID, bool isSaveContents)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo == null) return;

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

            string fileName = DataProvider.NodeDAO.GetOrderStringInPublishmentSystem(nodeID);

            string filePath = this.siteContentDirectoryPath + PathUtils.SeparatorChar + fileName + ".xml";

            AtomFeed feed = this.ExportNodeInfo(nodeInfo);

            if (isSaveContents)
            {
                string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxis);
                ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
                NameValueCollection collection = new NameValueCollection();
                foreach (int contentID in contentIDArrayList)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                    //ContentUtility.PutImagePaths(publishmentSystemInfo, contentInfo as BackgroundContentInfo, collection);
                    AtomEntry entry = this.ExportContentInfo(contentInfo);
                    feed.Entries.Add(entry);

                }
            }
            feed.Save(filePath);

            //  foreach (string imageUrl in collection.Keys)
            //  {
            //     string sourceFilePath = collection[imageUrl];
            //     string destFilePath = PathUtility.MapPath(this.siteContentDirectoryPath, imageUrl);
            //     DirectoryUtils.CreateDirectoryIfNotExists(destFilePath);
            ///     FileUtils.MoveFile(sourceFilePath, destFilePath, true);
            //  }
        }

        public bool ExportContents(PublishmentSystemInfo publishmentSystemInfo, int nodeID, ArrayList contentIDArrayList, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState)
        {
            string filePath = this.siteContentDirectoryPath + PathUtils.SeparatorChar + "contents.xml";
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeID);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            if (contentIDArrayList == null || contentIDArrayList.Count == 0)
            {
                contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayList(tableName, nodeID, isPeriods, dateFrom, dateTo, checkedState);
            }
            if (contentIDArrayList.Count == 0) return false;

            NameValueCollection collection = new NameValueCollection();

            foreach (int contentID in contentIDArrayList)
            {
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                try
                {
                    ContentUtility.PutImagePaths(publishmentSystemInfo, contentInfo as BackgroundContentInfo, collection);
                }
                catch { }
                AtomEntry entry = this.ExportContentInfo(contentInfo);
                feed.Entries.Add(entry);
            }
            feed.Save(filePath);

            foreach (string imageUrl in collection.Keys)
            {
                string sourceFilePath = collection[imageUrl];
                string destFilePath = PathUtility.MapPath(this.siteContentDirectoryPath, imageUrl);
                DirectoryUtils.CreateDirectoryIfNotExists(destFilePath);
                FileUtils.MoveFile(sourceFilePath, destFilePath, true);
            }

            return true;
        }

        private AtomFeed ExportNodeInfo(NodeInfo nodeInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.NodeID, nodeInfo.NodeID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.NodeName, nodeInfo.NodeName);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.NodeType, ENodeTypeUtils.GetValue(nodeInfo.NodeType));
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.PublishmentSystemID, nodeInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ContentModelID, nodeInfo.ContentModelID);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ParentID, nodeInfo.ParentID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ParentsPath, nodeInfo.ParentsPath);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ParentsCount, nodeInfo.ParentsCount.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ChildrenCount, nodeInfo.ChildrenCount.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.IsLastNode, nodeInfo.IsLastNode.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.NodeIndexName, nodeInfo.NodeIndexName);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.NodeGroupNameCollection, nodeInfo.NodeGroupNameCollection);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.Taxis, nodeInfo.Taxis.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.AddDate, nodeInfo.AddDate.ToLongDateString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ImageUrl, nodeInfo.ImageUrl);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.Content, AtomUtility.Encrypt(nodeInfo.Content));
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ContentNum, nodeInfo.ContentNum.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.FilePath, nodeInfo.FilePath);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ChannelFilePathRule, nodeInfo.ChannelFilePathRule);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ContentFilePathRule, nodeInfo.ContentFilePathRule);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.LinkUrl, nodeInfo.LinkUrl);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.LinkType, ELinkTypeUtils.GetValue(nodeInfo.LinkType));
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ChannelTemplateID, nodeInfo.ChannelTemplateID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ContentTemplateID, nodeInfo.ContentTemplateID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.Keywords, nodeInfo.Keywords);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.Description, nodeInfo.Description);
            AtomUtility.AddDcElement(feed.AdditionalElements, NodeAttribute.ExtendValues, nodeInfo.Additional.ToString());

            if (nodeInfo.ChannelTemplateID != 0)
            {
                string channelTemplateName = TemplateManager.GetTemplateName(nodeInfo.PublishmentSystemID, nodeInfo.ChannelTemplateID);
                AtomUtility.AddDcElement(feed.AdditionalElements, SiteContentIE.CHANNEL_TEMPLATE_NAME, channelTemplateName);
            }

            if (nodeInfo.ContentTemplateID != 0)
            {
                string contentTemplateName = TemplateManager.GetTemplateName(nodeInfo.PublishmentSystemID, nodeInfo.ContentTemplateID);
                AtomUtility.AddDcElement(feed.AdditionalElements, SiteContentIE.CONTENT_TEMPLATE_NAME, contentTemplateName);
            }

            if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Goods))
            {
                if (nodeInfo.Additional.SpecCount > 0)
                {
                    this.specIE.ExportSpec(nodeInfo.NodeID);
                }
                if (nodeInfo.Additional.FilterCount > 0)
                {
                    this.filterIE.ExportFilter(nodeInfo.NodeID);
                }
            }

            return feed;
        }

        private AtomEntry ExportContentInfo(ContentInfo contentInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.ID, contentInfo.ID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.NodeID, contentInfo.NodeID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.PublishmentSystemID, contentInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.AddUserName, contentInfo.AddUserName);
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.LastEditUserName, contentInfo.LastEditUserName);
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.LastEditDate, contentInfo.LastEditDate.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.Taxis, contentInfo.Taxis.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.ContentGroupNameCollection, contentInfo.ContentGroupNameCollection);
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.Tags, AtomUtility.Encrypt(contentInfo.Tags));
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.IsChecked, contentInfo.IsChecked.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.CheckedLevel, contentInfo.CheckedLevel.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.Comments, contentInfo.Comments.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.Photos, contentInfo.Photos.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.Hits, contentInfo.Hits.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.HitsByDay, contentInfo.HitsByDay.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.HitsByWeek, contentInfo.HitsByWeek.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.HitsByMonth, contentInfo.HitsByMonth.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.LastHitsDate, contentInfo.LastHitsDate.ToString());

            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.Title, AtomUtility.Encrypt(contentInfo.Title));
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.IsTop, contentInfo.IsTop.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, ContentAttribute.AddDate, contentInfo.AddDate.ToString());

            string starSetting = StarsManager.GetStarSettingToExport(this.publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID);
            AtomUtility.AddDcElement(entry.AdditionalElements, BackgroundContentAttribute.StarSetting, starSetting);

            foreach (string attributeName in contentInfo.Attributes.Keys)
            {
                if (!ContentAttribute.AllAttributes.Contains(attributeName.ToLower()))
                {
                    AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, AtomUtility.Encrypt(contentInfo.Attributes[attributeName]));
                }
            }

            if (contentInfo.Photos > 0)
            {
                this.photoIE.ExportPhoto(contentInfo.ID);
            }

            int skuCount = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(GoodsContentAttribute.SKUCount));
            if (skuCount > 0)
            {
                this.goodsIE.ExportGoods(contentInfo.NodeID, contentInfo.ID);
            }

            return entry;
        }

    }
}
