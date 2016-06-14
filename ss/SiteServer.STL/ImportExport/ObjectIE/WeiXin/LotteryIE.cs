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
    internal class LotteryIE
    {
        private int publishmentSystemID;

        private string directoryPath;

        public LotteryIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            //lottery
            List<LotteryInfo> lotteryInfoList = DataProviderWX.LotteryDAO.GetLotteryInfoList(this.publishmentSystemID);

            string lotteryDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(lotteryDirectoryPath);

            foreach (LotteryInfo lotteryInfo in lotteryInfoList)
            {
                string filePath = PathUtils.Combine(lotteryDirectoryPath, lotteryInfo.ID + ".xml");

                AtomFeed feed = ExportLotteryInfo(lotteryInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(lotteryInfo.KeywordID).Keywords;

                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                //lotteryLog
                List<LotteryLogInfo> lotteryLogInfoList = DataProviderWX.LotteryLogDAO.GetLotteryLogInfoList(this.publishmentSystemID, lotteryInfo.ID);

                foreach (LotteryLogInfo lotteryLogInfo in lotteryLogInfoList)
                {
                    AtomEntry entry = ExportLotteryLogInfo(lotteryLogInfo);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);

                //lotteryAward

                string lotteryAwardDirectoryPath = PathUtils.Combine(lotteryDirectoryPath, lotteryInfo.ID.ToString());

                DirectoryUtils.CreateDirectoryIfNotExists(lotteryAwardDirectoryPath);

                List<LotteryAwardInfo> lotteryAwardInfoList = DataProviderWX.LotteryAwardDAO.GetLotteryAwardInfoList(this.publishmentSystemID, lotteryInfo.ID);

                foreach (LotteryAwardInfo lotteryAwardInfo in lotteryAwardInfoList)
                {
                    string fileLotteryAwardPath = PathUtils.Combine(lotteryAwardDirectoryPath, lotteryAwardInfo.ID + ".xml");

                    AtomFeed feedLotteryAward = ExportLotteryAwardInfo(lotteryAwardInfo);

                    //lotteryWinner

                    List<LotteryWinnerInfo> lotteryWinnerInfoList = DataProviderWX.LotteryWinnerDAO.GetLotteryWinnerInfoList(this.publishmentSystemID, lotteryInfo.ID, lotteryAwardInfo.ID);

                    foreach (LotteryWinnerInfo lotteryWinnerInfo in lotteryWinnerInfoList)
                    {
                        AtomEntry entry = ExportLotteryWinnerInfo(lotteryWinnerInfo);
                        feedLotteryAward.Entries.Add(entry);
                    }

                    feedLotteryAward.Save(fileLotteryAwardPath);
                }
            }
        }

        private AtomFeed ExportLotteryInfo(LotteryInfo lotteryInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in LotteryAttribute.AllAttributes)
            {
                object valueObj = lotteryInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportLotteryLogInfo(LotteryLogInfo lotteryLogInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in LotteryLogAttribute.AllAttributes)
            {
                object valueObj = lotteryLogInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            return entry;
        }

        private AtomFeed ExportLotteryAwardInfo(LotteryAwardInfo lotteryAwardInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in LotteryAwardAttribute.AllAttributes)
            {
                object valueObj = lotteryAwardInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportLotteryWinnerInfo(LotteryWinnerInfo lotteryWinnerInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in LotteryWinnerAttribute.AllAttributes)
            {
                object valueObj = lotteryWinnerInfo.GetValue(attributeName);
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
                //lottery
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                LotteryInfo lotteryInfo = new LotteryInfo();

                foreach (string attributeNames in LotteryAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    lotteryInfo.SetValue(attributeNames, value);
                }

                lotteryInfo.PublishmentSystemID = this.publishmentSystemID;

                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                lotteryInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);

                int lotteryID = DataProviderWX.LotteryDAO.Insert(lotteryInfo);

                //lotteryLog
                foreach (AtomEntry entry in feed.Entries)
                {
                    LotteryLogInfo lotteryLogInfo = new LotteryLogInfo();

                    foreach (string attributeNames in LotteryLogAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        lotteryLogInfo.SetValue(attributeNames, value);
                    }

                    lotteryLogInfo.LotteryID = lotteryID;
                    lotteryLogInfo.PublishmentSystemID = this.publishmentSystemID;

                    DataProviderWX.LotteryLogDAO.Insert(lotteryLogInfo);
                }

                //lotteryAward
                if (!DirectoryUtils.IsDirectoryExists(this.directoryPath + PathUtils.SeparatorChar + lotteryInfo.ID)) return;

                string dicLotteryPath = DirectoryUtils.GetDirectoryPath(this.directoryPath + PathUtils.SeparatorChar + lotteryInfo.ID);

                string[] fileLotteryPaths = DirectoryUtils.GetFilePaths(dicLotteryPath);

                foreach (string fileLotteryPath in fileLotteryPaths)
                {
                    AtomFeed feedLottery = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(fileLotteryPath));

                    LotteryAwardInfo lotteryAwardInfo = new LotteryAwardInfo();

                    foreach (string attributeNames in LotteryAwardAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feedLottery.AdditionalElements, attributeNames));
                        lotteryAwardInfo.SetValue(attributeNames, value);
                    }
                    lotteryAwardInfo.LotteryID = lotteryID;
                    lotteryAwardInfo.PublishmentSystemID = this.publishmentSystemID;

                    int lotteryAwardID = DataProviderWX.LotteryAwardDAO.Insert(lotteryAwardInfo);

                    //lotteryWinner
                    foreach (AtomEntry entry in feedLottery.Entries)
                    {
                        LotteryWinnerInfo lotteryWinnerInfo = new LotteryWinnerInfo();

                        foreach (string attributeNames in LotteryWinnerAttribute.AllAttributes)
                        {
                            string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                            lotteryWinnerInfo.SetValue(attributeNames, value);
                        }

                        lotteryWinnerInfo.LotteryID = lotteryID;
                        lotteryWinnerInfo.AwardID = lotteryAwardID;
                        lotteryWinnerInfo.PublishmentSystemID = this.publishmentSystemID;

                        DataProviderWX.LotteryWinnerDAO.Insert(lotteryWinnerInfo);
                    }
                }
            }
        }
    }
}
