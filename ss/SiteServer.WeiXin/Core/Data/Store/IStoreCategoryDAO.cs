using BaiRong.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Core
{
    public interface IStoreCategoryDAO
    {
        int Insert(StoreCategoryInfo storeCategoryInfo);

        StoreCategoryInfo GetStoreCategoryInfo(int storeID);

        StoreCategoryInfo GetStoreCategoryInfoByParentID(int publishmentSystemID, int parentID);

        string GetSelectString(int publishmentSystemID);

        Dictionary<string, int> GetStoreCategoryDictionary(int publishmentSystemID);

        List<StoreCategoryInfo> GetStoreCategoryInfoList(int publishmentSystemID, int parentID);

        List<StoreCategoryInfo> GetAllStoreCategoryInfoList(int publishmentSystemID);

        string GetCategoryName(int storeID);

        int Insert(int publishmentSystemID, StoreCategoryInfo categoryInfo);

        void Update(int publishmentSystemID, StoreCategoryInfo categoryInfo);

        void UpdateTaxis(int publishmentSystemID, int selectedID, bool isSubtract);

        void Delete(int publishmentSystemID, int id);

        int GetNodeCount(int publishmentSystemID, int id);

        List<int> GetCategoryIDListByParentID(int publishmentSystemID, int parentID);

        List<int> GetAllCategoryIDList(int publishmentSystemID);

        List<int> GetCategoryIDListForLastNode(int publishmentSystemID, int categoryID);

        List<int> GetCategoryIDListByCategoryIDCollection(int publishmentSystemID, string idCollection);

        List<int> GetCategoryIDListByFirstCategoryIDArrayList(int publishmentSystemID, ArrayList firstIDArrayList);

        StoreCategoryInfo GetCategoryInfo(int categoryID);

        void UpdateStoreItemCount(int publishmentSystemID);

        List<StoreCategoryInfo> GetStoreCategoryInfoList(int publishmentSystemID);
    }
}
