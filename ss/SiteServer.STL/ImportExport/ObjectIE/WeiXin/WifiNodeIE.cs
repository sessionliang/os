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
    internal class WifiNodeIE
    {
        private readonly int publishmentSystemID;
        private readonly string filePath;

        public WifiNodeIE(int publishmentSystemID, string filePath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.filePath = filePath;
        }

        public void Export()
        {
            List<WifiNodeInfo> WifiNodeInfoList = DataProviderWX.WifiNodeDAO.GetWifiNodeInfoList(this.publishmentSystemID);

            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (WifiNodeInfo WifiNodeInfo in WifiNodeInfoList)
            {
                AtomEntry entry = ExportWifiNodeInfo(WifiNodeInfo);
                feed.Entries.Add(entry);
            }

            feed.Save(filePath);
        }

        private static AtomEntry ExportWifiNodeInfo(WifiNodeInfo WifiNodeInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in WifiNodeAttribute.AllAttributes)
            {
                object valueObj = WifiNodeInfo.GetValue(attributeName);
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
                WifiNodeInfo WifiNodeInfo = new WifiNodeInfo();

                foreach (string attributeName in WifiNodeAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    WifiNodeInfo.SetValue(attributeName, value);
                }

                WifiNodeInfo.PublishmentSystemID = this.publishmentSystemID;
                DataProviderWX.WifiNodeDAO.Insert(WifiNodeInfo);
            }
        }
    }
}
