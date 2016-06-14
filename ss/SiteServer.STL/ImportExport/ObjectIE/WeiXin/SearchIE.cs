using Atom.Core;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.STL.ImportExport
{
    internal class SearchIE
    {
        private readonly int publishmentSystemID;
        private readonly string directoryPath;

        public SearchIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }
        public void Export()
        {
            List<SearchInfo> searchInfoList = DataProviderWX.SearchDAO.GetSearchInfoList(this.publishmentSystemID);

            string searchDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(searchDirectoryPath);

            foreach (SearchInfo searchInfo in searchInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, searchInfo.ID + ".xml");

                AtomFeed feed = ExportSearchInfo(searchInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(searchInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                List<SearchNavigationInfo> searchNavigationInfoList = DataProviderWX.SearchNavigationDAO.GetSearchNavigationInfoList(this.publishmentSystemID, searchInfo.ID);

                foreach (SearchNavigationInfo searchNavigationInfo in searchNavigationInfoList)
                {
                    AtomEntry entry = ExportSearchNavigationInfo(searchNavigationInfo);

                    if (searchNavigationInfo.ChannelID != 0)
                    {
                        string orderString = DataProvider.NodeDAO.GetOrderStringInPublishmentSystem(searchNavigationInfo.ChannelID);
                        AtomUtility.AddDcElement(feed.AdditionalElements, "OrderString", AtomUtility.Encrypt(orderString));
                        if (searchNavigationInfo.ContentID != 0)
                        {
                            ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), searchNavigationInfo.ChannelID);
                            string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), searchNavigationInfo.ChannelID);
                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoNotTrash(tableStyle, tableName, searchNavigationInfo.ContentID);

                            AtomUtility.AddDcElement(feed.AdditionalElements, "ContentTitle", AtomUtility.Encrypt(contentInfo.Title));
                        }
                        else
                        {
                            AtomUtility.AddDcElement(feed.AdditionalElements, "ContentTitle", AtomUtility.Encrypt(""));
                        }
                    }
                    else
                    {
                        AtomUtility.AddDcElement(feed.AdditionalElements, "OrderString", AtomUtility.Encrypt(""));
                        AtomUtility.AddDcElement(feed.AdditionalElements, "ContentTitle", AtomUtility.Encrypt(""));
                    }
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);
            }
        }
        private static AtomFeed ExportSearchInfo(SearchInfo searchInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in SearchAttribute.AllAttributes)
            {
                object valueObj = searchInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }
        private static AtomEntry ExportSearchNavigationInfo(SearchNavigationInfo searchNavigationInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in SearchNavigationAttribute.AllAttributes)
            {
                object valueObj = searchNavigationInfo.GetValue(attributeName);
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

                SearchInfo searchInfo = new SearchInfo();

                foreach (string attributeNames in SearchAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    searchInfo.SetValue(attributeNames, value);
                }

                searchInfo.PublishmentSystemID = this.publishmentSystemID;
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                searchInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID, keywordName);
                int searchID = DataProviderWX.SearchDAO.Insert(searchInfo);

                foreach (AtomEntry entry in feed.Entries)
                {
                    SearchNavigationInfo searchNavigationInfo = new SearchNavigationInfo();

                    foreach (string attributeNames in SearchNavigationAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        searchNavigationInfo.SetValue(attributeNames, value);
                    }

                    searchNavigationInfo.SearchID = searchID;
                    searchNavigationInfo.PublishmentSystemID = this.publishmentSystemID;
                    searchNavigationInfo.FunctionID = 0;

                    string orderString = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "OrderString"));
                    if (orderString != "")
                    {
                        int nodeID = DataProvider.NodeDAO.GetNodeID(this.publishmentSystemID, orderString);
                        searchNavigationInfo.ChannelID = nodeID;
                        string contentTitle = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentTitle"));
                        if (contentTitle != "")
                        {
                            ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                            string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoByTitle(tableStyle, tableName, contentTitle);
                            searchNavigationInfo.ContentID = contentInfo.ID;
                        }
                        else
                        {
                            searchNavigationInfo.ContentID = 0;
                        }
                    }
                    else
                    {
                        searchNavigationInfo.ChannelID = 0;
                        searchNavigationInfo.ContentID = 0;
                    }

                    DataProviderWX.SearchNavigationDAO.Insert(searchNavigationInfo);
                }
            }
        }

    }
}
