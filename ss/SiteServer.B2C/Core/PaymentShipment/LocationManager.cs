using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
    public class LocationManager
    {
        public LocationManager() { }

        public static LocationInfo GetLocationInfo(int publishmentSystemID, int locationID)
        {
            DictionaryEntryArrayList dictionaryEntryArrayList = GetLocationInfoDictionaryEntryArrayList(publishmentSystemID);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                int theLocationID = (int)entry.Key;
                if (theLocationID == locationID)
                {
                    LocationInfo locationInfo = entry.Value as LocationInfo;
                    return locationInfo;
                }
            }
            return null;
        }

        public static List<LocationInfo> GetLocationInfoList(int publishmentSystemID, int parentID)
        {
            List<LocationInfo> list = new List<LocationInfo>();

            DictionaryEntryArrayList dictionaryEntryArrayList = GetLocationInfoDictionaryEntryArrayList(publishmentSystemID);

            foreach (DictionaryEntry entry in dictionaryEntryArrayList)
            {
                LocationInfo locationInfo = (LocationInfo)entry.Value;
                if (locationInfo.ParentID == parentID)
                {
                    list.Add(locationInfo);
                }
            }
            return list;
        }

        public static string GetThisLocationName(int publishmentSystemID, int locationID)
        {
            LocationInfo locationInfo = LocationManager.GetLocationInfo(publishmentSystemID, locationID);
            if (locationInfo != null)
            {
                return locationInfo.LocationName;
            }
            return string.Empty;
        }

        public static string GetLocationName(int publishmentSystemID, int locationID)
        {
            if (locationID > 0)
            {
                ArrayList locationNameArrayList = new ArrayList();

                string parentsPath = LocationManager.GetParentsPath(publishmentSystemID, locationID);
                ArrayList locationIDArrayList = new ArrayList();
                if (!string.IsNullOrEmpty(parentsPath))
                {
                    locationIDArrayList = TranslateUtils.StringCollectionToIntArrayList(parentsPath);
                }
                locationIDArrayList.Add(locationID);

                foreach (int theLocationID in locationIDArrayList)
                {
                    LocationInfo locationInfo = LocationManager.GetLocationInfo(publishmentSystemID, theLocationID);
                    if (locationInfo != null)
                    {
                        locationNameArrayList.Add(locationInfo.LocationName);
                    }
                }

                return TranslateUtils.ObjectCollectionToString(locationNameArrayList, " > ");
            }
            return string.Empty;
        }

        public static string GetParentsPath(int publishmentSystemID, int locationID)
        {
            string retval = string.Empty;
            LocationInfo locationInfo = LocationManager.GetLocationInfo(publishmentSystemID, locationID);
            if (locationInfo != null)
            {
                retval = locationInfo.ParentsPath;
            }
            return retval;
        }

        public static ArrayList GetLocationIDArrayList(int publishmentSystemID)
        {
            ICollection collection = GetLocationInfoDictionaryEntryArrayList(publishmentSystemID).Keys;
            ArrayList arraylist = new ArrayList();
            arraylist.AddRange(collection);
            return arraylist;
        }

        public static void ClearCache(int publishmentSystemID)
        {
            string cacheKey = string.Format(CACHE_KEY, publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        public static DictionaryEntryArrayList GetLocationInfoDictionaryEntryArrayList(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = string.Format(CACHE_KEY, publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    DictionaryEntryArrayList dictionaryFormDB = DataProviderB2C.LocationDAO.GetLocationInfoDictionaryEntryArrayList(publishmentSystemID);
                    DictionaryEntryArrayList sl = new DictionaryEntryArrayList();
                    foreach (DictionaryEntry entry in dictionaryFormDB)
                    {
                        LocationInfo locationInfo = entry.Value as LocationInfo;
                        if (locationInfo != null)
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
        private const string CACHE_KEY = "SiteServer.B2C.Core.LocationManager.{0}";
    }
}