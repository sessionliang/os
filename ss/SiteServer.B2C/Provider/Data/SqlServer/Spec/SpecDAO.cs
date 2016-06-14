using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class SpecDAO : DataProviderBase, ISpecDAO
	{
        private const string SQL_UPDATE_SPEC = "UPDATE b2c_Spec SET SpecName = @SpecName, IsIcon = @IsIcon, IsMultiple = @IsMultiple, IsRequired = @IsRequired, Description = @Description WHERE SpecID = @SpecID";

        private const string SQL_DELETE = "DELETE FROM b2c_Spec WHERE SpecID = @SpecID";

        private const string SQL_DELETE_ALL = "DELETE FROM b2c_Spec WHERE PublishmentSystemID = @PublishmentSystemID AND ChannelID = @ChannelID";

        private const string SQL_SELECT_SPEC = "SELECT SpecID, PublishmentSystemID, ChannelID, SpecName, IsIcon, IsMultiple, IsRequired, Description, Taxis FROM b2c_Spec WHERE SpecID = @SpecID";

        private const string SQL_SELECT_SPEC_BY_NAME = "SELECT SpecID, PublishmentSystemID, ChannelID, SpecName, IsIcon, IsMultiple, IsRequired, Description, Taxis FROM b2c_Spec WHERE PublishmentSystemID = @PublishmentSystemID AND ChannelID = @ChannelID AND SpecName = @SpecName ORDER BY Taxis";

        private const string SQL_SELECT_ALL_SPEC = "SELECT SpecID, PublishmentSystemID, ChannelID, SpecName, IsIcon, IsMultiple, IsRequired, Description, Taxis FROM b2c_Spec WHERE PublishmentSystemID = @PublishmentSystemID AND ChannelID = @ChannelID ORDER BY Taxis";

        private const string SQL_SELECT_ALL_SPEC_ID = "SELECT SpecID FROM b2c_Spec WHERE PublishmentSystemID = @PublishmentSystemID AND ChannelID = @ChannelID";

        private const string PARM_SPEC_ID = "@SpecID";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CHANNEL_ID = "@ChannelID";
        private const string PARM_SPEC_NAME = "@SpecName";
        private const string PARM_IS_ICON = "@IsIcon";
        private const string PARM_IS_MULTIPLE = "@IsMultiple";
        private const string PARM_IS_REQUIRED = "@IsRequired";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_TAXIS = "@Taxis";

		public int Insert(SpecInfo specInfo) 
		{
            int specID = 0;

            string sqlString = "INSERT INTO b2c_Spec (PublishmentSystemID, ChannelID, SpecName, IsIcon, IsMultiple, IsRequired, Description, Taxis) VALUES (@PublishmentSystemID, @ChannelID, @SpecName, @IsIcon, @IsMultiple, @IsRequired, @Description, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_Spec (SpecID, PublishmentSystemID, ChannelID, SpecName, IsIcon, IsMultiple, IsRequired, Description, Taxis) VALUES (b2c_Spec_SEQ.NEXTVAL, @PublishmentSystemID, @ChannelID, @SpecName, @IsIcon, @IsMultiple, @IsRequired, @Description, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(specInfo.PublishmentSystemID, specInfo.ChannelID) + 1;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, specInfo.PublishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, specInfo.ChannelID),
                this.GetParameter(PARM_SPEC_NAME, EDataType.NVarChar, 50, specInfo.SpecName),
                this.GetParameter(PARM_IS_ICON, EDataType.VarChar, 18, specInfo.IsIcon.ToString()),
                this.GetParameter(PARM_IS_MULTIPLE, EDataType.VarChar, 18, specInfo.IsMultiple.ToString()),
                this.GetParameter(PARM_IS_REQUIRED, EDataType.VarChar, 18, specInfo.IsRequired.ToString()),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, specInfo.Description),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        specID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "b2c_Spec");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            SpecManager.ClearCache(specInfo.PublishmentSystemID);

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(specInfo.PublishmentSystemID, specInfo.ChannelID);
            nodeInfo.Additional.SpecCount = this.GetCount(specInfo.PublishmentSystemID, specInfo.ChannelID);
            DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

            return specID;
		}

        public void Update(SpecInfo specInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_SPEC_NAME, EDataType.NVarChar, 50, specInfo.SpecName),
                this.GetParameter(PARM_IS_ICON, EDataType.VarChar, 18, specInfo.IsIcon.ToString()),
                this.GetParameter(PARM_IS_MULTIPLE, EDataType.VarChar, 18, specInfo.IsMultiple.ToString()),
                this.GetParameter(PARM_IS_REQUIRED, EDataType.VarChar, 18, specInfo.IsRequired.ToString()),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, specInfo.Description),
				this.GetParameter(PARM_SPEC_ID, EDataType.Integer, specInfo.SpecID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_SPEC, parms);

            SpecManager.ClearCache(specInfo.PublishmentSystemID);
		}

		public void Delete(int publishmentSystemID, int channelID, int specID)
		{
			IDbDataParameter[] deleteParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SPEC_ID, EDataType.Integer, specID)
			};

            this.ExecuteNonQuery(SQL_DELETE, deleteParms);

            SpecManager.ClearCache(publishmentSystemID);

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
            nodeInfo.Additional.SpecCount = this.GetCount(publishmentSystemID, channelID);
            DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
		}

        public void DeleteAll(int publishmentSystemID, int channelID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID)
			};

            this.ExecuteNonQuery(SQL_DELETE_ALL, parms);

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
            nodeInfo.Additional.SpecCount = 0;
            DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
        }

        public bool IsExists(int publishmentSystemID, int channelID, string specName)
		{
			bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID),
				this.GetParameter(PARM_SPEC_NAME, EDataType.NVarChar, 50, specName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SPEC_BY_NAME, parms))
			{
				if (rdr.Read()) 
				{					
					exists = true;
				}
				rdr.Close();
			}

			return exists;
		}

        public SpecInfo GetSpecInfo(int publishmentSystemID, int channelID, string specName)
        {
            SpecInfo specInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID),
				this.GetParameter(PARM_SPEC_NAME, EDataType.NVarChar, 50, specName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SPEC_BY_NAME, parms))
            {
                if (rdr.Read())
                {
                    specInfo = new SpecInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString(), rdr.GetInt32(8));
                }
                rdr.Close();
            }

            return specInfo;
        }

		public IEnumerable GetDataSource(int publishmentSystemID, int channelID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID)
			};
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_SPEC, parms);
			return enumerable;
		}

        public List<SpecInfo> GetSpecInfoList(int publishmentSystemID, int channelID)
        {
            List<SpecInfo> list = new List<SpecInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_SPEC, parms))
            {
                while (rdr.Read())
                {
                    SpecInfo specInfo = new SpecInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString(), rdr.GetInt32(8));
                    list.Add(specInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<int> GetSpecIDList(int publishmentSystemID, int channelID)
        {
            List<int> list = new List<int>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_SPEC_ID, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public Dictionary<int, SpecInfo> GetSpecInfoDictionary(int publishmentSystemID)
        {
            Dictionary<int, SpecInfo> dictionary = new Dictionary<int, SpecInfo>();

            List<SpecInfo> specInfoList = new List<SpecInfo>();

            string sqlString = string.Format("SELECT SpecID, PublishmentSystemID, ChannelID, SpecName, IsIcon, IsMultiple, IsRequired, Description, Taxis FROM b2c_Spec WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    SpecInfo specInfo = new SpecInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString(), rdr.GetInt32(8));
                    specInfoList.Add(specInfo);
                }
                rdr.Close();
            }

            foreach (SpecInfo specInfo in specInfoList)
            {
                dictionary.Add(specInfo.SpecID, specInfo);
            }

            return dictionary;
        }

        public IEnumerable GetStlDataSource(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID, int startNum, int totalNum)
        {
            string orderByString = "ORDER BY Taxis";

            string tableName = NodeManager.GetTableName(publishmentSystemInfo, channelID);
            string specIDCollection = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, GoodsContentAttribute.SpecIDCollection);
            if (!string.IsNullOrEmpty(specIDCollection))
            {
                string whereString = string.Format("WHERE PublishmentSystemID = {0} AND ChannelID = {1} AND SpecID IN ({2})", publishmentSystemInfo.PublishmentSystemID, channelID, specIDCollection);

                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("b2c_Spec", startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);

                return (IEnumerable)this.ExecuteReader(SQL_SELECT);
            }
            return null;
        }

        public string GetImportSpecName(int publishmentSystemID, int channelID, string specName)
        {
            string importSpecName;
            if (specName.IndexOf("_") != -1)
            {
                int specName_Count = 0;
                string lastSpecName = specName.Substring(specName.LastIndexOf("_") + 1);
                string firstSpecName = specName.Substring(0, specName.Length - lastSpecName.Length);
                try
                {
                    specName_Count = int.Parse(lastSpecName);
                }
                catch { }
                specName_Count++;
                importSpecName = firstSpecName + specName_Count;
            }
            else
            {
                importSpecName = specName + "_1";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID),
				this.GetParameter(PARM_SPEC_NAME, EDataType.NVarChar, 50, importSpecName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SPEC_BY_NAME, parms))
            {
                if (rdr.Read())
                {
                    importSpecName = GetImportSpecName(publishmentSystemID, channelID, importSpecName);
                }
                rdr.Close();
            }

            return importSpecName;
        }

        private int GetMaxTaxis(int publishmentSystemID, int channelID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM b2c_Spec WHERE PublishmentSystemID = {0} AND ChannelID = {1}", publishmentSystemID, channelID);
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxTaxis = rdr.GetInt32(0);
                    }
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, int channelID, int specID)
        {
            string sqlString = string.Format("SELECT TOP 1 SpecID, Taxis FROM b2c_Spec WHERE (Taxis > (SELECT Taxis FROM b2c_Spec WHERE SpecID = {0})) AND PublishmentSystemID = {1} AND ChannelID = {2} ORDER BY Taxis", specID, publishmentSystemID, channelID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(specID);

            if (higherID > 0)
            {
                SetTaxis(specID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int channelID, int specID)
        {
            string sqlString = string.Format("SELECT TOP 1 SpecID, Taxis FROM b2c_Spec WHERE (Taxis < (SELECT Taxis FROM b2c_Spec WHERE SpecID = {0})) AND PublishmentSystemID = {1} AND ChannelID = {2} ORDER BY Taxis DESC", specID, publishmentSystemID, channelID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(specID);

            if (lowerID > 0)
            {
                SetTaxis(specID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetTaxis(int specID)
        {
            string sqlString = string.Format("SELECT Taxis FROM b2c_Spec WHERE SpecID = {0}", specID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int specID, int taxis)
        {
            string sqlString = string.Format("UPDATE b2c_Spec SET Taxis = {0} WHERE SpecID = {1}", taxis, specID);
            this.ExecuteNonQuery(sqlString);
        }

        private int GetCount(int publishmentSystemID, int channelID)
        {
            int count = 0;

            string sqlString = "SELECT COUNT(*) FROM b2c_Spec WHERE PublishmentSystemID = @PublishmentSystemID AND ChannelID = @ChannelID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, channelID)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return count;
        }
	}
}