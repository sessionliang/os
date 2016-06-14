using System.Data;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using System;

namespace BaiRong.Provider.Data.SqlServer
{
	public class ConfigDAO : DataProviderBase, IConfigDAO
	{
		// Static constants
        private const string SQL_INSERT_CONFIG = "INSERT INTO bairong_Config (IsInitialized, DatabaseVersion, RestrictionBlackList, RestrictionWhiteList, IsRelatedUrl, RootUrl, UpdateDate, SettingsXML) VALUES (@IsInitialized, @DatabaseVersion, @RestrictionBlackList, @RestrictionWhiteList, @IsRelatedUrl, @RootUrl, @UpdateDate, @SettingsXML)";

        private const string SQL_SELECT_CONFIG = "SELECT IsInitialized, DatabaseVersion, RestrictionBlackList, RestrictionWhiteList, IsRelatedUrl, RootUrl, UpdateDate, SettingsXML FROM bairong_Config";

        private const string SQL_SELECT_IS_INITIALIZED = "SELECT IsInitialized FROM bairong_Config";

		private const string SQL_SELECT_DATABASE_VERSION = "SELECT DatabaseVersion FROM bairong_Config";

        private const string SQL_UPDATE_CONFIG = "UPDATE bairong_Config SET IsInitialized = @IsInitialized, DatabaseVersion = @DatabaseVersion, RestrictionBlackList = @RestrictionBlackList, RestrictionWhiteList = @RestrictionWhiteList, IsRelatedUrl = @IsRelatedUrl, RootUrl = @RootUrl, UpdateDate = @UpdateDate, SettingsXML = @SettingsXML";

		private const string PARM_IS_INITIALIZED = "@IsInitialized";
		private const string PARM_DATABASE_VERSION = "@DatabaseVersion";
        private const string PARM_RESTRICTION_BLACK_LIST = "@RestrictionBlackList";
        private const string PARM_RESTRICTION_WHITE_LIST = "@RestrictionWhiteList";
        private const string PARM_IS_RELATED_URL = "@IsRelatedUrl";
        private const string PARM_ROOT_URL = "@RootUrl";
        private const string PARM_UPDATE_DATE = "@UpdateDate";
		private const string PARM_SETTINGS_XML = "@SettingsXML";

		public void Insert(ConfigInfo info) 
		{
			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_INITIALIZED, EDataType.VarChar, 18, info.IsInitialized.ToString()),
				this.GetParameter(PARM_DATABASE_VERSION, EDataType.VarChar, 50, info.DatabaseVersion),
                this.GetParameter(PARM_RESTRICTION_BLACK_LIST, EDataType.NVarChar, 255, TranslateUtils.ObjectCollectionToString(info.RestrictionBlackList)),
                this.GetParameter(PARM_RESTRICTION_WHITE_LIST, EDataType.NVarChar, 255, TranslateUtils.ObjectCollectionToString(info.RestrictionWhiteList)),
                this.GetParameter(PARM_IS_RELATED_URL, EDataType.VarChar, 18, info.IsRelatedUrl.ToString()),
                this.GetParameter(PARM_ROOT_URL, EDataType.VarChar, 200, info.RootUrl),
                this.GetParameter(PARM_UPDATE_DATE, EDataType.DateTime, info.UpdateDate),
				this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, info.Additional.ToString())
			};

            this.ExecuteNonQuery(SQL_INSERT_CONFIG, insertParms);
            ConfigManager.IsChanged = true;
		}

		public void Update(ConfigInfo info) 
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_INITIALIZED, EDataType.VarChar, 18, info.IsInitialized.ToString()),
				this.GetParameter(PARM_DATABASE_VERSION, EDataType.VarChar, 50, info.DatabaseVersion),
                this.GetParameter(PARM_RESTRICTION_BLACK_LIST, EDataType.NVarChar, 255, TranslateUtils.ObjectCollectionToString(info.RestrictionBlackList)),
                this.GetParameter(PARM_RESTRICTION_WHITE_LIST, EDataType.NVarChar, 255, TranslateUtils.ObjectCollectionToString(info.RestrictionWhiteList)),
                this.GetParameter(PARM_IS_RELATED_URL, EDataType.VarChar, 18, info.IsRelatedUrl.ToString()),
                this.GetParameter(PARM_ROOT_URL, EDataType.VarChar, 200, info.RootUrl),
                this.GetParameter(PARM_UPDATE_DATE, EDataType.DateTime, info.UpdateDate),
				this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, info.Additional.ToString())
			};

            this.ExecuteNonQuery(SQL_UPDATE_CONFIG, updateParms);
            ConfigManager.IsChanged = true;
		}

		public bool IsInitialized()
		{
            bool isInitialized = false;

			try
			{
                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_IS_INITIALIZED)) 
				{
					if (rdr.Read()) 
					{
                        isInitialized = TranslateUtils.ToBool(rdr.GetValue(0).ToString());
					}
					rdr.Close();
				}
			}
			catch{}

            return isInitialized;
		}

		public string GetDatabaseVersion()
		{
			string databaseVersion = string.Empty;

			try
			{
				using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_DATABASE_VERSION)) 
				{
					if (rdr.Read()) 
					{
                        databaseVersion = rdr.GetValue(0).ToString();
					}
					rdr.Close();
				}
			}
			catch{}

			return databaseVersion;
		}

		public ConfigInfo GetConfigInfo()
		{
			ConfigInfo info = new ConfigInfo();
			info.IsInitialized = false;

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CONFIG))
            {
                if (rdr.Read())
                {
                    info = new ConfigInfo(TranslateUtils.ToBool(rdr.GetValue(0).ToString()), rdr.GetValue(1).ToString(), TranslateUtils.StringCollectionToStringCollection(rdr.GetValue(2).ToString()), TranslateUtils.StringCollectionToStringCollection(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetDateTime(6), rdr.GetValue(7).ToString());
                }
                rdr.Close();
            }

			return info;
		}

        public string GetGUID(string key)
        {
            key = "guid_" + key;

            string guid = ConfigManager.Additional.GetExtendedAttribute(key);
            if (string.IsNullOrEmpty(guid))
            {
                guid = StringUtils.GetShortGUID();
                ConfigManager.Additional.SetExtendedAttribute(key, guid);
                BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

            }
            return guid;
        }

        public int GetSiteCount()
        {
            string sqlString = "SELECT COUNT(*) FROM siteserver_PublishmentSystem";
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void Initialize()
        {
            ConfigInfo configInfo = new ConfigInfo(true, ProductManager.Version, new StringCollection(), new StringCollection(),true, string.Empty, DateTime.Now, string.Empty);
            this.Insert(configInfo);

            BaiRongDataProvider.IP2CityDAO.TranslateIP2City();
        }
	}
}
