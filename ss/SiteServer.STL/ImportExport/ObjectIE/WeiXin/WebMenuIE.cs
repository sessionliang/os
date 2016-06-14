using Atom.Core;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.STL.ImportExport
{
    internal class WebMenuIE
    {

        private readonly int publishmentSystemID;
        private readonly string directoryPath;

        public WebMenuIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }
        public void Export()
        {
            List<WebMenuInfo> webMenuInfoList = DataProviderWX.WebMenuDAO.GetWebMenuInfoList(this.publishmentSystemID);

            string webMenuDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(webMenuDirectoryPath);

            foreach (WebMenuInfo webMenuInfo in webMenuInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, webMenuInfo.ID + ".xml");

                AtomFeed feed = ExportWebMenuInfoFeed(webMenuInfo);

                List<WebMenuInfo> webMenuInfoParentList = DataProviderWX.WebMenuDAO.GetMenuInfoList(this.publishmentSystemID, webMenuInfo.ID);

                foreach (WebMenuInfo webMenuInfoParent in webMenuInfoParentList)
                {
                    AtomEntry entry = ExportWebMenuInfoEntry(webMenuInfoParent);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);
            }
        }

        private static AtomFeed ExportWebMenuInfoFeed(WebMenuInfo webMenuInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in WebMenuAttribute.AllAttributes)
            {
                object valueObj = webMenuInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            if (webMenuInfo.ChannelID != 0)
            {
                string orderString = DataProvider.NodeDAO.GetOrderStringInPublishmentSystem(webMenuInfo.ChannelID);
                AtomUtility.AddDcElement(feed.AdditionalElements, "OrderString", AtomUtility.Encrypt(orderString));
                ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(webMenuInfo.PublishmentSystemID), webMenuInfo.ChannelID);
                string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(webMenuInfo.PublishmentSystemID), webMenuInfo.ChannelID);
                if (webMenuInfo.ContentID != 0)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoNotTrash(tableStyle, tableName, webMenuInfo.ContentID);
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

            return feed;
        }

        private static AtomEntry ExportWebMenuInfoEntry(WebMenuInfo webMenuInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in WebMenuAttribute.AllAttributes)
            {
                object valueObj = webMenuInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            if (webMenuInfo.ChannelID != 0)
            {
                string orderString = DataProvider.NodeDAO.GetOrderStringInPublishmentSystem(webMenuInfo.ChannelID);
                AtomUtility.AddDcElement(entry.AdditionalElements, "OrderString", AtomUtility.Encrypt(orderString));
                ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(webMenuInfo.PublishmentSystemID), webMenuInfo.ChannelID);
                string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(webMenuInfo.PublishmentSystemID), webMenuInfo.ChannelID);
                if (webMenuInfo.ContentID != 0)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoNotTrash(tableStyle, tableName, webMenuInfo.ContentID);
                    AtomUtility.AddDcElement(entry.AdditionalElements, "ContentTitle", AtomUtility.Encrypt(contentInfo.Title));
                }
                else
                {
                    AtomUtility.AddDcElement(entry.AdditionalElements, "ContentTitle", AtomUtility.Encrypt(""));
                }
            }
            else
            {
                AtomUtility.AddDcElement(entry.AdditionalElements, "OrderString", AtomUtility.Encrypt(""));
                AtomUtility.AddDcElement(entry.AdditionalElements, "ContentTitle", AtomUtility.Encrypt(""));
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

                WebMenuInfo webMenuInfo = new WebMenuInfo();

                foreach (string attributeNames in WebMenuAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    webMenuInfo.SetValue(attributeNames, value);
                }

                webMenuInfo.PublishmentSystemID = this.publishmentSystemID;
                if (webMenuInfo.ChannelID != 0)
                {
                    string orderString = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "OrderString"));
                    int nodeID = DataProvider.NodeDAO.GetNodeID(this.publishmentSystemID, orderString);
                    webMenuInfo.ChannelID = nodeID;

                    string contentTitle = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "ContentTitle"));
                    if (contentTitle == "")
                    {
                        webMenuInfo.ContentID = 0;
                    }
                    else
                    {
                        ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                        string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoByTitle(tableStyle, tableName, contentTitle);
                        webMenuInfo.ContentID = contentInfo.ID;
                    }
                }
                else
                {
                    webMenuInfo.ChannelID = 0;
                    webMenuInfo.ContentID = 0;
                }

                int webMenuID = DataProviderWX.WebMenuDAO.Insert(webMenuInfo);

                foreach (AtomEntry entry in feed.Entries)
                {
                    WebMenuInfo webMenuParentInfo = new WebMenuInfo();

                    foreach (string attributeNames in WebMenuAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        webMenuParentInfo.SetValue(attributeNames, value);
                    }

                    webMenuParentInfo.PublishmentSystemID = this.publishmentSystemID;
                    if (webMenuParentInfo.ChannelID != 0)
                    {
                        string orderString = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "OrderString"));
                        int nodeID = DataProvider.NodeDAO.GetNodeID(this.publishmentSystemID, orderString);
                        webMenuParentInfo.ChannelID = nodeID;

                        string contentTitle = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentTitle"));
                        if (contentTitle == "")
                        {
                            webMenuInfo.ContentID = 0;
                        }
                        else
                        {
                            ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                            string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoByTitle(tableStyle, tableName, contentTitle);
                            webMenuParentInfo.ContentID = contentInfo.ID;
                        }
                    }
                    else
                    {
                        webMenuParentInfo.ChannelID = 0;
                        webMenuParentInfo.ContentID = 0;
                    }
                    webMenuParentInfo.ParentID = webMenuID;

                    DataProviderWX.WebMenuDAO.Insert(webMenuParentInfo);
                }
            }
        }
    }
}
