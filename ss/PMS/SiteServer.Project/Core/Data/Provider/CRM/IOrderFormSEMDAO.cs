using SiteServer.Project.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;

namespace SiteServer.Project.Core
{
    public interface IOrderFormSEMDAO
    {
        int Insert(OrderFormSEMInfo info);

        void Update(OrderFormSEMInfo info);

        void DeleteByOrderID(int orderID);

        OrderFormSEMInfo GetOrderFormSEMInfo(NameValueCollection form);

        OrderFormSEMInfo GetOrderFormSEMInfoByOrderID(int orderID);

        OrderFormSEMInfo GetOrderFormSEMInfo(int id);

        int GetOrderFormSEMID(int orderID);
    }
}
