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
    public class FTPStorageDAO : DataProviderBase, IFTPStorageDAO
	{
        private const string SQL_INSERT = "INSERT INTO bairong_FTPStorage(StorageID, Server, Port, UserName, Password, IsPassiveMode) VALUES (@StorageID, @Server, @Port, @UserName, @Password, @IsPassiveMode)";

        private const string SQL_SELECT = "SELECT StorageID, Server, Port, UserName, Password, IsPassiveMode FROM bairong_FTPStorage WHERE StorageID = @StorageID";

        private const string SQL_UPDATE = "UPDATE bairong_FTPStorage SET Server = @Server, Port = @Port, UserName = @UserName, Password = @Password, IsPassiveMode = @IsPassiveMode WHERE StorageID = @StorageID";

        private const string PARM_STORAGE_ID = "@StorageID";
        private const string PARM_SERVER = "@Server";
        private const string PARM_PORT = "@Port";
        private const string PARM_USERNAME = "@UserName";
        private const string PARM_PASSWORD = "@Password";
        private const string PARM_IS_PASSIVE_MODE = "@IsPassiveMode";

		public void Insert(FTPStorageInfo ftpStorageInfo)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, ftpStorageInfo.StorageID),
                this.GetParameter(PARM_SERVER, EDataType.VarChar, 200, ftpStorageInfo.Server),
                this.GetParameter(PARM_PORT, EDataType.Integer, ftpStorageInfo.Port),
                this.GetParameter(PARM_USERNAME, EDataType.VarChar, 200, ftpStorageInfo.UserName),
                this.GetParameter(PARM_PASSWORD, EDataType.VarChar, 200, ftpStorageInfo.Password),
                this.GetParameter(PARM_IS_PASSIVE_MODE, EDataType.VarChar, 18, ftpStorageInfo.IsPassiveMode.ToString())
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public void Update(FTPStorageInfo ftpStorageInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_SERVER, EDataType.VarChar, 200, ftpStorageInfo.Server),
                this.GetParameter(PARM_PORT, EDataType.Integer, ftpStorageInfo.Port),
                this.GetParameter(PARM_USERNAME, EDataType.VarChar, 200, ftpStorageInfo.UserName),
                this.GetParameter(PARM_PASSWORD, EDataType.VarChar, 200, ftpStorageInfo.Password),
                this.GetParameter(PARM_IS_PASSIVE_MODE, EDataType.VarChar, 18, ftpStorageInfo.IsPassiveMode.ToString()),
                this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, ftpStorageInfo.StorageID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public FTPStorageInfo GetFTPStorageInfo(int storageID)
		{
            FTPStorageInfo ftpStorageInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_STORAGE_ID, EDataType.Integer, storageID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    ftpStorageInfo = new FTPStorageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()));
                }
                rdr.Close();
            }

            return ftpStorageInfo;
		}
	}
}