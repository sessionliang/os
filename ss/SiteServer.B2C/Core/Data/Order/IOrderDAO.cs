using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.B2C.Model;
using System.Collections;
using System.Data;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Core
{
    public interface IOrderDAO
    {
        int Insert(OrderInfo orderInfo);

        int Insert(IDbTransaction trans, OrderInfo orderInfo);

        void Update(OrderInfo orderInfo);

        void Payment(int orderID);

        void Delete(int orderID);

        void Delete(List<int> orderIDList);

        OrderInfo GetOrderInfo(int orderID);

        OrderInfo GetOrderInfo(string orderSN);

        OrderInfo GetLatestOrderInfo(int publishmentSystemID, string userName);

        List<OrderInfo> GetOrderInfoList(int publishmentSystemID, string userName);

        List<OrderInfo> GetOrderInfoList(int publishmentSystemID, string userName, bool isCompleted);

        List<OrderInfo> GetOrderInfoList(int publishmentSystemID, string userName, bool isCompleted, bool isPayment);

        List<OrderInfo> GetOrderInfoList(int publishmentSystemID, string userName, string isCompleted, string isPayment, int pageIndex, int prePageNum, int orderTime, string keywords);

        string GetSelectString(int publishmentSystemID);

        string GetSelectString(int publishmentSystemID, string orderStatus, string paymentStatus, string shipmentStatus, string orderSN, string keyword);

        string GetSortFieldName();

        /// <summary>
        /// 提交订单（事物）
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool SubmitOrderWithTrans(PublishmentSystemInfo publishmentSystemInfo, int consigneeID, int shipmentID, int invoiceID, int paymentID, List<CartInfo> cartInfoList, out string errorMessage);

        /// <summary>
        /// 获得该用户的订单总数
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        int GetCountByUser(string userName, string isCompleted, string isPayment);

        /// <summary>
        /// 获取用户的最新订单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        OrderInfo GetLatestOrderInfo(string userName);

        /// <summary>
        /// 统计订单信息
        /// </summary>
        /// <param name="noPay"></param>
        /// <param name="noCompleted"></param>
        /// <param name="noComment"></param>
        /// <returns></returns>
        bool GetOrderStatistic(string userName, out int noPay, out int noCompleted, out int comment, out int total, out int commentTotal);

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
        List<OrderInfo> GetOrderInfoList(string userName, string isCompleted, string isPayment, int pageIndex, int prePageNum, int orderTime, string keywords);
    }
}
