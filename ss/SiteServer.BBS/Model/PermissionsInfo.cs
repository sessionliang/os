using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model; 
 
namespace SiteServer.BBS.Model
{
    public class PermissionsInfo
	{
        private int userGroupID;
        private int publishmentSystemID;
        private int forumID;
        private string forbidden;

        //public PermissionsInfo()
        //{
        //    this.userGroupID = 0;
        //    this.publishmentSystemID = 0;
        //    this.forumID = 0;
        //    this.forbidden = string.Empty;
        //}

        public PermissionsInfo(int userGroupID, int publishmentSystemID, int forumID, string forbidden)
		{
            this.userGroupID = userGroupID;
            this.publishmentSystemID = publishmentSystemID;
            this.forumID = forumID;
            this.forbidden = forbidden;
		}

        public int UserGroupID
		{
            get { return userGroupID; }
            set { userGroupID = value; }
		}

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int ForumID
        {
            get { return forumID; }
            set { forumID = value; }
        }

        public string Forbidden
		{
            get { return forbidden; }
            set { forbidden = value; }
		}
	}
}
