using System;

namespace BaiRong.Model
{
    public class AreaInfo
    {
        private int areaID;
        private string areaName;
        private int parentID;
        private string parentsPath;
        private int parentsCount;
        private int childrenCount;
        private bool isLastNode;
        private int taxis;
        private int countOfAdmin;

        public AreaInfo()
        {
            this.areaID = 0;
            this.areaName = string.Empty;
            this.parentID = 0;
            this.parentsPath = string.Empty;
            this.parentsCount = 0;
            this.childrenCount = 0;
            this.isLastNode = false;
            this.taxis = 0;
            this.countOfAdmin = 0;
        }

        public AreaInfo(int areaID, string areaName, int parentID, string parentsPath, int parentsCount, int childrenCount, bool isLastNode, int taxis, int countOfAdmin)
        {
            this.areaID = areaID;
            this.areaName = areaName;
            this.parentID = parentID;
            this.parentsPath = parentsPath;
            this.parentsCount = parentsCount;
            this.childrenCount = childrenCount;
            this.isLastNode = isLastNode;
            this.taxis = taxis;
            this.countOfAdmin = countOfAdmin;
        }

        public int AreaID
        {
            get { return areaID; }
            set { areaID = value; }
        }

        public string AreaName
        {
            get { return areaName; }
            set { areaName = value; }
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

        public int CountOfAdmin
        {
            get { return countOfAdmin; }
            set { countOfAdmin = value; }
        }
    }
}
