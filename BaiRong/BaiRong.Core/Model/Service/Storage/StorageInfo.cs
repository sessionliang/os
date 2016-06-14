using System;
using BaiRong.Model;

namespace BaiRong.Model.Service
{
    public class StorageInfo
    {
        private int storageID;
        private string storageName;
        private string storageUrl;
        private EStorageType storageType;
        private bool isEnabled;
        private string description;
        private DateTime addDate;

        public StorageInfo()
        {
            this.storageID = 0;
            this.storageName = string.Empty;
            this.storageUrl = string.Empty;
            this.storageType = EStorageType.Ftp;
            this.isEnabled = true;
            this.description = string.Empty;
            this.addDate = DateTime.Now;
        }

        public StorageInfo(int storageID, string storageName, string storageUrl, EStorageType storageType, bool isEnabled, string description, DateTime addDate)
        {
            this.storageID = storageID;
            this.storageName = storageName;
            this.storageUrl = storageUrl;
            this.storageType = storageType;
            this.isEnabled = isEnabled;
            this.description = description;
            this.addDate = addDate;
        }

        public int StorageID
        {
            get { return storageID; }
            set { storageID = value; }
        }

        public string StorageName
        {
            get { return storageName; }
            set { storageName = value; }
        }

        public string StorageUrl
        {
            get { return storageUrl; }
            set { storageUrl = value; }
        }

        public EStorageType StorageType
        {
            get { return storageType; }
            set { storageType = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
    }
}
