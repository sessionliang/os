using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;

using System.Text;

using SiteServer.BBS.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;
using BaiRong.Model;

namespace SiteServer.BBS.Core
{
    public class BBSUserManager
    {
        private BBSUserManager() { }
        const string cacheKey = "SiteServer.BBS.Core.BBSUserManager";
        private static object lockObject = new object();

        public static BBSUserInfo GetCurrentUserInfo()
        {
            return BBSUserManager.GetBBSUserInfo(BaiRongDataProvider.UserDAO.CurrentGroupSN, BaiRongDataProvider.UserDAO.CurrentUserName);
        }

        public static bool IsExists(string groupSN, string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                BBSUserInfo userExtendInfo = BBSUserManager.GetBBSUserInfo(groupSN, userName);
                if (userExtendInfo != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static BBSUserInfo GetBBSUserInfo(string groupSN, string userName)
        {
            return GetBBSUserInfo(groupSN, userName, false);
        }

        public static BBSUserInfo GetBBSUserInfo(string groupSN, string userName, bool flush)
        {
            Dictionary<string, BBSUserInfo> dictionary = GetActiveUsers();
            string userKey = UserManager.GetUserKey(groupSN, userName);

            BBSUserInfo userExtendInfo = null;

            if (!flush)
            {
                if (dictionary.ContainsKey(userKey))
                {
                    userExtendInfo = dictionary[userKey];
                }
            }

            if (userExtendInfo == null)
            {
                userExtendInfo = DataProvider.BBSUserDAO.GetBBSUserInfo(groupSN, userName);

                if (userExtendInfo == null)
                {
                    if (!string.IsNullOrEmpty(userName) && userName!="游客")
                    {
                        try
                        {
                            int userID = BaiRongDataProvider.UserDAO.GetUserID(groupSN, userName);
                            userExtendInfo = new BBSUserInfo(userID, groupSN, userName);
                            DataProvider.BBSUserDAO.Insert(userExtendInfo);
                        }
                        catch { }
                    }
                }

                if (userExtendInfo != null)
                {
                    UpdateCache(dictionary, userExtendInfo, groupSN, userName);
                }

                if (userExtendInfo == null)
                {
                    int userID = BaiRongDataProvider.UserDAO.GetUserID(groupSN, userName);
                    userExtendInfo = new BBSUserInfo(userID, groupSN, userName);
                }
            }

            return userExtendInfo;
        }

        public static int GetPostCount(string groupSN, string userName)
        {
            BBSUserInfo userInfo = BBSUserManager.GetBBSUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                return userInfo.PostCount;
            }
            return 0;
        }

        public static int GetPrestige(string groupSN, string userName)
        {
            BBSUserInfo userInfo = BBSUserManager.GetBBSUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                return userInfo.Prestige;
            }
            return 0;
        }

        public static int GetContribution(string groupSN, string userName)
        {
            BBSUserInfo userInfo = BBSUserManager.GetBBSUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                return userInfo.Contribution;
            }
            return 0;
        }

