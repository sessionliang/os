using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Generic;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public sealed class UserGroupManager
	{
        private UserGroupManager()
		{
			
		}

        public static UserGroupInfo GetCurrent(string groupSN)
        {
            if (BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                return UserGroupManager.GetGroupInfoByGroupType(groupSN, EUserGroupType.Guest);
            }
            return UserManager.GetGroupInfo(groupSN, BaiRongDataProvider.UserDAO.CurrentUserName);
        }

        public static UserGroupInfo GetGroupInfo(string groupSN, int groupID)
		{
            UserGroupInfo groupInfo = null;

            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserGroupInfoDictionaryEntryArrayList(groupSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                int theGroupID = (int)entry.Key;
                if (theGroupID == groupID)
                {
                    groupInfo = entry.Value as UserGroupInfo;
                    break;
                }
            }

            if (groupInfo == null)
            {
                if (groupID == 0)
                {
                    groupInfo = UserGroupManager.GetGroupInfoByGroupType(groupSN, EUserGroupType.Guest);
                }
                else
                {
                    groupInfo = UserGroupManager.GetGroupInfoByGroupType(groupSN, EUserGroupType.Credits);
                }
            }

            return groupInfo;
		}

        public static EUserGroupType GetGroupType(string groupSN, int groupID)
        {
            return UserGroupManager.GetGroupInfo(groupSN, groupID).GroupType;
        }

        public static string GetIconUrl(string groupSN, int groupID)
        {
            string iconUrl = "user_normal.gif";
            EUserGroupType groupType = UserGroupManager.GetGroupType(groupSN, groupID);
            if (groupType == EUserGroupType.Guest)
            {
                iconUrl = "user_guest.gif";
            }
            else if (groupType == EUserGroupType.Moderator)
            {
                iconUrl = "user_moderator.gif";
            }
            else if (groupType == EUserGroupType.SuperModerator)
            {
                iconUrl = "user_supermoderator.gif";
            }
            else if (groupType == EUserGroupType.Administrator)
            {
                iconUrl = "user_administrator.gif";
            }
            return iconUrl;
        }

        public static ArrayList GetGroupIDArrayList(string groupSN)
		{
            ICollection collection = GetUserGroupInfoDictionaryEntryArrayList(groupSN).Keys;
            ArrayList arraylist = new ArrayList();
            arraylist.AddRange(collection);
            return arraylist;
		}

        public static bool IsGroupIDInGroupNames(string groupSN, int groupID, ArrayList groupNameArrayList)
        {
            DictionaryEntryArrayList groupInfoDictionaryEntryArrayList = UserGroupManager.GetUserGroupInfoDictionaryEntryArrayList(groupSN);
            foreach (DictionaryEntry entry in groupInfoDictionaryEntryArrayList)
            {
                UserGroupInfo groupInfo = entry.Value as UserGroupInfo;
                if (groupNameArrayList.Contains(groupInfo.GroupName) && groupInfo.GroupID == groupID)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsExists(string groupSN, int groupID)
        {
            if (groupID <= 0) return false;
            return UserGroupManager.GetGroupIDArrayList(groupSN).Contains(groupID);
        }

        public static void ClearCache(string groupSN)
		{
            string cacheKey = GetCacheKey(groupSN);
            CacheUtils.Remove(cacheKey);
		}

        public static DictionaryEntryArrayList GetUserGroupInfoDictionaryEntryArrayList(string groupSN)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(groupSN);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    DictionaryEntryArrayList dictionaryFormDB = BaiRongDataProvider.UserGroupDAO.GetUserGroupInfoDictionaryEntryArrayList(groupSN);
                    if (dictionaryFormDB.Count == 0)
                    {
                        BaiRongDataProvider.UserGroupDAO.CreateDefaultUserGroup(groupSN);
                        dictionaryFormDB = BaiRongDataProvider.UserGroupDAO.GetUserGroupInfoDictionaryEntryArrayList(groupSN);
                    }
                    DictionaryEntryArrayList sl = new DictionaryEntryArrayList();
                    foreach (DictionaryEntry entry in dictionaryFormDB)
                    {
                        UserGroupInfo groupInfo = entry.Value as UserGroupInfo;
                        //ÅäÖÃÂ·¾¶
                        if (groupInfo != null)
                        {
                            sl.Add(entry);
                        }
                    }
                    CacheUtils.Max(cacheKey, sl);
                    return sl;
                }
                return CacheUtils.Get(cacheKey) as DictionaryEntryArrayList;
            }
        }

        private static readonly object lockObject = new object();

        private static string GetCacheKey(string groupSN)
        {
            return string.Format("BaiRong.Core.UserGroupManager.{0}", groupSN);
        }

        //----------------//

        public static string GetColorGroupName(string groupSN, int groupID)
        {
            return GetColorGroupName(UserGroupManager.GetGroupInfo(groupSN, groupID));
        }

        public static string GetColorGroupName(UserGroupInfo groupInfo)
        {
            string groupName = string.Empty;

            if (groupInfo != null)
            {
                groupName = groupInfo.GroupName;
                if (!string.IsNullOrEmpty(groupInfo.Color))
                {
                    groupName = string.Format("<span style='color:{0}'>{1}</span>", groupInfo.Color, groupName);
                }
            }

            return groupName;
        }

        public static string GetGroupName(string groupSN, int groupID)
        {
            string groupName = string.Empty;

            UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(groupSN, groupID);

            if (groupInfo != null)
            {
                groupName = groupInfo.GroupName;
            }

            return groupName;
        }

        public static UserGroupInfo GetGroupInfoByGroupType(string groupSN, EUserGroupType type)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserGroupInfoDictionaryEntryArrayList(groupSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                UserGroupInfo groupInfo = entry.Value as UserGroupInfo;
                if (groupInfo.GroupType == type)
                {
                    return groupInfo;
                }
            }
            return new UserGroupInfo(0, groupSN, string.Empty, type, 0, 0, 0, string.Empty, string.Empty);
        }

        public static int GetGroupIDByGroupType(string groupSN, EUserGroupType type)
        {
            UserGroupInfo groupInfo = GetGroupInfoByGroupType(groupSN, type);
            return groupInfo.GroupID; 
        }

        public static UserGroupInfo GetGroupInfoByCredits(string groupSN, int credits)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserGroupInfoDictionaryEntryArrayList(groupSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                UserGroupInfo groupInfo = entry.Value as UserGroupInfo;
                if (groupInfo.GroupType == EUserGroupType.Credits && groupInfo.CreditsFrom <= credits && groupInfo.CreditsTo > credits)
                {
                    return groupInfo;
                }
            }
            return new UserGroupInfo(0, groupSN, string.Empty, EUserGroupType.Guest, 0, 0, 0, string.Empty, string.Empty);
        }

        public static int GetUserGroupIDByCredits(string groupSN, int credits)
        {
            return UserGroupManager.GetGroupInfoByCredits(groupSN, credits).GroupID;
        }

        public static string GetUserGroupNameByCredits(string groupSN, int credits)
        {
            return UserGroupManager.GetGroupInfoByCredits(groupSN, credits).GroupName;
        }

        public static ArrayList GetGroupInfoArrayList(string groupSN)
        {
            ArrayList arraylist = new ArrayList();
            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserGroupInfoDictionaryEntryArrayList(groupSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                UserGroupInfo groupInfo = entry.Value as UserGroupInfo;
                arraylist.Add(groupInfo);
            }
            return arraylist;
        }

        public static bool IsSuperModerator(string groupSN, int groupID)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserGroupInfoDictionaryEntryArrayList(groupSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                UserGroupInfo groupInfo = entry.Value as UserGroupInfo;
                if ((groupInfo.GroupType == EUserGroupType.Administrator || groupInfo.GroupType == EUserGroupType.SuperModerator) && groupID == groupInfo.GroupID)
                {
                    return true;
                }
            }
            return false;
        }
	}
}
