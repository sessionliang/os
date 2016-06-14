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
    public class OrderItemDAO : DataProviderBase, IOrderItemDAO
    {
        private const string TABLE_NAME = "b2c_OrderItem";

        public void Insert(List<OrderItemInfo> itemInfoList)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (OrderItemInfo itemInfo in itemInfoList)
                        {
                            IDbDataParameter[] parms = null;
                            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

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

        public void Insert(IDbTransaction trans, List<OrderItemInfo> itemInfoList)
        {
            if (trans != null)
            {
                foreach (OrderItemInfo itemInfo in itemInfoList)
                {
                    IDbDataParameter[] parms = null;
                    string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

                    this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                }
            }
        }

        public void Delete(int itemID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, itemID);
            this.ExecuteNonQuery(sqlString);
        }

        public OrderItemInfo GetItemInfo(int itemID)
        {
            OrderItemInfo orderItemInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", itemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderItemInfo = new OrderItemInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        orderItemInfo.SetValue(columnName, rdr.GetValue(i));
                    }
                }
                rdr.Close();
            }

            return orderItemInfo;
        }


        public List<OrderItemInfo> GetItemInfoList(int orderID)
        {
            List<OrderItemInfo> list = new List<OrderItemInfo>();
            string SQL_WHERE = string.Empty;
            string SQL_SELECT = string.Empty;
            if (orderID > 0)
            {
                SQL_WHERE = string.Format("WHERE {0} = {1}", OrderItemAttribute.OrderID, orderID);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("{0}.*", TABLE_NAME), SQL_WHERE, "ORDER BY ID ASC");
            }
            else
            {
                return new List<OrderItemInfo>();
            }

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    OrderItemInfo itemInfo = new OrderItemInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        itemInfo.SetValue(columnName, rdr.GetValue(i));
                    }

                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<OrderItemInfo> GetItemInfoList(int orderID, int pageIndex, int prePageNum)
        {
            List<OrderItemInfo> list = new List<OrderItemInfo>();
            string SQL_WHERE = string.Empty;
            string SQL_SELECT = string.Empty;
            string SQL_JOIN = string.Format("RIGHT JOIN [b2c_Order] ON [{0}].OrderID = [b2c_Order].ID", TABLE_NAME);
            if (orderID > 0)
            {
                SQL_WHERE = string.Format("WHERE {0} = {1}", OrderItemAttribute.OrderID, orderID);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("{0}.*", TABLE_NAME), SQL_WHERE, "ORDER BY ID ASC", SQL_JOIN);
            }
            else
            {
                //SQL_WHERE = string.Format("WHERE [b2c_Order].OrderStatus = 'completed'", OrderItemAttribute.OrderID, orderID);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("{0}.*", TABLE_NAME), SQL_WHERE, "ORDER BY ID ASC", SQL_JOIN);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(this.ConnectionString, SQL_SELECT, (pageIndex - 1) * prePageNum + 1, prePageNum, "ORDER BY ID ASC");
            }

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    OrderItemInfo itemInfo = new OrderItemInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        itemInfo.SetValue(columnName, rdr.GetValue(i));
                    }

                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<OrderItemInfo> GetItemInfoList(string userName, int orderID)
        {
            List<OrderItemInfo> list = new List<OrderItemInfo>();
            string SQL_WHERE = string.Empty;
            string SQL_SELECT = string.Empty;
            string SQL_JOIN = string.Format("RIGHT JOIN [b2c_Order] ON [{0}].OrderID = [b2c_Order].ID", TABLE_NAME);
            if (orderID > 0)
            {
                SQL_WHERE = string.Format("WHERE {0} = {1} AND [b2c_Order].{2} = '{3}' ", OrderItemAttribute.OrderID, orderID, OrderAttribute.UserName, userName);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("{0}.*", TABLE_NAME), SQL_WHERE, "ORDER BY ID ASC", SQL_JOIN);
            }
            else
            {
                return new List<OrderItemInfo>();
            }

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    OrderItemInfo itemInfo = new OrderItemInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        itemInfo.SetValue(columnName, rdr.GetValue(i));
                    }

                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<OrderItemInfo> GetItemInfoList(string userName, int orderID, int pageIndex, int prePageNum)
        {
            List<OrderItemInfo> list = new List<OrderItemInfo>();
            string SQL_WHERE = string.Empty;
            string SQL_SELECT = string.Empty;
            string SQL_JOIN = string.Format("RIGHT JOIN [b2c_Order] ON [{0}].OrderID = [b2c_Order].ID", TABLE_NAME);
            if (orderID > 0)
            {
                SQL_WHERE = string.Format("WHERE {0} = {1} AND [b2c_Order].{2} = '{3}' ", OrderItemAttribute.OrderID, orderID, OrderAttribute.UserName, userName);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("{0}.*", TABLE_NAME), SQL_WHERE, "ORDER BY ID ASC", SQL_JOIN);
            }
            else
            {
                //SQL_WHERE = string.Format("WHERE [b2c_Order].OrderStatus = 'completed'", OrderItemAttribute.OrderID, orderID);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("{0}.*", TABLE_NAME), SQL_WHERE, "ORDER BY ID ASC", SQL_JOIN);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(this.ConnectionString, SQL_SELECT, (pageIndex - 1) * prePageNum + 1, prePageNum, "ORDER BY ID ASC");
            }

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    OrderItemInfo itemInfo = new OrderItemInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        itemInfo.SetValue(columnName, rdr.GetValue(i));
                    }

                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public int GetCountByUser(string userName)
        {
            int retval = 0;
            string SQL_SELECT = string.Format("SELECT COUNT(*) FROM {0} RIGHT JOIN [b2c_Order] ON [{0}].OrderID = [b2c_Order].ID WHERE [b2c_Order].UserName = '{1}' AND [b2c_Order].OrderStatus = 'completed'", TABLE_NAME, PageUtils.FilterSql(userName));
            object temp = this.ExecuteScalar(SQL_SELECT);
            retval = TranslateUtils.ToInt(temp != null ? temp.ToString() : string.Empty);
            return retval;
        }

        public int GetCountByUser(string userName, int orderID, string status)
        {
            int retval = 0;
            string SQL_SELECT = string.Empty;
            if (orderID > 0)
            {
                SQL_SELECT = string.Format("SELECT COUNT(*) FROM {0} RIGHT JOIN [b2c_Order] ON [{0}].OrderID = [b2c_Order].ID WHERE [b2c_Order].UserName = '{1}' AND [b2c_Order].ID = {2}", TABLE_NAME, userName, orderID);
            }
            else
            {
                SQL_SELECT = string.Format("SELECT COUNT(*) FROM {0} RIGHT JOIN [b2c_Order] ON [{0}].OrderID = [b2c_Order].ID", TABLE_NAME, orderID);
            }
            if (!string.IsNullOrEmpty(status))
            {
                SQL_SELECT += string.Format(" [b2c_Order].OrderStatus = '{0}' ", status);
            }

            object temp = this.ExecuteScalar(SQL_SELECT);
            retval = TranslateUtils.ToInt(temp != null ? temp.ToString() : string.Empty);
            return retval;
        }
    }
}
