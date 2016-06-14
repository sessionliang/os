using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace BaiRong.Core.Data.Provider
{
	public interface IRoleDAO
	{
        void InsertRole(string roleName, ArrayList productIDArrayList, string creatorUserName, string description);

        void UpdateRole(string roleName, ArrayList productIDArrayList, string description);

        List<string> GetProductIDList(string roleName);

        bool DeleteRole(string roleName);

        string GetRoleDescription(string roleName);

        string GetRolesCreatorUserName(string roleName);

        ArrayList GetRoleNameArrayListByCreatorUserName(string creatorUserName);

        string[] GetAllRoles();

        string[] GetAllRolesByCreatorUserName(string creatorUserName);

        string[] GetRolesForUser(string userName);

        string[] GetUsersInRole(string roleName);

        void RemoveUserFromRole(string userName, string roleName);

        void RemoveUserFromRoles(string userName, string[] roleNames);

        bool IsRoleExists(string roleName);

        void AddUserToRoles(string userName, string[] roleNames);

        void AddUserToRole(string userName, string roleName);

        string[] FindUsersInRole(string roleName, string usernameToMatch);

        bool IsUserInRole(string userName, string roleName);
	}
}
