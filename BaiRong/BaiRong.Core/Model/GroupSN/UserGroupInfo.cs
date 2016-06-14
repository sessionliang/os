using System;
using BaiRong.Model;

namespace BaiRong.Model
{
	public class UserGroupInfo
	{
        private int groupID;
        private string groupSN;
        private string groupName;
        private EUserGroupType groupType;
        private int creditsFrom;
        private int creditsTo;
        private int stars;
        private string color;
        private string extendValues;

        //public UserGroupInfo()
        //{
        //    this.groupID = 0;
        //    this.publishmentSystemID = 0;
        //    this.groupName = string.Empty;
        //    this.groupType = EUserGroupType.Credits;
        //    this.creditsFrom = 0;
        //    this.creditsTo = 0;
        //    this.stars = 0;
        //    this.color = string.Empty;
        //    this.extendValues = string.Empty;
        //}

        public UserGroupInfo(int groupID, string groupSN, string groupName, EUserGroupType groupType, int creditsFrom, int creditsTo, int stars, string color, string extendValues) 
		{
            this.groupID = groupID;
            this.groupSN = groupSN;
            this.groupName = groupName;
            this.groupType = groupType;
            this.creditsFrom = creditsFrom;
            this.creditsTo = creditsTo;
            this.stars = stars;
            this.color = color;
            this.extendValues = extendValues;
		}

        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        public string GroupSN
        {
            get { return groupSN; }
            set { groupSN = value; }
        }

        public string GroupName
		{
            get { return groupName; }
            set { groupName = value; }
		}

        public EUserGroupType GroupType
        {
            get { return groupType; }
            set { groupType = value; }
        }

        public int CreditsFrom
        {
            get { return creditsFrom; }
            set { creditsFrom = value; }
        }

        public int CreditsTo
        {
            get { return creditsTo; }
            set { creditsTo = value; }
        }

        public int Stars
		{
            get { return stars; }
            set { stars = value; }
		}

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string ExtendValues
        {
            get
            {
                return extendValues;
            }
            set
            {
                extendValues = value;
            }
        }

        UserGroupInfoExtend additional;
        public UserGroupInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new UserGroupInfoExtend(this.extendValues);
                }
                return this.additional;
            }
            set
            {
                this.additional = value;
            }
        }
	}
}
