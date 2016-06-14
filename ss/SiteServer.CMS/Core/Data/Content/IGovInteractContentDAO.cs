using System;
using System.Collections;
using System.Data;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
	public interface IGovInteractContentDAO
	{
        void UpdateState(PublishmentSystemInfo publishmentSystemInfo, int contentID, EGovInteractState state);

        void UpdateDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int contentID, int departmentID);

        void UpdateDepartmentID(PublishmentSystemInfo publishmentSystemInfo, ArrayList idCollection, int departmentID);

        GovInteractContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string queryCode);

        GovInteractContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int nodeID, NameValueCollection form);

        GovInteractContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int contentID);

        int GetNodeID(PublishmentSystemInfo publishmentSystemInfo, int contentID);

        int GetContentNum(PublishmentSystemInfo publishmentSystemInfo);

        int GetCountByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID, DateTime startDate, DateTime endDate);

        string GetSelectCommendByNodeID(PublishmentSystemInfo publishmentSystemInfo, int nodeID);

        string GetSelectCommendByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID);

        string GetSelectStringByState(PublishmentSystemInfo publishmentSystemInfo, int nodeID, params EGovInteractState[] states);

        string GetSelectSqlStringWithChecked(PublishmentSystemInfo publishmentSystemInfo, int nodeID, bool isReplyExists, bool isReply, int startNum, int totalNum, string whereString, string orderByString, NameValueCollection otherAttributes);

        string GetSelectString(PublishmentSystemInfo publishmentSystemInfo, int nodeID);

        string GetSelectString(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string state, string dateFrom, string dateTo, string keyword);

        int GetCountByNodeID(PublishmentSystemInfo publishmentSystemInfo, int nodeID);

        int GetCountByPublishmentSystemID(PublishmentSystemInfo publishmentSystemInfo);

        int GetCountByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID);

        int GetCountByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID, int nodeID, DateTime begin, DateTime end);

        int GetCountByDepartmentIDAndState(PublishmentSystemInfo publishmentSystemInfo, int departmentID, EGovInteractState state);

        int GetCountByDepartmentIDAndState(PublishmentSystemInfo publishmentSystemInfo, int departmentID, int nodeID, EGovInteractState state, DateTime begin, DateTime end);

        int GetCountByDepartmentIDAndState(PublishmentSystemInfo publishmentSystemInfo, int departmentID, EGovInteractState state, DateTime begin, DateTime end);
	}
}
