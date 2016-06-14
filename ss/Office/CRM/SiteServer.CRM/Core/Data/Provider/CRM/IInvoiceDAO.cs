using SiteServer.CRM.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.CRM.Core
{
	public interface IInvoiceDAO
	{
        int Insert(InvoiceInfo invoiceInfo);

        void Update(InvoiceInfo invoiceInfo);

        void Delete(List<int> deleteIDList);

        InvoiceInfo GetInvoiceInfo(int invoiceID);

        InvoiceInfo GetInvoiceInfoByOrderID(int orderID);

        List<int> GetOrderIDList(List<int> idList);

        int GetCount();

        string GetSelectString();

        string GetSelectString(string keyword);

        string GetInvoiceByString();

        string GetSortFieldName();
	}
}
