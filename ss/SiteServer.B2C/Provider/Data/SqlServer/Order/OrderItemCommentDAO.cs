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
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class OrderItemCommentDAO : DataProviderBase, IOrderItemCommentDAO
    {
        private const string TABLE_NAME = "b2c_OrderItemComment";

        public void Insert(OrderItemCommentInfo itemCommentInfo)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        IDbDataParameter[] parms = null;
                        string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemCommentInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                        //更新商品的评论数
                        OrderItemInfo orderItemInfo = DataProviderB2C.OrderItemDAO.GetItemInfo(itemCommentInfo.OrderItemID);
                        OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderItemInfo.OrderID);
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(orderInfo.PublishmentSystemID);
                        if (orderItemInfo != null && publishmentSystemInfo != null)
                        {
                            GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, orderItemInfo.ChannelID, orderItemInfo.GoodsID);
                            if (contentInfo != null)
                            {
                                contentInfo.Comments++;
                            }

                            string tableName = NodeManager.GetTableName(publishmentSystemInfo, orderItemInfo.ChannelID);
                            string SQL_UPDATE_COMMENTS = string.Format("UPDATE {0} SET Comments = {1} WHERE ID = {2}", tableName, contentInfo.Comments, contentInfo.ID);
                            this.ExecuteNonQuery(trans, SQL_UPDATE_COMMENTS);
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

        public void Insert(List<OrderItemCommentInfo> itemCommentInfoList)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (OrderItemCommentInfo itemCommentInfo in itemCommentInfoList)
                        {
                            IDbDataParameter[] parms = null;
                            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemCommentInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

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

        public void Insert(IDbTransaction trans, List<OrderItemCommentInfo> itemCommentInfoList)
        {
            if (trans != null)
            {
                foreach (OrderItemCommentInfo itemCommentInfo in itemCommentInfoList)
                {
                    IDbDataParameter[] parms = null;
                    string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemCommentInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

                    this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                }
            }
        }

        public void Delete(int itemID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, itemID);
            this.ExecuteNonQuery(sqlString);
        }

        public OrderItemCommentInfo GetItemCommentInfo(int itemID)
        {
            OrderItemCommentInfo orderItemCommentInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", itemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderItemCommentInfo = new OrderItemCommentInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        orderItemCommentInfo.SetValue(columnName, rdr.GetValue(i));
                    }
                }
                rdr.Close();
            }

            return orderItemCommentInfo;
        }

        public List<OrderItemCommentInfo> GetItemCommentInfoList(int orderItemID)
        {
            List<OrderItemCommentInfo> list = new List<OrderItemCommentInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", OrderItemCommentAttribute.OrderItemID, orderItemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID ASC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    OrderItemCommentInfo itemCommentInfo = new OrderItemCommentInfo();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        string columnName = rdr.GetName(i);
                        itemCommentInfo.SetValue(columnName, rdr.GetValue(i));
                    }

                    list.Add(itemCommentInfo);
                }
                rdr.Close();
            }

            return list;
        }


        public int GetCount(string where)
        {
            int retval = 0;
            string SQL_SELECT = string.Format("SELECT COUNT(*) FROM {0} INNER JOIN [b2c_OrderItem] ON [{0}].OrderItemID = [b2c_OrderItem].ID  INNER JOIN [b2c_Order] ON [b2c_OrderItem].OrderID = [b2c_Order].ID ", TABLE_NAME);
            if (!string.IsNullOrEmpty(where))
            {
                SQL_SELECT += string.Format(" WHERE {0} ", where);
            }
            object temp = this.ExecuteScalar(SQL_SELECT);
            retval = TranslateUtils.ToInt(temp != null ? temp.ToString() : string.Empty);
            return retval;
        }

        public List<OrderItemCommentInfo> GetItemCommentInfoList(string where, int pageIndex, int prePageNum)
        {
            List<OrderItemCommentInfo> list = new List<OrderItemCommentInfo>();
            string SQL_WHERE = " WHERE 1=1 ";
            string SQL_SELECT = string.Empty;
            string SQL_JOIN = string.Format("INNER JOIN [b2c_OrderItem] ON [{0}].OrderItemID = [b2c_OrderItem].ID  INNER JOIN [b2c_Order] ON [b2c_OrderItem].OrderID = [b2c_Order].ID", TABLE_NAME);
            if (!string.IsNullOrEmpty(where))
            {
                SQL_WHERE = string.Format(" AND {0} ", where);
            }
            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("{0}.*", TABLE_NAME), SQL_WHERE, "ORDER BY ID ASC", SQL_JOIN);

            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(this.ConnectionString, SQL_SELECT, (pageIndex - 1) * prePageNum + 1, prePageNum, "ORDER BY ID ASC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        OrderItemCommentInfo orderItemCommentInfo = new OrderItemCommentInfo();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            string columnName = rdr.GetName(i);
                            orderItemCommentInfo.SetValue(columnName, rdr.GetValue(i));
                        }

                        list.Add(orderItemCommentInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public bool GetOrderItemCommentStatistic(int contentID, out int goodCount, out int middelCount, out int badCount, out int totalCount)
        {
            goodCount = middelCount = badCount = totalCount = 0;

            string SQL_STATISTIC = @"SELECT 
(SELECT COUNT(1) FROM b2c_OrderItemComment 
WHERE OrderItemID IN
(
 SELECT ID FROM b2c_OrderItem WHERE ContentID = @ContentID
)
) as totalCount,
(SELECT COUNT(1) FROM b2c_OrderItemComment 
WHERE OrderItemID IN
(
 SELECT ID FROM b2c_OrderItem WHERE ContentID = @ContentID
)
AND Star = 5) as goodCount,
(SELECT COUNT(1) FROM b2c_OrderItemComment 
WHERE OrderItemID IN
(
 SELECT ID FROM b2c_OrderItem WHERE ContentID = @ContentID
)
AND Star > 1 AND Star < 5) as middleCount,
(SELECT COUNT(1) FROM b2c_OrderItemComment 
WHERE OrderItemID IN
(
 SELECT ID FROM b2c_OrderItem WHERE ContentID = @ContentID
)
AND Star = 1) as badCount";

            IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter("@ContentID", EDataType.Integer,contentID)
                };
            using (IDataReader rdr = this.ExecuteReader(SQL_STATISTIC, selectParms))
            {
                if (rdr.Read())
                {
                    totalCount = rdr.GetInt32(0);
                    goodCount = rdr.GetInt32(1);
                    middelCount = rdr.GetInt32(2);
                    badCount = rdr.GetInt32(3);
                }
                rdr.Close();
            }
            return true;
        }
    }
}
