using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.WeiXin.Core
{
    public interface ICouponSNDAO
    {
        int Insert(CouponSNInfo couponSNInfo);
        void Insert(int publishmentSystemID, int couponID, int totalNum);

        void Insert(int publishmentSystemID, int couponID, List<string> snList);

        void Update(CouponSNInfo couponSNInfo);

        void UpdateStatus(ECouponStatus status, List<int> snIDList);

        void Delete(int snID);

        void Delete(List<int> snIDList);

        CouponSNInfo GetSNInfo(int snID);

        List<CouponSNInfo> GetSNInfoByCookieSN(int publishmentSystemID, string cookieSN, string uniqueID);

        int Hold(int publishmentSystemID, int actID, string cookieSN);

        int GetTotalNum(int publishmentSystemID, int couponID);

        int GetHoldNum(int publishmentSystemID, int couponID);

        int GetCashNum(int publishmentSystemID, int couponID);

        string GetSelectString(int publishmentSystemID, int couponID);

        List<CouponSNInfo> GetCouponSNInfoList(int publishmentSystemID,int couponID);
    }
}
