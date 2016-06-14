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
    public class LocalStorageDAO : DataProviderBase, ILocalStorageDAO
	{
        private const string SQL_INSERT = "INSERT INTO bairong_LocalStorage(StorageID, DirectoryPath) VALUES (@StorageID, @DirectoryPath)";

        private const string SQL_SELECT = "SELECT StorageID, DirectoryPath FROM bairong_LocalStorage WHERE StorageID = @StorageID";

        private const string SQL_SELECT_DIRECTORY_PATH = "SELECT DirectoryPath FROM bairong_LocalStorage WHERE StorageID = @StorageID";

        private const string SQL_UPDATE = "UPDATE bairong_LocalStorage SET DirectoryPath = @DirectoryPath WHERE StorageID = @StorageID";

        private const string PARM_STORAGE_ID = "@StorageID";
        private const string PARM_DIRECTORY_PATH = "@DirectoryPath";

		public void Insert(LocalStorageInfo localStorageInfo)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, localStorageInfo.StorageID),
                this.GetParameter(PARM_DIRECTORY_PATH, EDataType.NVarChar, 255, localStorageInfo.DirectoryPath)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public void Update(LocalStorageInfo localStorageInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_DIRECTORY_PATH, EDataType.NVarChar, 255, localStorageInfo.DirectoryPath),
                this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, localStorageInfo.StorageID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public LocalStorageInfo GetLocalStorageInfo(int storageID)
		{
            LocalStorageInfo localStorageInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, storageID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    localStorageInfo = new LocalStorageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString());
                }
                rdr.Close();
            }

            return localStorageInfo;
		}

        public string GetDirectoryPath(int storageID)
        {
            string directoryPath = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, storageID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_DIRECTORY_PATH, parms))
            {
                if (rdr.Read())
                {
                    directoryPath = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return directoryPath;
        }
	}
}