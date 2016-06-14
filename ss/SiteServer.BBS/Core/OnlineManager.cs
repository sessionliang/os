using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;

using System.Web;

namespace SiteServer.BBS.Core
{
    public class OnlineManager
    {
        public static void ClearCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        public static bool IsOnlineByUserName(int publishmentSystemID, string userName)
        {
            Hashtable hashtable = GetOnlineInfoHashtable(publishmentSystemID);
            OnlineInfo onlineInfo = hashtable[userName] as OnlineInfo;
            if (onlineInfo != null && !onlineInfo.IsHide)
            {
                return true;
            }
            return false;
        }

        private static readonly object lockObject = new object();

        private static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.OnlineManager.{0}", publishmentSystemID);
        }

        public static ArrayList GetUserNameArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            Hashtable hashtable = GetOnlineInfoHashtable(publishmentSystemID);
            foreach (OnlineInfo onlineInfo in hashtable.Values)
            {
                if (!string.IsNullOrEmpty(onlineInfo.UserName) && !onlineInfo.IsHide)
                {
                    arraylist.Add(onlineInfo.UserName);
                }
            }
            return arraylist;
        }

        public static int GetOnlineCount(int publishmentSystemID)
        {
            Hashtable hashtable = GetOnlineInfoHashtable(publishmentSystemID);
            return hashtable.Count;
        }

        private static Hashtable GetOnlineInfoHashtable(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Hashtable hashtable = DataProvider.OnlineDAO.GetOnlineInfoHashtable(publishmentSystemID);
                    CacheUtils.Insert(cacheKey, hashtable, CacheUtils.HourFactor);
                    return hashtable;
                }
                return CacheUtils.Get(cacheKey) as Hashtable;
            }
        }

        public static OnlineInfo GetOnlineInfo(int publishmentSystemID)
        {
            OnlineInfo onlineInfo = null;
            Hashtable hashtable = GetOnlineInfoHashtable(publishmentSystemID);

            //非匿名用户
            if (!BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                onlineInfo = hashtable[BaiRongDataProvider.UserDAO.CurrentUserName] as OnlineInfo;
            }
            else
            {
                onlineInfo = hashtable[PageUtils.SessionID] as OnlineInfo;
            }

            return onlineInfo;
        }

        /// <summary>
        /// 用户在线信息维护。判断当前用户的身份(会员还是游客),是否在在线列表中存在,如果存在则更新会员的当前动,不存在则建立.
        /// </summary>
        public static void UpdateOnlineUser(int publishmentSystemID)
        {
            OnlineInfo onlineInfo = OnlineManager.GetOnlineInfo(publishmentSystemID);

            if (onlineInfo == null)
            {
                onlineInfo = new OnlineInfo(0, publishmentSystemID, BaiRongDataProvider.UserDAO.CurrentUserName, PageUtils.SessionID, DateTime.Now, PageUtils.GetIPAddress(), false);
                DataProvider.OnlineDAO.Insert(publishmentSystemID, onlineInfo);
            }
            else
            {
                DataProvider.OnlineDAO.ActiveTime(onlineInfo);
            }
        }

        public static void AddHideOnlineUser(int publishmentSystemID, string userName)
        {
            OnlineInfo onlineInfo = new OnlineInfo(0, publishmentSystemID, userName, PageUtils.SessionID, DateTime.Now, PageUtils.GetIPAddress(), true);
            DataProvider.OnlineDAO.Insert(publishmentSystemID, onlineInfo);
        }

        public static void InsertCache(int publishmentSystemID, OnlineInfo onlineInfo)
        {
            Hashtable hashtable = GetOnlineInfoHashtable(publishmentSystemID);
            if (!string.IsNullOrEmpty(onlineInfo.UserName))
            {
                hashtable[onlineInfo.UserName] = onlineInfo;
            }
            else
            {
                hashtable[onlineInfo.SessionID] = onlineInfo;
            }
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
            CacheUtils.Insert(cacheKey, hashtable, CacheUtils.HourFactor);
        }

        public static void DeleteCache(int publishmentSystemID, string userName, string sessionID)
        {
            Hashtable hashtable = GetOnlineInfoHashtable(publishmentSystemID);
            if (!string.IsNullOrEmpty(userName))
            {
                hashtable.Remove(userName);
            }
            else if (!string.IsNullOrEmpty(sessionID))
            {
                hashtable.Remove(sessionID);
            }
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
            CacheUtils.Insert(cacheKey, hashtable, CacheUtils.HourFactor);
        }
    }
}
