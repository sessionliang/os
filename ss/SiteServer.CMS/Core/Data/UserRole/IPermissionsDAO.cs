using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IPermissionsDAO
	{
        void InsertRoleAndPermissions(string roleName, ArrayList modules, string creatorUserName, string description, ArrayList generalPermissionArrayList, ArrayList psPermissionsInRolesInfoArrayList);

        void UpdatePublishmentPermissions(string roleName, ArrayList psPermissionsInRolesInfoArrayList);

		void DeleteRoleAndPermissions(string roleName);
	}
}
