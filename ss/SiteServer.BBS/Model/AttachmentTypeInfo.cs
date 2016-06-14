using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model; 
 
namespace SiteServer.BBS.Model
{
    public class AttachmentTypeInfo
    { 
        private int id;
        private int publishmentSystemID;
        private string fileExtName;
        private int maxSize;

        //public AttachmentTypeInfo()
        //{
        //    this.id = 0;
        //    this.publishmentSystemID = 0;
        //    this.fileExtName = string.Empty;
        //    this.maxSize = 0;
        //}

        public AttachmentTypeInfo(int id, int publishmentSystemID, string fileExtName, int maxSize)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.fileExtName = fileExtName;
            this.maxSize = maxSize;
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

        public string FileExtName
        {
            get { return fileExtName; }
            set { fileExtName = value; }
        }

        public int MaxSize
        {
            get { return maxSize; }
            set { maxSize = value; }
        }
    }
}
