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
    internal class StoreIE
    {
        private readonly int publishmentSystemID;
        private string directoryPath;

        public StoreIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            List<StoreInfo> storeInfoList = DataProviderWX.StoreDAO.GetStoreInfoList(this.publishmentSystemID);

            string storeDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(storeDirectoryPath);

            foreach (StoreInfo storeInfo in storeInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, storeInfo.ID + ".xml");

                AtomFeed feed = ExportStoreInfo(storeInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(storeInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                List<StoreItemInfo> storeItemInfoList = DataProviderWX.StoreItemDAO.GetStoreItemInfoList(this.publishmentSystemID, storeInfo.ID);
                foreach (StoreItemInfo storeItemInfo in storeItemInfoList)
                {
                    AtomEntry entry = ExportStoreItemInfo(storeItemInfo);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);
            }

            string fileStoreCategoryPath = PathUtils.Combine(directoryPath, "storeCategory.xml");

            List<StoreCategoryInfo> storeCategoryInfoList = DataProviderWX.StoreCategoryDAO.GetStoreCategoryInfoList(this.publishmentSystemID);

            AtomFeed feedStoreCategory = AtomUtility.GetEmptyFeed();

            foreach (StoreCategoryInfo storeCategoryInfo in storeCategoryInfoList)
            {
                AtomEntry entry = ExportStoreCategoryInfo(storeCategoryInfo);
                feedStoreCategory.Entries.Add(entry);
            }

            feedStoreCategory.Save(fileStoreCategoryPath);

        }

        private AtomFeed ExportStoreInfo(StoreInfo storeInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in StoreAttribute.AllAttributes)
            {
                object valueObj = storeInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportStoreItemInfo(StoreItemInfo storeItemInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in StoreItemAttribute.AllAttributes)
            {
                object valueObj = storeItemInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            return entry;
        }

        private AtomEntry ExportStoreCategoryInfo(StoreCategoryInfo storeCategoryInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in StoreCategoryAttribute.AllAttributes)
            {
                object valueObj = storeCategoryInfo.GetValue(attributeName);
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
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                StoreInfo storeInfo = new StoreInfo();
                foreach (string attributeNames in StoreAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    storeInfo.SetValue(attributeNames, value);
                }
                storeInfo.PublishmentSystemID = this.publishmentSystemID;


                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                storeInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);

                int storeID = DataProviderWX.StoreDAO.Insert(storeInfo);

                foreach (AtomEntry entry in feed.Entries)
                {
                    StoreItemInfo storeItemInfo = new StoreItemInfo();

                    foreach (string attributeNames in StoreItemAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        storeItemInfo.SetValue(attributeNames, value);
                    }

                    storeItemInfo.StoreID = storeID;
                    storeItemInfo.PublishmentSystemID = this.publishmentSystemID;

                    int storeItemID = DataProviderWX.StoreItemDAO.Insert(storeItemInfo);

                }
            }

            //storecategory
            string fileStoreCategory = this.directoryPath + PathUtils.SeparatorChar + "storeCategory.xml";

            if (!FileUtils.IsFileExists(fileStoreCategory)) return;

            AtomFeed feedStoreCategory = AtomFeed.Load(fileStoreCategory);

            foreach (AtomEntry entry in feedStoreCategory.Entries)
            {
                StoreCategoryInfo storeCategoryInfo = new StoreCategoryInfo();

                foreach (string attributeName in StoreCategoryAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    storeCategoryInfo.SetValue(attributeName, value);
                }

                storeCategoryInfo.PublishmentSystemID = this.publishmentSystemID;

                DataProviderWX.StoreCategoryDAO.Insert(storeCategoryInfo);
            }

        }
    }
}
