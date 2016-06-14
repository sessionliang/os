using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using SiteServer.BBS.Core;
using BaiRong.Model;
using BaiRong.Core;


namespace SiteServer.BBS.Provider.SqlServer
{
    public class ConfigurationDAO : DataProviderBase, IConfigurationDAO
    {
        private const string SQL_INSERT = "INSERT INTO bbs_Configuration (PublishmentSystemID, SettingsXML) VALUES (@PublishmentSystemID, @SettingsXML)";

        private const string SQL_SELECT_ALL = "SELECT PublishmentSystemID, SettingsXML FROM bbs_Configuration WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_UPDATE = "UPDATE bbs_Configuration SET SettingsXML = @SettingsXML WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

        public void Update(ConfigurationInfo info)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, info.Additional.ToString()),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
            ConfigurationManager.IsChanged = true;
        }

        public ConfigurationInfo GetConfigurationInfo(int publishmentSystemID)
        {
            ConfigurationInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                if (rdr.Read())
                {
                    info = new ConfigurationInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString());
                }
                rdr.Close();
            }

            if (info == null)
            {
                DataProvider.FaceDAO.CreateDefaultFace(publishmentSystemID);
                DataProvider.IdentifyDAO.CreateDefaultIdentify(publishmentSystemID);
                DataProvider.KeywordsCategoryDAO.CreateDefaultKeywordsCategory(publishmentSystemID);
                DataProvider.NavigationDAO.CreateDefaultNavigation(publishmentSystemID);
                BaiRongDataProvider.UserGroupDAO.CreateDefaultUserGroup(SiteServer.CMS.Core.PublishmentSystemManager.GetGroupSN(publishmentSystemID));

                info = new ConfigurationInfo();
                IDbDataParameter[] insertParms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				    this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, info.Additional.ToString())
			    };

                this.ExecuteNonQuery(SQL_INSERT, insertParms);

                ConfigurationManager.IsChanged = true;
            }

            return info;
        }

    }
}