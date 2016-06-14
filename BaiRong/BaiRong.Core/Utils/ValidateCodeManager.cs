using System.Web.UI;
using System;

namespace BaiRong.Core
{
    public class ValidateCodeManager : BaiRong.Core.VCManager
	{
        public override string GetImageUrl(bool isBigImage)
        {
            string pageName = (isBigImage) ? "validateCode2.aspx" : "validateCode.aspx";
            return PageUtils.Services.GetUrl(string.Format("{0}?cookieName={1}&isCrossDomain={2}&_r={3}", pageName, this.cookieName, this.isCorssDomain, StringUtils.GetRandomInt(1, 1000)));
        }

        public const int CODE_SEND_MAIL = -99;
        public const int CODE_MAIL_SUBSCRIBE = -98;

        public static ValidateCodeManager GetInstance(int publishmentSystemID, int styleID, bool isCrossDomain)
        {
            ValidateCodeManager vc = new ValidateCodeManager();
            vc.cookieName = ValidateCodeManager.GetCookieName(publishmentSystemID, styleID);
            vc.isCorssDomain = isCrossDomain;
            return vc;
        }

        public static ValidateCodeManager GetInstanceOfVote(bool isCrossDomain)
        {
            ValidateCodeManager vc = new ValidateCodeManager();
            vc.cookieName = ValidateCodeManager.VoteCookieName;
            vc.isCorssDomain = isCrossDomain;
            return vc;
        }

        public static ValidateCodeManager GetInstanceOfPageAdminLogin(bool isCrossDomain)
        {
            ValidateCodeManager vc = new ValidateCodeManager();
            vc.cookieName = VCManager.AdminLoginCookieName;
            vc.isCorssDomain = isCrossDomain;
            return vc;
        }

        public static ValidateCodeManager GetInstanceOfPageLogin(bool isCrossDomain)
        {
            ValidateCodeManager vc = new ValidateCodeManager();
            vc.cookieName = VCManager.LoginCookieName;
            vc.isCorssDomain = isCrossDomain;
            return vc;
        }

        private static string GetCookieName(int publishmentSystemID, int styleID)
        {
            return string.Format("SITESERVER.p{0}.s{1}", publishmentSystemID, styleID);
        }

        private const string VoteCookieName = "SITESERVER.VC.Vote";
	}
}
