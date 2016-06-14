using System;
using System.Data;
using System.Collections;
using BaiRong.Model;
using System.Collections.Generic;

namespace BaiRong.Core.Data.Provider
{
	public interface IAdministratorDAO
	{
        void Insert(AdministratorInfo info);

        void Update(AdministratorInfo info);

        void UpdateLastActivityDateAndPublishmentSystemID(string userName, int publishmentSystemID);

        void UpdateLastActivityDateAndProductID(string userName, string productID);

        void UpdateLastActivityDate(string userName, string productID);

        void UpdatePublishmentSystemIDCollection(string userName, string publishmentSystemIDCollection);

        void UpdatePublishmentSystemID(string userName, int publishmentSystemID);

        void Delete(string userName);

        void LockUsers(ArrayList userNameArrayList, bool isLockOut);

        bool IsUserNameExists(string userName);

        AdministratorInfo GetAdministratorInfo(string userName);

        int GetDepartmentID(string userName);

        int GetAreaID(string userName);

        string GetSelectCommand(bool isConsoleAdministrator, string creatorUserName, int departmentID);

        string GetSelectCommand(string searchWord, string roleName, int dayOfLastActivity, bool isConsoleAdministrator, string creatorUserName, int departmentID, int areaID);

        string GetSortFieldName();

        int GetNumberOfUsersOnline(int userIsOnlineTimeWindow);

        string GetCreatorUserName(string userName);

        List<int> GetPublishmentSystemIDList(string userName);

        int GetPublishmentSystemID(string userName);

        string GetDisplayName(string userName);

        string GetTheme(string userName);

        string GetLanguage(string userName);

        string GetLastProductID(string userName);

        ArrayList GetUserNameArrayListByCreatorUserName(string creatorUserName);

        ArrayList GetUserNameArrayList();

        ArrayList GetUserNameArrayList(List<int> departmentIDList);

        ArrayList GetUserNameArrayList(int departmentID, bool isAll);

        ArrayList GetUserNameArrayList(string searchWord, int dayOfCreation, int dayOfLastActivity, bool isChecked);

        bool ChangePassword(string userName, EPasswordFormat passwordFormat, string password);

        bool CreateUser(AdministratorInfo userInfo, out string errorMessage);

        bool ValidateUser(string userName, string password, out string errorMessage);

        bool CheckPassword(string password, string dbpassword, EPasswordFormat passwordFormat, string passwordSalt);

        string GetPassword(string password, EPasswordFormat passwordFormat, string passwordSalt);

        string UserName { get; }

        bool IsAuthenticated { get; }

        string GetRedirectUrl(string userName, bool createPersistentCookie);

        void RedirectFromLoginPage(string userName, bool createPersistentCookie);

        void Login(string userName, bool createPersistentCookie);

        void Logout();
	}
}
