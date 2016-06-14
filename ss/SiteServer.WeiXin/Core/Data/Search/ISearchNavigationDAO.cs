using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ISearchNavigationDAO
	{
        int Insert(SearchNavigationInfo searchNavigationInfo);

        void Update(SearchNavigationInfo searchNavigationInfo);

        void UpdateSearchID(int publishmentSystemID, int searchID);

        void Delete(int publishmentSystemID, int searchNavigationID);

        void Delete(int publishmentSystemID, List<int> searchNavigationIDList);

        void DeleteAllNotInIDList(int publishmentSystemID, int searchID, List<int> idList);

        SearchNavigationInfo GetSearchNavigationInfo(int searchNavigationID);

        List<SearchNavigationInfo> GetSearchNavigationInfoList(int publishmentSystemID, int searchID);

        string GetSelectString(int publishmentSystemID);

        List<SearchNavigationInfo> GetSearchNavigationInfoList(int publishmentSystemID);
	}
}
