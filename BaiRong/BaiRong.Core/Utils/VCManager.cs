using System.Web.UI;
using BaiRong.Core;
using System;

namespace BaiRong.Core
{
    public class VCManager
    {
        protected const string AdminLoginCookieName = "BAIRONG.VC.ADMINLOGIN";
        protected const string LoginCookieName = "BAIRONG.VC.LOGIN";
        protected const string ValidateCodeCookieName = "BAIRONG.VC.ValidateCode";

        public const string AttributeName = "validateCode";

        protected string cookieName;

        protected bool isCorssDomain;

        public static VCManager GetInstanceOfAdminLogin()
        {
            VCManager vc = new VCManager();
            vc.cookieName = VCManager.AdminLoginCookieName;
            return vc;
        }

        public static VCManager GetInstanceOfLogin()
        {
            VCManager vc = new VCManager();
            vc.cookieName = VCManager.LoginCookieName;
            return vc;
        }

        public static VCManager GetInstanceOfValidateCode()
        {
            VCManager vc = new VCManager();
            vc.cookieName = VCManager.ValidateCodeCookieName;
            return vc;
        }

        protected VCManager() { }

        public string GetCookieName()
        {
            return this.cookieName;
        }

        public static string CreateValidateCode(bool isBigImage)
        {
            string validateCode = "";

            char[] s = null;
            if (isBigImage)
            {
                //s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'I', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };//枚举数组
                s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };//枚举数组
            }
            else
            {
                s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };//枚举数组
            }
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                validateCode += s[r.Next(0, s.Length)].ToString();
            }

            return validateCode;
        }

        public static string CreateValidateCode(bool isBigImage, int num)
        {
            string validateCode = "";
            if (num == 0)
                return CreateValidateCode(isBigImage);

            char[] s = null;
            if (isBigImage)
            {
                //s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'I', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };//枚举数组
                s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };//枚举数组
            }
            else
            {
                s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };//枚举数组
            }
            Random r = new Random();
            for (int i = 0; i < num; i++)
            {
                validateCode += s[r.Next(0, s.Length)].ToString();
            }

            return validateCode;
        }

        public virtual string GetImageUrl(bool isBigImage)
        {
            string pageName = (isBigImage) ? "validateCode2.aspx" : "validateCode.aspx";
            //return PageUtils.GetAdminDirectoryUrl(string.Format("{0}?cookieName={1}&isCrossDomain={2}", pageName, this.cookieName, this.isCorssDomain));
            return PageUtils.Services.GetUrl(string.Format("{0}?cookieName={1}&isCrossDomain={2}", pageName, this.cookieName, this.isCorssDomain));
        }

        /// <summary>
        /// cors跨域，通过api显示验证码
        /// </summary>
        /// <param name="isBigImage"></param>
        /// <returns></returns>
        public virtual string GetImageUrl(bool isBigImage, bool isCorsCross, string proco)
        {
            if (!isCorsCross)
            {
                return GetImageUrl(isBigImage);
            }
            else
            {
                return PageUtils.API.GetValidateCodeUrl(isBigImage, this.cookieName, isCorsCross, proco);
            }
        }

        public bool IsCodeValid(string validateCode)
        {
            string code = string.Empty;
            if (this.isCorssDomain)
            {
                code = DbCacheManager.Get(this.cookieName);
            }
            else
            {
                code = CookieUtils.GetCookie(this.cookieName);
            }
            bool isValid = StringUtils.EqualsIgnoreCase(code, validateCode);

            if (isValid)
            {
                if (isCorssDomain)
                {
                    DbCacheManager.Remove(this.cookieName);
                }
                else
                {
                    CacheUtils.Remove(this.cookieName);
                }
            }

            return isValid;
        }
    }
}
