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
    internal class ConferenceIE
    {
        private readonly int publishmentSystemID;
        private readonly string directoryPath;

        public ConferenceIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }
        public void Export()
        {
            List<ConferenceInfo> conferenceInfoList = DataProviderWX.ConferenceDAO.GetConferenceInfoList(this.publishmentSystemID);

            string conferenceDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(conferenceDirectoryPath);

            foreach (ConferenceInfo conferenceInfo in conferenceInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, conferenceInfo.ID + ".xml");

                AtomFeed feed = ExportConferenceInfo(conferenceInfo);
                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(conferenceInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                List<ConferenceContentInfo> conferenceContentInfoList = DataProviderWX.ConferenceContentDAO.GetConferenceContentInfoList(this.publishmentSystemID, conferenceInfo.ID);

                foreach (ConferenceContentInfo conferenceContentInfo in conferenceContentInfoList)
                {
                    AtomEntry entry = ExportConferenceContentInfo(conferenceContentInfo);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);
            }
        }
        private static AtomFeed ExportConferenceInfo(ConferenceInfo conferenceInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in ConferenceAttribute.AllAttributes)
            {
                object valueObj = conferenceInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }
        private static AtomEntry ExportConferenceContentInfo(ConferenceContentInfo conferenceContentInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in ConferenceContentAttribute.AllAttributes)
            {
                object valueObj = conferenceContentInfo.GetValue(attributeName);
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
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                ConferenceInfo conferenceInfo = new ConferenceInfo();

                foreach (string attributeNames in ConferenceAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    conferenceInfo.SetValue(attributeNames, value);
                }

                conferenceInfo.PublishmentSystemID = this.publishmentSystemID;
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                conferenceInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID, keywordName);

                int conferenceID = DataProviderWX.ConferenceDAO.Insert(conferenceInfo);

                foreach (AtomEntry entry in feed.Entries)
                {
                    ConferenceContentInfo conferenceContentInfo = new ConferenceContentInfo();

                    foreach (string attributeNames in ConferenceContentAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        conferenceContentInfo.SetValue(attributeNames, value);
                    }

                    conferenceContentInfo.ConferenceID = conferenceID;
                    conferenceContentInfo.PublishmentSystemID = this.publishmentSystemID;

                    DataProviderWX.ConferenceContentDAO.Insert(conferenceContentInfo);
                }
            }
        }
    }
}
