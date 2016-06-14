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
    public class OrderRefundDAO : DataProviderBase, IOrderRefundDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }
        public int Insert(OrderRefundInfo refundInfo)
        {
            int refundID = 0;

            refundInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(refundInfo.Attributes, this.ConnectionString, OrderRefundInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        refundID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, OrderRefundInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return refundID;
        }

        public void Update(OrderRefundInfo refundInfo)
        {
            refundInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(refundInfo.Attributes, this.ConnectionString, OrderRefundInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", OrderRefundInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public ArrayList GetOrderIDArrayList(ArrayList idArrayList)
        {
            string sqlString = string.Format("SELECT OrderID FROM {0} WHERE ID IN ({1})", OrderRefundInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));
            return BaiRongDataProvider.DatabaseDAO.GetIntArrayList(this.ConnectionString, sqlString);
        }

        public OrderRefundInfo GetOrderRefundInfo(int orderID, NameValueCollection form)
        {
            OrderRefundInfo refundInfo = new OrderRefundInfo(0, orderID);

            foreach (string name in form.AllKeys)
            {
                if (OrderRefundAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        refundInfo.SetExtendedAttribute(name, value.Trim());
                    }
                }
            }

            return refundInfo;
        }

        public OrderRefundInfo GetOrderRefundInfo(int refundID)
        {
            OrderRefundInfo refundInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", refundID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderRefundInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    refundInfo = new OrderRefundInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, refundInfo);
                }
                rdr.Close();
            }

            if (refundInfo != null) refundInfo.AfterExecuteReader();
            return refundInfo;
        }

        public OrderRefundInfo GetOrderRefundInfoByOrderID(int orderID)
        {
            OrderRefundInfo refundInfo = null;
            string SQL_WHERE = string.Format("WHERE OrderID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderRefundInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    refundInfo = new OrderRefundInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, refundInfo);
                }
                rdr.Close();
            }

            if (refundInfo != null) refundInfo.AfterExecuteReader();
            return refundInfo;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", OrderRefundInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectString()
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderRefundInfo.TableName, 0, SqlUtils.Asterisk, null, null);
        }

        public string GetSelectString(string keyword)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" (OrderSN LIKE '%{0}%' OR LoginName LIKE '%{0}%' OR AccountRealName LIKE '%{0}%' OR AccountAlipayNo LIKE '%{0}%' OR AccountBankName LIKE '%{0}%' OR AccountBankCardNo LIKE '%{0}%' OR Reason LIKE '%{0}%') ", keyword);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, OrderRefundInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetOrderRefundByString()
        {
            return "ORDER BY ID DESC";
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
