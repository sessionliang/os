using System;
using System.Collections;
using System.Data;
using BaiRong.Model;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicContentDAO
	{
        GovPublicContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int contentID);

        int GetContentNum(PublishmentSystemInfo publishmentSystemInfo);

        int GetCountByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID, DateTime startDate, DateTime endDate);

        string GetSelectCommendByNodeID(PublishmentSystemInfo publishmentSystemInfo, int nodeID);

        string GetSelectCommendByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID);

        string GetSelectCommendByCategoryID(PublishmentSystemInfo publishmentSystemInfo, string classCode, int categoryID);

        void CreateIdentifier(PublishmentSystemInfo publishmentSystemInfo, int nodeID, bool isAll);
	}
}
