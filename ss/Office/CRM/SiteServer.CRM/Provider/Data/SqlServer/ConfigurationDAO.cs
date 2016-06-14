using System.Data;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;

namespace SiteServer.CRM.Provider.Data.SqlServer
{
	public class ConfigurationDAO : DataProviderBase, IConfigurationDAO
	{
		// Static constants
        private const string SQL_INSERT_CONFIGURATION = "INSERT INTO pms_Configuration (SettingsXML) VALUES (@SettingsXML)";

        private const string SQL_SELECT_CONFIGURATION = "SELECT SettingsXML FROM pms_Configuration";

        private const string SQL_UPDATE_CONFIGURATION = "UPDATE pms_Configuration SET SettingsXML = @SettingsXML";

		private const string PARM_SETTINGS_XML = "@SettingsXML";

		public void Update(ConfigurationInfo info) 
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, info.Additional.ToString())
			};

            this.ExecuteNonQuery(SQL_UPDATE_CONFIGURATION, updateParms);
            ConfigurationManager.IsChanged = true;
		}

		public ConfigurationInfo GetConfigurationInfo()
		{
            ConfigurationInfo configurationInfo = null;

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CONFIGURATION))
            {
                if (rdr.Read())
                {
                    configurationInfo = new ConfigurationInfo(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            if (configurationInfo == null)
            {
                configurationInfo = this.Initialize();
            }

            return configurationInfo;
		}

        private ConfigurationInfo Initialize()
        {
            ConfigurationInfo configurationInfo = new ConfigurationInfo();
            IDbDataParameter[] parms = new IDbDataParameter[]
		    {
			    this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, configurationInfo.Additional.ToString())
		    };

            this.ExecuteNonQuery(SQL_INSERT_CONFIGURATION, parms);
            ConfigurationManager.IsChanged = true;

            return configurationInfo;
        }

	}
}
