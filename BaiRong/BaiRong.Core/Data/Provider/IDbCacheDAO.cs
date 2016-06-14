using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IDbCacheDAO
	{
		void Insert(string cacheKey, string cacheValue);

		void Remove(string cacheKey);

        void Clear();
		
        bool IsExists(string cacheKey);
		
        string GetCacheValue(string cacheKey);

        int GetCount();
    }
}
