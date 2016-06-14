using System;
using System.Collections;
using System.Text;
using System.Data;
using BaiRong.Model;
using System.Configuration;

namespace BaiRong.Core
{
    public class LevelRuleManager
    {
        private static readonly object lockObject = new object();
        private static string GetCacheKey()
        {
            return string.Format("BaiRong.Core.LevelRuleManager");
        }

        public static void RemoveCache()
        {
            string cacheKey = GetCacheKey();
            CacheUtils.Remove(cacheKey);
        }

        public static Hashtable GetLevelRuleInfoHashtable()
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey();
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Hashtable hashtable = BaiRongDataProvider.LevelRuleDAO.GetLevelRuleInfoHashtable();
                    CacheUtils.Max(cacheKey, hashtable);
                    return hashtable;
                }
                return CacheUtils.Get(cacheKey) as Hashtable;
            }
        }

        public static LevelRuleInfo GetLevelRuleInfo(ELevelRule ruleType)
        {
            LevelRuleInfo ruleInfo = null;
            Hashtable hashtable = LevelRuleManager.GetLevelRuleInfoHashtable();
            if (hashtable != null)
            {                 
                if (ruleInfo == null)
                {
                    ruleInfo = hashtable[ELevelRuleUtils.GetValue(ruleType)] as LevelRuleInfo;
                }
            }
            if (ruleInfo == null)
            {
                ruleInfo = ELevelRuleUtils.GetLevelRuleInfo(ruleType);
            }
            return ruleInfo;
        }

        public static string GetLevelCalculate()
        {
            StringBuilder builder = new StringBuilder();

            UserConfigInfoExtend additional = UserConfigManager.Additional;

            //if (additional.CreditMultiplierPostCount > 0)
            //{
            //    builder.AppendFormat("发帖数(PostCount) × {0} + ", additional.CreditMultiplierPostCount);
            //}
            //if (additional.CreditMultiplierPostDigestCount > 0)
            //{
            //    builder.AppendFormat("精华帖数(PostDigestCount) × {0} + ", additional.CreditMultiplierPostDigestCount);
            //}
            if (additional.CreditMultiplier > 0)
            {
                builder.AppendFormat("{0}(CreditNum) × {1} + ", additional.CreditNumName, additional.CreditMultiplier);
            }
            if (additional.CashMultiplier > 0)
            {
                builder.AppendFormat("{0}(CashNum) × {1} + ", additional.CashNumName, additional.CashMultiplier);
            }
            if (builder.Length > 0)
            {
                builder.Length -= 2;
            }
            return builder.ToString();
        }
    }
}
