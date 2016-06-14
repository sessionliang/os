using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;
using SiteServer.WeiXin.Model;
using SiteServer.WeiXin.Core;

namespace SiteServer.STL.ImportExport
{
    internal class MapIE
    {
        private readonly int publishmentSystemID;
        private readonly string filePath;

        public MapIE(int publishmentSystemID, string filePath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.filePath = filePath;
        }

        public void Export()
        {
            List<MapInfo> mapInfoList = DataProviderWX.MapDAO.GetMapInfoList(this.publishmentSystemID);

            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (MapInfo mapInfo in mapInfoList)
            {
                AtomEntry entry = ExportMapInfo(mapInfo);
                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(mapInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(entry.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));
                feed.Entries.Add(entry);
            }

            feed.Save(filePath);
        }

        private static AtomEntry ExportMapInfo(MapInfo mapInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in MapAttribute.AllAttributes)
            {
                object valueObj = mapInfo.GetValue(attributeName);
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
            if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            foreach (AtomEntry entry in feed.Entries)
            {
                MapInfo mapInfo = new MapInfo();

                foreach (string attributeName in MapAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    mapInfo.SetValue(attributeName, value);
                }
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "KeywordName"));
                mapInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID, keywordName);

                mapInfo.PublishmentSystemID = this.publishmentSystemID;
                DataProviderWX.MapDAO.Insert(mapInfo);
            }
        }

    }
}
