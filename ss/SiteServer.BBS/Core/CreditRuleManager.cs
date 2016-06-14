using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Core
{
    public class CreditRuleManager
    {
        private static readonly object lockObject = new object();
        private static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.CreditRuleManager.{0}", publishmentSystemID);
        }

        public static void RemoveCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        public static Hashtable GetCreditRuleInfoHashtable(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Hashtable hashtable = DataProvider.CreditRuleDAO.GetCreditRuleInfoHashtable(publishmentSystemID);
                    CacheUtils.Max(cacheKey, hashtable);
                    return hashtable;
                }
                return CacheUtils.Get(cacheKey) as Hashtable;
            }
        }

        public static CreditRuleInfo GetCreditRuleInfo(int publishmentSystemID, ECreditRule ruleType, int forumID)
        {
            CreditRuleInfo ruleInfo = null;
            Hashtable hashtable = CreditRuleManager.GetCreditRuleInfoHashtable(publishmentSystemID);
            if (hashtable != null)
            {
                if (forumID > 0)
                {
                    ruleInfo = hashtable[ECreditRuleUtils.GetValue(ruleType) + forumID] as CreditRuleInfo;
                }
                if (ruleInfo == null)
                {
                    ruleInfo = hashtable[ECreditRuleUtils.GetValue(ruleType)] as CreditRuleInfo;
                }
            }
            if (ruleInfo == null)
            {
                ruleInfo = ECreditRuleUtils.GetCreditRuleInfo(publishmentSystemID, ruleType);
            }
            return ruleInfo;
        }

        public static string GetCreditCalculate(int publishmentSystemID)
        {
            StringBuilder builder = new StringBuilder();

            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            if (additional.CreditMultiplierPostCount > 0)
            {
                builder.AppendFormat("发帖数(PostCount) × {0} + ", additional.CreditMultiplierPostCount);
            }
            if (additional.CreditMultiplierPostDigestCount > 0)
            {
                builder.AppendFormat("精华帖数(PostDigestCount) × {0} + ", additional.CreditMultiplierPostDigestCount);
            }
            if (additional.CreditMultiplierPrestige > 0)
            {
                builder.AppendFormat("{0}(Prestige) × {1} + ", additional.CreditNamePrestige, additional.CreditMultiplierPrestige);
            }
            if (additional.CreditMultiplierContribution > 0)
            {
                builder.AppendFormat("{0}(Contribution) × {1} + ", additional.CreditNameContribution, additional.CreditMultiplierContribution);
            }
            if (additional.CreditMultiplierCurrency > 0)
            {
                builder.AppendFormat("{0}(Currency) × {1} + ", additional.CreditNameCurrency, additional.CreditMultiplierCurrency);
            }
            if (additional.CreditMultiplierExtCredit1 > 0)
            {
                builder.AppendFormat("{0}(ExtCredit1) × {1} + ", additional.CreditNameExtCredit1, additional.CreditMultiplierExtCredit1);
            }
            if (additional.CreditMultiplierExtCredit2 > 0)
            {
                builder.AppendFormat("{0}(ExtCredit2) × {1} + ", additional.CreditNameExtCredit2, additional.CreditMultiplierExtCredit2);
            }
            if (additional.CreditMultiplierExtCredit3 > 0)
            {
                builder.AppendFormat("{0}(ExtCredit3) × {1} + ", additional.CreditNameExtCredit3, additional.CreditMultiplierExtCredit3);
            }
            if (builder.Length > 0)
            {
                builder.Length -= 2;
            }
            return builder.ToString();
        }
    }
}
