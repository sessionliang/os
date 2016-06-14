using System;

namespace SiteServer.CMS.Model
{
	[Serializable]
	public class SystemPermissionsInfo
	{
		private string roleName;
		private int publishmentSystemID;
		private string nodeIDCollection;
		private string channelPermissions;
		private string websitePermissions;

		public SystemPermissionsInfo()
		{
			this.roleName = string.Empty;
			this.publishmentSystemID = 0;
			this.nodeIDCollection = string.Empty;
			this.channelPermissions = string.Empty;
			this.websitePermissions = string.Empty;
		}

        public SystemPermissionsInfo(string roleName, int publishmentSystemID, string nodeIDCollection, string channelPermissions, string websitePermissions) 
		{
			this.roleName = roleName;
			this.publishmentSystemID = publishmentSystemID;
			this.nodeIDCollection = nodeIDCollection;
			this.channelPermissions = channelPermissions;
			this.websitePermissions = websitePermissions;
		}

		public string RoleName
		{
			get{ return roleName; }
			set{ roleName = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

		public string NodeIDCollection
		{
			get{ return nodeIDCollection; }
			set{ nodeIDCollection = value; }
		}

		public string ChannelPermissions
		{
			get{ return channelPermissions; }
			set{ channelPermissions = value; }
		}

		public string WebsitePermissions
		{
			get{ return websitePermissions; }
			set{ websitePermissions = value; }
		}


	}
}
