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
    internal class AlbumIE
    {
        private readonly int publishmentSystemID;
        private readonly string directoryPath;

        public AlbumIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            List<AlbumInfo> albumInfoList = DataProviderWX.AlbumDAO.GetAlbumInfoList(this.publishmentSystemID);

            string albumDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(albumDirectoryPath);

            foreach (AlbumInfo albumInfo in albumInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, albumInfo.ID + ".xml");

                AtomFeed feed = ExportAlbumInfo(albumInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(albumInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                feed.Save(filePath);

                string albumContentDirectoryPath = PathUtils.Combine(albumDirectoryPath, albumInfo.ID.ToString());

                DirectoryUtils.CreateDirectoryIfNotExists(albumContentDirectoryPath);

                List<AlbumContentInfo> albumContentInfoList = DataProviderWX.AlbumContentDAO.GetAlbumContentInfoList(this.publishmentSystemID, albumInfo.ID, 0);

                foreach (AlbumContentInfo albumContentInfo in albumContentInfoList)
                {
                    string fileMessgeContentPath = PathUtils.Combine(albumContentDirectoryPath, albumContentInfo.ID + ".xml");

                    AtomFeed feedAlbumContent = ExportAlbumContentInfoFeed(albumContentInfo);

                    List<AlbumContentInfo> albumContentInfoListEntry = DataProviderWX.AlbumContentDAO.GetAlbumContentInfoList(this.publishmentSystemID, albumInfo.ID, albumContentInfo.ID);

                    foreach (AlbumContentInfo albumContentInfoEntry in albumContentInfoListEntry)
                    {
                        AtomEntry entry = ExportAlbumContentInfoEntry(albumContentInfoEntry);
                        feedAlbumContent.Entries.Add(entry);
                    }

                    feedAlbumContent.Save(fileMessgeContentPath);
                }
            }
        }
        private static AtomFeed ExportAlbumInfo(AlbumInfo albumInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in AlbumAttribute.AllAttributes)
            {
                object valueObj = albumInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }
        private static AtomFeed ExportAlbumContentInfoFeed(AlbumContentInfo albumContentInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in AlbumContentAttribute.AllAttributes)
            {
                object valueObj = albumContentInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }
        private static AtomEntry ExportAlbumContentInfoEntry(AlbumContentInfo albumContentInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in AlbumContentAttribute.AllAttributes)
            {
                object valueObj = albumContentInfo.GetValue(attributeName);
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

                AlbumInfo albumInfo = new AlbumInfo();

                foreach (string attributeNames in AlbumAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    albumInfo.SetValue(attributeNames, value);
                }

                albumInfo.PublishmentSystemID = this.publishmentSystemID;
                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                albumInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);

                int albumID = DataProviderWX.AlbumDAO.Insert(albumInfo);

                if (!DirectoryUtils.IsDirectoryExists(this.directoryPath + PathUtils.SeparatorChar + albumInfo.ID)) return;

                string dicAlbumPath = DirectoryUtils.GetDirectoryPath(this.directoryPath + PathUtils.SeparatorChar + albumInfo.ID);

                string[] fileAlbumPaths = DirectoryUtils.GetFilePaths(dicAlbumPath);

                foreach (string fileAlbumPath in fileAlbumPaths)
                {
                    AtomFeed feedAlbum = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(fileAlbumPath));

                    AlbumContentInfo albumContentInfo = new AlbumContentInfo();

                    foreach (string attributeNames in AlbumContentAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feedAlbum.AdditionalElements, attributeNames));
                        albumContentInfo.SetValue(attributeNames, value);
                    }
                    albumContentInfo.AlbumID = albumID;
                    albumContentInfo.PublishmentSystemID = this.publishmentSystemID;

                    int albumContentID = DataProviderWX.AlbumContentDAO.Insert(albumContentInfo);

                    foreach (AtomEntry entry in feedAlbum.Entries)
                    {
                        AlbumContentInfo albumContentInfoEntry = new AlbumContentInfo();

                        foreach (string attributeNames in AlbumContentAttribute.AllAttributes)
                        {
                            string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                            albumContentInfoEntry.SetValue(attributeNames, value);
                        }

                        albumContentInfoEntry.AlbumID = albumID;
                        albumContentInfoEntry.ParentID = albumContentID;
                        albumContentInfoEntry.PublishmentSystemID = this.publishmentSystemID;

                        DataProviderWX.AlbumContentDAO.Insert(albumContentInfoEntry);
                    }
                }
            }
        }
    }
}
