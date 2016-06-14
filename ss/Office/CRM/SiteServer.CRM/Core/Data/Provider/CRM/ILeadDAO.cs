using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface ILeadDAO
	{
        int Insert(LeadInfo leadInfo);

        void Update(LeadInfo leadInfo);

        void UpdateStatus(int leadID, ELeadStatus status);

        void Delete(List<int> deleteIDList);

        LeadInfo GetLeadInfo(int leadID);

        int GetCount();

        int GetCountByDepartmentID(int departmentID);

        int GetCountByDepartmentID(int departmentID, DateTime begin, DateTime end);

        int GetCountByDepartmentIDAndStatus(int departmentID, ELeadStatus status, DateTime begin, DateTime end);

        int GetCountByChargeUserName(string chargeUserName, DateTime begin, DateTime end);

        int GetCountByChargeUserNameAndStatus(string chargeUserName, ELeadStatus status, DateTime begin, DateTime end);

        Dictionary<string, int> GetSourceDictionaryByDepartmentID(int departmentID, DateTime begin, DateTime end);

        Dictionary<string, int> GetSourceDictionaryByChargeUserName(string chargeUserName, DateTime begin, DateTime end);

        string GetSelectString(string userName);

        string GetSelectString(string userName, string status, string keyword);

        string GetOrderByString();

        string GetSortFieldName();
	}
}
