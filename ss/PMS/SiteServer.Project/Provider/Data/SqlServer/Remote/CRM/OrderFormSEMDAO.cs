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
    public class OrderFormSEMDAO : DataProviderBase, IOrderFormSEMDAO
    {
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }
        public int Insert(OrderFormSEMInfo info)
        {
            int contentID = 0;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, this.ConnectionString, OrderFormSEMInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, OrderFormSEMInfo.TableName);

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

        public void Update(OrderFormSEMInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, this.ConnectionString, OrderFormSEMInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void DeleteByOrderID(int orderID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE OrderID = {1}", OrderFormSEMInfo.TableName, orderID);
            this.ExecuteNonQuery(sqlString);
        }

        public OrderFormSEMInfo GetOrderFormSEMInfo(NameValueCollection form)
        {
            OrderFormSEMInfo orderFormInfo = new OrderFormSEMInfo();

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

        public OrderFormSEMInfo GetOrderFormSEMInfoByOrderID(int orderID)
        {
            OrderFormSEMInfo info = null;
            string SQL_WHERE = string.Format("WHERE OrderID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderFormSEMInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new OrderFormSEMInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public OrderFormSEMInfo GetOrderFormSEMInfo(int id)
        {
            OrderFormSEMInfo info = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", id);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderFormSEMInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new OrderFormSEMInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetOrderFormSEMID(int orderID)
        {
            int orderFormID = 0;

            string sqlString = string.Format("SELECT ID FROM {0} WHERE OrderID = {1}", OrderFormSEMInfo.TableName, orderID);
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
    }
}
