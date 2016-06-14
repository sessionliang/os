using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
	public interface IHistoryDAO
    {
        int Insert(HistoryInfo followInfo);

        void Delete(ArrayList deleteIDArrayList);

        HistoryInfo GetHistoryInfo(int followID);

        List<HistoryInfo> GetHistoryInfoList(string userName);

        int GetCount(string userName);

        string GetSelectString(string userName);

        string GetSortFieldName();

        List<HistoryInfo> GetUserHistorysByPage(string userName, int pageIndex, int prePageNum);
    }
}
