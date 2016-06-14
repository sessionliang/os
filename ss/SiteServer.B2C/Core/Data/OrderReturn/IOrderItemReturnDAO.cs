using System.Collections;
using System.Collections.Generic;
using SiteServer.B2C.Model;
using System.Data;

namespace SiteServer.B2C.Core
{
    public interface IOrderItemReturnDAO
    {
        void Insert(OrderItemReturnInfo itemReturnInfo);

        void Insert(List<OrderItemReturnInfo> itemReturnInfoList);

        void Insert(IDbTransaction trans, List<OrderItemReturnInfo> itemCommnetInfoList);

        void Delete(int itemReturnID);

        OrderItemReturnInfo GetItemReturnInfo(int itemReturnID);

        List<OrderItemReturnInfo> GetItemReturnInfoList(int orderItemID);

        List<OrderItemReturnInfo> GetItemReturnInfoList(string where, int pageIndex, int prePageNum);

        int GetCount(string where);

        OrderItemReturnInfo GetItemReturnInfoByOrderItemID(int orderItemID);

        string GetSelectString(int publishmentSystemID, string status, string returnType, string auditStatus, string returnOrderStatus, string returnMoneyStatus, string returnSN, string contact, string keyword);
        string GetSelectString(int publishmentSystemID, string status);
        void UpdateStatus(List<int> idList, OrderItemReturnStatus oirsInfo);
    }
}
