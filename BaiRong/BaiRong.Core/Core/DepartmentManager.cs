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
    public class DepartmentManager
    {
        public DepartmentManager() { }

        public static DepartmentInfo GetDepartmentInfo(int departmentID)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetDepartmentInfoDictionaryEntryArrayList();

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                int theDepartmentID = (int)entry.Key;
                if (theDepartmentID == departmentID)
                {
                    DepartmentInfo departmentInfo = entry.Value as DepartmentInfo;
                    return departmentInfo;
                }
            }
            return null;
        }

        public static string GetThisDepartmentName(int departmentID)
        {
            DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
            if (departmentInfo != null)
            {
                return departmentInfo.DepartmentName;
            }
            return string.Empty;
        }

        public static string GetDepartmentName(int departmentID)
        {
            if (departmentID > 0)
            {
                ArrayList departmentNameArrayList = new ArrayList();

                string parentsPath = DepartmentManager.GetParentsPath(departmentID);
                ArrayList departmentIDArrayList = new ArrayList();
                if (!string.IsNullOrEmpty(parentsPath))
                {
                    departmentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(parentsPath);
                }
                departmentIDArrayList.Add(departmentID);

                foreach (int theDepartmentID in departmentIDArrayList)
                {
                    DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(theDepartmentID);
                    if (departmentInfo != null)
                    {
                        departmentNameArrayList.Add(departmentInfo.DepartmentName);
                    }
                }

                return TranslateUtils.ObjectCollectionToString(departmentNameArrayList, " > ");
            }
            return string.Empty;
        }

        public static string GetDepartmentCode(int departmentID)
        {
            if (departmentID > 0)
            {
                DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
                if (departmentInfo != null)
                {
                    return departmentInfo.Code;
                }
            }
            return string.Empty;
        }

        public static string GetParentsPath(int departmentID)
        {
            string retval = string.Empty;
            DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
            if (departmentInfo != null)
            {
                retval = departmentInfo.ParentsPath;
            }
            return retval;
        }

        public static ArrayList GetDepartmentIDArrayList()
        {
            ICollection collection = GetDepartmentInfoDictionaryEntryArrayList().Keys;
            ArrayList arraylist = new ArrayList();
            arraylist.AddRange(collection);
            return arraylist;
        }

        public static void ClearCache()
        {
            CacheUtils.Remove(cacheKey);
        }

        public static DictionaryEntryArrayList GetDepartmentInfoDictionaryEntryArrayList()
        {
            lock (lockObject)
            {
                if (CacheUtils.Get(cacheKey) == null)
                {
                    DictionaryEntryArrayList dictionaryFormDB = BaiRongDataProvider.DepartmentDAO.GetDepartmentInfoDictionaryEntryArrayList();
                    DictionaryEntryArrayList sl = new DictionaryEntryArrayList();
                    foreach (DictionaryEntry entry in dictionaryFormDB)
                    {
                        DepartmentInfo departmentInfo = entry.Value as DepartmentInfo;
                        if (departmentInfo != null)
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
        private const string cacheKey = "BaiRong.Core.DepartmentManager";
    }
}