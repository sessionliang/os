using SiteServer.Project.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;

namespace SiteServer.Project.Core
{
    public interface IOrderFormDAO
    {
        int Insert(OrderFormInfo info);

        void Update(OrderFormInfo info);

        void UpdateIsCompleted(int contentID);

        void DeleteByOrderID(int orderID);

        OrderFormInfo GetOrderFormInfo(NameValueCollection form);

        OrderFormInfo GetOrderFormInfoByOrderID(int orderID);

        OrderFormInfo GetOrderFormInfo(int id);

        int GetOrderFormID(int orderID);

        bool IsCompleted(int orderID, out int orderFormID);
    }
}
