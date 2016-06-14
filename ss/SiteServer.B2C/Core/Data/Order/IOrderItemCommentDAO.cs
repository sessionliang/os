using System.Collections;
using System.Collections.Generic;
using SiteServer.B2C.Model;
using System.Data;

namespace SiteServer.B2C.Core
{
    public interface IOrderItemCommentDAO
    {
        void Insert(OrderItemCommentInfo itemCommentInfo);

        void Insert(List<OrderItemCommentInfo> itemCommentInfoList);

        void Insert(IDbTransaction trans, List<OrderItemCommentInfo> itemCommnetInfoList);

        void Delete(int itemCommentID);

        OrderItemCommentInfo GetItemCommentInfo(int itemCommentID);

        List<OrderItemCommentInfo> GetItemCommentInfoList(int orderItemID);
        List<OrderItemCommentInfo> GetItemCommentInfoList(string where, int pageIndex, int prePageNum);
        int GetCount(string where);

        bool GetOrderItemCommentStatistic(int contentID, out int goodCount, out int middelCount, out int badCount, out int totalCount);
    }
}
