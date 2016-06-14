using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core
{
    public sealed class ForumManager
    {
        private ForumManager()
        {
        }

        private static readonly object lockObject = new object();

        public static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.ForumManager.{0}", publishmentSystemID);
        }

        public static Hashtable GetForumInfoHashtable(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = ForumManager.GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    Hashtable hashtable = DataProvider.ForumDAO.GetForumInfoHashtable(publishmentSystemID);
                    CacheUtils.Max(cacheKey, hashtable);
                    return hashtable;
                }
                return CacheUtils.Get(cacheKey) as Hashtable;
            }
        }

        public static ForumInfo GetForumInfo(int publishmentSystemID, int forumID)
        {
            ForumInfo forumInfo = null;
            Hashtable hashtable = ForumManager.GetForumInfoHashtable(publishmentSystemID);
            if (hashtable != null)
            {
                forumInfo = hashtable[forumID] as ForumInfo;
            }
            return forumInfo;
        }

        public static DateTime GetAddDate(int publishmentSystemID, int forumID)
        {
            DateTime retval = DateTime.MinValue;
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                retval = forumInfo.AddDate;
            }
            return retval;
        }

        public static int GetParentID(int publishmentSystemID, int forumID)
        {
            int retval = 0;
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                retval = forumInfo.ParentID;
            }
            return retval;
        }

        public static int GetAreaID(int publishmentSystemID, int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                if (forumInfo.ParentID == 0)
                {
                    return forumInfo.ForumID;
                }
                else
                {
                    return GetAreaID(publishmentSystemID, forumInfo.ParentID);
                }
            }
            else
            {
                return forumID;
            }
        }

        public static string GetParentsPath(int publishmentSystemID, int forumID)
        {
            string retval = string.Empty;
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                retval = forumInfo.ParentsPath;
            }
            return retval;
        }

        public static int GetTopLevel(int publishmentSystemID, int forumID)
        {
            string parentsPath = ForumManager.GetParentsPath(publishmentSystemID, forumID);
            if (string.IsNullOrEmpty(parentsPath))
            {
                return 0;
            }

            return parentsPath.Split(',').Length;
        }

        public static string GetForumName(int publishmentSystemID, int forumID)
        {
            string retval = string.Empty;
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                retval = forumInfo.ForumName;
            }
            return retval;
        }

        public static int ThreadCount(int publishmentSystemID, int forumID)
        {
            int retval = 0;
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                retval = forumInfo.ThreadCount;
            }
            return retval;
        }

        public static string GetForumNameNavigation(int publishmentSystemID, int forumID)
        {
            ArrayList forumNameArrayList = new ArrayList();

            string parentsPath = ForumManager.GetParentsPath(publishmentSystemID, forumID);
            ArrayList forumIDArrayList = new ArrayList();
            if (!string.IsNullOrEmpty(parentsPath))
            {
                forumIDArrayList = TranslateUtils.StringCollectionToIntArrayList(parentsPath);
            }
            forumIDArrayList.Add(forumID);

            foreach (int theForumID in forumIDArrayList)
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, theForumID);
                if (forumInfo != null)
                {
                    forumNameArrayList.Add(forumInfo.ForumName);
                }
            }

            return TranslateUtils.ObjectCollectionToString(forumNameArrayList, " > ");
        }

        public static void RemoveCache(int publishmentSystemID)
        {
            string cacheKey = ForumManager.GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        public static void UpdateForumCache(int publishmentSystemID, ForumInfo forumInfo)
        {
            Hashtable hashtable = ForumManager.GetForumInfoHashtable(publishmentSystemID);
            if (hashtable != null)
            {
                hashtable[forumInfo.ForumID] = forumInfo;
            }
        }

        public static string GetSelectText(ForumInfo forumInfo, bool[] isLastNodeArray, bool isShowContentNum)
        {
            string retval = string.Empty;
            
            if (forumInfo.IsLastNode == false)
            {
                isLastNodeArray[forumInfo.ParentsCount] = false;
            }
            else
            {
                isLastNodeArray[forumInfo.ParentsCount] = true;
            }
            for (int i = 1; i < forumInfo.ParentsCount; i++)
            {
                if (isLastNodeArray[i])
                {
                    retval = String.Concat(retval, "¡¡");
                }
                else
                {
                    retval = String.Concat(retval, "©¦");
                }
            }
            if (forumInfo.IsLastNode == true)
            {
                retval = String.Concat(retval, "©¸");
            }
            else
            {
                retval = String.Concat(retval, "©À");
            }
            retval = String.Concat(retval, forumInfo.ForumName);

            if (isShowContentNum)
            {
                retval = String.Concat(retval, " (", forumInfo.ThreadCount, ")");
            }
            return retval;
        }

        public static void AddListItems(int publishmentSystemID, ListItemCollection listItemCollection, bool isShowContentNum)
        {
            ArrayList arraylist = DataProvider.ForumDAO.GetForumIDArrayList(publishmentSystemID);
            int count = arraylist.Count;
            bool[] isLastNodeArray = new bool[count + 1];
            foreach (int forumID in arraylist)
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);

                ListItem listitem = new ListItem(ForumManager.GetSelectText(forumInfo, isLastNodeArray, isShowContentNum), forumID.ToString());
                if (!string.IsNullOrEmpty(forumInfo.LinkUrl))
                {
                    listitem.Attributes.Add("style", "color:gray;");
                }
                listItemCollection.Add(listitem);
            }
        }
    }

}
