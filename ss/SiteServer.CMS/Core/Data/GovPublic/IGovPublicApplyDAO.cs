using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicApplyDAO
	{
        string TableName
        {
            get;
        }

        int Insert(GovPublicApplyInfo info);

        void Update(GovPublicApplyInfo info);

        void UpdateState(int applyID, EGovPublicApplyState state);

        void UpdateDepartmentID(int applyID, int departmentID);

        void UpdateDepartmentID(ArrayList idCollection, int departmentID);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int styleID);

        GovPublicApplyInfo GetApplyInfo(int publishmentSystemID, int styleID, NameValueCollection form);

        GovPublicApplyInfo GetApplyInfo(int publishmentSystemID, bool isOrganization, string queryName, string queryCode);

        GovPublicApplyInfo GetApplyInfo(int applyID);

        EGovPublicApplyState GetState(int applyID);

        int GetCountByStyleID(int styleID);

        int GetCountByPublishmentSystemID(int publishmentSystemID);

        int GetCountByDepartmentID(int publishmentSystemID, int departmentID);

        int GetCountByDepartmentID(int publishmentSystemID, int departmentID, DateTime begin, DateTime end);

        int GetCountByDepartmentIDAndState(int publishmentSystemID, int departmentID, EGovPublicApplyState state);

        int GetCountByDepartmentIDAndState(int publishmentSystemID, int departmentID, EGovPublicApplyState state, DateTime begin, DateTime end);

        string GetSelectStringByState(int publishmentSystemID, params EGovPublicApplyState[] states);

        string GetSelectString(int publishmentSystemID);

        string GetSelectString(int publishmentSystemID, string state, string keyword);

        string GetSortFieldName();
	}
}
