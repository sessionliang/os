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
    public class FilterDAO : DataProviderBase, IFilterDAO
    {
        private const string SQL_UPDATE_IS_DEFAULT_VALUES = "UPDATE b2c_Filter SET IsDefaultValues = @IsDefaultValues WHERE FilterID = @FilterID";

        private const string SQL_UPDATE_DISPLAY_NAME = "UPDATE b2c_Filter SET FilterName = @FilterName WHERE FilterID = @FilterID";

        private const string SQL_DELETE_FILTER = "DELETE FROM b2c_Filter WHERE FilterID = @FilterID";

        private const string SQL_DELETE_ALL = "DELETE FROM b2c_Filter WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID";

        private const string SQL_SELECT_FILTER = "SELECT FilterID, PublishmentSystemID, NodeID, AttributeName, FilterName, IsDefaultValues, Taxis FROM b2c_Filter WHERE FilterID = @FilterID";

        private const string SQL_SELECT_FILTER_BY_NAME = "SELECT FilterID, PublishmentSystemID, NodeID, AttributeName, FilterName, IsDefaultValues, Taxis FROM b2c_Filter WHERE NodeID = @NodeID AND AttributeName = @AttributeName";

        private const string SQL_SELECT_ALL_FILTER = "SELECT FilterID, PublishmentSystemID, NodeID, AttributeName, FilterName, IsDefaultValues, Taxis FROM b2c_Filter WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID ORDER BY Taxis";

        private const string SQL_SELECT_ATTRIBUTE_NAME = "SELECT AttributeName FROM b2c_Filter WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID ORDER BY Taxis DESC";

        private const string SQL_SELECT_COUNT = "SELECT COUNT(*) FROM b2c_Filter WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID";

        private const string PARM_FILTER_ID = "@FilterID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_ATTRIBUTE_NAME = "@AttributeName";
        private const string PARM_FILTER_NAME = "@FilterName";
        private const string PARM_IS_DEFAULT_VALUES = "@IsDefaultValues";
        private const string PARM_TAXIS = "@Taxis";

        public int Insert(FilterInfo filterInfo)
        {
            int filterID = 0;

            string sqlString = "INSERT INTO b2c_Filter (PublishmentSystemID, NodeID, AttributeName, FilterName, IsDefaultValues, Taxis) VALUES (@PublishmentSystemID, @NodeID, @AttributeName, @FilterName, @IsDefaultValues, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_Filter (FilterID, PublishmentSystemID, NodeID, AttributeName, FilterName, IsDefaultValues, Taxis) VALUES (b2c_Filter_SEQ.NEXTVAL, @PublishmentSystemID, @NodeID, @AttributeName, @FilterName, @IsDefaultValues, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(filterInfo.NodeID) + 1;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, filterInfo.PublishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, filterInfo.NodeID),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 200, filterInfo.AttributeName),
                this.GetParameter(PARM_FILTER_NAME, EDataType.NVarChar, 50, filterInfo.FilterName),
                this.GetParameter(PARM_IS_DEFAULT_VALUES, EDataType.VarChar, 18, filterInfo.IsDefaultValues.ToString()),
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
                        filterID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "b2c_Filter");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(filterInfo.PublishmentSystemID, filterInfo.NodeID);
            nodeInfo.Additional.FilterCount = DataProviderB2C.FilterDAO.GetCount(filterInfo.PublishmentSystemID, filterInfo.NodeID);
            DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

            return filterID;
        }

        public void Update(FilterInfo filterInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_FILTER_NAME, EDataType.NVarChar, 50, filterInfo.FilterName),
                this.GetParameter(PARM_FILTER_ID, EDataType.Integer, filterInfo.FilterID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_DISPLAY_NAME, parms);
        }

        public void UpdateIsDefaultValues(bool isDefaultValues, int filterID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_IS_DEFAULT_VALUES, EDataType.VarChar, 18, isDefaultValues.ToString()),
                this.GetParameter(PARM_FILTER_ID, EDataType.Integer, filterID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_IS_DEFAULT_VALUES, parms);
        }

        public void Delete(int publishmentSystemID, int nodeID, int filterID)
        {
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_FILTER_ID, EDataType.Integer, filterID)
            };

            this.ExecuteNonQuery(SQL_DELETE_FILTER, deleteParms);

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            nodeInfo.Additional.FilterCount = DataProviderB2C.FilterDAO.GetCount(publishmentSystemID, nodeID);
            DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
        }

        public void DeleteAll(int publishmentSystemID, int nodeID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            this.ExecuteNonQuery(SQL_DELETE_ALL, parms);

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            nodeInfo.Additional.FilterCount = 0;
            DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
        }

        public FilterInfo GetFilterInfo(int filterID)
        {
            FilterInfo filterInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_FILTER_ID, EDataType.Integer, filterID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_FILTER, parms))
            {
                if (rdr.Read())
                {
                    filterInfo = new FilterInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6));
                }
                rdr.Close();
            }

            return filterInfo;
        }

        public FilterInfo GetFilterInfo(int nodeID, string attributeName)
        {
            FilterInfo filterInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 200, attributeName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_FILTER_BY_NAME, parms))
            {
                if (rdr.Read())
                {
                    filterInfo = new FilterInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6));
                }
                rdr.Close();
            }

            return filterInfo;
        }

        public bool IsExists(int nodeID, string attributeName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 200, attributeName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_FILTER_BY_NAME, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public int GetCount(int publishmentSystemID, int nodeID)
        {
            int count = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_COUNT, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        count = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }

            return count;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, int nodeID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_FILTER, parms);
            return enumerable;
        }

        public List<FilterInfo> GetFilterInfoList(int publishmentSystemID, int nodeID)
        {
            List<FilterInfo> list = new List<FilterInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_FILTER, parms))
            {
                while (rdr.Read())
                {
                    FilterInfo filterInfo = new FilterInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6));
                    list.Add(filterInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<string> GetAttributeNameList(int publishmentSystemID, int nodeID)
        {
            List<string> list = new List<string>();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ATTRIBUTE_NAME, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToDown(int nodeID, int filterID)
        {
            string sqlString = string.Format("SELECT TOP 1 FilterID, Taxis FROM b2c_Filter WHERE ((Taxis > (SELECT Taxis FROM b2c_Filter WHERE FilterID = {0})) AND NodeID ={1}) ORDER BY Taxis", filterID, nodeID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(filterID);

            if (higherID != 0)
            {
                SetTaxis(filterID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToUp(int nodeID, int filterID)
        {
            string sqlString = string.Format("SELECT TOP 1 FilterID, Taxis FROM b2c_Filter WHERE ((Taxis < (SELECT Taxis FROM b2c_Filter WHERE (FilterID = {0}))) AND NodeID = {1}) ORDER BY Taxis DESC", filterID, nodeID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(filterID);

            if (lowerID != 0)
            {
                SetTaxis(filterID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int nodeID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM b2c_Filter WHERE NodeID = {0}", nodeID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int filterID)
        {
            string sqlString = string.Format("SELECT Taxis FROM b2c_Filter WHERE (FilterID = {0})", filterID);
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

        private void SetTaxis(int filterID, int taxis)
        {
            string sqlString = string.Format("UPDATE b2c_Filter SET Taxis = {0} WHERE FilterID = {1}", taxis, filterID);
            this.ExecuteNonQuery(sqlString);
        }

        public IEnumerable GetStlDataSource(int publishmentSystemID, int nodeID, int startNum, int totalNum)
        {
            string orderByString = "ORDER BY Taxis";

            string whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1}", publishmentSystemID, nodeID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("b2c_Filter", startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);

            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }
    }
}
