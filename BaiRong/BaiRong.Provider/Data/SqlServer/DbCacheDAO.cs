using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;

namespace BaiRong.Provider.Data.SqlServer
{
    public class DbCacheDAO : DataProviderBase, IDbCacheDAO
    {
        private const string SQL_SELECT_CACHE_VALUE = "SELECT CacheValue FROM bairong_Cache WHERE CacheKey = @CacheKey";

        private const string SQL_SELECT_CACHE_COUNT = "SELECT COUNT(*) FROM bairong_Cache";

        private const string SQL_INSERT = "INSERT INTO bairong_Cache (CacheKey, CacheValue) VALUES (@CacheKey, @CacheValue)";

        private const string SQL_DELETE = "DELETE FROM bairong_Cache WHERE CacheKey = @CacheKey";
        private const string SQL_DELETE_ALL = "DELETE FROM bairong_Cache";

        private const string PARM_CACHE_KEY = "@CacheKey";
        private const string PARM_CACHE_VALUE = "@CacheValue";

        public void Insert(string cacheKey, string cacheValue)
        {
            this.Remove(cacheKey);

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CACHE_KEY, EDataType.VarChar, 200, cacheKey),
				this.GetParameter(PARM_CACHE_VALUE, EDataType.NText, cacheValue)
			};

            this.ExecuteNonQuery(SQL_INSERT, insertParms);
        }

        public void Remove(string cacheKey)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CACHE_KEY, EDataType.VarChar, 200, cacheKey)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public void Clear()
        {
            this.ExecuteNonQuery(SQL_DELETE_ALL);
        }

        public bool IsExists(string cacheKey)
        {
            bool retval = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CACHE_KEY, EDataType.VarChar, 200, cacheKey)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CACHE_VALUE, parms))
            {
                if (rdr.Read())
                {
                    retval = true;
                }
                rdr.Close();
            }
            return retval;
        }

        public string GetCacheValue(string cacheKey)
        {
            string retval = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CACHE_KEY, EDataType.VarChar, 200, cacheKey)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CACHE_VALUE, parms))
            {
                if (rdr.Read())
                {
                    retval = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return retval;
        }


        public int GetCount()
        {
            int count = 0;
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CACHE_COUNT))
            {
                if (rdr.Read())
                {
                    count = TranslateUtils.ToInt(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return count;
        }
    }
}
