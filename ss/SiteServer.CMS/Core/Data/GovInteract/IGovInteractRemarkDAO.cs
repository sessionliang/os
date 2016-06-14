using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface IGovInteractRemarkDAO
	{
        void Insert(GovInteractRemarkInfo remarkInfo);

        void Delete(int remarkID);

        GovInteractRemarkInfo GetRemarkInfo(int remarkID);

        ArrayList GetRemarkInfoArrayList(int publishmentSystemID, int contentID);

        IEnumerable GetDataSourceByContentID(int publishmentSystemID, int contentID);
	}
}
