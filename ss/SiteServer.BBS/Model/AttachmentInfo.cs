using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model; 
 
namespace SiteServer.BBS.Model
{
    public class AttachmentInfo
    { 
        private int id;
        private int publishmentSystemID;
        private int threadID;
        private int postID;
        private string userName;
        private bool isInContent;
        private bool isImage;
        private int price;
        private string fileName;
        private string fileType;
        private int fileSize;
        private string attachmentUrl;
        private string imageUrl;
        private int downloads; 
        private DateTime addDate;
        private string description;

        //public AttachmentInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.threadID = 0;
        //    this.postID = 0;
        //    this.userName = string.Empty;
        //    this.isInContent = false;
        //    this.isImage = false;
        //    this.price = 0;
        //    this.fileName = string.Empty;
        //    this.fileType = string.Empty;
        //    this.fileSize = 0;
        //    this.attachmentUrl = string.Empty;
        //    this.imageUrl = string.Empty;
        //    this.downloads = 0;
        //    this.addDate = DateTime.Now;
        //    this.description = string.Empty;
        //}

        public AttachmentInfo(int id, int publishmentSystemID, int threadID, int postID, string userName, bool isInContent, bool isImage, int price, string fileName, string fileType, int fileSize, string attachmentUrl, string imageUrl, int downloads, DateTime addDate, string description)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.threadID = threadID;
            this.postID = postID;
            this.userName = userName;
            this.isInContent = isInContent;
            this.isImage = isImage;
            this.price = price;
            this.fileName = fileName;
            this.fileType = fileType;
            this.fileSize = fileSize;
            this.attachmentUrl = attachmentUrl;
            this.imageUrl = imageUrl;
            this.downloads = downloads;
            this.addDate = addDate;
            this.description = description;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int ThreadID
        {
            get { return threadID; }
            set { threadID = value; }
        }

        public int PostID
        {
            get { return postID; }
            set { postID = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public bool IsInContent
        {
            get { return isInContent; }
            set { isInContent = value; }
        }

        public bool IsImage
        {
            get { return isImage; }
            set { isImage = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string FileType
        {
            get { return fileType; }
            set { fileType = value; }
        }

        public int FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public string AttachmentUrl
        {
            get { return attachmentUrl; }
            set { attachmentUrl = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public int Downloads
        {
            get { return downloads; }
            set { downloads = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
