using System;
using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using System.Collections.Generic;

namespace SiteServer.STL.ImportExport.B2C
{
	internal class GoodsIE
	{
        private PublishmentSystemInfo publishmentSystemInfo;
		private string directoryPath;

        public GoodsIE(PublishmentSystemInfo publishmentSystemInfo, string directoryPath)
		{
            this.publishmentSystemInfo = publishmentSystemInfo;
			this.directoryPath = directoryPath;
		}

        public void ExportGoods(int channelID, int contentID)
		{
            this.ExportSpecComboInfo(channelID, contentID);
            this.ExportGoodsInfo(contentID);
		}

        private void ExportSpecComboInfo(int channelID, int contentID)
        {
            string filePath = PathUtils.Combine(this.directoryPath, "combo_" + contentID + ".xml");

            AtomFeed feed = AtomUtility.GetEmptyFeed();

            List<SpecComboInfo> specComboInfoList = new List<SpecComboInfo>();

            List<int> specIDList = DataProviderB2C.SpecDAO.GetSpecIDList(this.publishmentSystemInfo.PublishmentSystemID, channelID);

            foreach (int specID in specIDList)
            {
                specComboInfoList.AddRange(DataProviderB2C.SpecComboDAO.GetSpecComboInfoList(this.publishmentSystemInfo.PublishmentSystemID, contentID, specID));
            }

            foreach (SpecComboInfo comboInfo in specComboInfoList)
            {
                AddAtomEntryToSpecComboInfo(feed, comboInfo);
            }

            feed.Save(filePath);
        }

        private void ExportGoodsInfo(int contentID)
        {
            string filePath = PathUtils.Combine(this.directoryPath, "goods_" + contentID + ".xml");

            AtomFeed feed = AtomUtility.GetEmptyFeed();

            List<GoodsInfo> goodsInfoList = DataProviderB2C.GoodsDAO.GetGoodsInfoList(this.publishmentSystemInfo.PublishmentSystemID, contentID);

            foreach (GoodsInfo goodsInfo in goodsInfoList)
            {
                AddAtomEntryToGoodsInfo(feed, goodsInfo);
            }

            feed.Save(filePath);
        }

        private static void AddAtomEntryToSpecComboInfo(AtomFeed feed, SpecComboInfo comboInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "ComboID", comboInfo.ComboID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", comboInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "ContentID", comboInfo.ContentID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "SpecID", comboInfo.SpecID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "ItemID", comboInfo.ItemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "Title", AtomUtility.Encrypt(comboInfo.Title));//加密
            AtomUtility.AddDcElement(entry.AdditionalElements, "IconUrl", comboInfo.IconUrl);
            AtomUtility.AddDcElement(entry.AdditionalElements, "PhotoIDCollection", comboInfo.PhotoIDCollection);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", comboInfo.Taxis.ToString());

            feed.Entries.Add(entry);
		}

        private static void AddAtomEntryToGoodsInfo(AtomFeed feed, GoodsInfo goodsInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "GoodsID", goodsInfo.GoodsID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", goodsInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "ContentID", goodsInfo.ContentID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "ComboIDCollection", goodsInfo.ComboIDCollection);
            AtomUtility.AddDcElement(entry.AdditionalElements, "SpecIDCollection", goodsInfo.SpecIDCollection);
            AtomUtility.AddDcElement(entry.AdditionalElements, "SpecItemIDCollection", goodsInfo.SpecItemIDCollection);
            AtomUtility.AddDcElement(entry.AdditionalElements, "GoodsSN", goodsInfo.GoodsSN);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Stock", goodsInfo.Stock.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "PriceMarket", goodsInfo.PriceMarket.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "PriceSale", goodsInfo.PriceSale.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsOnSale", goodsInfo.IsOnSale.ToString());

            feed.Entries.Add(entry);
        }

        public void ImportGoods(int contentIDFromFile, int contentID, Dictionary<int, int> specIDDictionary, Dictionary<int, int> itemIDDictionary)
		{
            Dictionary<int, int> comboIDDictionary = this.ImportSpecComboInfo(contentIDFromFile, contentID, specIDDictionary, itemIDDictionary);
            this.ImportGoodsInfo(contentIDFromFile, contentID, specIDDictionary, itemIDDictionary, comboIDDictionary);
		}