        public static int GetCurrency(string groupSN, string userName)
        {
            BBSUserInfo userInfo = BBSUserManager.GetBBSUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                return userInfo.Currency;
            }
            return 0;
        }

        private static void UpdateCache(Dictionary<string, BBSUserInfo> dictionary, BBSUserInfo userExtendInfo, string groupSN, string userName)
        {
            lock (lockObject)
            {
                string userKey = UserManager.GetUserKey(groupSN, userName);
                if (dictionary.ContainsKey(userKey))
                {
                    dictionary[userKey] = userExtendInfo;
                }
                else
                {
                    dictionary.Add(userKey, userExtendInfo);
                }
            }
        }

        public static void RemoveCache(string groupSN, string userName)
        {
            Dictionary<string, BBSUserInfo> dictionary = GetActiveUsers();
            string userKey = UserManager.GetUserKey(groupSN, userName);

            lock (lockObject)
            {
                dictionary.Remove(userKey);
            }
        }

        public static void Clear()
        {
            CacheUtils.Remove(cacheKey);
        }

        public static Dictionary<string, BBSUserInfo> GetActiveUsers()
        {
            Dictionary<string, BBSUserInfo> dictionary = CacheUtils.Get(cacheKey) as Dictionary<string, BBSUserInfo>;
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, BBSUserInfo>();
                CacheUtils.Insert(cacheKey, dictionary, null, CacheUtils.HourFactor * 12);
            }
            return dictionary;
        }

        public static void AddCredits(int publishmentSystemID, string userName, ECreditRule ruleType)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                CreditRuleInfo ruleInfo = CreditRuleManager.GetCreditRuleInfo(publishmentSystemID, ruleType, 0);
                CreditRuleLogInfo logInfo = DataProvider.CreditRuleLogDAO.GetCreditRuleLogInfo(publishmentSystemID, userName, ruleType);
                if (logInfo == null)
                {
                    logInfo = new CreditRuleLogInfo(0, publishmentSystemID, userName, ruleType, 0, 0, 0, 0, 0, 0, 0, 0, DateTime.Now);
                }

                bool isAddCredit = false;
                if (logInfo.ID == 0 || ruleInfo.PeriodType == EPeriodType.None)
                {
                    isAddCredit = true;
                }
                else
                {
                    if (ruleInfo.PeriodType == EPeriodType.Everyday)
                    {
                        if (logInfo.LastDate.AddDays(1) < DateTime.Now)//一天外
                        {
                            logInfo.PeriodCount = 1;
                            isAddCredit = true;
                        }
                        else
                        {
                            if (ruleInfo.MaxNum == 0 || logInfo.PeriodCount <= ruleInfo.MaxNum)
                            {
                                logInfo.PeriodCount += 1;
                                isAddCredit = true;
                            }
                        }
                    }
                    else if (ruleInfo.PeriodType == EPeriodType.Hour)
                    {
                        if (logInfo.LastDate.AddHours(ruleInfo.PeriodCount) < DateTime.Now)
                        {
                            logInfo.PeriodCount = 1;
                            isAddCredit = true;
                        }
                        else
                        {
                            if (ruleInfo.MaxNum == 0 || logInfo.PeriodCount <= ruleInfo.MaxNum)
                            {
                                logInfo.PeriodCount += 1;
                                isAddCredit = true;
                            }
                        }
                    }
                    else if (ruleInfo.PeriodType == EPeriodType.Minute)
                    {
                        if (logInfo.LastDate.AddMinutes(ruleInfo.PeriodCount) < DateTime.Now)
                        {
                            logInfo.PeriodCount = 1;
                            isAddCredit = true;
                        }
                        else
                        {
                            if (ruleInfo.MaxNum == 0 || logInfo.PeriodCount <= ruleInfo.MaxNum)
                            {
                                logInfo.PeriodCount += 1;
                                isAddCredit = true;
                            }
                        }
                    }
                }

                if (isAddCredit)
                {
                    ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);
                    logInfo.TotalCount += 1;
                    logInfo.Prestige += ruleInfo.Prestige;
                    logInfo.Contribution += ruleInfo.Contribution;
                    logInfo.Currency += ruleInfo.Currency;
                    int extCredit1 = 0;
                    if (additional.CreditUsingExtCredit1)
                    {
                        extCredit1 = ruleInfo.ExtCredit1;
                        logInfo.ExtCredit1 += extCredit1;
                    }
                    int extCredit2 = 0;
                    if (additional.CreditUsingExtCredit2)
                    {
                        extCredit2 = ruleInfo.ExtCredit2;
                        logInfo.ExtCredit2 += extCredit2;
                    }
                    int extCredit3 = 0;
                    if (additional.CreditUsingExtCredit3)
                    {
                        extCredit3 = ruleInfo.ExtCredit3;
                        logInfo.ExtCredit3 += extCredit3;
                    }
                    logInfo.LastDate = DateTime.Now;
                    if (logInfo.ID > 0)
                    {
                        DataProvider.CreditRuleLogDAO.Update(logInfo);
                    }
                    else
                    {
                        DataProvider.CreditRuleLogDAO.Insert(logInfo);
                    }

                    int credits = ruleInfo.Prestige * additional.CreditMultiplierPrestige + ruleInfo.Contribution * additional.CreditMultiplierContribution + ruleInfo.Currency * additional.CreditMultiplierCurrency + extCredit1 * additional.CreditMultiplierExtCredit1 + extCredit2 * additional.CreditMultiplierExtCredit2 + extCredit3 * additional.CreditMultiplierExtCredit3;

                    string groupSN = PublishmentSystemManager.GetGroupSN(publishmentSystemID);

                    DataProvider.BBSUserDAO.UpdateCredit(groupSN, userName, credits, ruleInfo.Prestige, ruleInfo.Contribution, ruleInfo.Currency, extCredit1, extCredit2, extCredit3);
                }
            }
        }

        public static bool IsModerator(ForumInfo forumInfo)
        {
            if (!BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                string groupSN = PublishmentSystemManager.GetGroupSN(forumInfo.PublishmentSystemID);

                if (UserGroupManager.IsSuperModerator(groupSN, UserManager.GetGroupID(groupSN, BaiRongDataProvider.UserDAO.CurrentUserName)))
                {
                    return true;
                }
                else
                {
                    return StringUtils.Contains(forumInfo.Additional.Moderators, BaiRongDataProvider.UserDAO.CurrentUserName);
                }
            }
            return false;
        }
    }
}
