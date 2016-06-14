using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Model;
using SiteServer.Project.Core;

using System.Collections.Generic;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class TouchDAO : DataProviderBase, ITouchDAO
    {
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }

        private const string TABLE_NAME = "crm_Touch";

        public int Insert(TouchInfo touchInfo)
        {
            int touchID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(touchInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        touchID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return touchID;
        }

        public void Update(TouchInfo touchInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(touchInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int touchID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, touchID);
            this.ExecuteNonQuery(sqlString);
        }

        public TouchInfo GetTouchInfo(int touchID)
        {
            TouchInfo touchInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", touchID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    touchInfo = new TouchInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        touchInfo.SetValue(columnName, rdr.GetValue(i));
                    }
                }
                rdr.Close();
            }

            return touchInfo;
        }

        public bool IsMessageIDExists(int leadID, int orderID, int messageID)
        {
            bool exists = false;

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND {4} = {5}", TouchAttribute.LeadID, leadID, TouchAttribute.OrderID, orderID, TouchAttribute.MessageID, messageID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, TouchAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public List<TouchInfo> GetTouchInfoList(int leadID, int orderID)
        {
            List<TouchInfo> list = new List<TouchInfo>();

            if (leadID > 0 || orderID > 0)
            {
                string SQL_WHERE = string.Empty;
                if (leadID > 0)
                {
                    SQL_WHERE = string.Format("WHERE {0} = {1}", TouchAttribute.LeadID, leadID);
                }
                else if (orderID > 0)
                {
                    SQL_WHERE = string.Format("WHERE {0} = {1}", TouchAttribute.OrderID, orderID);
                }
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    while (rdr.Read())
                    {
                        TouchInfo touchInfo = new TouchInfo();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            string columnName = rdr.GetName(i);
                            touchInfo.SetValue(columnName, rdr.GetValue(i));
                        }

                        list.Add(touchInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public Dictionary<int, int> GetCountByLeadIDList(List<int> leadIDList)
        {
            Dictionary<int, int> counts = new Dictionary<int, int>();

            if (leadIDList != null && leadIDList.Count > 0)
            {
                string sqlString = string.Format("SELECT LeadID, COUNT(*) FROM {0} GROUP BY LeadID HAVING LeadID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(leadIDList));

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        counts.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                    }
                    rdr.Close();
                }
            }

            return counts;
        }

        public Dictionary<int, int> GetCountByOrderIDList(List<int> orderIDList)
        {
            Dictionary<int, int> counts = new Dictionary<int, int>();

            if (orderIDList != null && orderIDList.Count > 0)
            {
                string sqlString = string.Format("SELECT OrderID, COUNT(*) FROM {0} GROUP BY OrderID HAVING OrderID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(orderIDList));

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        counts.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                    }
                    rdr.Close();
                }
            }

            return counts;
        }
    }
}
