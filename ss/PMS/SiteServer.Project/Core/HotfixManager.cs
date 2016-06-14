using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.Project.Model;

namespace SiteServer.Project.Core
{
    public class HotfixManager
    {
        public HotfixManager() { }

        public static HotfixInfo GetHotfixInfo(int hotfixID)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetHotfixInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                int theHotfixID = (int)entry.Key;
                if (theHotfixID == hotfixID)
                {
                    HotfixInfo hotfixInfo = entry.Value as HotfixInfo;
                    return hotfixInfo;
                }
            }
            return null;
        }

        public static ArrayList GetHotfixIDArrayList()
        {
            ICollection collection = GetHotfixInfoDictionaryEntryArrayList().Keys;
            ArrayList arraylist = new ArrayList();
            arraylist.AddRange(collection);
            return arraylist;
        }

        public static void ClearCache()
        {
            CacheUtils.Remove(cacheKey);
        }

        public static DictionaryEntryArrayList GetHotfixInfoDictionaryEntryArrayList()
        {
            lock (lockObject)
            {
                if (CacheUtils.Get(cacheKey) == null)
                {
                    DictionaryEntryArrayList dictionaryFormDB = DataProvider.HotfixDAO.GetHotfixInfoDictionaryEntryArrayListEnabled();
                    DictionaryEntryArrayList sl = new DictionaryEntryArrayList();
                    foreach (DictionaryEntry entry in dictionaryFormDB)
                    {
                        HotfixInfo hotfixInfo = entry.Value as HotfixInfo;
                        if (hotfixInfo != null)
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

        /****************** Cache *********************/

        private static readonly object lockObject = new object();
        private const string cacheKey = "SiteServer.Project.Core.HotfixManager";
    }
}