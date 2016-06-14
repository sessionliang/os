using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ICouponActDAO
    {
        int Insert(CouponActInfo actInfo);

        void Update(CouponActInfo actInfo);

        void UpdateUserCount(int actID, int publishmentSystemID);

        void UpdatePVCount(int actID, int publishmentSystemID);

        void Delete(List<int> actIDList);

        CouponActInfo GetActInfo(int actID);

        List<CouponActInfo> GetActInfoListByKeywordID(int publishmentSystemID, int keywordID);

        List<int> GetActIDList(int publishmentSystemID);

        int GetKeywordID(int actID);

        string GetTitle(int actID);

        string GetSelectString(int publishmentSystemID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        List<CouponActInfo> GetCouponActInfoList(int publishmentSystemID);
    }
}
