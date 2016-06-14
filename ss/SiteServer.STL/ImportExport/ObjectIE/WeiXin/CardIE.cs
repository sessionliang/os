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
    internal class CardIE
    {
        private int publishmentSystemID;

        private string directoryPath;

        public CardIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            //card
            List<CardInfo> cardInfoList = DataProviderWX.CardDAO.GetCardInfoList(this.publishmentSystemID);

            string cardDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(cardDirectoryPath);

            foreach (CardInfo cardInfo in cardInfoList)
            {
                string filePath = PathUtils.Combine(cardDirectoryPath, cardInfo.ID + ".xml");

                AtomFeed feed = ExportCardInfo(cardInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(cardInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                //cardentitysn

                List<CardEntitySNInfo> cardEntitySNInfoList = DataProviderWX.CardEntitySNDAO.GetCardEntitySNInfoList(this.publishmentSystemID, cardInfo.ID);

                foreach (CardEntitySNInfo cardEntitySNInfo in cardEntitySNInfoList)
                {
                    AtomEntry entry = ExportCardEntitySNInfo(cardEntitySNInfo);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);

                //cardsn

                string cardSNDirectoryPath = PathUtils.Combine(cardDirectoryPath, cardInfo.ID.ToString());

                DirectoryUtils.CreateDirectoryIfNotExists(cardSNDirectoryPath);

                List<CardSNInfo> cardSNInfoList = DataProviderWX.CardSNDAO.GetCardSNInfoList(this.publishmentSystemID, cardInfo.ID);

                foreach (CardSNInfo cardSNInfo in cardSNInfoList)
                {
                    string filecardCashLogPath = PathUtils.Combine(cardSNDirectoryPath, cardSNInfo.ID + ".xml"); ;

                    AtomFeed feedCardSN = ExportCardSNInfo(cardSNInfo);

                    //cardcashlog

                    List<CardCashLogInfo> cardCashLogInfoList = DataProviderWX.CardCashLogDAO.GetCardCashLogInfoList(this.publishmentSystemID, cardInfo.ID, cardSNInfo.ID);

                    foreach (CardCashLogInfo cardCashLogInfo in cardCashLogInfoList)
                    {
                        AtomEntry entry = ExportCardCashLogInfo(cardCashLogInfo);
                        feedCardSN.Entries.Add(entry);
                    }

                    feedCardSN.Save(filecardCashLogPath);
                }
            }

            string fileCardSignLogPath = PathUtils.Combine(directoryPath, "cardSignLog.xml");

            List<CardSignLogInfo> cardSignLogInfoList = DataProviderWX.CardSignLogDAO.GetCardSignLogInfoList(this.publishmentSystemID);

            AtomFeed feedCardSignLog = AtomUtility.GetEmptyFeed();

            foreach (CardSignLogInfo cardSignLogInfo in cardSignLogInfoList)
            {
                AtomEntry entry = ExportCardSignLogInfo(cardSignLogInfo);
                feedCardSignLog.Entries.Add(entry);
            }

            feedCardSignLog.Save(fileCardSignLogPath);
        }

        private AtomFeed ExportCardInfo(CardInfo cardInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in CardAttribute.AllAttributes)
            {
                object valueObj = cardInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportCardEntitySNInfo(CardEntitySNInfo cardEntitySNInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in CardEntitySNAttribute.AllAttributes)
            {
                object valueObj = cardEntitySNInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            return entry;
        }

        private AtomFeed ExportCardSNInfo(CardSNInfo cardSNInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in CardSNAttribute.AllAttributes)
            {
                object valueObj = cardSNInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportCardCashLogInfo(CardCashLogInfo cardCashLogInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in CardCashLogAttribute.AllAttributes)
            {
                object valueObj = cardCashLogInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            return entry;
        }

        private AtomEntry ExportCardSignLogInfo(CardSignLogInfo cardSignLogInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in CardSignLogAttribute.AllAttributes)
            {
                object valueObj = cardSignLogInfo.GetValue(attributeName);
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
                //card
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                CardInfo cardInfo = new CardInfo();

                foreach (string attributeNames in CardAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    cardInfo.SetValue(attributeNames, value);
                }

                cardInfo.PublishmentSystemID = this.publishmentSystemID;
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                cardInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);
                int cardID = DataProviderWX.CardDAO.Insert(cardInfo);

                //cardEntitySN
                foreach (AtomEntry entry in feed.Entries)
                {
                    CardEntitySNInfo cardEntitySNInfo = new CardEntitySNInfo();

                    foreach (string attributeNames in CardEntitySNAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        cardEntitySNInfo.SetValue(attributeNames, value);
                    }

                    cardEntitySNInfo.CardID = cardID;
                    cardEntitySNInfo.PublishmentSystemID = this.publishmentSystemID;

                    DataProviderWX.CardEntitySNDAO.Insert(cardEntitySNInfo);
                }

                //CardSN
                if (!DirectoryUtils.IsDirectoryExists(this.directoryPath + PathUtils.SeparatorChar + cardInfo.ID)) return;

                string dicCardSNPath = DirectoryUtils.GetDirectoryPath(this.directoryPath + PathUtils.SeparatorChar + cardInfo.ID);

                string[] fileCardSNPaths = DirectoryUtils.GetFilePaths(dicCardSNPath);

                foreach (string fileCardSNPath in fileCardSNPaths)
                {
                    AtomFeed feedCardSN = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(fileCardSNPath));

                    CardSNInfo cardSNInfo = new CardSNInfo();

                    foreach (string attributeNames in CardSNAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feedCardSN.AdditionalElements, attributeNames));
                        cardSNInfo.SetValue(attributeNames, value);
                    }
                    cardSNInfo.CardID = cardID;
                    cardSNInfo.PublishmentSystemID = this.publishmentSystemID;

                    int cardSNID = DataProviderWX.CardSNDAO.Insert(cardSNInfo);

                    //cardCashLog
                    foreach (AtomEntry entry in feedCardSN.Entries)
                    {
                        CardCashLogInfo cardCashLogInfo = new CardCashLogInfo();

                        foreach (string attributeNames in CardCashLogAttribute.AllAttributes)
                        {
                            string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                            cardCashLogInfo.SetValue(attributeNames, value);
                        }

                        cardCashLogInfo.CardID = cardID;
                        cardCashLogInfo.CardSNID = cardSNID;
                        cardCashLogInfo.PublishmentSystemID = this.publishmentSystemID;

                        DataProviderWX.CardCashLogDAO.Insert(cardCashLogInfo);
                    }
                }
            }

            //cardSignLog
            string fileCardSignLog = this.directoryPath + PathUtils.SeparatorChar + "cardSignLog.xml";

            if (!FileUtils.IsFileExists(fileCardSignLog)) return;

            AtomFeed feedCardSignLog = AtomFeed.Load(fileCardSignLog);

            foreach (AtomEntry entry in feedCardSignLog.Entries)
            {
                CardSignLogInfo cardSignLogInfo = new CardSignLogInfo();

                foreach (string attributeName in CardSignLogAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    cardSignLogInfo.SetValue(attributeName, value);
                }

                cardSignLogInfo.PublishmentSystemID = this.publishmentSystemID;

                DataProviderWX.CardSignLogDAO.Insert(cardSignLogInfo);
            }
        }
    }
}
