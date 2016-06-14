using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.B2C.Model;
using System.Collections;

namespace SiteServer.B2C.Core
{
    public interface IPromotionDAO
    {
        int Insert(PromotionInfo promotionInfo);

        void Update(PromotionInfo promotionInfo);

        void Delete(List<int> promotionIDList);

        void UpdateIsEnabled(List<int> promotionIDList, bool isEnabled);

        PromotionInfo GetPromotionInfo(int promotionID);

        string GetSelectString(int publishmentSystemID);

        List<PromotionInfo> GetEnabledPromotionInfoList(int publishmentSystemID);

        List<PromotionInfo> GetPromotionInfoList(int publishmentSystemID);

        List<int> GetEnabledPromotionIDList(int publishmentSystemID);
     }
}
