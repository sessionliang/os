using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface ICouponDAO
	{
        int Insert(CouponInfo couponInfo);

        void Update(CouponInfo couponInfo);

        void UpdateTotalNum(int couponID, int totalNum);

        void UpdateActID(int couponID, int actID);

        void Delete(int couponID);

        void Delete(List<int> couponIDList);

        CouponInfo GetCouponInfo(int couponID);

        List<CouponInfo> GetCouponInfoList(int publishmentSystemID, int actID);

        List<CouponInfo> GetAllCouponInfoList(int publishmentSystemID);

        string GetSelectString(int publishmentSystemID);

        Dictionary<string, int> GetCouponDictionary(int actID);

        List<CouponInfo> GetCouponInfoList(int publishmentSystemID);        

	}
}
