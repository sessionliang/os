using System;
namespace SiteServer.CMS.Model
{
    public class GovInteractPermissionsInfo
    {
        private string userName;
        private int nodeID;
        private string permissions;

        public GovInteractPermissionsInfo()
        {
            this.userName = string.Empty;
            this.nodeID = 0;
            this.permissions = string.Empty;
        }

        public GovInteractPermissionsInfo(string userName, int nodeID, string permissions)
        {
            this.userName = userName;
            this.nodeID = nodeID;
            this.permissions = permissions;
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public int NodeID
        {
            get { return nodeID; }
            set { nodeID = value; }
        }

        public string Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }
    }
}
