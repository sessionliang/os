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
    internal class CountIE
    {
        private readonly int publishmentSystemID;
        private readonly string filePath;

        public CountIE(int publishmentSystemID, string filePath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.filePath = filePath;
        }

        public void Export()
        {
            List<CountInfo> countInfoList = DataProviderWX.CountDAO.GetCountInfoList(this.publishmentSystemID);

            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (CountInfo countInfo in countInfoList)
            {
                AtomEntry entry = ExportCountInfo(countInfo);
                feed.Entries.Add(entry);
            }

            feed.Save(filePath);
        }

        private static AtomEntry ExportCountInfo(CountInfo countInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in CountAttribute.AllAttributes)
            {
                object valueObj = countInfo.GetValue(attributeName);
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
                CountInfo countInfo = new CountInfo();

                foreach (string attributeName in CountAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    countInfo.SetValue(attributeName, value);
                }

                countInfo.PublishmentSystemID = this.publishmentSystemID;
                countInfo.CountType = ECountTypeUtils.GetEnumType(AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "CountType")));

                DataProviderWX.CountDAO.Insert(countInfo);
            }
        }
    }
}
