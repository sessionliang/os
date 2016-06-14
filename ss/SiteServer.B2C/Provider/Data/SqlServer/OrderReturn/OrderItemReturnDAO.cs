using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using BaiRong.Model;
using System.Data;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.Data;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class OrderItemReturnDAO : DataProviderBase, IOrderItemReturnDAO
    {
        private const string TABLE_NAME = "b2c_OrderItemReturn";

        public void Insert(OrderItemReturnInfo itemReturnInfo)
        {
            try
            {
                IDbDataParameter[] parms = null;
                string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemReturnInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);
                this.ExecuteNonQuery(SQL_INSERT, parms);
            }
            catch
            {
                throw;
            }
        }

        public void Insert(List<OrderItemReturnInfo> itemReturnInfoList)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (OrderItemReturnInfo itemReturnInfo in itemReturnInfoList)
                        {
                            IDbDataParameter[] parms = null;
                            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemReturnInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

                            this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Insert(IDbTransaction trans, List<OrderItemReturnInfo> itemReturnInfoList)
        {
            if (trans != null)
            {
                foreach (OrderItemReturnInfo itemReturnInfo in itemReturnInfoList)
                {
                    IDbDataParameter[] parms = null;
                    string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemReturnInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

                    this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                }
            }
        }

        public void Delete(int itemID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, itemID);
            this.ExecuteNonQuery(sqlString);
        }

        public OrderItemReturnInfo GetItemReturnInfo(int itemID)
        {
            OrderItemReturnInfo orderItemReturnInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", itemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderItemReturnInfo = new OrderItemReturnInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        orderItemReturnInfo.SetValue(columnName, rdr.GetValue(i));
                    }
                }
                rdr.Close();
            }

            return orderItemReturnInfo;
        }

        public List<OrderItemReturnInfo> GetItemReturnInfoList(int orderItemID)
        {
            List<OrderItemReturnInfo> list = new List<OrderItemReturnInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", OrderItemReturnAttribute.OrderItemID, orderItemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID ASC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    OrderItemReturnInfo itemReturnInfo = new OrderItemReturnInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        itemReturnInfo.SetValue(columnName, rdr.GetValue(i));
                    }

                    list.Add(itemReturnInfo);
                }
                rdr.Close();
            }

            return list;
        }


        public int GetCount(string where)
        {
            int retval = 0;
            string SQL_SELECT = string.Format("SELECT COUNT(*) FROM {0} ", TABLE_NAME);
            if (!string.IsNullOrEmpty(where))
            {
                SQL_SELECT += string.Format(" WHERE {0} ", where);
            }
            object temp = this.ExecuteScalar(SQL_SELECT);
            retval = TranslateUtils.ToInt(temp != null ? temp.ToString() : string.Empty);
            return retval;
        }

        public List<OrderItemReturnInfo> GetItemReturnInfoList(string where, int pageIndex, int prePageNum)
        {
            List<OrderItemReturnInfo> list = new List<OrderItemReturnInfo>();
            string SQL_WHERE = " WHERE 1=1 ";
            string SQL_SELECT = string.Empty;
            //string SQL_JOIN = string.Format("INNER JOIN [b2c_OrderItem] ON [{0}].OrderItemID = [b2c_OrderItem].ID  INNER JOIN [b2c_Order] ON [b2c_OrderItem].OrderID = [b2c_Order].ID", TABLE_NAME);
            if (!string.IsNullOrEmpty(where))
            {
                SQL_WHERE = string.Format(" AND {0} ", where);
            }
            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("{0}.*", TABLE_NAME), SQL_WHERE, "ORDER BY ID ASC");

            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(this.ConnectionString, SQL_SELECT, (pageIndex - 1) * prePageNum + 1, prePageNum, "ORDER BY ID ASC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        OrderItemReturnInfo orderItemReturnInfo = new OrderItemReturnInfo();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            string columnName = rdr.GetName(i);
                            orderItemReturnInfo.SetValue(columnName, rdr.GetValue(i));
                        }

                        list.Add(orderItemReturnInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public OrderItemReturnInfo GetItemReturnInfoByOrderItemID(int orderItemID)
        {
            OrderItemReturnInfo orderItemReturnInfo = null;

            string SQL_WHERE = string.Format("WHERE OrderItemID = {0}", orderItemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderItemReturnInfo = new OrderItemReturnInfo(rdr);
                }
                rdr.Close();
            }

            return orderItemReturnInfo;
        }




        public string GetSelectString(int publishmentSystemID, string status, string returnType, string auditStatus, string returnOrderStatus, string returnMoneyStatus, string returnSN, string contact, string keyword)
        {
            string whereString = string.Format(" {0} = {1}", OrderItemReturnAttribute.PublishmentSystemID, publishmentSystemID);
            whereString += string.Format(" AND ({0} = '{1}')", OrderItemReturnAttribute.Status, status);
            if (!string.IsNullOrEmpty(returnType))
            {
                whereString += string.Format(" AND ({0} = '{1}')", OrderItemReturnAttribute.Type, returnType);
            }
            if (!string.IsNullOrEmpty(auditStatus))
            {
                whereString += string.Format(" AND ({0} = '{1}')", OrderItemReturnAttribute.AuditStatus, auditStatus);
            }
            if (!string.IsNullOrEmpty(returnOrderStatus))
            {
                whereString += string.Format(" AND ({0} = '{1}')", OrderItemReturnAttribute.ReturnOrderStatus, returnOrderStatus);
            }
            if (!string.IsNullOrEmpty(returnMoneyStatus))
            {
                whereString += string.Format(" AND ({0} = '{1}')", OrderItemReturnAttribute.ReturnMoneyStatus, returnMoneyStatus);
            }
            if (!string.IsNullOrEmpty(returnSN))
            {
                whereString += string.Format(" AND ({0} = '{1}')", OrderItemReturnAttribute.ID, returnSN);
            }
            if (!string.IsNullOrEmpty(contact))
            {
                whereString += string.Format(" AND ({0} LIKE '%{1}%')", OrderItemReturnAttribute.Contact, contact);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" AND ({0} LIKE '%{1}%' OR {2} LIKE '%{1}%')", OrderItemReturnAttribute.Description, keyword, OrderItemReturnAttribute.GoodsSN);
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, "WHERE" + whereString);
        }
        public string GetSelectString(int publishmentSystemID, string status)
        {
            string whereString = string.Format(" {0} = {1}", OrderItemReturnAttribute.PublishmentSystemID, publishmentSystemID);
            whereString += string.Format(" AND ({0} = '{1}')", OrderItemReturnAttribute.Status, status);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, "WHERE" + whereString);
        }

        public void UpdateStatus(List<int> idList, OrderItemReturnStatus oirsInfo)
        {

            string updateSql = "";
            updateSql += "UPDATE [b2c_OrderItemReturn]";
            updateSql += "   SET";
            #region set
            if (oirsInfo.AuditStatus != null)
            {
                updateSql += " [AuditStatus] = '" + oirsInfo.AuditStatus.ToString() + "',";
            }
            if (oirsInfo.AuditUser != null)
            {
                updateSql += " [AuditUser] = '" + oirsInfo.AuditUser.ToString() + "',";
            }
            if (oirsInfo.AuditDate != null)
            {
                updateSql += " [AuditDate] = '" + oirsInfo.AuditDate.ToString() + "',";
            }
            if (oirsInfo.ReturnOrderStatus != null)
            {
                updateSql += " [ReturnOrderStatus] = '" + oirsInfo.ReturnOrderStatus.ToString() + "',";
            }
            if (oirsInfo.ReturnOrderUser != null)
            {
                updateSql += " [ReturnOrderUser] = '" + oirsInfo.ReturnOrderUser.ToString() + "',";
            }
            if (oirsInfo.ReturnOrderDate != null)
            {
                updateSql += " [ReturnOrderDate] = '" + oirsInfo.ReturnOrderDate.ToString() + "',";
            }
            if (oirsInfo.ReturnMoneyStatus != null)
            {
                updateSql += " [ReturnMoneyStatus] = '" + oirsInfo.ReturnMoneyStatus.ToString() + "',";
            }
            if (oirsInfo.ReturnMoneyUser != null)
            {
                updateSql += " [ReturnMoneyUser] = '" + oirsInfo.ReturnMoneyUser.ToString() + "',";
            }
            if (oirsInfo.ReturnMoneyDate != null)
            {
                updateSql += " [ReturnMoneyDate] = '" + oirsInfo.ReturnMoneyDate.ToString() + "',";
            }

            if (oirsInfo.Status != null)
            {
                updateSql += " [Status] = '" + oirsInfo.Status.ToString() + "'";
            }
            else
            {
                updateSql += " [Status] = [Status]";
            }
            #endregion
            #region where 
            string ids = "";
            if (idList.Count > 0)
            {
                foreach (var item in idList)
                {
                    ids += "," + item.ToString(); ;
                }

                if (ids.Length > 0)
                {
                    ids = ids.Substring(1);
                    updateSql += " where id in (" + ids + ")";

                    this.ExecuteNonQuery(updateSql);
                }
            }

            #endregion



        }
    }
}
