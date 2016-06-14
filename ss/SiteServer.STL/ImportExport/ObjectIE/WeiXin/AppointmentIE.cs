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
    internal class AppointmentIE
    {
        private int publishmentSystemID;

        private string directoryPath;

        public AppointmentIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            //appointment
            List<AppointmentInfo> appointmentInfoList = DataProviderWX.AppointmentDAO.GetAppointmentInfoList(this.publishmentSystemID);

            string appointmentDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(appointmentDirectoryPath);

            foreach (AppointmentInfo appointmentInfo in appointmentInfoList)
            {
                string filePath = PathUtils.Combine(appointmentDirectoryPath, appointmentInfo.ID + ".xml");

                AtomFeed feed = ExportAppointmentInfo(appointmentInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(appointmentInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));


                feed.Save(filePath);

                //appointmentItem

                string appointmentItemDirectoryPath = PathUtils.Combine(appointmentDirectoryPath, appointmentInfo.ID.ToString());

                DirectoryUtils.CreateDirectoryIfNotExists(appointmentItemDirectoryPath);

                List<AppointmentItemInfo> appointmentItemInfoList = DataProviderWX.AppointmentItemDAO.GetItemInfoList(this.publishmentSystemID, appointmentInfo.ID);

                foreach (AppointmentItemInfo appointmentItemInfo in appointmentItemInfoList)
                {
                    string fileAppointmentItemPath = PathUtils.Combine(appointmentItemDirectoryPath, appointmentItemInfo.ID + ".xml"); ;

                    AtomFeed feedAppointmentItem = ExportAppointmentItemInfo(appointmentItemInfo);

                    //appointmentContent

                    List<AppointmentContentInfo> appointmentContentInfoList = DataProviderWX.AppointmentContentDAO.GetAppointmentContentInfoList(this.publishmentSystemID, appointmentInfo.ID, appointmentItemInfo.ID);

                    foreach (AppointmentContentInfo appointmentContentInfo in appointmentContentInfoList)
                    {
                        AtomEntry entry = ExportAppointmentContentInfo(appointmentContentInfo);
                        feedAppointmentItem.Entries.Add(entry);
                    }

                    feedAppointmentItem.Save(fileAppointmentItemPath);
                }
            }
        }

        private AtomFeed ExportAppointmentInfo(AppointmentInfo appointmentInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in AppointmentAttribute.AllAttributes)
            {
                object valueObj = appointmentInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomFeed ExportAppointmentItemInfo(AppointmentItemInfo appointmentItemInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in AppointmentItemAttribute.AllAttributes)
            {
                object valueObj = appointmentItemInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportAppointmentContentInfo(AppointmentContentInfo appointmentContentInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in AppointmentContentAttribute.AllAttributes)
            {
                object valueObj = appointmentContentInfo.GetValue(attributeName);
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
                //appointment
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                AppointmentInfo appointmentInfo = new AppointmentInfo();

                foreach (string attributeNames in AppointmentAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    appointmentInfo.SetValue(attributeNames, value);
                }
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                appointmentInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);
                appointmentInfo.PublishmentSystemID = this.publishmentSystemID;

                int appointmentID = DataProviderWX.AppointmentDAO.Insert(appointmentInfo);

                //appointmentItem
                if (!DirectoryUtils.IsDirectoryExists(this.directoryPath + PathUtils.SeparatorChar + appointmentInfo.ID)) return;

                string dicAppointmentPath = DirectoryUtils.GetDirectoryPath(this.directoryPath + PathUtils.SeparatorChar + appointmentInfo.ID);

                string[] fileAppointmentPaths = DirectoryUtils.GetFilePaths(dicAppointmentPath);

                foreach (string fileAppointmentPath in fileAppointmentPaths)
                {
                    AtomFeed feedAppointment = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(fileAppointmentPath));

                    AppointmentItemInfo appointmentItemInfo = new AppointmentItemInfo();

                    foreach (string attributeNames in AppointmentItemAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feedAppointment.AdditionalElements, attributeNames));
                        appointmentItemInfo.SetValue(attributeNames, value);
                    }
                    appointmentItemInfo.AppointmentID = appointmentID;
                    appointmentItemInfo.PublishmentSystemID = this.publishmentSystemID;

                    int appointmentItemID = DataProviderWX.AppointmentItemDAO.Insert(appointmentItemInfo);

                    //appointmentContent
                    foreach (AtomEntry entry in feedAppointment.Entries)
                    {
                        AppointmentContentInfo appointmentContentInfo = new AppointmentContentInfo();

                        foreach (string attributeNames in AppointmentContentAttribute.AllAttributes)
                        {
                            string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                            appointmentContentInfo.SetValue(attributeNames, value);
                        }

                        appointmentContentInfo.AppointmentID = appointmentID;
                        appointmentContentInfo.AppointmentItemID = appointmentItemID;
                        appointmentContentInfo.PublishmentSystemID = this.publishmentSystemID;

                        DataProviderWX.AppointmentContentDAO.Insert(appointmentContentInfo);
                    }
                }
            }
        }
    }
}
