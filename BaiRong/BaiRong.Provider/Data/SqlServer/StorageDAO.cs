using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model.Service;

namespace BaiRong.Provider.Data.SqlServer
{
    public class StorageDAO : DataProviderBase, IStorageDAO
	{
        private const string SQL_DELETE = "DELETE FROM bairong_Storage WHERE StorageID = @StorageID";

        private const string SQL_SELECT = "SELECT StorageID, StorageName, StorageUrl, StorageType, IsEnabled, Description, AddDate FROM bairong_Storage WHERE StorageID = @StorageID";

        private const string SQL_UPDATE = "UPDATE bairong_Storage SET StorageName = @StorageName, StorageUrl = @StorageUrl, StorageType = @StorageType, IsEnabled = @IsEnabled, Description = @Description WHERE StorageID = @StorageID";

        private const string SQL_SELECT_ALL = "SELECT StorageID, StorageName, StorageUrl, StorageType, IsEnabled, Description, AddDate FROM bairong_Storage ORDER BY StorageID";

        private const string SQL_SELECT_ALL_ENABLED = "SELECT StorageID, StorageName, StorageUrl, StorageType, IsEnabled, Description, AddDate FROM bairong_Storage WHERE IsEnabled = @IsEnabled ORDER BY StorageID";

        private const string SQL_SELECT_BY_NAME = "SELECT StorageID, StorageName, StorageUrl, StorageType, IsEnabled, Description, AddDate FROM bairong_Storage WHERE StorageName = @StorageName";

        private const string SQL_UPDATE_STATE = "UPDATE bairong_Storage SET IsEnabled = @IsEnabled WHERE StorageID = @StorageID";

        private const string PARM_STORAGE_ID = "@StorageID";
        private const string PARM_STORAGE_NAME = "@StorageName";
        private const string PARM_STORAGE_URL = "@StorageUrl";
        private const string PARM_STORAGE_TYPE = "@StorageType";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ADD_DATE = "@AddDate";

		public int Insert(StorageInfo storageInfo)
		{
            int storageID = 0;

            string sqlString = "INSERT INTO bairong_Storage (StorageName, StorageUrl, StorageType, IsEnabled, Description, AddDate) VALUES (@StorageName, @StorageUrl, @StorageType, @IsEnabled, @Description, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_Storage(StorageID, StorageName, StorageUrl, StorageType, IsEnabled, Description, AddDate) VALUES (bairong_Storage_SEQ.NEXTVAL, @StorageName, @StorageUrl, @StorageType, @IsEnabled, @Description, @AddDate)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_STORAGE_NAME, EDataType.NVarChar, 50, storageInfo.StorageName),
                this.GetParameter(PARM_STORAGE_URL, EDataType.VarChar, 200, storageInfo.StorageUrl),
                this.GetParameter(PARM_STORAGE_TYPE, EDataType.VarChar, 50, EStorageTypeUtils.GetValue(storageInfo.StorageType)),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, storageInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, storageInfo.Description),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, storageInfo.AddDate)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);

                        storageID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_Storage");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return storageID;
		}

		public void Delete(int storageID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, storageID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public void Update(StorageInfo storageInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_STORAGE_NAME, EDataType.NVarChar, 50, storageInfo.StorageName),
                this.GetParameter(PARM_STORAGE_URL, EDataType.VarChar, 200, storageInfo.StorageUrl),
                this.GetParameter(PARM_STORAGE_TYPE, EDataType.VarChar, 50, EStorageTypeUtils.GetValue(storageInfo.StorageType)),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, storageInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, storageInfo.Description),
                this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, storageInfo.StorageID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateState(int storageID, bool isEnabled)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, isEnabled.ToString()),
                this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, storageID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_STATE, updateParms);
        }

        public StorageInfo GetStorageInfo(int storageID)
		{
            StorageInfo storageInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, storageID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    int i = 0;
                    storageInfo = new StorageInfo(rdr.GetInt32(i++), rdr.GetValue(i++).ToString(), rdr.GetValue(i++).ToString(), EStorageTypeUtils.GetEnumType(rdr.GetValue(i++).ToString()), TranslateUtils.ToBool(rdr.GetValue(i++).ToString()), rdr.GetValue(i++).ToString(), rdr.GetDateTime(i++));
                }
                rdr.Close();
            }

            return storageInfo;
		}

		public IEnumerable GetDataSource()
		{
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL);
			return enumerable;
		}

        public bool IsExists(string storageName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STORAGE_NAME, EDataType.NVarChar, 50, storageName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_NAME, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public SortedList GetStorageInfoSortedList()
        {
            SortedList sortedlist = new SortedList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL))
            {
                while (rdr.Read())
                {
                    int i = 0;
                    StorageInfo storageInfo = new StorageInfo(rdr.GetInt32(i++), rdr.GetValue(i++).ToString(), rdr.GetValue(i++).ToString(), EStorageTypeUtils.GetEnumType(rdr.GetValue(i++).ToString()), TranslateUtils.ToBool(rdr.GetValue(i++).ToString()), rdr.GetValue(i++).ToString(), rdr.GetDateTime(i++));
                    sortedlist.Add(storageInfo.StorageID, storageInfo);
                }
                rdr.Close();
            }
            return sortedlist;
        }

        public ArrayList GetStorageInfoArrayListEnabled()
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, true.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_ENABLED, parms))
            {
                while (rdr.Read())
                {
                    int i = 0;
                    StorageInfo storageInfo = new StorageInfo(rdr.GetInt32(i++), rdr.GetValue(i++).ToString(), rdr.GetValue(i++).ToString(), EStorageTypeUtils.GetEnumType(rdr.GetValue(i++).ToString()), TranslateUtils.ToBool(rdr.GetValue(i++).ToString()), rdr.GetValue(i++).ToString(), rdr.GetDateTime(i++));
                    arraylist.Add(storageInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }
	}
}