using BaiRong.Core;
using BaiRong.Model;
using Senparc.Weixin.MP;
using SiteServer.API.Model.WX;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SiteServer.API.Controllers.WX
{
    public class WX_CouponController : ApiController
    {
        [HttpGet]
        [ActionName("GetCouponParameter")]
        public IHttpActionResult GetCouponParameter(int id)
        {         

            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            string poweredBy = string.Empty;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            CouponParameter parameter = new CouponParameter { IsSuccess = false, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

            CouponSNInfo snInfo = DataProviderWX.CouponSNDAO.GetSNInfo(id);
            if (snInfo != null)
            {
                CouponInfo couponInfo = DataProviderWX.CouponDAO.GetCouponInfo(snInfo.CouponID);
                if (couponInfo != null)
                {
                    CouponActInfo actInfo = DataProviderWX.CouponActDAO.GetActInfo(couponInfo.ActID);

                    parameter.IsSuccess = true;
                    parameter.ImageUrl = CouponManager.GetContentImageUrl(publishmentSystemInfo, actInfo.ContentImageUrl);
                    parameter.Title = couponInfo.Title;
                    parameter.SN = snInfo.SN;
                    parameter.Status = snInfo.Status;
                    parameter.HoldDate = DateUtils.GetDateAndTimeString(snInfo.HoldDate, EDateFormatType.Chinese, ETimeFormatType.ShortTime);
                    parameter.CouponList = new List<Coupon>();
                    parameter.ConponActInfo = actInfo;

                    foreach (var val in DataProviderWX.CouponDAO.GetCouponDictionary(actInfo.ID))
                    {
                        Coupon coupon = new Coupon { Title = val.Key, TotalNum = val.Value };
                        parameter.CouponList.Add(coupon);
                    }
                    parameter.AwardCode = actInfo.AwardCode;
                    parameter.Description = actInfo.ContentDescription;
                    parameter.Usage = actInfo.ContentUsage;
                }
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("GetCouponParameterInfo")]
        public IHttpActionResult GetCouponParameterInfo(int id)
        {
            string uniqueID = RequestUtils.GetQueryString("wxOpenID");

            WeiXinManager.GetCookieWXOpenID(uniqueID);  

            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            string poweredBy = string.Empty;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            CouponParameter parameter = new CouponParameter { IsSuccess = false, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

            // Mr.wu begin
            DataProviderWX.CouponActDAO.UpdatePVCount(id, publishmentSystemInfo.PublishmentSystemID);

            CouponActInfo actInfo = DataProviderWX.CouponActDAO.GetActInfo(id);

            if (actInfo != null)
            {
                string cookieSN = WeiXinManager.GetCookieSN();

                List<CouponSNInfo> couponSnList = DataProviderWX.CouponSNDAO.GetSNInfoByCookieSN(publishmentSystemInfo.PublishmentSystemID, cookieSN, uniqueID);

                if (couponSnList.Count > 0)
                {
                    for (int i = 0; i < couponSnList.Count; i++)
                    {
                        CouponInfo couponInfo = DataProviderWX.CouponDAO.GetCouponInfo(couponSnList[i].CouponID);
                        if (couponInfo != null)
                        {
                            if (couponInfo.ActID == id)
                            {
                                parameter.CouponSNInfo = couponSnList[i];
                                break;
                            }
                        }
                    }
                }
                // Mr.wu end
                parameter.IsSuccess = true;
                parameter.ImageUrl = CouponManager.GetContentImageUrl(publishmentSystemInfo, actInfo.ContentImageUrl);
                parameter.Title = actInfo.Title;
                parameter.ConponActInfo = actInfo;
                parameter.Description = actInfo.ContentDescription;
                parameter.Usage = actInfo.ContentUsage;
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("SubmitApplication")]
        public IHttpActionResult SubmitApplication(int id)
        {
            try
            {
                CouponActInfo actInfo = DataProviderWX.CouponActDAO.GetActInfo(id);

                string publishmentSystemID = actInfo.PublishmentSystemID.ToString();

                List<CouponInfo> couponList = DataProviderWX.CouponDAO.GetCouponInfoList(Convert.ToInt32(publishmentSystemID), id);

                int TotalNum = 0;
                int HoldNum = 0;
                int CashNum = 0;

                for (int i = 0; i < couponList.Count; i++)
                {
                    TotalNum += couponList[i].TotalNum;
                    HoldNum += DataProviderWX.CouponSNDAO.GetHoldNum(Convert.ToInt32(publishmentSystemID), couponList[i].ID);
                    CashNum += DataProviderWX.CouponSNDAO.GetCashNum(Convert.ToInt32(publishmentSystemID), couponList[i].ID);
                }
                string uniqueID = WeiXinManager.GetCookieWXOpenID("");
                string realName = RequestUtils.GetQueryString("realName");
                string mobile = RequestUtils.GetQueryString("mobile");
                string email = RequestUtils.GetQueryString("email");
                string address = RequestUtils.GetQueryString("address");

                DataProviderWX.CouponActDAO.UpdateUserCount(id, Convert.ToInt32(publishmentSystemID));

                int snID = CouponManager.AddApplication(Convert.ToInt32(publishmentSystemID), id, uniqueID, realName, mobile, email, address);

                if (TotalNum == HoldNum + CashNum && snID == 0)
                {
                    CouponParameter errorParameter = new CouponParameter { IsSuccess = false, ErrorMessage = "非常抱歉 , 该优惠券已经被领完啦 ! 欢迎继续关注我们下次活动.", SnID = 0 };
                    return Ok(errorParameter);
                }

                CouponParameter parameter = new CouponParameter { IsSuccess = true, SnID = snID };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                CouponParameter parameter = new CouponParameter { IsSuccess = false, SnID = 0 };

                return Ok(parameter);
            }
        }

        // Mr.wu Begin

        [HttpGet]
        [ActionName("SubmitAwardCode")]
        public IHttpActionResult SubmitAwardCode(int id)
        {
            try
            {

                bool isSuccess = false;
                string errorMessage = "对不起，您输入的兑奖密码不正确，请重新输入！";

                if (id > 0)
                {
                    DataProviderWX.CouponSNDAO.UpdateStatus(ECouponStatus.Cash, TranslateUtils.ToIntList(id));
                    isSuccess = true;
                    errorMessage = string.Empty;
                }

                CouponParameter parameter = new CouponParameter { IsSuccess = isSuccess, ErrorMessage = errorMessage };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                CouponParameter parameter = new CouponParameter { IsSuccess = false, IsPoweredBy = true, PoweredBy = string.Empty, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        // Mr.wu End
    }
}
