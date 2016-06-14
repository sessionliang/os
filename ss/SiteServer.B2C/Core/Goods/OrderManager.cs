using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;

using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using System.Text;
using System;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Core
{
	public class OrderManager
	{
        public static string GetOrderSN()
        {
            return StringUtils.GetShortGUID(true);
        }

        public static string GetInvoiceSN()
        {
            return StringUtils.GetShortGUID(true);
        }

        public static string GetLocation(ConsigneeInfo consigneeInfo)
        {
            string retval = consigneeInfo.Province;
            if (!string.IsNullOrEmpty(consigneeInfo.City))
            {
                retval += "-" + consigneeInfo.City;
            }
            if (!string.IsNullOrEmpty(consigneeInfo.Area))
            {
                retval += "-" + consigneeInfo.Area;
            }
            return retval;
        }

        public static string GetOrderStatus(OrderInfo orderInfo)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(EOrderStatusUtils.GetText(orderInfo.OrderStatus)).Append(",");
            builder.Append(EPaymentStatusUtils.GetText(orderInfo.PaymentStatus)).Append(",");
            builder.Append(EShipmentStatusUtils.GetText(orderInfo.ShipmentStatus));

            return builder.ToString();
        }

        public static string GetOrderUser(string groupSN, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return "匿名用户";
            }
            else
            {
                int userID = BaiRongDataProvider.UserDAO.GetUserID(groupSN, userName);
                return UserManager.GetAccount(userID, true, true);
            }
        }
	}
}
