using System;

namespace BaiRong.Model
{
	[Serializable]
	public class PermissionsInRolesInfo
	{

		private string roleName;
		private string generalPermissions;

		public PermissionsInRolesInfo()
		{
			this.roleName = string.Empty;
			this.generalPermissions = string.Empty;
		}

        public PermissionsInRolesInfo(string roleName, string generalPermissions) 
		{
			this.roleName = roleName;
			this.generalPermissions = generalPermissions;
		}

		public string RoleName
		{
			get{ return roleName; }
			set{ roleName = value; }
		}

		public string GeneralPermissions
		{
			get{ return generalPermissions; }
			set{ generalPermissions = value; }
		}


	}
}
