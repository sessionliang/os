using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface ICollectLogDAO
	{
        void Insert(CollectLogInfo logInfo);

        void DeleteAll(int collectID);

        void Delete(List<int> logIDList);

        int GetCount(int collectID);

        bool IsCollectd(int collectID, string cookieSN, string wxOpenID);

        string GetSelectString(int collectID);

        List<int> GetVotedItemIDList(int collectID, string cookieSN);

        List<CollectLogInfo> GetCollectLogInfoList(int  publishmentSystemID, int collectID, int collectItemID);
	}
}
