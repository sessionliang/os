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
	internal class SpecIE
	{
		private readonly int publishmentSystemID;
		private readonly string directoryPath;

        public SpecIE(int publishmentSystemID, string directoryPath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.directoryPath = directoryPath;
		}

        public void ExportSpec(int channelID)
		{
            int specCount = NodeManager.GetNodeInfo(this.publishmentSystemID, channelID).Additional.SpecCount;
            if (specCount > 0)
            {
                string specDirectoryPath = PathUtils.Combine(this.directoryPath, channelID.ToString());
                DirectoryUtils.CreateDirectoryIfNotExists(specDirectoryPath);

                List<SpecInfo> specInfoList = DataProviderB2C.SpecDAO.GetSpecInfoList(this.publishmentSystemID, channelID);

                int i = 1;
                foreach (SpecInfo specInfo in specInfoList)
                {
                    string filePath = PathUtils.Combine(specDirectoryPath, string.Format("{0}.xml", i++));

                    AtomFeed feed = ExportSpecInfo(specInfo);

                    List<SpecItemInfo> specItemInfoList = DataProviderB2C.SpecItemDAO.GetSpecItemInfoList(specInfo.SpecID);

                    foreach (SpecItemInfo specItemInfo in specItemInfoList)
                    {
                        AddAtomEntry(feed, specItemInfo);
                    }
                    feed.Save(filePath);
                }
            }            
		}

        private static AtomFeed ExportSpecInfo(SpecInfo specInfo)
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

            AtomUtility.AddDcElement(feed.AdditionalElements, "SpecID", specInfo.SpecID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "PublishmentSystemID", specInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "ChannelID", specInfo.ChannelID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "SpecName", AtomUtility.Encrypt(specInfo.SpecName));//加密
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsIcon", specInfo.IsIcon.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsMultiple", specInfo.IsMultiple.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsRequired", specInfo.IsRequired.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "Description", AtomUtility.Encrypt(specInfo.Description));//加密
            AtomUtility.AddDcElement(feed.AdditionalElements, "Taxis", specInfo.Taxis.ToString());

			return feed;
		}

        private static void AddAtomEntry(AtomFeed feed, SpecItemInfo specItemInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "ItemID", specItemInfo.ItemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", specItemInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "SpecID", specItemInfo.SpecID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "Title", AtomUtility.Encrypt(specItemInfo.Title));//加密
            AtomUtility.AddDcElement(entry.AdditionalElements, "IconUrl", specItemInfo.IconUrl);
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsDefault", specItemInfo.IsDefault.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", specItemInfo.Taxis.ToString());

            feed.Entries.Add(entry);
		}

        public void ImportSpec(int channelIDFromFile, int channelID, bool overwrite)
		{
            string specDirectoryPath = PathUtils.Combine(this.directoryPath, channelIDFromFile.ToString());
            if (!DirectoryUtils.IsDirectoryExists(specDirectoryPath)) return;

            Dictionary<int, int> specIDDictionary = new Dictionary<int, int>();
            Dictionary<int, int> itemIDDictionary = new Dictionary<int, int>();

            string[] filePaths = DirectoryUtils.GetFilePaths(specDirectoryPath);
            if (filePaths != null && filePaths.Length > 0)
            {
                for (int i = 1; i <= filePaths.Length; i++)
                {
                    string filePath = PathUtils.Combine(specDirectoryPath, string.Format("{0}.xml", i));

                    AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                    int specIDFromFile = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "SpecID"));
                    string specName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "SpecName"));
                    bool isIcon = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsIcon"));
                    bool isMultiple = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsMultiple"));
                    bool isRequired = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsRequired"));
                    string description = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "Description"));

                    SpecInfo specInfo = new SpecInfo(0, this.publishmentSystemID, channelID, specName, isIcon, isMultiple, isRequired, description, 0);

                    SpecInfo srcSpecInfo = DataProviderB2C.SpecDAO.GetSpecInfo(this.publishmentSystemID, channelID, specName);
                    if (srcSpecInfo != null)
                    {
                        if (overwrite)
                        {
                            DataProviderB2C.SpecDAO.Delete(this.publishmentSystemID, channelID, srcSpecInfo.SpecID);
                        }
                        else
                        {
                            specInfo.SpecName = DataProviderB2C.SpecDAO.GetImportSpecName(publishmentSystemID, channelID, specInfo.SpecName);
                        }
                    }

                    int specID = DataProviderB2C.SpecDAO.Insert(specInfo);

                    specIDDictionary.Add(specIDFromFile, specID);

                    foreach (AtomEntry entry in feed.Entries)
                    {
                        int itemIDFromFile = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ItemID"));
                        string itemTitle = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Title"));
                        string itemIconUrl = AtomUtility.GetDcElementContent(entry.AdditionalElements, "IconUrl");
                        bool itemIsDefault = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsDefault"));

                        SpecItemInfo specItemInfo = new SpecItemInfo(0, this.publishmentSystemID, specID, itemTitle, itemIconUrl, itemIsDefault, 0);
                        int itemID = DataProviderB2C.SpecItemDAO.Insert(this.publishmentSystemID, specItemInfo);

                        itemIDDictionary.Add(itemIDFromFile, itemID);
                    }
                }
            }

            IECacheUtils.CacheSpecIDDictionaryAndItemIDDictionary(specIDDictionary, itemIDDictionary);
		}
	}
}
