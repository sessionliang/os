using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ISearchDAO
	{
        int Insert(SearchInfo searchInfo);

        void Update(SearchInfo searchInfo);

        void Delete(int publishmentSystemID, int searchID);

        void Delete(int publishmentSystemID, List<int> searchIDList);
  
        void AddPVCount(int searchID);

        SearchInfo GetSearchInfo(int searchID);

        List<SearchInfo> GetSearchInfoListByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int searchID);

        string GetSelectString(int publishmentSystemID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        List<SearchInfo> GetSearchInfoList(int publishmentSystemID);
	}
}
