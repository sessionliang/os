using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Core
{

    public sealed class UserLevelManager
    {
        private UserLevelManager()
        {

        }
        public static UserLevelInfo GetCurrent(string levelSN)
        {
            if (BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                return UserLevelManager.GetLevelInfoByLevelType(levelSN, EUserLevelType.Guest);
            }
            return UserManager.GetLevelInfo(levelSN, BaiRongDataProvider.UserDAO.CurrentUserName);
        }

        public static UserLevelInfo GetLevelInfo(string levelSN, int levelID)
        {
            UserLevelInfo levelInfo = null;

            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserLevelInfoDictionaryEntryArrayList(levelSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                int theLevelID = (int)entry.Key;
                if (theLevelID == levelID)
                {
                    levelInfo = entry.Value as UserLevelInfo;
                    break;
                }
            }

            if (levelInfo == null)
            {
                if (levelID == 0)
                {
                    levelInfo = UserLevelManager.GetLevelInfoByLevelType(levelSN, EUserLevelType.Guest);
                }
                else
                {
                    levelInfo = UserLevelManager.GetLevelInfoByLevelType(levelSN, EUserLevelType.Credits);
                }
            }

            return levelInfo;
        }

        public static EUserLevelType GetLevelType(string levelSN, int levelID)
        {
            return UserLevelManager.GetLevelInfo(levelSN, levelID).LevelType;
        }

        public static string GetIconUrl(string levelSN, int levelID)
        {
            string iconUrl = "user_normal.gif";
            EUserLevelType levelType = UserLevelManager.GetLevelType(levelSN, levelID);
            if (levelType == EUserLevelType.Guest)
            {
                iconUrl = "user_guest.gif";
            }
            else if (levelType == EUserLevelType.Moderator)
            {
                iconUrl = "user_moderator.gif";
            }
            else if (levelType == EUserLevelType.SuperModerator)
            {
                iconUrl = "user_supermoderator.gif";
            }
            else if (levelType == EUserLevelType.Administrator)
            {
                iconUrl = "user_administrator.gif";
            }
            return iconUrl;
        }

        public static ArrayList GetLevelIDArrayList(string levelSN)
        {
            ICollection collection = GetUserLevelInfoDictionaryEntryArrayList(levelSN).Keys;
            ArrayList arraylist = new ArrayList();
            arraylist.AddRange(collection);
            return arraylist;
        }

        public static bool IsLevelIDInLevelNames(string levelSN, int levelID, ArrayList levelNameArrayList)
        {
            DictionaryEntryArrayList levelInfoDictionaryEntryArrayList = UserLevelManager.GetUserLevelInfoDictionaryEntryArrayList(levelSN);
            foreach (DictionaryEntry entry in levelInfoDictionaryEntryArrayList)
            {
                UserLevelInfo levelInfo = entry.Value as UserLevelInfo;
                if (levelNameArrayList.Contains(levelInfo.LevelName) && levelInfo.ID == levelID)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsExists(string levelSN, int levelID)
        {
            if (levelID <= 0) return false;
            return UserLevelManager.GetLevelIDArrayList(levelSN).Contains(levelID);
        }

        public static void ClearCache(string levelSN)
        {
            string cacheKey = GetCacheKey(levelSN);
            CacheUtils.Remove(cacheKey);
        }

        public static DictionaryEntryArrayList GetUserLevelInfoDictionaryEntryArrayList(string levelSN)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(levelSN);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    DictionaryEntryArrayList dictionaryFormDB = BaiRongDataProvider.UserLevelDAO.GetUserLevelInfoDictionaryEntryArrayList(levelSN);
                    if (dictionaryFormDB.Count == 0)
                    {
                        BaiRongDataProvider.UserLevelDAO.CreateDefaultUserLevel(levelSN);
                        dictionaryFormDB = BaiRongDataProvider.UserLevelDAO.GetUserLevelInfoDictionaryEntryArrayList(levelSN);
                    }
                    DictionaryEntryArrayList sl = new DictionaryEntryArrayList();
                    foreach (DictionaryEntry entry in dictionaryFormDB)
                    {
                        UserLevelInfo levelInfo = entry.Value as UserLevelInfo;
                        //配置路径
                        if (levelInfo != null)
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

        private static string GetCacheKey(string levelSN)
        {
            return string.Format("BaiRong.Core.UserLevelManager.{0}", levelSN);
        }

        public static string GetColorLevelName(string levelSN, int levelID)
        {
            return GetColorLevelName(UserLevelManager.GetLevelInfo(levelSN, levelID));
        }

        public static string GetColorLevelName(UserLevelInfo levelInfo)
        {
            string levelName = string.Empty;

            if (levelInfo != null)
            {
                levelName = levelInfo.LevelName;
                if (!string.IsNullOrEmpty(levelInfo.Color))
                {
                    levelName = string.Format("<span style='color:{0}'>{1}</span>", levelInfo.Color, levelName);
                }
            }

            return levelName;
        }

        public static string GetLevelName(string levelSN, int levelID)
        {
            string levelName = string.Empty;

            UserLevelInfo levelInfo = UserLevelManager.GetLevelInfo(levelSN, levelID);

            if (levelInfo != null)
            {
                levelName = levelInfo.LevelName;
            }

            return levelName;
        }

        public static UserLevelInfo GetLevelInfoByLevelType(string levelSN, EUserLevelType type)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserLevelInfoDictionaryEntryArrayList(levelSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                UserLevelInfo levelInfo = entry.Value as UserLevelInfo;
                if (levelInfo.LevelType == type)
                {
                    return levelInfo;
                }
            }
            return new UserLevelInfo(0, levelSN, string.Empty, type, 0, 0, 0, string.Empty, string.Empty);
        }

        public static int GetLevelIDByLevelType(string levelSN, EUserLevelType type)
        {
            UserLevelInfo levelInfo = GetLevelInfoByLevelType(levelSN, type);
            return levelInfo.ID;
        }

        public static UserLevelInfo GetLevelInfoByCredits(string levelSN, int credits)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserLevelInfoDictionaryEntryArrayList(levelSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                UserLevelInfo levelInfo = entry.Value as UserLevelInfo;
                if (levelInfo.LevelType == EUserLevelType.Credits && levelInfo.MinNum <= credits && levelInfo.MaxNum > credits)
                {
                    return levelInfo;
                }
            }
            return new UserLevelInfo(0, levelSN, string.Empty, EUserLevelType.Guest, 0, 0, 0, string.Empty, string.Empty);
        }

        public static int GetUserLevelIDByCredits(string levelSN, int credits)
        {
            return UserLevelManager.GetLevelInfoByCredits(levelSN, credits).ID;
        }

        public static string GetUserLevelNameByCredits(string levelSN, int credits)
        {
            return UserLevelManager.GetLevelInfoByCredits(levelSN, credits).LevelName;
        }

        public static ArrayList GetLevelInfoArrayList(string levelSN)
        {
            ArrayList arraylist = new ArrayList();
            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserLevelInfoDictionaryEntryArrayList(levelSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                UserLevelInfo levelInfo = entry.Value as UserLevelInfo;
                arraylist.Add(levelInfo);
            }
            return arraylist;
        }

        public static bool IsSuperModerator(string levelSN, int levelID)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetUserLevelInfoDictionaryEntryArrayList(levelSN);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                UserLevelInfo levelInfo = entry.Value as UserLevelInfo;
                if ((levelInfo.LevelType == EUserLevelType.Administrator || levelInfo.LevelType == EUserLevelType.SuperModerator) && levelID == levelInfo.ID)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
