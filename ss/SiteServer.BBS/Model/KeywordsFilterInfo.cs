using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Model
{
    public class KeywordsFilterInfo
    {
        private int id;
        private int publishmentSystemID;
        private int categoryid;
        private int grade;
        private string name;
        private string replacement;
        private int taxis;

        //public KeywordsFilterInfo()
        //{
 
        //}

        public KeywordsFilterInfo(int id, int publishmentSystemID, int categoryid, int grade, string name, string replacement, int taxis)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.categoryid = categoryid;
            this.grade = grade;
            this.name = name;
            this.replacement = replacement;
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

        public int CategoryID
        {
            get { return categoryid; }
            set { categoryid = value; }
        }

        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Replacement
        {
            get { return replacement; }
            set { replacement = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
    }
}
