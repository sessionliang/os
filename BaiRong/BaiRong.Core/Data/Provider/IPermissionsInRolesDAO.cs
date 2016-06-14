using System.Collections;
using System.Data;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IPermissionsInRolesDAO
	{
        void InsertRoleAndPermissions(string roleName, ArrayList modules, string creatorUserName, string description, ArrayList generalPermissionArrayList);

		void InsertWithTrans(PermissionsInRolesInfo info, IDbTransaction trans);

		void DeleteWithTrans(string roleName, IDbTransaction trans);

        void Delete(string roleName);

        void UpdateRoleAndGeneralPermissions(string roleName, ArrayList modules, string description, ArrayList generalPermissionArrayList);

        ArrayList GetGeneralPermissionArrayList(string[] roles);
	}
}
