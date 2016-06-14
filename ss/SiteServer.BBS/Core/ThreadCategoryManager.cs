using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.BBS.Model;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.BBS.Core {

    public sealed class ThreadCategoryManager
    {

        private ThreadCategoryManager() {
        }

        private static readonly object lockObject = new object();

        private static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.ThreadCategoryManager.{0}", publishmentSystemID);
        }

        public static ArrayList GetThreadCategoryInfoPairArrayList(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    ArrayList arraylist = DataProvider.ThreadCategoryDAO.GetThreadCategoryInfoPairArrayList(publishmentSystemID);
                    CacheUtils.Max(cacheKey, arraylist);
                    return arraylist;
                }
                return CacheUtils.Get(cacheKey) as ArrayList;
            }
        }

        public static ThreadCategoryInfo GetThreadCategoryInfo(int publishmentSystemID, int categoryID)
        {

            ThreadCategoryInfo info = null;
            ArrayList arraylist = GetThreadCategoryInfoPairArrayList(publishmentSystemID);
            if (arraylist != null)
            {
                foreach (KeyValuePair<int, ThreadCategoryInfo> kvp in arraylist)
                {
                    if (kvp.Key == categoryID)
                    {
                        info = kvp.Value;
                    }
                }
            }
            return info;
        }

        public static ArrayList GetThreadCategoryInfoArrayList(int publishmentSystemID, int forumID)
        {
            ArrayList retval = new ArrayList();

            ArrayList arraylist = GetThreadCategoryInfoPairArrayList(publishmentSystemID);
            if (arraylist != null)
            {
                foreach (KeyValuePair<int, ThreadCategoryInfo> kvp in arraylist)
                {
                    if (kvp.Value.ForumID == forumID)
                    {
                        retval.Add(kvp.Value);
                    }
                }
            }
            return retval;
        }

        public static string GetCategoryName(int publishmentSystemID, int categoryID)
        {
            string retval = string.Empty;
            ThreadCategoryInfo info = GetThreadCategoryInfo(publishmentSystemID, categoryID);
            if (info != null)
            {
                retval = info.CategoryName; 
            }
            return retval;
        }

        public static void RemoveCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        public static void UpdateTaxis(int publishmentSystemID, int categoryID, bool isAdd)
        {
            UpdateTaxis(publishmentSystemID, categoryID, isAdd, 0);
        }

        public static void UpdateTaxis(int publishmentSystemID, int categoryID, bool isAdd, int num)
        {
            ThreadCategoryInfo info = GetThreadCategoryInfo(publishmentSystemID, categoryID);
            if (info != null)
            {
                if (isAdd)
                {
                    DataProvider.ThreadCategoryDAO.TaxisAdd(publishmentSystemID, categoryID, info.ForumID, num);
                }
                else
                {
                    DataProvider.ThreadCategoryDAO.TaxisSubtract(publishmentSystemID, categoryID, info.ForumID, num);
                }
            }
        }

        public static string GetCategorySelectHtml(int publishmentSystemID, int forumID, int categoryID)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList categoryInfoArrayList = ThreadCategoryManager.GetThreadCategoryInfoArrayList(publishmentSystemID, forumID);
            if (categoryInfoArrayList.Count > 0)
            {
                builder.Append(@"<select id=""categoryID"" name=""categoryID"">");
                builder.Append(@"<option value=""0"">选择分类</option>");
                foreach (ThreadCategoryInfo categoryInfo in categoryInfoArrayList)
                {
                    string select = (categoryID == categoryInfo.CategoryID) ? @" selected=""selected""" : string.Empty;
                    builder.AppendFormat(@"<option value=""{0}""{1}>{2}</option>", categoryInfo.CategoryID, select, categoryInfo.CategoryName);
                }
                builder.Append(@"</select><br />");
            }

            return builder.ToString();
        }

        public static bool IsCategoryExists(int publishmentSystemID, int forumID)
        {
            ArrayList categoryInfoArrayList = ThreadCategoryManager.GetThreadCategoryInfoArrayList(publishmentSystemID, forumID);
            return categoryInfoArrayList.Count > 0;
        }
    }
}
