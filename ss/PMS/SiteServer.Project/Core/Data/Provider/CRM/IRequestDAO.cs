using SiteServer.Project.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;

namespace SiteServer.Project.Core
{
	public interface IRequestDAO
	{
        int Insert(RequestInfo requestInfo);

        void Update(RequestInfo requestInfo);

        void UpdateStatus(int requestID, ERequestStatus status);

        void Delete(ArrayList deleteIDArrayList);

        RequestInfo GetRequestInfo(NameValueCollection form);

        RequestInfo GetRequestInfo(int requestID);

        RequestInfo GetLastRequestInfo(string domain);

        int GetCount();

        int GetCountByDepartmentID(int departmentID);

        int GetCountByDepartmentID(int departmentID, DateTime begin, DateTime end);

        int GetCountByDepartmentIDAndStatus(int departmentID, ERequestStatus status, DateTime begin, DateTime end);

        int GetCountByAddUserName(string chargeUserName, DateTime begin, DateTime end);

        int GetCountByAddUserNameAndStatus(string chargeUserName, ERequestStatus status, DateTime begin, DateTime end);

        string GetSelectStringByStatus(string chargeUserName, ERequestStatus status);

        string GetSelectString(string userName);

        string GetSelectString(string userName, string status, string keyword);

        string GetSelectString(int licenseID, string domain);

        string GetSelectString(int licenseID, string domain, string status, string keyword);

        string GetSortFieldName();
	}
}
