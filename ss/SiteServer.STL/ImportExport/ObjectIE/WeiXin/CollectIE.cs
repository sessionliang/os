using Atom.Core;
using BaiRong.Core;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.STL.ImportExport
{
    internal class CollectIE
    {

        private int publishmentSystemID;

        private string directoryPath;

        public CollectIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            //collect
            List<CollectInfo> collectInfoList = DataProviderWX.CollectDAO.GetCollectInfoList(this.publishmentSystemID);

            string collectDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(collectDirectoryPath);

            foreach (CollectInfo collectInfo in collectInfoList)
            {
                string filePath = PathUtils.Combine(collectDirectoryPath, collectInfo.ID + ".xml");

                AtomFeed feed = ExportCollectInfo(collectInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(collectInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                feed.Save(filePath);

                //collectItem

                string collectItemDirectoryPath = PathUtils.Combine(collectDirectoryPath, collectInfo.ID.ToString());

                DirectoryUtils.CreateDirectoryIfNotExists(collectItemDirectoryPath);

                List<CollectItemInfo> collectItemInfoList = DataProviderWX.CollectItemDAO.GetCollectItemInfoList(collectInfo.ID);

                foreach (CollectItemInfo collectItemInfo in collectItemInfoList)
                {
                    string fileCollectItemPath = PathUtils.Combine(collectItemDirectoryPath, collectItemInfo.ID + ".xml"); ;

                    AtomFeed feedCollectItem = ExportCollectItemInfo(collectItemInfo);

                    //collectLog

                    List<CollectLogInfo> collectLogInfoList = DataProviderWX.CollectLogDAO.GetCollectLogInfoList(this.publishmentSystemID, collectInfo.ID, collectItemInfo.ID);

                    foreach (CollectLogInfo collectLogInfo in collectLogInfoList)
                    {
                        AtomEntry entry = ExportCollectLogInfo(collectLogInfo);
                        feedCollectItem.Entries.Add(entry);
                    }

                    feedCollectItem.Save(fileCollectItemPath);
                }
            }
        }

        private AtomFeed ExportCollectInfo(CollectInfo collectInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in CollectAttribute.AllAttributes)
            {
                object valueObj = collectInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomFeed ExportCollectItemInfo(CollectItemInfo collectItemInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in CollectItemAttribute.AllAttributes)
            {
                object valueObj = collectItemInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportCollectLogInfo(CollectLogInfo collectLogInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in CollectLogAttribute.AllAttributes)
            {
                object valueObj = collectLogInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            return entry;
        }

        public void Import()
        {
            if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return;

            string[] filePaths = DirectoryUtils.GetFilePaths(this.directoryPath);

            foreach (string filePath in filePaths)
            {
                //collect
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                CollectInfo collectInfo = new CollectInfo();

                foreach (string attributeNames in CollectAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    collectInfo.SetValue(attributeNames, value);
                }

                collectInfo.PublishmentSystemID = this.publishmentSystemID;

                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                collectInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);

                int collectID = DataProviderWX.CollectDAO.Insert(collectInfo);

                //collectItem
                if (!DirectoryUtils.IsDirectoryExists(this.directoryPath + PathUtils.SeparatorChar + collectInfo.ID)) return;

                string dicCollectPath = DirectoryUtils.GetDirectoryPath(this.directoryPath + PathUtils.SeparatorChar + collectInfo.ID);

                string[] fileCollectPaths = DirectoryUtils.GetFilePaths(dicCollectPath);

                foreach (string fileCollectPath in fileCollectPaths)
                {
                    AtomFeed feedCollect = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(fileCollectPath));

                    CollectItemInfo collectItemInfo = new CollectItemInfo();

                    foreach (string attributeNames in CollectItemAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feedCollect.AdditionalElements, attributeNames));
                        collectItemInfo.SetValue(attributeNames, value);
                    }
                    collectItemInfo.CollectID = collectID;
                    collectItemInfo.PublishmentSystemID = this.publishmentSystemID;

                    int collectItemID = DataProviderWX.CollectItemDAO.Insert(collectItemInfo);

                    //collectLog
                    foreach (AtomEntry entry in feedCollect.Entries)
                    {
                        CollectLogInfo collectLogInfo = new CollectLogInfo();

                        foreach (string attributeNames in CollectLogAttribute.AllAttributes)
                        {
                            string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                            collectLogInfo.SetValue(attributeNames, value);
                        }

                        collectLogInfo.CollectID = collectID;
                        collectLogInfo.ItemID = collectItemID;
                        collectLogInfo.PublishmentSystemID = this.publishmentSystemID;

                        DataProviderWX.CollectLogDAO.Insert(collectLogInfo);
                    }
                }
            }
        }

    }
}
