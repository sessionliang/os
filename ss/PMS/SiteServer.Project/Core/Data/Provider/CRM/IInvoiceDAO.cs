using SiteServer.Project.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace SiteServer.Project.Core
{
	public interface IInvoiceDAO
	{
        int Insert(InvoiceInfo invoiceInfo);

        void Update(InvoiceInfo invoiceInfo);

        void Delete(ArrayList deleteIDArrayList);

        InvoiceInfo GetInvoiceInfo(NameValueCollection form);

        InvoiceInfo GetInvoiceInfo(int invoiceID);

        InvoiceInfo GetInvoiceInfoByOrderID(int orderID);

        ArrayList GetOrderIDArrayList(ArrayList idArrayList);

        int GetCount();

        string GetSelectString();

        string GetSelectString(string keyword);

        string GetInvoiceByString();

        string GetSortFieldName();
	}
}
