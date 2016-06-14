using SiteServer.Project.Model;
using System.Collections;

namespace SiteServer.Project.Core
{
	public interface IProjectLogDAO
	{
        void Insert(ProjectLogInfo logInfo);

        void Delete(ArrayList idArrayList);

        void DeleteAll();

        IEnumerable GetDataSourceByApplyID(int applyID);

        ArrayList GetLogInfoArrayList(int applyID);

        string GetSelectCommend();

        string GetSelectCommend(string keyword, string dateFrom, string dateTo);
	}
}
