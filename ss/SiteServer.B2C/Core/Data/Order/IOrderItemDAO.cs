using System.Collections;
using System.Collections.Generic;
using SiteServer.B2C.Model;
using System.Data;

namespace SiteServer.B2C.Core
{
    public interface IOrderItemDAO
    {
        void Insert(List<OrderItemInfo> itemInfoList);

        void Insert(IDbTransaction trans, List<OrderItemInfo> itemInfoList);

        void Delete(int itemID);

        OrderItemInfo GetItemInfo(int itemID);

        List<OrderItemInfo> GetItemInfoList(int orderID);

        List<OrderItemInfo> GetItemInfoList(int orderID, int pageIndex, int prePageNum);

        List<OrderItemInfo> GetItemInfoList(string userName, int orderID);

        List<OrderItemInfo> GetItemInfoList(string userName, int orderID, int pageIndex, int prePageNum);

        int GetCountByUser(string userName);

        int GetCountByUser(string userName, int orderID, string status);
    }
}
