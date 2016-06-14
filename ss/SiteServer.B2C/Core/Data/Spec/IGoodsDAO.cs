using System.Collections;
using System.Collections.Generic;
using SiteServer.B2C.Model;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Core
{
    public interface IGoodsDAO
    {
        void Insert(GoodsInfo goodsInfo);

        void Update(GoodsInfo goodsInfo);

        void Delete(int publishmentSystemID, int contentID, string itemIDCollection);

        void DeleteNotInList(int publishmentSystemID, int contentID, ArrayList goodsIDArrayList);

        List<GoodsInfo> GetGoodsInfoList(int publishmentSystemID, int contentID);

        GoodsInfo GetGoodsInfo(int goodsID);

        GoodsInfo GetGoodsInfoForDefault(int goodsID, GoodsContentInfo contentInfo);

        Hashtable GetItemIDCollectionHashtable(int publishmentSystemID, int contentID);

        int GetGoodsID(int publishmentSystemID, int contentID, string itemIDCollection);

        decimal GetPriceSale(int goodsID);
    }
}
