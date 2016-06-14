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
    internal class KeyWordIE
    {

        private int publishmentSystemID;

        private string directoryPath;

        public KeyWordIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {

            List<KeywordInfo> keywordInfoList = DataProviderWX.KeywordDAO.GetKeywordInfoList(this.publishmentSystemID);

            string keyWordDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(keyWordDirectoryPath);

            foreach (KeywordInfo keywordInfo in keywordInfoList)
            {
                string filePath = PathUtils.Combine(keyWordDirectoryPath, keywordInfo.KeywordID + ".xml");

                AtomFeed feed = ExportKeywordInfo(keywordInfo);

                List<KeywordMatchInfo> keywordMatchInfoList = DataProviderWX.KeywordMatchDAO.GetKeywordMatchInfoList(this.publishmentSystemID, keywordInfo.KeywordID);

                foreach (KeywordMatchInfo keywordMatchInfo in keywordMatchInfoList)
                {
                    AtomEntry entry = ExportKeywordMatchInfo(keywordMatchInfo);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);

                string keywordResourceDirectoryPath = PathUtils.Combine(keyWordDirectoryPath, keywordInfo.KeywordID.ToString());

                DirectoryUtils.CreateDirectoryIfNotExists(keywordResourceDirectoryPath);

                List<KeywordResourceInfo> keywordResourceInfoList = DataProviderWX.KeywordResourceDAO.GetKeywordResourceInfoList(this.publishmentSystemID, keywordInfo.KeywordID);

                AtomFeed feedKeywordResource = AtomUtility.GetEmptyFeed();

                string filekeywordResourcePath = PathUtils.Combine(keywordResourceDirectoryPath, keywordInfo.KeywordID + ".xml");

                foreach (KeywordResourceInfo keywordResourceInfo in keywordResourceInfoList)
                {                  

                    AtomEntry entry = ExportKeywordResourceInfo(keywordResourceInfo);
                    feedKeywordResource.Entries.Add(entry);                    
                }

                feedKeywordResource.Save(filekeywordResourcePath);
            }
        }

        private AtomFeed ExportKeywordInfo(KeywordInfo keywordInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in KeyWordAttribute.AllAttributes)
            {
                object valueObj = keywordInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportKeywordMatchInfo(KeywordMatchInfo keywordMatchInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in KeywordMatchAttribute.AllAttributes)
            {
                object valueObj = keywordMatchInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            return entry;
        }

        private AtomEntry ExportKeywordResourceInfo(KeywordResourceInfo keywordResourceInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in KeywordResourceAttribute.AllAttributes)
            {
                object valueObj = keywordResourceInfo.GetValue(attributeName);
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
                //Keyword
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                KeywordInfo keywordInfo = new KeywordInfo();

                foreach (string attributeNames in KeyWordAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    keywordInfo.SetValue(attributeNames, value);
                }

                keywordInfo.KeywordType = EKeywordTypeUtils.GetEnumType(AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordType")));
                keywordInfo.MatchType = EMatchTypeUtils.GetEnumType(AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "MatchType")));

                keywordInfo.PublishmentSystemID = this.publishmentSystemID;

                int keywordID = DataProviderWX.KeywordDAO.Insert(keywordInfo);

                //keywordmatchinfo
                //foreach (AtomEntry entry in feed.Entries)
                //{
                //    KeywordMatchInfo keywordMatchInfo = new KeywordMatchInfo();

                //    foreach (string attributeNames in KeywordMatchAttribute.AllAttributes)
                //    {
                //        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                //        keywordMatchInfo.SetValue(attributeNames, value);
                //    }

                //    keywordMatchInfo.KeywordType = EKeywordTypeUtils.GetEnumType(AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "KeywordType")));
                //    keywordMatchInfo.MatchType = EMatchTypeUtils.GetEnumType(AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "MatchType")));
                //    keywordMatchInfo.KeywordID = keywordID;
                //    keywordMatchInfo.PublishmentSystemID = this.publishmentSystemID;

                //    DataProviderWX.KeywordMatchDAO.Insert(keywordMatchInfo);
                //}

                //keywordresourceinfo
                if (!DirectoryUtils.IsDirectoryExists(this.directoryPath + PathUtils.SeparatorChar + keywordInfo.KeywordID)) return;

                string dicKeywordPath = DirectoryUtils.GetDirectoryPath(this.directoryPath + PathUtils.SeparatorChar + keywordInfo.KeywordID);

                string[] fileKeyWordPaths = DirectoryUtils.GetFilePaths(dicKeywordPath);

                foreach (string fileKeyWordPath in fileKeyWordPaths)
                {
                    AtomFeed feedKeyWordResource = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(fileKeyWordPath));

                    foreach (AtomEntry entry in feedKeyWordResource.Entries)
                    {
                        KeywordResourceInfo keywordResourceInfo = new KeywordResourceInfo();

                        foreach (string attributeNames in KeywordResourceAttribute.AllAttributes)
                        {
                            string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                            keywordResourceInfo.SetValue(attributeNames, value);
                        }

                        keywordResourceInfo.KeywordID = keywordID;
                        keywordResourceInfo.PublishmentSystemID = this.publishmentSystemID;
                        keywordResourceInfo.ResourceType = EResourceTypeUtils.GetEnumType(AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ResourceType")));
                        
                        DataProviderWX.KeywordResourceDAO.Insert(keywordResourceInfo);
                    }
                }
            }
        }

    }
}
