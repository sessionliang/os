using System;
using System.Data;
using System.Collections;

using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicCategoryDAO
	{
        int Insert(GovPublicCategoryInfo categoryInfo);

        void Update(GovPublicCategoryInfo categoryInfo);

        void UpdateTaxis(string classCode, int publishmentSystemID, int selectedCategoryID, bool isSubtract);

        void UpdateContentNum(PublishmentSystemInfo publishmentSystemInfo, string contentAttributeName, int categoryID);

        void Delete(int categoryID);

        GovPublicCategoryInfo GetCategoryInfo(int categoryID);

        string GetCategoryName(int categoryID);

        int GetNodeCount(int categoryID);

        bool IsExists(int categoryID);

        ArrayList GetCategoryIDArrayList(string classCode, int publishmentSystemID);

        ArrayList GetCategoryIDArrayListByParentID(string classCode, int publishmentSystemID, int parentID);

        ArrayList GetCategoryIDArrayListForDescendant(string classCode, int publishmentSystemID, int categoryID);
	}
}
