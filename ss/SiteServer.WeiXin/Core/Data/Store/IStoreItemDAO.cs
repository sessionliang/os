using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Core
{
    public interface IStoreItemDAO
    {
        int Insert(StoreItemInfo storeItemInfo);
        int Insert(int publishmentSystemID, StoreItemInfo storeItemInfo);

        void Update(int publishmentSystemID, StoreItemInfo storeItemInfo);

        void Delete(int publishmentSystemID, int storeItemID);

        void Delete(int publishmentSystemID, List<int> storeItemIDList);

        StoreItemInfo GetStoreItemInfo(int storeItemID);

        StoreItemInfo GetStoreItemInfoByParentID(int publishmentSystemID, int parentID);

        string GetSelectString(int storeID);
        
        List<StoreItemInfo> GetStoreItemInfoListByCategoryID(int publishmentSystemID, int categoryID);

        List<StoreItemInfo> GetAllStoreItemInfoList(int publishmentSystemID);

        void DeleteAll(int publishmentSystemID, int storeID);

        List<StoreItemInfo> GetAllStoreItemInfoListByLocation(int publishmentSystemID, string location_X);

        int GetCount(int publishmentSystemID, int categoryID);

        int GetAllCount(int publishmentSystemID, int categoryID);

        List<StoreItemInfo> GetStoreItemInfoList(int publishmentSystemID, int storeID);
    }
}
