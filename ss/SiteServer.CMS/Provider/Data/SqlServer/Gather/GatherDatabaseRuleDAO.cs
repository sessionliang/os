using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GatherDatabaseRuleDAO : DataProviderBase, IGatherDatabaseRuleDAO
    {
        private const string SQL_SELECT_GATHER_RULE = "SELECT GatherRuleName, PublishmentSystemID, DatabaseType, ConnectionString, RelatedTableName, RelatedIdentity, RelatedOrderBy, WhereString, TableMatchID, NodeID, GatherNum, IsChecked, IsOrderByDesc, LastGatherDate, IsAutoCreate FROM siteserver_GatherDatabaseRule WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_TABLE_MATCH_ID = "SELECT TableMatchID FROM siteserver_GatherDatabaseRule WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL_GATHER_RULE_BY_PS_ID = "SELECT GatherRuleName, PublishmentSystemID, DatabaseType, ConnectionString, RelatedTableName, RelatedIdentity, RelatedOrderBy, WhereString, TableMatchID, NodeID, GatherNum, IsChecked, IsOrderByDesc, LastGatherDate, IsAutoCreate FROM siteserver_GatherDatabaseRule WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_GATHER_RULE_NAME_BY_PS_ID = "SELECT GatherRuleName FROM siteserver_GatherDatabaseRule WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_INSERT_GATHER_RULE = @"
INSERT INTO siteserver_GatherDatabaseRule 
(GatherRuleName, PublishmentSystemID, DatabaseType, ConnectionString, RelatedTableName, RelatedIdentity, RelatedOrderBy, WhereString, TableMatchID, NodeID, GatherNum, IsChecked, IsOrderByDesc, LastGatherDate, IsAutoCreate) VALUES (@GatherRuleName, @PublishmentSystemID, @DatabaseType, @ConnectionString, @RelatedTableName, @RelatedIdentity, @RelatedOrderBy, @WhereString, @TableMatchID, @NodeID, @GatherNum, @IsChecked, @IsOrderByDesc, @LastGatherDate, @IsAutoCreate)";

        private const string SQL_UPDATE_GATHER_RULE = @"
UPDATE siteserver_GatherDatabaseRule SET 
DatabaseType = @DatabaseType, ConnectionString = @ConnectionString, RelatedTableName = @RelatedTableName, RelatedIdentity = @RelatedIdentity, RelatedOrderBy = @RelatedOrderBy, WhereString = @WhereString, TableMatchID = @TableMatchID, NodeID = @NodeID, GatherNum = @GatherNum, IsChecked = @IsChecked, IsOrderByDesc = @IsOrderByDesc, LastGatherDate = @LastGatherDate, IsAutoCreate = @IsAutoCreate WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_UPDATE_LAST_GATHER_DATE = "UPDATE siteserver_GatherDatabaseRule SET LastGatherDate = @LastGatherDate WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_DELETE_GATHER_RULE = "DELETE FROM siteserver_GatherDatabaseRule WHERE GatherRuleName = @GatherRuleName AND PublishmentSystemID = @PublishmentSystemID";

        private const string PARM_GATHER_RULE_NAME = "@GatherRuleName";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_DATABASE_TYPE = "@DatabaseType";
        private const string PARM_CONNECTION_STRING = "@ConnectionString";
        private const string PARM_RELATED_TABLE_NAME = "@RelatedTableName";
        private const string PARM_RELATED_IDENTITY = "@RelatedIdentity";
        private const string PARM_RELATED_ORDER_BY = "@RelatedOrderBy";
        private const string PARM_WHERE_STRING = "@WhereString";
        private const string PARM_TABLE_MATCH_ID = "@TableMatchID";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_GATHER_NUM = "@GatherNum";
        private const string PARM_IS_CHECKED = "@IsChecked";
        private const string PARM_IS_ORDER_BY_DESC = "@IsOrderByDesc";
        private const string PARM_LAST_GATHER_DATE = "@LastGatherDate";
        private const string PARM_IS_AUTO_CREATE = "@IsAutoCreate";

        public void Insert(GatherDatabaseRuleInfo gatherDatabaseRuleInfo)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(GatherDatabaseRuleDAO.PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherDatabaseRuleInfo.GatherRuleName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, gatherDatabaseRuleInfo.PublishmentSystemID),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_DATABASE_TYPE, EDataType.VarChar, 50, EDatabaseTypeUtils.GetValue(gatherDatabaseRuleInfo.DatabaseType)),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_CONNECTION_STRING, EDataType.VarChar, 255, gatherDatabaseRuleInfo.ConnectionString),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_RELATED_TABLE_NAME, EDataType.VarChar, 255, gatherDatabaseRuleInfo.RelatedTableName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_RELATED_IDENTITY, EDataType.VarChar, 255, gatherDatabaseRuleInfo.RelatedIdentity),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_RELATED_ORDER_BY, EDataType.VarChar, 255, gatherDatabaseRuleInfo.RelatedOrderBy),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_WHERE_STRING, EDataType.NVarChar, 255, gatherDatabaseRuleInfo.WhereString),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_TABLE_MATCH_ID, EDataType.Integer, gatherDatabaseRuleInfo.TableMatchID),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_NODE_ID, EDataType.Integer, gatherDatabaseRuleInfo.NodeID),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_GATHER_NUM, EDataType.Integer, gatherDatabaseRuleInfo.GatherNum),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_IS_CHECKED, EDataType.VarChar, 18, gatherDatabaseRuleInfo.IsChecked.ToString()),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_IS_ORDER_BY_DESC, EDataType.VarChar, 18, gatherDatabaseRuleInfo.IsOrderByDesc.ToString()),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, gatherDatabaseRuleInfo.LastGatherDate),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_IS_AUTO_CREATE, EDataType.VarChar, 18, gatherDatabaseRuleInfo.IsAutoCreate.ToString())
            };

            this.ExecuteNonQuery(SQL_INSERT_GATHER_RULE, insertParms);
        }

        public void UpdateLastGatherDate(string gatherRuleName, int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(GatherDatabaseRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, DateTime.Now),
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_LAST_GATHER_DATE, parms);
        }

        public void Update(GatherDatabaseRuleInfo gatherDatabaseRuleInfo)
        {

            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(GatherDatabaseRuleDAO.PARM_DATABASE_TYPE, EDataType.VarChar, 50, EDatabaseTypeUtils.GetValue(gatherDatabaseRuleInfo.DatabaseType)),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_CONNECTION_STRING, EDataType.VarChar, 255, gatherDatabaseRuleInfo.ConnectionString),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_RELATED_TABLE_NAME, EDataType.VarChar, 255, gatherDatabaseRuleInfo.RelatedTableName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_RELATED_IDENTITY, EDataType.VarChar, 255, gatherDatabaseRuleInfo.RelatedIdentity),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_RELATED_ORDER_BY, EDataType.VarChar, 255, gatherDatabaseRuleInfo.RelatedOrderBy),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_WHERE_STRING, EDataType.NVarChar, 255, gatherDatabaseRuleInfo.WhereString),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_TABLE_MATCH_ID, EDataType.Integer, gatherDatabaseRuleInfo.TableMatchID),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_NODE_ID, EDataType.Integer, gatherDatabaseRuleInfo.NodeID),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_GATHER_NUM, EDataType.Integer, gatherDatabaseRuleInfo.GatherNum),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_IS_CHECKED, EDataType.VarChar, 18, gatherDatabaseRuleInfo.IsChecked.ToString()),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_IS_ORDER_BY_DESC, EDataType.VarChar, 18, gatherDatabaseRuleInfo.IsOrderByDesc.ToString()),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_LAST_GATHER_DATE, EDataType.DateTime, gatherDatabaseRuleInfo.LastGatherDate),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherDatabaseRuleInfo.GatherRuleName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_IS_AUTO_CREATE, EDataType.VarChar, 18, gatherDatabaseRuleInfo.IsAutoCreate.ToString()),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, gatherDatabaseRuleInfo.PublishmentSystemID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_GATHER_RULE, updateParms);
        }


        public void Delete(string gatherRuleName, int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            this.ExecuteNonQuery(SQL_DELETE_GATHER_RULE, parms);
        }

        public GatherDatabaseRuleInfo GetGatherDatabaseRuleInfo(string gatherRuleName, int publishmentSystemID)
        {
            GatherDatabaseRuleInfo gatherDatabaseRuleInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_RULE, parms))
            {
                if (rdr.Read())
                {
                    gatherDatabaseRuleInfo = new GatherDatabaseRuleInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), EDatabaseTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), TranslateUtils.ToBool(rdr.GetValue(12).ToString()), rdr.GetDateTime(13), TranslateUtils.ToBool(rdr.GetValue(14).ToString()));
                }
                rdr.Close();
            }

            return gatherDatabaseRuleInfo;
        }

        public string GetImportGatherRuleName(int publishmentSystemID, string gatherRuleName)
        {
            string importGatherRuleName = "";
            if (gatherRuleName.IndexOf("_") != -1)
            {
                int gatherRuleName_Count = 0;
                string lastGatherRuleName = gatherRuleName.Substring(gatherRuleName.LastIndexOf("_") + 1);
                string firstGatherRuleName = gatherRuleName.Substring(0, gatherRuleName.Length - lastGatherRuleName.Length);
                try
                {
                    gatherRuleName_Count = int.Parse(lastGatherRuleName);
                }
                catch { }
                gatherRuleName_Count++;
                importGatherRuleName = firstGatherRuleName + gatherRuleName_Count;
            }
            else
            {
                importGatherRuleName = gatherRuleName + "_1";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, importGatherRuleName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_RULE, parms))
            {
                if (rdr.Read())
                {
                    importGatherRuleName = GetImportGatherRuleName(publishmentSystemID, importGatherRuleName);
                }
                rdr.Close();
            }

            return importGatherRuleName;
        }

        public IEnumerable GetDataSource(int publishmentSystemID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_GATHER_RULE_BY_PS_ID, parms);
            return enumerable;
        }

        public ArrayList GetGatherDatabaseRuleInfoArrayList(int publishmentSystemID)
        {
            ArrayList list = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_GATHER_RULE_BY_PS_ID, parms))
            {
                while (rdr.Read())
                {
                    GatherDatabaseRuleInfo gatherDatabaseRuleInfo = new GatherDatabaseRuleInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), EDatabaseTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), TranslateUtils.ToBool(rdr.GetValue(12).ToString()), rdr.GetDateTime(13), TranslateUtils.ToBool(rdr.GetValue(14).ToString()));
                }
                rdr.Close();
            }

            return list;
        }

        public ArrayList GetGatherRuleNameArrayList(int publishmentSystemID)
        {
            ArrayList list = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_GATHER_RULE_NAME_BY_PS_ID, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return list;
        }

        public int GetTableMatchID(string gatherRuleName, int publishmentSystemID)
        {
            int tableMatchID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GATHER_RULE_NAME, EDataType.NVarChar, 50, gatherRuleName),
                this.GetParameter(GatherDatabaseRuleDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_MATCH_ID, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        tableMatchID = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }

            return tableMatchID;
        }


        public void OpenAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection)
        {
            string sql = string.Format("UPDATE siteserver_GatherDatabaseRule SET IsAutoCreate = 'True' WHERE PublishmentSystemID = {0} AND GatherRuleName in ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(gatherRuleNameCollection));
            this.ExecuteNonQuery(sql);
        }

        public void CloseAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection)
        {
            string sql = string.Format("UPDATE siteserver_GatherDatabaseRule SET IsAutoCreate = 'False' WHERE PublishmentSystemID = {0} AND GatherRuleName in ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(gatherRuleNameCollection));
            this.ExecuteNonQuery(sql);
        }

    }
}
