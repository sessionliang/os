using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ICollectItemDAO
    {
        int Insert(CollectItemInfo itemInfo);

        void Update(CollectItemInfo itemInfo);

        void Delete(int publishmentSystemID, List<int> collectItemIDList);

        void DeleteAll(int publishmentSystemID, int collectID);

        CollectItemInfo GetCollectItemInfo(int itemID);

        List<CollectItemInfo> GetCollectItemInfoList(int collectID);

        void AddVoteNum(int collectID, int itemID);

        Dictionary<string, int> GetItemIDCollectionWithRank(int collectID);

        string GetSelectString(int publishmentSystemID, int collectID);

        void Audit(int publishmentSystemID, int collectItemID);
    }
}
