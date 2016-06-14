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
	internal class FilterIE
	{
        private PublishmentSystemInfo publishmentSystemInfo;
		private string directoryPath;

        public FilterIE(PublishmentSystemInfo publishmentSystemInfo, string directoryPath)
		{
            this.publishmentSystemInfo = publishmentSystemInfo;
			this.directoryPath = directoryPath;
		}

        public void ExportFilter(int channelID)
		{
            int filterCount = NodeManager.GetNodeInfo(this.publishmentSystemInfo.PublishmentSystemID, channelID).Additional.FilterCount;
            if (filterCount > 0)
            {
                string filterDirectoryPath = PathUtils.Combine(this.directoryPath, channelID.ToString());
                DirectoryUtils.CreateDirectoryIfNotExists(filterDirectoryPath);

                List<FilterInfo> filterInfoList = DataProviderB2C.FilterDAO.GetFilterInfoList(this.publishmentSystemInfo.PublishmentSystemID, channelID);

                int i = 1;
                foreach (FilterInfo filterInfo in filterInfoList)
                {
                    string filePath = PathUtils.Combine(filterDirectoryPath, string.Format("{0}.xml", i++));

                    AtomFeed feed = ExportFilterInfo(filterInfo);

                    List<FilterItemInfo> filterItemInfoList = DataProviderB2C.FilterItemDAO.GetFilterItemInfoList(filterInfo.FilterID);

                    foreach (FilterItemInfo filterItemInfo in filterItemInfoList)
                    {
                        AddAtomEntry(feed, filterItemInfo);
                    }
                    feed.Save(filePath);
                }                
            }
		}

        private static AtomFeed ExportFilterInfo(FilterInfo filterInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            AtomUtility.AddDcElement(feed.AdditionalElements, "FilterID", filterInfo.FilterID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "PublishmentSystemID", filterInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "NodeID", filterInfo.NodeID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "AttributeName", filterInfo.AttributeName);
            AtomUtility.AddDcElement(feed.AdditionalElements, "FilterName", filterInfo.FilterName);
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsDefaultValues", filterInfo.IsDefaultValues.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "Taxis", filterInfo.Taxis.ToString());

            return feed;
        }

        private static void AddAtomEntry(AtomFeed feed, FilterItemInfo filterItemInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "ItemID", filterItemInfo.ItemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "FilterID", filterItemInfo.FilterID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "Title", AtomUtility.Encrypt(filterItemInfo.Title));//加密
            AtomUtility.AddDcElement(entry.AdditionalElements, "Value", AtomUtility.Encrypt(filterItemInfo.Value));//加密
            AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", filterItemInfo.Taxis.ToString());
            

            feed.Entries.Add(entry);
		}

		public void ImportFilter(int channelIDFromFile, int channelID)
		{
            string filterDirectoryPath = PathUtils.Combine(this.directoryPath, channelIDFromFile.ToString());
            if (!DirectoryUtils.IsDirectoryExists(filterDirectoryPath)) return;

            string[] filePaths = DirectoryUtils.GetFilePaths(filterDirectoryPath);
            if (filePaths != null && filePaths.Length > 0)
            {
                for (int i = 1; i <= filePaths.Length; i++)
                {
                    string filePath = PathUtils.Combine(filterDirectoryPath, string.Format("{0}.xml", i));

                    AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                    string attributeName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "AttributeName");
                    string filterName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "FilterName");
                    bool isDefaultValues = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsDefaultValues"));

                    FilterInfo filterInfo = new FilterInfo(0, this.publishmentSystemInfo.PublishmentSystemID, channelID, attributeName, filterName, isDefaultValues, 0);
                    int filterID = DataProviderB2C.FilterDAO.Insert(filterInfo);

                    foreach (AtomEntry entry in feed.Entries)
                    {
                        string title = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Title"));
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Value"));

                        FilterItemInfo filterItemInfo = new FilterItemInfo(0, filterID, title, value, 0);

                        DataProviderB2C.FilterItemDAO.Insert(filterItemInfo);
                    }
                }
            }
		}

	}
}
