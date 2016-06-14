using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;

namespace SiteServer.CRM.Core
{
	public interface IApplyDAO
	{
        int Insert(ApplyInfo applyinfo);

        void Update(ApplyInfo applyinfo);

        void UpdateState(int applyID, EApplyState state);

        void Delete(ArrayList deleteIDArrayList);

        ApplyInfo GetApplyInfo(int projectID, NameValueCollection form);

        ApplyInfo GetApplyInfo(int projectID, string queryCode);

        ApplyInfo GetApplyInfo(int applyID);

        EApplyState GetState(int applyID);

        ArrayList GetAddUserNameArrayList(int projectID);

        ArrayList GetUserNameArrayList(int projectID);

        int GetCountByProjectID(int projectID);

        int GetCount();

        int GetCountByDepartmentID(int departmentID);

        int GetCountByDepartmentID(int departmentID, int projectID, DateTime begin, DateTime end);

        int GetCountByDepartmentIDAndState(int departmentID, EApplyState state);

        int GetCountByDepartmentIDAndState(int departmentID, int projectID, EApplyState state, DateTime begin, DateTime end);

        string GetSelectStringByState(int projectID, params EApplyState[] states);

        string GetSelectString(int projectID);

        string GetSelectStringToWork(int projectID);

        string GetSelectString(int projectID, string state, int typeID, string addUserName, string userName, string keyword);

        string GetSelectStringToWork(int projectID, string state, int typeID, string addUserName, string userName, string keyword);

        string GetSortFieldName();
	}
}
