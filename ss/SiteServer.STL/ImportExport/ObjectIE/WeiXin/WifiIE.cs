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
    internal class WifiIE
    {
        private readonly int publishmentSystemID;
        private readonly string filePath;

        public WifiIE(int publishmentSystemID, string filePath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.filePath = filePath;
        }

        public void Export()
        {
            List<WifiInfo> WifiInfoList = DataProviderWX.WifiDAO.GetWifiInfoList(this.publishmentSystemID);

            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (WifiInfo WifiInfo in WifiInfoList)
            {
                AtomEntry entry = ExportWifiInfo(WifiInfo);
                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(WifiInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(entry.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));
                feed.Entries.Add(entry);
            }

            feed.Save(filePath);
        }

        private static AtomEntry ExportWifiInfo(WifiInfo WifiInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in WifiAttribute.AllAttributes)
            {
                object valueObj = WifiInfo.GetValue(attributeName);
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
                WifiInfo WifiInfo = new WifiInfo();

                foreach (string attributeName in WifiAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    WifiInfo.SetValue(attributeName, value);
                }

                WifiInfo.PublishmentSystemID = this.publishmentSystemID;
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "KeywordName"));
                WifiInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID, keywordName);
                DataProviderWX.WifiDAO.Insert(WifiInfo);
            }
        }
    }
}
