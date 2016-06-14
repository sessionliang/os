using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using SiteServer.WeiXin.Model;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.WeiXin.Core
{
    public class WeiXinManager
    {
        private const string COOKIE_SN_NAME = "CookieSN";

        private const string COOKIE_WXOPENID_NAME = "CookieWXOpenID";

        public static string GetCookieSN()
        {
            if (CookieUtils.IsExists(WeiXinManager.COOKIE_SN_NAME))
            {
                return CookieUtils.GetCookie(WeiXinManager.COOKIE_SN_NAME);
            }
            else
            {
                string value = StringUtils.GetShortGUID();
                CookieUtils.SetCookie(WeiXinManager.COOKIE_SN_NAME, value, DateTime.MaxValue);
                return value;
            }
        }

        public static string GetCookieWXOpenID(string wxOpenID)
        {
            if (CookieUtils.IsExists(WeiXinManager.COOKIE_WXOPENID_NAME))
            {
                return CookieUtils.GetCookie(WeiXinManager.COOKIE_WXOPENID_NAME);
            }
            else
            {
                CookieUtils.SetCookie(WeiXinManager.COOKIE_WXOPENID_NAME, wxOpenID, DateTime.MaxValue);
                return wxOpenID;
            }
        }

        public static AccountInfo GetAccountInfo(int publishmentSystemID)
        {
            return DataProviderWX.AccountDAO.GetAccountInfo(publishmentSystemID);
        }
        public static bool IsBinding(AccountInfo accountInfo)
        {
            bool isBinding = false;

            EWXAccountType accountType = EWXAccountTypeUtils.GetEnumType(accountInfo.AccountType);
            if (accountType == EWXAccountType.Subscribe)
            {
                isBinding = accountInfo.IsBinding;
            }
            else
            {
                isBinding = accountInfo.IsBinding && !string.IsNullOrEmpty(accountInfo.AppID) && !string.IsNullOrEmpty(accountInfo.AppSecret);
            }

            return isBinding;
        }

        public static int Lottery(Dictionary<int, decimal> idWithProbabilityDictionary)
        {
            int id = 0;

            decimal totalProbability = 0;
            foreach (var item in idWithProbabilityDictionary)
            {
                totalProbability += item.Value;
            }
            if (totalProbability < 100)
            {
                idWithProbabilityDictionary.Add(0, 100 - totalProbability);
            }

            decimal startProbability = 0;
            Dictionary<int, KeyValuePair<decimal, decimal>> idWithAreaDictionary = new Dictionary<int, KeyValuePair<decimal, decimal>>();
            foreach (var item in idWithProbabilityDictionary)
            {
                decimal start = startProbability;
                decimal end = start + item.Value;

                idWithAreaDictionary.Add(item.Key, new KeyValuePair<decimal, decimal>(start, end));

                startProbability = end + 1;
            }

            Random random = new Random();
            int r = random.Next(1, 10000);

            foreach (var item in idWithAreaDictionary)
            {
                KeyValuePair<decimal, decimal> area = item.Value;
                if (r >= area.Key * 100 && r <= area.Value * 100)
                {
                    id = item.Key;
                    break;
                }
            }

            return id;
        }

        public static bool IsPoweredBy(PublishmentSystemInfo publishmentSystemInfo, out string poweredBy)
        {
            poweredBy = string.Empty;
            bool isPoweredBy = false;
            
            if (publishmentSystemInfo.Additional.WX_IsPoweredBy)
            {
                isPoweredBy = true;
                poweredBy = publishmentSystemInfo.Additional.WX_PoweredBy;
            }
            else
            {
                if (FileConfigManager.Instance.OEMConfig.IsOEM)
                {
                    isPoweredBy = true;
                }
            }

            return isPoweredBy;
        }
	}
}
