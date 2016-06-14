using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IOrderDAO
	{
        int Insert(OrderInfo orderInfo);

        void Update(OrderInfo orderInfo);

        void UpdateIsLicense(int orderID, bool isLicense);

        void UpdateIsContract(int orderID, bool isContract);

        void UpdateIsContract(List<int> orderIDList, bool isContract);

        void UpdateIsRefund(int orderID, bool isRefund);

        void UpdateIsRefund(ArrayList orderIDArrayList, bool isRefund);

        void UpdateIsInvoice(int orderID, bool isInvoice);

        void UpdateIsInvoice(List<int> orderIDList, bool isInvoice);

        void Delete(ArrayList deleteIDArrayList);

        OrderInfo GetOrderInfo(NameValueCollection form);

        OrderInfo GetOrderInfo(int orderID);

        OrderInfo GetOrderInfo(string domain);

        OrderInfo GetOrderInfoBySN(string sn);

        int GetOrderID(string domain);

        string GetOrderSN(int orderID);

        int GetCount();

        int GetMobanUsedCount(string mobanID);

        string GetSelectString(EOrderType orderType);

        string GetSelectString(EOrderType orderType, string type, string loginName, string keyword);

        string GetOrderByString();

        string GetSortFieldName();
	}
}
