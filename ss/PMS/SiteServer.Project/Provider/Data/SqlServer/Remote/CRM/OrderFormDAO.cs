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
    public class OrderFormDAO : DataProviderBase, IOrderFormDAO
    {
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }

        public int Insert(OrderFormInfo info)
        {
            int contentID = 0;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, this.ConnectionString, OrderFormInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, OrderFormInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return contentID;
        }

        public void Update(OrderFormInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, this.ConnectionString, OrderFormInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateIsCompleted(int contentID)
        {
            string sqlString = string.Format("UPDATE {0} SET IsCompleted = '{1}' AND CurrentPageID = 0 WHERE ID = {2}", OrderFormInfo.TableName, true.ToString(), contentID);

            this.ExecuteNonQuery(sqlString);
        }

        public void DeleteByOrderID(int orderID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE OrderID = {1}", OrderFormInfo.TableName, orderID);
            this.ExecuteNonQuery(sqlString);
        }

        public OrderFormInfo GetOrderFormInfo(NameValueCollection form)
        {
            OrderFormInfo orderFormInfo = new OrderFormInfo();

            foreach (string name in form.AllKeys)
            {
                string value = form[name];
                if (!string.IsNullOrEmpty(value))
                {
                    orderFormInfo.SetExtendedAttribute(name, value.Trim());
                }
            }

            return orderFormInfo;
        }

        public OrderFormInfo GetOrderFormInfoByOrderID(int orderID)
        {
            OrderFormInfo info = null;
            string SQL_WHERE = string.Format("WHERE OrderID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderFormInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new OrderFormInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public OrderFormInfo GetOrderFormInfo(int id)
        {
            OrderFormInfo info = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", id);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderFormInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new OrderFormInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetOrderFormID(int orderID)
        {
            int orderFormID = 0;

            string sqlString = string.Format("SELECT ID FROM {0} WHERE OrderID = {1}", OrderFormInfo.TableName, orderID);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    orderFormID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return orderFormID;
        }

        public bool IsCompleted(int orderID, out int orderFormID)
        {
            orderFormID = 0;
            bool retval = false;

            string sqlString = string.Format("SELECT ID, IsCompleted FROM {0} WHERE OrderID = {1}", OrderFormInfo.TableName, orderID);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    orderFormID = rdr.GetInt32(0);
                    retval = TranslateUtils.ToBool(rdr.GetValue(1).ToString());
                }
                rdr.Close();
            }

            return retval;
        }
    }
}
