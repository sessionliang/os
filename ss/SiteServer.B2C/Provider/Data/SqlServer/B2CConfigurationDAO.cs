using System.Data;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core.AuxiliaryTable;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class B2CConfigurationDAO : DataProviderBase, IB2CConfigurationDAO
	{
        private const string SQL_INSERT = "INSERT INTO b2c_Configuration (NodeID, IsVirtualGoods, SettingsXML) VALUES (@NodeID, @IsVirtualGoods, @SettingsXML)";

        private const string SQL_SELECT = "SELECT NodeID, IsVirtualGoods, SettingsXML FROM b2c_Configuration WHERE NodeID = @NodeID";

        private const string SQL_UPDATE = "UPDATE b2c_Configuration SET IsVirtualGoods = @IsVirtualGoods, SettingsXML = @SettingsXML WHERE NodeID = @NodeID";

		private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_IS_VIRTUAL_GOODS = "@IsVirtualGoods";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

        public void Update(B2CConfigurationInfo configurationInfo) 
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_IS_VIRTUAL_GOODS, EDataType.VarChar, 18, configurationInfo.IsVirtualGoods.ToString()),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, configurationInfo.Additional.ToString()),
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, configurationInfo.NodeID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
            B2CConfigurationManager.IsChanged = true;
		}

        public B2CConfigurationInfo GetConfigurationInfo(int nodeID)
		{
            B2CConfigurationInfo configurationInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    configurationInfo = new B2CConfigurationInfo(rdr.GetInt32(0), TranslateUtils.ToBool(rdr.GetValue(1).ToString()), rdr.GetValue(2).ToString());
                }
                rdr.Close();
            }

            if (configurationInfo == null)
            {
                configurationInfo = this.Insert(nodeID);
            }

            return configurationInfo;
		}

        private B2CConfigurationInfo Insert(int nodeID)
        {
            B2CConfigurationInfo configurationInfo = new B2CConfigurationInfo(nodeID, false, string.Empty);

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, configurationInfo.NodeID),
                this.GetParameter(PARM_IS_VIRTUAL_GOODS, EDataType.VarChar, 18, configurationInfo.IsVirtualGoods.ToString()),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, configurationInfo.Additional.ToString())
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
            B2CConfigurationManager.IsChanged = true;

            return configurationInfo;
        }

	}
}
