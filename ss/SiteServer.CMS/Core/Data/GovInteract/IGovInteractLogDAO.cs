using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface IGovInteractLogDAO
	{
        void Insert(GovInteractLogInfo logInfo);

        void Delete(ArrayList idArrayList);

        void DeleteAll(int publishmentSystemID);

        IEnumerable GetDataSourceByContentID(int publishmentSystemID, int contentID);

        ArrayList GetLogInfoArrayList(int publishmentSystemID, int contentID);

        string GetSelectCommend(int publishmentSystemID);

        string GetSelectCommend(int publishmentSystemID, string keyword, string dateFrom, string dateTo);
	}
}
