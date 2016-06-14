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

namespace SiteServer.STL.ImportExport
{
	internal class PhotoIE
	{
        private PublishmentSystemInfo publishmentSystemInfo;
		private string directoryPath;

        public PhotoIE(PublishmentSystemInfo publishmentSystemInfo, string directoryPath)
		{
            this.publishmentSystemInfo = publishmentSystemInfo;
			this.directoryPath = directoryPath;
		}

        public void ExportPhoto(int contentID)
		{
            string filePath = PathUtils.Combine(this.directoryPath, contentID + ".xml");

            AtomFeed feed = AtomUtility.GetEmptyFeed();

            List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(this.publishmentSystemInfo.PublishmentSystemID, contentID);

            foreach (PhotoInfo photoInfo in photoInfoList)
			{
                AddAtomEntry(feed, photoInfo);
			}
			feed.Save(filePath);
		}

        private static void AddAtomEntry(AtomFeed feed, PhotoInfo photoInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "ID", photoInfo.ID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", photoInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "ContentID", photoInfo.ContentID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "SmallUrl", photoInfo.SmallUrl);
            AtomUtility.AddDcElement(entry.AdditionalElements, "MiddleUrl", photoInfo.MiddleUrl);
            AtomUtility.AddDcElement(entry.AdditionalElements, "LargeUrl", photoInfo.LargeUrl);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", photoInfo.Taxis.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "Description", AtomUtility.Encrypt(photoInfo.Description));//加密

            feed.Entries.Add(entry);
		}

		public void ImportPhoto(int contentIDFromFile, int contentID)
		{
            string filePath = PathUtils.Combine(this.directoryPath, contentIDFromFile + ".xml");
            if (!FileUtils.IsFileExists(filePath)) return;

            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            foreach (AtomEntry entry in feed.Entries)
            {
                string smallUrl = AtomUtility.GetDcElementContent(entry.AdditionalElements, "SmallUrl");
                string middleUrl = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MiddleUrl");
                string largeUrl = AtomUtility.GetDcElementContent(entry.AdditionalElements, "LargeUrl");
                string description = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Description"));

                PhotoInfo photoInfo = new PhotoInfo(0, this.publishmentSystemInfo.PublishmentSystemID, contentID, smallUrl, middleUrl, largeUrl, 0, description);

                DataProvider.PhotoDAO.Insert(photoInfo);
            }
		}

	}
}
