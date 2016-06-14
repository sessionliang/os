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
    internal class VoteIE
    {
        private readonly int publishmentSystemID;

        private string directoryPath;

        public VoteIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            List<VoteInfo> voteInfoList = DataProviderWX.VoteDAO.GetVoteInfoList(this.publishmentSystemID);

            string voteDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(voteDirectoryPath);

            foreach (VoteInfo voteInfo in voteInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, voteInfo.ID + ".xml");

                AtomFeed feed = ExportVoteInfo(voteInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(voteInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                List<VoteItemInfo> voteItemInfoList = DataProviderWX.VoteItemDAO.GetVoteItemInfoList(this.publishmentSystemID, voteInfo.ID);
                foreach (VoteItemInfo voteItemInfo in voteItemInfoList)
                {
                    AtomEntry entry = ExportVoteItemInfo(voteItemInfo);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);
            }

            string fileVoteLogPath = PathUtils.Combine(directoryPath, "voteLog.xml");

            List<VoteLogInfo> voteLogInfoList = DataProviderWX.VoteLogDAO.GetVoteLogInfoList(this.publishmentSystemID);

            AtomFeed feedVoteLog = AtomUtility.GetEmptyFeed();

            foreach (VoteLogInfo voteLogInfo in voteLogInfoList)
            {
                AtomEntry entry = ExportVoteLogInfo(voteLogInfo);
                feedVoteLog.Entries.Add(entry);
            }

            feedVoteLog.Save(fileVoteLogPath);

        }

        private AtomFeed ExportVoteInfo(VoteInfo voteInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in VoteAttribute.AllAttributes)
            {
                object valueObj = voteInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportVoteItemInfo(VoteItemInfo voteItemInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in VoteItemAttribute.AllAttributes)
            {
                object valueObj = voteItemInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            return entry;
        }

        private AtomEntry ExportVoteLogInfo(VoteLogInfo voteLogInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in VoteLogAttribute.AllAttributes)
            {
                object valueObj = voteLogInfo.GetValue(attributeName);
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

                VoteInfo voteInfo = new VoteInfo();
                foreach (string attributeNames in VoteAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    voteInfo.SetValue(attributeNames, value);
                }
                voteInfo.PublishmentSystemID = this.publishmentSystemID;
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                voteInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);

                int voteID = DataProviderWX.VoteDAO.Insert(voteInfo);

                DbCacheManager.Insert("SiteServer.STL.ImportExport.ImportVote_" + voteInfo.ID, voteID.ToString());

                foreach (AtomEntry entry in feed.Entries)
                {
                    VoteItemInfo voteItemInfo = new VoteItemInfo();

                    foreach (string attributeNames in VoteItemAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        voteItemInfo.SetValue(attributeNames, value);
                    }

                    voteItemInfo.VoteID = voteID;
                    voteItemInfo.PublishmentSystemID = this.publishmentSystemID;

                    int voteItemID = DataProviderWX.VoteItemDAO.Insert(voteItemInfo);

                    DbCacheManager.Insert("SiteServer.STL.ImportExport.ImportVoteItem_" + voteItemInfo.ID, voteItemID.ToString());
                }
            }

            //votelog
            string fileVoteLog = this.directoryPath + PathUtils.SeparatorChar + "voteLog.xml";

            if (!FileUtils.IsFileExists(fileVoteLog)) return;

            AtomFeed feedVoteLog = AtomFeed.Load(fileVoteLog);

            foreach (AtomEntry entry in feedVoteLog.Entries)
            {
                VoteLogInfo voteLogInfo = new VoteLogInfo();

                foreach (string attributeName in VoteLogAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    voteLogInfo.SetValue(attributeName, value);
                }

                voteLogInfo.VoteID = TranslateUtils.ToInt(DbCacheManager.Get("SiteServer.STL.ImportExport.ImportVote_" + voteLogInfo.VoteID), 0);

                string newItemIDCollections = "";

                string[] ItemIDCollections = voteLogInfo.ItemIDCollection.Split(',');

                foreach (string item in ItemIDCollections)
                {
                    newItemIDCollections += DbCacheManager.Get("SiteServer.STL.ImportExport.ImportVoteItem_" + item) + ",";
                }

                voteLogInfo.ItemIDCollection = newItemIDCollections.Substring(0, newItemIDCollections.Length - 1);

                voteLogInfo.PublishmentSystemID = this.publishmentSystemID;

                DataProviderWX.VoteLogDAO.Insert(voteLogInfo);
            }

        }
    }

}