        private Dictionary<int, int> ImportSpecComboInfo(int contentIDFromFile, int contentID, Dictionary<int, int> specIDDictionary, Dictionary<int, int> itemIDDictionary)
        {
            Dictionary<int, int> comboIDDictionary = new Dictionary<int,int>();

            string filePath = PathUtils.Combine(this.directoryPath, "combo_" + contentIDFromFile + ".xml");
            if (!FileUtils.IsFileExists(filePath)) return comboIDDictionary;

            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            foreach (AtomEntry entry in feed.Entries)
            {
                int comboIDFromFile = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ComboID"));
                int specIDFromFile = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SpecID"));
                int itemIDFromFile = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ItemID"));
                string title = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Title"));
                string iconUrl = AtomUtility.GetDcElementContent(entry.AdditionalElements, "IconUrl");
                string photoIDCollection = AtomUtility.GetDcElementContent(entry.AdditionalElements, "PhotoIDCollection");

                int specID = 0;
                int itemID = 0;
                if (specIDDictionary.TryGetValue(specIDFromFile, out specID) && itemIDDictionary.TryGetValue(itemIDFromFile, out itemID))
                {
                    SpecComboInfo comboInfo = new SpecComboInfo(0, this.publishmentSystemInfo.PublishmentSystemID, contentID, specID, itemID, title, iconUrl, string.Empty, 0);
                    int comboID = DataProviderB2C.SpecComboDAO.Insert(comboInfo);

                    comboIDDictionary.Add(comboIDFromFile, comboID);
                }
            }

            return comboIDDictionary;
        }

        private void ImportGoodsInfo(int contentIDFromFile, int contentID, Dictionary<int, int> specIDDictionary, Dictionary<int, int> itemIDDictionary, Dictionary<int, int> comboIDDictionary)
        {
            string filePath = PathUtils.Combine(this.directoryPath, "goods_" + contentIDFromFile + ".xml");
            if (!FileUtils.IsFileExists(filePath)) return;

            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            foreach (AtomEntry entry in feed.Entries)
            {
                List<int> comboIDListFromFile = TranslateUtils.StringCollectionToIntList(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ComboIDCollection"));
                List<int> specIDListFromFile = TranslateUtils.StringCollectionToIntList(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SpecIDCollection"));
                List<int> specItemIDListFromFile = TranslateUtils.StringCollectionToIntList(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SpecItemIDCollection"));
                string goodsSN = AtomUtility.GetDcElementContent(entry.AdditionalElements, "GoodsSN");
                int stock = TranslateUtils.ToIntWithNagetive(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Stock"));
                decimal priceMarket = TranslateUtils.ToDecimal(AtomUtility.GetDcElementContent(entry.AdditionalElements, "PriceMarket"));
                decimal priceSale = TranslateUtils.ToDecimal(AtomUtility.GetDcElementContent(entry.AdditionalElements, "PriceSale"));
                bool IsOnSale = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsOnSale"));

                List<int> comboIDList = new List<int>();
                List<int> specIDList = new List<int>();
                List<int> specItemIDList = new List<int>();

                foreach (int comboIDFromFile in comboIDListFromFile)
                {
                    if (comboIDDictionary.ContainsKey(comboIDFromFile))
                    {
                        comboIDList.Add(comboIDDictionary[comboIDFromFile]);
                    }
                }

                foreach (int specIDFromFile in specIDListFromFile)
                {
                    if (specIDDictionary.ContainsKey(specIDFromFile))
                    {
                        specIDList.Add(specIDDictionary[specIDFromFile]);
                    }
                }

                foreach (int specItemIDFromFile in specItemIDListFromFile)
                {
                    if (itemIDDictionary.ContainsKey(specItemIDFromFile))
                    {
                        specItemIDList.Add(itemIDDictionary[specItemIDFromFile]);
                    }
                }

                GoodsInfo goodsInfo = new GoodsInfo(0, this.publishmentSystemInfo.PublishmentSystemID, contentID, TranslateUtils.ObjectCollectionToString(comboIDList), TranslateUtils.ObjectCollectionToString(specIDList), TranslateUtils.ObjectCollectionToString(specItemIDList), goodsSN, stock, priceMarket, priceSale, IsOnSale);

                DataProviderB2C.GoodsDAO.Insert(goodsInfo);
            }
        }
	}
}
