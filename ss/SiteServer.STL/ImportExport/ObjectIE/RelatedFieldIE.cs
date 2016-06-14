using System;
using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
	internal class RelatedFieldIE
	{
		private readonly int publishmentSystemID;
		private readonly string directoryPath;

        public RelatedFieldIE(int publishmentSystemID, string directoryPath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.directoryPath = directoryPath;
		}

        public void ExportRelatedField(RelatedFieldInfo relatedFieldInfo)
		{
            string filePath = this.directoryPath + PathUtils.SeparatorChar + relatedFieldInfo.RelatedFieldID + ".xml";

            AtomFeed feed = ExportRelatedFieldInfo(relatedFieldInfo);

            ArrayList relatedFieldItemInfoArrayList = DataProvider.RelatedFieldItemDAO.GetRelatedFieldItemInfoArrayList(relatedFieldInfo.RelatedFieldID, 0);

            foreach (RelatedFieldItemInfo relatedFieldItemInfo in relatedFieldItemInfoArrayList)
			{
                AddAtomEntry(feed, relatedFieldItemInfo, 1);
			}
			feed.Save(filePath);
		}

        private static AtomFeed ExportRelatedFieldInfo(RelatedFieldInfo relatedFieldInfo)
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

            AtomUtility.AddDcElement(feed.AdditionalElements, "RelatedFieldID", relatedFieldInfo.RelatedFieldID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "RelatedFieldName", relatedFieldInfo.RelatedFieldName);
            AtomUtility.AddDcElement(feed.AdditionalElements, "PublishmentSystemID", relatedFieldInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "TotalLevel", relatedFieldInfo.TotalLevel.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "Prefixes", relatedFieldInfo.Prefixes);
            AtomUtility.AddDcElement(feed.AdditionalElements, "Suffixes", relatedFieldInfo.Suffixes);

			return feed;
		}

        private static void AddAtomEntry(AtomFeed feed, RelatedFieldItemInfo relatedFieldItemInfo, int level)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "ID", relatedFieldItemInfo.ID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "RelatedFieldID", relatedFieldItemInfo.RelatedFieldID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "ItemName", relatedFieldItemInfo.ItemName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "ItemValue", relatedFieldItemInfo.ItemValue);
            AtomUtility.AddDcElement(entry.AdditionalElements, "ParentID", relatedFieldItemInfo.ParentID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", relatedFieldItemInfo.Taxis.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "Level", level.ToString());

            feed.Entries.Add(entry);

            ArrayList relatedFieldItemInfoArrayList = DataProvider.RelatedFieldItemDAO.GetRelatedFieldItemInfoArrayList(relatedFieldItemInfo.RelatedFieldID, relatedFieldItemInfo.ID);

            foreach (RelatedFieldItemInfo itemInfo in relatedFieldItemInfoArrayList)
            {
                AddAtomEntry(feed, itemInfo, level + 1);
            }
		}

		public void ImportRelatedField(bool overwrite)
		{
			if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return;
			string[] filePaths = DirectoryUtils.GetFilePaths(this.directoryPath);

			foreach (string filePath in filePaths)
			{
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                string relatedFieldName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "RelatedFieldName");
                int totalLevel = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "TotalLevel"));
                string prefixes = AtomUtility.GetDcElementContent(feed.AdditionalElements, "Prefixes");
                string suffixes = AtomUtility.GetDcElementContent(feed.AdditionalElements, "Suffixes");

                RelatedFieldInfo relatedFieldInfo = new RelatedFieldInfo(0, relatedFieldName, this.publishmentSystemID, totalLevel, prefixes, suffixes);

                RelatedFieldInfo srcRelatedFieldInfo = DataProvider.RelatedFieldDAO.GetRelatedFieldInfo(publishmentSystemID, relatedFieldName);
                if (srcRelatedFieldInfo != null)
                {
                    if (overwrite)
                    {
                        DataProvider.RelatedFieldDAO.Delete(srcRelatedFieldInfo.RelatedFieldID);
                    }
                    else
                    {
                        relatedFieldInfo.RelatedFieldName = DataProvider.RelatedFieldDAO.GetImportRelatedFieldName(publishmentSystemID, relatedFieldInfo.RelatedFieldName);
                    }
                }

                int relatedFieldID = DataProvider.RelatedFieldDAO.Insert(relatedFieldInfo);

                int lastInertedLevel = 1;
                int lastInsertedParentID = 0;
                int lastInsertedID = 0;
				foreach (AtomEntry entry in feed.Entries)
				{
                    string itemName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ItemName");
                    string itemValue = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ItemValue");
                    int level = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Level"));
                    int parentID = 0;
                    if (level > 1)
                    {
                        if (level != lastInertedLevel)
                        {
                            parentID = lastInsertedID;
                        }
                        else
                        {
                            parentID = lastInsertedParentID;
                        }
                    }

                    RelatedFieldItemInfo relatedFieldItemInfo = new RelatedFieldItemInfo(0, relatedFieldID, itemName, itemValue, parentID, 0);
                    lastInsertedID = DataProvider.RelatedFieldItemDAO.Insert(relatedFieldItemInfo);
                    lastInsertedParentID = parentID;
                    lastInertedLevel = level;
				}
			}
		}

	}
}
