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
    internal class MenuIE
    {

        private readonly int publishmentSystemID;
        private readonly string directoryPath;

        public MenuIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }
        public void Export()
        {
            List<MenuInfo> menuInfoList = DataProviderWX.MenuDAO.GetMenuInfoList(this.publishmentSystemID);

            string menuDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(menuDirectoryPath);

            foreach (MenuInfo menuInfo in menuInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, menuInfo.MenuID + ".xml");

                AtomFeed feed = ExportMenuInfoFeed(menuInfo);

                List<MenuInfo> menuInfoParentList = DataProviderWX.MenuDAO.GetMenuInfoList(this.publishmentSystemID, menuInfo.MenuID);

                foreach (MenuInfo menuInfoParent in menuInfoParentList)
                {
                    AtomEntry entry = ExportMenuInfoEntry(menuInfoParent);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);
            }
        }

        private static AtomFeed ExportMenuInfoFeed(MenuInfo menuInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in MenuAttribute.AllAttributes)
            {
                object valueObj = menuInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            if (menuInfo.ChannelID != 0)
            {
                string orderString = DataProvider.NodeDAO.GetOrderStringInPublishmentSystem(menuInfo.ChannelID);
                AtomUtility.AddDcElement(feed.AdditionalElements, "OrderString", AtomUtility.Encrypt(orderString));
                ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(menuInfo.PublishmentSystemID), menuInfo.ChannelID);
                string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(menuInfo.PublishmentSystemID), menuInfo.ChannelID);
                if (menuInfo.ContentID != 0)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoNotTrash(tableStyle, tableName, menuInfo.ContentID);
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

        private static AtomEntry ExportMenuInfoEntry(MenuInfo menuInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in MenuAttribute.AllAttributes)
            {
                object valueObj = menuInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            if (menuInfo.ChannelID != 0)
            {
                string orderString = DataProvider.NodeDAO.GetOrderStringInPublishmentSystem(menuInfo.ChannelID);
                AtomUtility.AddDcElement(entry.AdditionalElements, "OrderString", AtomUtility.Encrypt(orderString));
                ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(menuInfo.PublishmentSystemID), menuInfo.ChannelID);
                string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(menuInfo.PublishmentSystemID), menuInfo.ChannelID);
                if (menuInfo.ContentID != 0)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoNotTrash(tableStyle, tableName, menuInfo.ContentID);
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

                MenuInfo menuInfo = new MenuInfo();

                foreach (string attributeNames in MenuAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    menuInfo.SetValue(attributeNames, value);
                }

                menuInfo.PublishmentSystemID = this.publishmentSystemID;
                if (menuInfo.ChannelID != 0)
                {
                    string orderString = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "OrderString"));
                    int nodeID = DataProvider.NodeDAO.GetNodeID(this.publishmentSystemID, orderString);
                    menuInfo.ChannelID = nodeID;

                    string contentTitle = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "ContentTitle"));
                    if (contentTitle == "")
                    {
                        menuInfo.ContentID = 0;
                    }
                    else
                    {
                        ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                        string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoByTitle(tableStyle, tableName, contentTitle);
                        if (contentInfo != null)
                            menuInfo.ContentID = contentInfo.ID;
                    }
                }
                else
                {
                    menuInfo.ChannelID = 0;
                    menuInfo.ContentID = 0;
                }

                menuInfo.MenuType = EMenuTypeUtils.GetEnumType(AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "MenuType")));

                int menuID = DataProviderWX.MenuDAO.Insert(menuInfo);

                foreach (AtomEntry entry in feed.Entries)
                {
                    MenuInfo menuParentInfo = new MenuInfo();

                    foreach (string attributeNames in MenuAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        menuParentInfo.SetValue(attributeNames, value);
                    }

                    menuParentInfo.PublishmentSystemID = this.publishmentSystemID;
                    if (menuParentInfo.ChannelID != 0)
                    {
                        string orderString = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "OrderString"));
                        int nodeID = DataProvider.NodeDAO.GetNodeID(this.publishmentSystemID, orderString);
                        menuParentInfo.ChannelID = nodeID;

                        string contentTitle = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentTitle"));
                        if (contentTitle == "")
                        {
                            menuParentInfo.ContentID = 0;
                        }
                        else
                        {
                            ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                            string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), nodeID);
                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoByTitle(tableStyle, tableName, contentTitle);
                            if (contentInfo != null)
                                menuParentInfo.ContentID = contentInfo.ID;
                        }
                    }
                    else
                    {
                        menuParentInfo.ChannelID = 0;
                        menuParentInfo.ContentID = 0;
                    }
                    menuParentInfo.ParentID = menuID;
                    menuParentInfo.MenuType = EMenuTypeUtils.GetEnumType(AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuType")));

                    DataProviderWX.MenuDAO.Insert(menuParentInfo);
                }
            }
        }
    }
}
