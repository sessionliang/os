using System;

namespace BaiRong.Model.Service
{
    public class FTPStorageInfo
    {
        private int storageID;        
        private string server;
        private int port;
        private string userName;
        private string password;
        private bool isPassiveMode;

        public FTPStorageInfo()
        {
            this.storageID = 0;
            this.server = string.Empty;
            this.port = 0;
            this.userName = string.Empty;
            this.password = string.Empty;
            this.isPassiveMode = true;
        }

        public FTPStorageInfo(int storageID, string server, int port, string userName, string password, bool isPassiveMode)
        {
            this.storageID = storageID;
            this.server = server;
            this.port = port;
            this.userName = userName;
            this.password = password;
            this.isPassiveMode = isPassiveMode;
        }

        public int StorageID
        {
            get { return storageID; }
            set { storageID = value; }
        }

        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public bool IsPassiveMode
        {
            get { return isPassiveMode; }
            set { isPassiveMode = value; }
        }
    }
}
