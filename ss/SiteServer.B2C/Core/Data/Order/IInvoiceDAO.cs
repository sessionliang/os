using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.B2C.Model;
using System.Collections;

namespace SiteServer.B2C.Core
{
    public interface IInvoiceDAO
    {
        int Insert(InvoiceInfo invoiceInfo);

        void Update(InvoiceInfo invoiceInfo);

        void SetDefault(int invoiceID);

        void Delete(int invoiceID);

        InvoiceInfo GetInvoiceInfo(int invoiceID);

        List<InvoiceInfo> GetInvoiceInfoList(string groupSN, string userName);
     }
}
