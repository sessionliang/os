using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IOrderRefundDAO
	{
        int Insert(OrderRefundInfo refundInfo);

        void Update(OrderRefundInfo refundInfo);

        void Delete(ArrayList deleteIDArrayList);

        OrderRefundInfo GetOrderRefundInfo(int orderID, NameValueCollection form);

        OrderRefundInfo GetOrderRefundInfo(int orderID);

        OrderRefundInfo GetOrderRefundInfoByOrderID(int orderID);

        ArrayList GetOrderIDArrayList(ArrayList idArrayList);

        int GetCount();

        string GetSelectString();

        string GetSelectString(string keyword);

        string GetOrderRefundByString();

        string GetSortFieldName();
	}
}
