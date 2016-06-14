using System;

namespace SiteServer.B2C.Model
{
    public class LocationInfo
    {
        private int id;
        private int publishmentSystemID;
        private string locationName;
        private int parentID;
        private string parentsPath;
        private int parentsCount;
        private int childrenCount;
        private bool isLastNode;
        private int taxis;

        public LocationInfo()
        {
            this.id = 0;
            this.publishmentSystemID = 0;
            this.locationName = string.Empty;
            this.parentID = 0;
            this.parentsPath = string.Empty;
            this.parentsCount = 0;
            this.childrenCount = 0;
            this.isLastNode = false;
            this.taxis = 0;
        }

        public LocationInfo(int id, int publishmentSystemID, string locationName, int parentID, string parentsPath, int parentsCount, int childrenCount, bool isLastNode, int taxis)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.locationName = locationName;
            this.parentID = parentID;
            this.parentsPath = parentsPath;
            this.parentsCount = parentsCount;
            this.childrenCount = childrenCount;
            this.isLastNode = isLastNode;
            this.taxis = taxis;
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

        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; }
        }

        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public string ParentsPath
        {
            get { return parentsPath; }
            set { parentsPath = value; }
        }

        public int ParentsCount
        {
            get { return parentsCount; }
            set { parentsCount = value; }
        }

        public int ChildrenCount
        {
            get { return childrenCount; }
            set { childrenCount = value; }
        }

        public bool IsLastNode
        {
            get { return isLastNode; }
            set { isLastNode = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
    }
}
