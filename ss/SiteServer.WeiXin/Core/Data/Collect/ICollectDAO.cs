using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface ICollectDAO
	{
        int Insert(CollectInfo collectInfo);

        void Update(CollectInfo collectInfo);

        void Delete(int publishmentSystemID, int collectID);

        void Delete(int publishmentSystemID, List<int> collectIDList);

        void AddUserCount(int collectID);

        void AddPVCount(int collectID);

        CollectInfo GetCollectInfo(int collectID);

        List<CollectInfo> GetCollectInfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int collectID);

        string GetSelectString(int publishmentSystemID);

        List<CollectInfo> GetCollectInfoList(int publishmentSystemID);
	}
}
