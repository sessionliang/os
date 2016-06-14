using System;

namespace BaiRong.Model.Service
{
    public class LocalStorageInfo
    {
        private int storageID;        
        private string directoryPath;

        public LocalStorageInfo()
        {
            this.storageID = 0;
            this.directoryPath = string.Empty;
        }

        public LocalStorageInfo(int storageID, string directoryPath)
        {
            this.storageID = storageID;
            this.directoryPath = directoryPath;
        }

        public int StorageID
        {
            get { return storageID; }
            set { storageID = value; }
        }

        public string DirectoryPath
        {
            get { return directoryPath; }
            set { directoryPath = value; }
        }
    }
}
