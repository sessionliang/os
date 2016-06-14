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
    internal class View360IE
    {
        private readonly int publishmentSystemID;
		private readonly string filePath;

        public View360IE(int publishmentSystemID, string filePath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.filePath = filePath;
		}

		public void Export()
		{
            List<View360Info> View360InfoList = DataProviderWX.View360DAO.GetView360InfoList(this.publishmentSystemID);

			AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (View360Info View360Info in View360InfoList)
			{
                AtomEntry entry = ExportView360Info(View360Info);
                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(View360Info.KeywordID).Keywords;
                AtomUtility.AddDcElement(entry.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));
				feed.Entries.Add(entry);               
			}

			feed.Save(filePath);
		}

        private static AtomEntry ExportView360Info(View360Info View360Info)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in View360Attribute.AllAttributes)
            {
                object valueObj = View360Info.GetValue(attributeName);
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
                View360Info View360Info = new View360Info();

                foreach (string attributeName in View360Attribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    View360Info.SetValue(attributeName, value);
                }

                View360Info.PublishmentSystemID = this.publishmentSystemID;
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "KeywordName"));
                View360Info.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);
                DataProviderWX.View360DAO.Insert(View360Info);
			}
		}
    }
}
