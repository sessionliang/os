using System.Collections;
using System.Data;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface IGovInteractPermissionsDAO
	{
        void Insert(int publishmentSystemID, GovInteractPermissionsInfo permissionsInfo);

        void Delete(string userName, int nodeID);

        void Update(GovInteractPermissionsInfo permissionsInfo);

        GovInteractPermissionsInfo GetPermissionsInfo(string userName, int nodeID);

        SortedList GetPermissionSortedList(string userName);
	}
}
