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
using ConferenceManager = SiteServer.WeiXin.Core.ConferenceManager;

namespace SiteServer.API.Controllers.WX
{
    public class WX_ConferenceController : ApiController
    {
        private ConferenceParameter GetConferenceParameter(ConferenceInfo conferenceInfo, string cookieSN, string wxOpenID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(conferenceInfo.PublishmentSystemID);
            string poweredBy = string.Empty;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            conferenceInfo.BackgroundImageUrl = ComponentsManager.GetBackgroundImageUrl(publishmentSystemInfo, conferenceInfo.BackgroundImageUrl);
            conferenceInfo.EndImageUrl = ConferenceManager.GetEndImageUrl(publishmentSystemInfo, conferenceInfo.EndImageUrl);

            List<ConferenceGuestInfo> guestInfoList = new List<ConferenceGuestInfo>();
            guestInfoList = TranslateUtils.JsonToObject(conferenceInfo.GuestList, guestInfoList) as List<ConferenceGuestInfo>;
            if (guestInfoList != null)
            {
                foreach (ConferenceGuestInfo guestInfo in guestInfoList)
                {
                    guestInfo.picUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, guestInfo.picUrl));
                }
            }
            conferenceInfo.GuestList = TranslateUtils.ObjectToJson(guestInfoList);

            bool isApplied = DataProviderWX.ConferenceContentDAO.IsApplied(conferenceInfo.ID, cookieSN, wxOpenID);
            bool isEnd = false;
            if (conferenceInfo.EndDate < DateTime.Now)
            {
                isEnd = true;
            }

            return new ConferenceParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, IsApplied = isApplied, ConferenceInfo = conferenceInfo, IsEnd = isEnd };
        }

        [HttpGet]
        [ActionName("GetConferenceParameter")]
        public IHttpActionResult GetConferenceParameter(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

                DataProviderWX.ConferenceDAO.AddPVCount(id);

                ConferenceInfo conferenceInfo = DataProviderWX.ConferenceDAO.GetConferenceInfo(id);

                ConferenceParameter parameter = this.GetConferenceParameter(conferenceInfo, cookieSN, wxOpenID);

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                ConferenceParameter parameter = new ConferenceParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitApplication")]
        public IHttpActionResult SubmitApplication(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");
                string realName = RequestUtils.GetQueryString("realName");
                string mobile = RequestUtils.GetQueryString("mobile");
                string email = RequestUtils.GetQueryString("email");
                string company = RequestUtils.GetQueryString("company");
                string position = RequestUtils.GetQueryString("position");
                string note = RequestUtils.GetQueryString("note");

                ConferenceInfo conferenceInfo = DataProviderWX.ConferenceDAO.GetConferenceInfo(id);

                ConferenceManager.AddContent(conferenceInfo.PublishmentSystemID, id, realName, mobile, email, company, position, note, PageUtils.GetIPAddress(), cookieSN, wxOpenID, UserManager.Current.UserName);

                ConferenceParameter parameter = new ConferenceParameter { IsSuccess = true, ErrorMessage = string.Empty };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                ConferenceParameter parameter = new ConferenceParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }
    }
}
