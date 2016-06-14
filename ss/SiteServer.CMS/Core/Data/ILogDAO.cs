using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface ILogDAO
	{
        void Insert(LogInfo log);

        void Delete(ArrayList idArrayList);

        void DeleteAll();

        string GetSelectCommend();

        string GetSelectCommend(int publishmentSystemID, string logType, string userName, string keyword, string dateFrom, string dateTo);
	}
}
