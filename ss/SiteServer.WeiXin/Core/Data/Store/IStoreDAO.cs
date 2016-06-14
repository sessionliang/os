using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Core
{
    public interface IStoreDAO
    {
        int Insert(StoreInfo storeInfo);

        void Update(StoreInfo storeInfo);

        void Delete(int publishmentSystemID, int storeID);

        void Delete(int publishmentSystemID, List<int> storeIDList);         

        void AddPVCount(int storeID);

        StoreInfo GetStoreInfo(int storeID);

        List<StoreInfo> GetStoreInfoListByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int storeID);

        string GetSelectString(int publishmentSystemID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        List<StoreInfo> GetStoreInfoList(int publishmentSystemID);
    }
}
