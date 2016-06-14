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
    public class OrderDAO : DataProviderBase, IOrderDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }
        public int Insert(OrderInfo orderInfo)
        {
            int orderID = 0;

            orderInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(orderInfo.Attributes, this.ConnectionString, OrderInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        orderID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, OrderInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return orderID;
        }

        public void Update(OrderInfo orderInfo)
        {
            orderInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(orderInfo.Attributes, this.ConnectionString, OrderInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateIsRefund(int orderID, bool isRefund)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID = {3}", OrderInfo.TableName, OrderAttribute.IsRefund, isRefund.ToString(), orderID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIsRefund(ArrayList orderIDArrayList, bool isRefund)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID IN ({3})", OrderInfo.TableName, OrderAttribute.IsRefund, isRefund.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(orderIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIsInvoice(int orderID, bool isInvoice)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID = {3}", OrderInfo.TableName, OrderAttribute.IsInvoice, isInvoice.ToString(), orderID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIsInvoice(ArrayList orderIDArrayList, bool isInvoice)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID IN ({3})", OrderInfo.TableName, OrderAttribute.IsInvoice, isInvoice.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(orderIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIsLicense(int orderID, bool isLicense)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID = {3}", OrderInfo.TableName, OrderAttribute.IsLicense, isLicense.ToString(), orderID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIsContract(int orderID, bool isContract)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID = {3}", OrderInfo.TableName, OrderAttribute.IsContract, isContract.ToString(), orderID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIsContract(ArrayList orderIDArrayList, bool isContract)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID IN ({3})", OrderInfo.TableName, OrderAttribute.IsContract, isContract.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(orderIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", OrderInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public OrderInfo GetOrderInfo(NameValueCollection form)
        {
            OrderInfo orderInfo = new OrderInfo(0);

            foreach (string name in form.AllKeys)
            {
                if (OrderAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        orderInfo.SetExtendedAttribute(name, value.Trim());
                    }
                }
            }

            return orderInfo;
        }

        public OrderInfo GetOrderInfo(int orderID)
        {
            OrderInfo orderInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderInfo = new OrderInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, orderInfo);
                }
                rdr.Close();
            }

            if (orderInfo != null) orderInfo.AfterExecuteReader();
            return orderInfo;
        }

        public OrderInfo GetOrderInfo(string domain)
        {
            OrderInfo orderInfo = null;
            string SQL_WHERE = string.Format("WHERE DomainTemp = '{0}' OR DomainFormal = '{0}'", domain);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderInfo = new OrderInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, orderInfo);
                }
                rdr.Close();
            }

            if (orderInfo != null) orderInfo.AfterExecuteReader();
            return orderInfo;
        }

        public OrderInfo GetOrderInfoBySN(string sn)
        {
            OrderInfo orderInfo = null;
            string SQL_WHERE = string.Format("WHERE SN = '{0}'", sn);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderInfo = new OrderInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, orderInfo);
                }
                rdr.Close();
            }

            if (orderInfo != null) orderInfo.AfterExecuteReader();
            return orderInfo;
        }

        public int GetOrderID(string domain)
        {
            if (!string.IsNullOrEmpty(domain))
            {
                string sqlString = string.Format("SELECT ID FROM {0} WHERE DomainTemp = '{1}' OR DomainFormal = '{1}'", OrderInfo.TableName, domain.ToLower());
                return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
            }
            return 0;
        }

        public string GetOrderSN(int orderID)
        {
            string sn = string.Empty;
            if (orderID > 0)
            {
                string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", OrderAttribute.SN, OrderInfo.TableName, orderID);
                return BaiRongDataProvider.DatabaseDAO.GetString(this.ConnectionString, sqlString);
            }
            return sn;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", OrderInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public int GetMobanUsedCount(string mobanID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE MobanID = '{1}'", OrderInfo.TableName, mobanID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectString(EOrderType orderType)
        {
            string whereString = string.Format("WHERE OrderType = '{0}'", EOrderTypeUtils.GetValue(orderType));
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSelectString(EOrderType orderType, string type, string loginName, string keyword)
        {
            string whereString = string.Format("WHERE OrderType = '{0}'", EOrderTypeUtils.GetValue(orderType));
            if (!string.IsNullOrEmpty(loginName))
            {
                whereString += string.Format(" AND LoginName LIKE '%{0}%'", loginName);
            }
            if (!string.IsNullOrEmpty(type))
            {
                whereString += " AND ";
                if (type == OrderAttribute.IsInvoice)
                {
                    whereString += string.Format(" {0} = '{1}'", OrderAttribute.IsInvoice, true.ToString());
                }
                else if (type == OrderAttribute.IsRefund)
                {
                    whereString += string.Format(" {0} = '{1}'", OrderAttribute.IsRefund, true.ToString());
                }
                else if (type == OrderAttribute.IsReNew)
                {
                    whereString += string.Format(" {0} = '{1}'", OrderAttribute.IsReNew, true.ToString());
                }
                else
                {
                    whereString += string.Format(" {0} = '{1}'", OrderAttribute.Status, type);
                }
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" AND (SN LIKE '%{0}%' OR MobanID LIKE '%{0}%' OR DomainTemp LIKE '%{0}%' OR DomainFormal LIKE '%{0}%' OR Email LIKE '%{0}%' OR ContactName LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR QQ LIKE '%{0}%' OR AliWangWang LIKE '%{0}%' OR InvoiceTitle LIKE '%{0}%' OR InvoiceReceiver LIKE '%{0}%' OR InvoiceTel LIKE '%{0}%' OR InvoiceAddress LIKE '%{0}%') ", keyword);
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetOrderByString()
        {
            return "ORDER BY AddDate DESC";
        }

        public string GetSortFieldName()
        {
            return "AddDate";
        }
	}
}
