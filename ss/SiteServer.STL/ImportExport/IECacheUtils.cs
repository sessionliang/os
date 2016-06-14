using System;
using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using System.Collections.Generic;

namespace SiteServer.STL.ImportExport
{
	internal class IECacheUtils
	{
        private const string CACHE_SPEC = "SiteServer.STL.ImportExport.IECacheUtils.CACHE_SPEC";
        private const string CACHE_ITEM = "SiteServer.STL.ImportExport.IECacheUtils.CACHE_ITEM";

        public static void CacheSpecIDDictionaryAndItemIDDictionary(Dictionary<int, int> specIDDictionary, Dictionary<int, int> itemIDDictionary)
        {
            DbCacheManager.Remove(IECacheUtils.CACHE_SPEC);
            DbCacheManager.Remove(IECacheUtils.CACHE_ITEM);

            DbCacheManager.Insert(IECacheUtils.CACHE_SPEC, TranslateUtils.IntDictionaryToStringCollection(specIDDictionary));
            DbCacheManager.Insert(IECacheUtils.CACHE_ITEM, TranslateUtils.IntDictionaryToStringCollection(itemIDDictionary));
        }

        public static Dictionary<int, int> GetSpecIDDictionary()
        {
            return TranslateUtils.StringCollectionToIntDictionary(DbCacheManager.Get(IECacheUtils.CACHE_SPEC));
        }

        public static Dictionary<int, int> GetItemIDDictionary()
        {
            return TranslateUtils.StringCollectionToIntDictionary(DbCacheManager.Get(IECacheUtils.CACHE_ITEM));
        }
	}
}
