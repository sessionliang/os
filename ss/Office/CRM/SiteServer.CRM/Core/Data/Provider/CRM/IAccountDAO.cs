using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IAccountDAO
	{
        int Insert(AccountInfo accountInfo);

        void Update(AccountInfo accountInfo);

        void UpdateStatus(int accountID, EAccountStatus status);

        void Delete(List<int> deleteIDList);

        AccountInfo GetAccountInfo(int accountID);

        string GetAccountName(int accountID);

        int GetCount(int publishmentSystemID);

        int GetCountByDepartmentID(int publishmentSystemID, int departmentID);

        int GetCountByDepartmentID(int departmentID, DateTime begin, DateTime end);

        int GetCountByDepartmentIDAndStatus(int departmentID, EAccountStatus status, DateTime begin, DateTime end);

        int GetCountByUserName(string chargeUserName, DateTime begin, DateTime end);

        int GetCountByUserNameAndStatus(string chargeUserName, EAccountStatus status, DateTime begin, DateTime end);

        string GetSelectStringByStatus(string chargeUserName, EAccountStatus status);

        string GetSelectString(string userName);

        string GetSelectString(string userName, string accountType, string keyword);

        string GetSortFieldName();
	}
}
