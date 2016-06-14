using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicApplyLogDAO
	{
        void Insert(GovPublicApplyLogInfo logInfo);

        void Delete(ArrayList idArrayList);

        void DeleteAll(int publishmentSystemID);

        IEnumerable GetDataSourceByApplyID(int applyID);

        ArrayList GetLogInfoArrayList(int applyID);

        string GetSelectCommend(int publishmentSystemID);

        string GetSelectCommend(int publishmentSystemID, string keyword, string dateFrom, string dateTo);
	}
}
