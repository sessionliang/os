using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using BaiRong.Model;
using System.Data;
using BaiRong.Core;
using System.Collections;
using SiteServer.B2C.Core;
using BaiRong.Core.Data;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class OrderDAO : DataProviderBase, IOrderDAO
    {
        private const string TABLE_NAME = "b2c_Order";

        public const string PARM_USER_NAME = "@UserName";

        public int Insert(OrderInfo orderInfo)
        {
            int orderID = 0;

            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(orderInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                        orderID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);
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

        public int Insert(IDbTransaction trans, OrderInfo orderInfo)
        {
            if (trans == null)
                return 0;
            else
            {
                int orderID = 0;
                IDbDataParameter[] parms = null;
                string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(orderInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);
                this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                orderID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);
                return orderID;
            }
        }

        public void Update(OrderInfo orderInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(orderInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Payment(int orderID)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}', {3} = getdate() WHERE {4} = {5}", TABLE_NAME, OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid), OrderAttribute.TimePayment, OrderAttribute.ID, orderID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int orderID)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<OrderItemInfo> orderItemList = DataProviderB2C.OrderItemDAO.GetItemInfoList(orderID);
                        List<int> orderItemIDList = new List<int>();
                        foreach (OrderItemInfo item in orderItemList)
                        {
                            orderItemIDList.Add(item.ID);
                        }


                        if (orderItemIDList.Count > 0)
                        {
                            string sqlDeleteComment = string.Format("DELETE FROM {0} WHERE OrderItemID in ({1})", "b2c_OrderItemComment", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(orderItemIDList));
                            this.ExecuteNonQuery(trans, sqlDeleteComment);
                        }
                        string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, orderID);
                        this.ExecuteNonQuery(trans, sqlString);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }

                }
            }

        }

        public void Delete(List<int> orderIDList)
        {
            if (orderIDList != null && orderIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM b2c_Order WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(orderIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public OrderInfo GetOrderInfo(int orderID)
        {
            OrderInfo orderInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderInfo = new OrderInfo(rdr);
                }
                rdr.Close();
            }

            return orderInfo;
        }

        public OrderInfo GetOrderInfo(string orderSN)
        {
            OrderInfo orderInfo = null;

            string SQL_WHERE = string.Format("WHERE {0} = '{1}'", OrderAttribute.OrderSN, orderSN);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    orderInfo = new OrderInfo(rdr);
                }
                rdr.Close();
            }

            return orderInfo;
        }

        public OrderInfo GetLatestOrderInfo(int publishmentSystemID, string userName)
        {
            OrderInfo orderInfo = null;
            // updat by wujq
            // string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = '{3}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, userName);

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 1, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
                };


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                if (rdr.Read())
                {
                    orderInfo = new OrderInfo(rdr);
                }
                rdr.Close();
            }

            return orderInfo;
        }

        public List<OrderInfo> GetOrderInfoList(int publishmentSystemID, string userName)
        {
            List<OrderInfo> list = new List<OrderInfo>();

            // updat by wujq

            //string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = '{3}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, userName);

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
                };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                while (rdr.Read())
                {
                    OrderInfo orderInfo = new OrderInfo(rdr);

                    list.Add(orderInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<OrderInfo> GetOrderInfoList(int publishmentSystemID, string userName, bool isCompleted)
        {
            List<OrderInfo> list = new List<OrderInfo>();

            string SQL_WHERE = string.Empty;

            if (isCompleted)
            {
                SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName AND {3} = '{4}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed));
            }
            else
            {
                SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName AND {3} <> '{4}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed));
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                while (rdr.Read())
                {
                    OrderInfo orderInfo = new OrderInfo(rdr);

                    list.Add(orderInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<OrderInfo> GetOrderInfoList(int publishmentSystemID, string userName, bool isCompleted, bool isPayment)
        {
            List<OrderInfo> list = new List<OrderInfo>();

            string SQL_WHERE = string.Empty;
            if (isCompleted)
            {
                if (isPayment)
                {
                    SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName AND {3} = '{4}' AND {5} = '{6}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else
                {
                    SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName AND {3} = '{4}' AND {5} <> '{6}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
            }
            else
            {
                if (isPayment)
                {
                    SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName AND {3} <> '{4}' AND {5} = '{6}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else
                {
                    SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName AND {3} <> '{4}' AND {5} <> '{6}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
            }

            if (isPayment == false)
            {
                SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = @UserName AND {3} <> '{4}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");


            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                while (rdr.Read())
                {
                    OrderInfo orderInfo = new OrderInfo(rdr);

                    list.Add(orderInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<OrderInfo> GetOrderInfoList(int publishmentSystemID, string userName, string isCompletedStr, string isPaymentStr, int pageIndex, int prePageNum, int orderTime, string keywords)
        {
            List<OrderInfo> list = new List<OrderInfo>();

            string SQL_WHERE = string.Format(" WHERE {0} = {1} AND {2} = '{3}' ", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, userName);
            string SQL_JOIN = string.Empty;

            if (!string.IsNullOrEmpty(keywords))
            {
                SQL_JOIN += string.Format(" LEFT JOIN [b2c_OrderItem] ON [b2c_OrderItem].OrderID = [b2c_Order].ID ", TABLE_NAME);
            }

            bool isCompleted = false;
            bool isPayment = false;
            if (!string.IsNullOrEmpty(isCompletedStr) && (isCompleted = TranslateUtils.ToBool(isCompletedStr)))
            {
                if (!string.IsNullOrEmpty(isPaymentStr) && (isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} = '{1}' AND {2} = '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else if (!string.IsNullOrEmpty(isPaymentStr) && !(isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} = '{1}' AND {2} <> '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
            }
            else if (!string.IsNullOrEmpty(isCompletedStr) && !(isCompleted = TranslateUtils.ToBool(isCompletedStr)))
            {
                if (!string.IsNullOrEmpty(isPaymentStr) && (isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} <> '{1}' AND {2} = '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else if (!string.IsNullOrEmpty(isPaymentStr) && !(isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} <> '{1}' AND {2} <> '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
            }

            if (!string.IsNullOrEmpty(keywords))
            {
                SQL_WHERE += string.Format(" AND ([b2c_OrderItem].Title like '%{0}%' OR [{1}].OrderSN like '%{0}%') ", keywords, TABLE_NAME);
            }

            if (orderTime > 0)
            {
                SQL_WHERE += string.Format(" AND TimeOrder > {1} - {2} ", TABLE_NAME, SqlUtils.GetDefaultDateString(this.DataBaseType), orderTime);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("[{0}].*", TABLE_NAME), SQL_WHERE, "ORDER BY ID DESC", SQL_JOIN);
            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(this.ConnectionString, SQL_SELECT, (pageIndex - 1) * prePageNum + 1, prePageNum, string.Empty);

            //IDbDataParameter[] selectParms = new IDbDataParameter[]
            //{
            //    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            //};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        OrderInfo orderInfo = new OrderInfo(rdr);

                        list.Add(orderInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", OrderAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectString(int publishmentSystemID, string orderStatus, string paymentStatus, string shipmentStatus, string orderSN, string keyword)
        {
            string whereString = string.Format(" {0} = {1}", OrderAttribute.PublishmentSystemID, publishmentSystemID);
            if (!string.IsNullOrEmpty(orderStatus))
            {
                whereString += string.Format(" AND ({0} = '{1}')", OrderAttribute.OrderStatus, orderStatus);
            }
            if (!string.IsNullOrEmpty(paymentStatus))
            {
                whereString += string.Format(" AND ({0} = '{1}')", OrderAttribute.PaymentStatus, paymentStatus);
            }
            if (!string.IsNullOrEmpty(shipmentStatus))
            {
                whereString += string.Format(" AND ({0} = '{1}')", OrderAttribute.ShipmentStatus, shipmentStatus);
            }
            if (!string.IsNullOrEmpty(orderSN))
            {
                whereString += string.Format(" AND ({0} LIKE '%{1}%')", OrderAttribute.OrderSN, orderSN);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" AND ({0} LIKE '%{1}%')", OrderAttribute.Summary, keyword);
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, "WHERE" + whereString);
        }

        public string GetSortFieldName()
        {
            return OrderAttribute.ID;
        }

        /// <summary>
        /// 提交订单（事物）
        /// </summary>
        /// <param name="consigneeID">收货人ID</param>
        /// <param name="shipmentID">配送方式ID</param>
        /// <param name="invoiceID">发票ID</param>
        /// <param name="cartInfoList">购物车详情</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns></returns>
        public bool SubmitOrderWithTrans(PublishmentSystemInfo publishmentSystemInfo, int consigneeID, int shipmentID, int invoiceID, int paymentID, List<CartInfo> cartInfoList, out string errorMessage)
        {
            bool retval = true;
            errorMessage = string.Empty;
            using (IDbConnection conn = GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string orderSN = OrderManager.GetOrderSN();
                        string userName = RequestUtils.CurrentUserName;
                        string ipAddress = PageUtils.GetIPAddress();
                        string orderStatus = EOrderStatusUtils.GetValue(EOrderStatus.Handling);
                        string paymentStatus = EPaymentStatusUtils.GetValue(EPaymentStatus.Unpaid);
                        string shipmentStatus = EShipmentStatusUtils.GetValue(EShipmentStatus.UnShipment);
                        DateTime timeOrder = DateTime.Now;
                        DateTime timePayment = DateUtils.SqlMinValue;
                        DateTime timeShipment = DateUtils.SqlMinValue;
                        string summary = string.Empty;

                        decimal checkTotalPrice = 0;

                        AmountInfo amountInfo = PriceManager.GetAmountInfoByCarts(publishmentSystemInfo, cartInfoList);
                        OrderInfo orderInfo = new OrderInfo { PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID, OrderSN = orderSN, UserName = userName, IPAddress = ipAddress, OrderStatus = orderStatus, PaymentStatus = paymentStatus, ShipmentStatus = shipmentStatus, TimeOrder = timeOrder, TimePayment = timePayment, TimeShipment = timeShipment, ConsigneeID = consigneeID, PaymentID = paymentID, ShipmentID = shipmentID, InvoiceID = invoiceID, PriceTotal = amountInfo.PriceTotal, PriceShipment = amountInfo.PriceShipment, PriceReturn = amountInfo.PriceReturn, PriceActual = amountInfo.PriceActual, Summary = summary };

                        int orderID = this.Insert(trans, orderInfo);
                        if (orderID > 0)
                        {
                            List<OrderItemInfo> itemInfoList = new List<OrderItemInfo>();
                            List<int> cartIDList = new List<int>();
                            foreach (CartInfo cartInfo in cartInfoList)
                            {
                                cartIDList.Add(cartInfo.CartID);

                                GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, cartInfo.ChannelID, cartInfo.ContentID);
                                GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(cartInfo.GoodsID, contentInfo);
                                if (contentInfo == null)
                                    continue;
                                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID);
                                if (nodeInfo == null)
                                    continue;
                                if (contentInfo != null && goodsInfo != null)
                                {
                                    decimal priceSale = PriceManager.GetPrice(contentInfo, goodsInfo);

                                    OrderItemInfo itemInfo = new OrderItemInfo { OrderID = orderID, ChannelID = cartInfo.ChannelID, ContentID = cartInfo.ContentID, GoodsID = cartInfo.GoodsID, GoodsSN = goodsInfo.GoodsSN, Title = contentInfo.Title, ThumbUrl = contentInfo.ThumbUrl, PriceSale = priceSale, PurchaseNum = cartInfo.PurchaseNum, IsShipment = false };
                                    itemInfoList.Add(itemInfo);

                                    //商品库存和规格库存为-1时，不限制商品数量
                                    if (contentInfo.Stock != -1 && goodsInfo.Stock != -1)
                                    {
                                        if (goodsInfo.Stock < cartInfo.PurchaseNum || goodsInfo.Stock == 0)
                                            throw new Exception("对不起，库存数量不足，商品【" + contentInfo.Title + "（货号：" + goodsInfo.GoodsSN + "）】剩余库存量为【" + goodsInfo.Stock + "】");
                                        if (contentInfo.Stock < cartInfo.PurchaseNum || contentInfo.Stock == 0)
                                            throw new Exception("对不起，库存数量不足，商品【" + contentInfo.Title + "】剩余库存量为【" + contentInfo.Stock + "】");
                                    }
                                    checkTotalPrice += priceSale * cartInfo.PurchaseNum;
                                }
                            }

                            if (checkTotalPrice != orderInfo.PriceTotal)
                            {
                                retval = false;
                                errorMessage = "对不起，订单数据错误，提交订单失败！";
                                trans.Rollback();
                            }

                            DataProviderB2C.OrderItemDAO.Insert(trans, itemInfoList);

                            DataProviderB2C.CartDAO.Delete(trans, cartIDList);
                        }
                        trans.Commit();

                    }
                    catch (Exception ex)
                    {
                        retval = false;
                        errorMessage = ex.Message;
                        trans.Rollback();
                    }
                }
            }
            return retval;
        }

        /// <summary>
        /// 通过用户名获取该用户的所有订单
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="isCompleted"></param>
        /// <param name="isPayment"></param>
        /// <returns></returns>
        public int GetCountByUser(string userName, string isCompletedStr, string isPaymentStr)
        {
            int retval = 0;

            string SQL_WHERE = string.Format(" WHERE {0} = '{1}' ", OrderAttribute.UserName, userName);

            bool isCompleted = false;
            bool isPayment = false;
            if (!string.IsNullOrEmpty(isCompletedStr) && (isCompleted = TranslateUtils.ToBool(isCompletedStr)))
            {
                if (!string.IsNullOrEmpty(isPaymentStr) && (isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} = '{1}' AND {2} = '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else if (!string.IsNullOrEmpty(isPaymentStr) && !(isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} = '{1}' AND {2} <> '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else if (string.IsNullOrEmpty(isPaymentStr))
                {
                    //已关闭
                    SQL_WHERE += string.Format(" AND {0} = '{1}' ", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed));
                }
            }
            else if (!string.IsNullOrEmpty(isCompletedStr) && !(isCompleted = TranslateUtils.ToBool(isCompletedStr)))
            {
                if (!string.IsNullOrEmpty(isPaymentStr) && (isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} <> '{1}' AND {2} = '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else if (!string.IsNullOrEmpty(isPaymentStr) && !(isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} <> '{1}' AND {2} <> '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
            }

            string SQL_SELECT = string.Format("SELECT COUNT(*) FROM {0} {1}", TABLE_NAME, SQL_WHERE);


            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            };

            object temp = this.ExecuteScalar(SQL_SELECT, selectParms);
            retval = TranslateUtils.ToInt(temp != null ? temp.ToString() : string.Empty);

            return retval;
        }


        /// <summary>
        /// 获取用户的最新订单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public OrderInfo GetLatestOrderInfo(string userName)
        {
            OrderInfo orderInfo = null;
            // updat by wujq
            // string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = '{3}'", OrderAttribute.PublishmentSystemID, publishmentSystemID, OrderAttribute.UserName, userName);

            string SQL_WHERE = string.Format("WHERE {0} = @UserName", OrderAttribute.UserName);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 1, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
                };


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                if (rdr.Read())
                {
                    orderInfo = new OrderInfo(rdr);
                }
                rdr.Close();
            }

            return orderInfo;
        }


        /// <summary>
        /// 统计订单信息
        /// </summary>
        /// <param name="noPay"></param>
        /// <param name="noCompleted"></param>
        /// <param name="noComment"></param>
        /// <returns></returns>
        public bool GetOrderStatistic(string userName, out int noPay, out int noCompleted, out int comment, out int total, out int commentTotal)
        {
            total = noPay = noCompleted = comment = commentTotal = 0;
            bool isSuccess = false;

            string SQL_STATISTIC = @"
SELECT
(SELECT COUNT(*) FROM ( SELECT b2c_Order.OrderSN as os FROM b2c_Order
INNER JOIN b2c_OrderItem ON b2c_Order.ID = b2c_OrderItem.OrderID
INNER JOIN b2c_OrderItemComment ON b2c_OrderItem.ID = b2c_OrderItemComment.OrderItemID
WHERE UserName = @UserName AND OrderStatus = 'Completed'
GROUP BY b2c_Order.OrderSN) tmp ) as noComment, --已评价

(SELECT COUNT(1) as commentTotal FROM b2c_Order WHERE UserName = @UserName AND OrderStatus = 'Completed') as commentTotal, --已/未评价

(SELECT COUNT(1) as noPay FROM b2c_Order WHERE UserName = @UserName AND OrderStatus = 'Handling' AND PaymentStatus = 'Unpaid') as noPay, --待付款

(SELECT COUNT(1) as noCompleted FROM b2c_Order WHERE UserName = @UserName AND OrderStatus = 'Handling' AND PaymentStatus = 'Paid' AND ShipmentStatus = 'UnShipment') as noCompleted, --待发货

(SELECT COUNT(1) as total FROM b2c_Order WHERE UserName = @UserName) as  total";

            IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255,userName)
                };
            using (IDataReader rdr = this.ExecuteReader(SQL_STATISTIC, selectParms))
            {
                if (rdr.Read())
                {
                    comment = rdr.GetInt32(0);
                    commentTotal = rdr.GetInt32(1);
                    noPay = rdr.GetInt32(2);
                    noCompleted = rdr.GetInt32(3);
                    total = rdr.GetInt32(4);
                }
                rdr.Close();
            }

            return isSuccess;
        }

        /// <summary>
        /// 获取用户所有站的订单
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="isCompleted"></param>
        /// <param name="isPayment"></param>
        /// <param name="pageIndex"></param>
        /// <param name="prePageNum"></param>
        /// <param name="orderTime"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public List<OrderInfo> GetOrderInfoList(string userName, string isCompletedStr, string isPaymentStr, int pageIndex, int prePageNum, int orderTime, string keywords)
        {
            List<OrderInfo> list = new List<OrderInfo>();

            string SQL_WHERE = string.Format(" WHERE {0} = '{1}' ", OrderAttribute.UserName, userName);
            string SQL_JOIN = string.Empty;

            if (!string.IsNullOrEmpty(keywords))
            {
                SQL_JOIN += string.Format(" LEFT JOIN [b2c_OrderItem] ON [b2c_OrderItem].OrderID = [b2c_Order].ID ", TABLE_NAME);
            }

            bool isCompleted = false;
            bool isPayment = false;
            if (!string.IsNullOrEmpty(isCompletedStr) && (isCompleted = TranslateUtils.ToBool(isCompletedStr)))
            {
                if (!string.IsNullOrEmpty(isPaymentStr) && (isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} = '{1}' AND {2} = '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else if (!string.IsNullOrEmpty(isPaymentStr) && !(isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} = '{1}' AND {2} <> '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else if (string.IsNullOrEmpty(isPaymentStr))
                {
                    //已关闭
                    SQL_WHERE += string.Format(" AND {0} = '{1}' ", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed));
                }
            }
            else if (!string.IsNullOrEmpty(isCompletedStr) && !(isCompleted = TranslateUtils.ToBool(isCompletedStr)))
            {
                if (!string.IsNullOrEmpty(isPaymentStr) && (isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} <> '{1}' AND {2} = '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
                else if (!string.IsNullOrEmpty(isPaymentStr) && !(isPayment = TranslateUtils.ToBool(isPaymentStr)))
                {
                    SQL_WHERE += string.Format(" AND {0} <> '{1}' AND {2} <> '{3}'", OrderAttribute.OrderStatus, EOrderStatusUtils.GetValue(EOrderStatus.Completed), OrderAttribute.PaymentStatus, EPaymentStatusUtils.GetValue(EPaymentStatus.Paid));
                }
            }

            if (!string.IsNullOrEmpty(keywords))
            {
                SQL_WHERE += string.Format(" AND ([b2c_OrderItem].Title like '%{0}%' OR [{1}].OrderSN like '%{0}%') ", keywords, TABLE_NAME);
            }

            if (orderTime > 0)
            {
                SQL_WHERE += string.Format(" AND TimeOrder > {1} - {2} ", TABLE_NAME, SqlUtils.GetDefaultDateString(this.DataBaseType), orderTime);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, string.Format("[{0}].*", TABLE_NAME), SQL_WHERE, "ORDER BY ID DESC", SQL_JOIN);
            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlStringByQueryString(this.ConnectionString, SQL_SELECT, (pageIndex - 1) * prePageNum + 1, prePageNum, string.Empty);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        OrderInfo orderInfo = new OrderInfo(rdr);

                        list.Add(orderInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }
    }
}
