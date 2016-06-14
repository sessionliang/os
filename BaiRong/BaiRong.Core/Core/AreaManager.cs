using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public class AreaManager
    {
        public AreaManager() { }

        public static AreaInfo GetAreaInfo(int areaID)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetAreaInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                int theAreaID = (int)entry.Key;
                if (theAreaID == areaID)
                {
                    AreaInfo areaInfo = entry.Value as AreaInfo;
                    return areaInfo;
                }
            }
            return null;
        }

        public static string GetThisAreaName(int areaID)
        {
            AreaInfo areaInfo = AreaManager.GetAreaInfo(areaID);
            if (areaInfo != null)
            {
                return areaInfo.AreaName;
            }
            return string.Empty;
        }

        public static string GetAreaName(int areaID)
        {
            if (areaID > 0)
            {
                ArrayList areaNameArrayList = new ArrayList();

                string parentsPath = AreaManager.GetParentsPath(areaID);
                ArrayList areaIDArrayList = new ArrayList();
                if (!string.IsNullOrEmpty(parentsPath))
                {
                    areaIDArrayList = TranslateUtils.StringCollectionToIntArrayList(parentsPath);
                }
                areaIDArrayList.Add(areaID);

                foreach (int theAreaID in areaIDArrayList)
                {
                    AreaInfo areaInfo = AreaManager.GetAreaInfo(theAreaID);
                    if (areaInfo != null)
                    {
                        areaNameArrayList.Add(areaInfo.AreaName);
                    }
                }

                return TranslateUtils.ObjectCollectionToString(areaNameArrayList, " > ");
            }
            return string.Empty;
        }

        public static string GetParentsPath(int areaID)
        {
            string retval = string.Empty;
            AreaInfo areaInfo = AreaManager.GetAreaInfo(areaID);
            if (areaInfo != null)
            {
                retval = areaInfo.ParentsPath;
            }
            return retval;
        }

        public static ArrayList GetAreaIDArrayList()
        {
            ICollection collection = GetAreaInfoDictionaryEntryArrayList().Keys;
            ArrayList arraylist = new ArrayList();
            arraylist.AddRange(collection);
            return arraylist;
        }

        public static void ClearCache()
        {
            CacheUtils.Remove(cacheKey);
        }

        public static DictionaryEntryArrayList GetAreaInfoDictionaryEntryArrayList()
        {
            lock (lockObject)
            {
                if (CacheUtils.Get(cacheKey) == null)
                {
                    DictionaryEntryArrayList dictionaryFormDB = BaiRongDataProvider.AreaDAO.GetAreaInfoDictionaryEntryArrayList();
                    DictionaryEntryArrayList sl = new DictionaryEntryArrayList();
                    foreach (DictionaryEntry entry in dictionaryFormDB)
                    {
                        AreaInfo areaInfo = entry.Value as AreaInfo;
                        if (areaInfo != null)
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
        private const string cacheKey = "BaiRong.Core.AreaManager";
    }
}