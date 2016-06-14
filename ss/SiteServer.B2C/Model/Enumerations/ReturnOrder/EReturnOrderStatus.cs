using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.B2C.Model
{
    /// <summary>
    /// 退换货收货状态
    /// </summary>
    public enum EReturnOrderStatus
    {
        Received,       //处理中
        Nrcy       //已退货
    }

    public class EReturnOrderStatusUtils
    {
        public static string GetValue(EReturnOrderStatus type)
        {
            if (type == EReturnOrderStatus.Received)
            {
                return "Received";
            }
            else if (type == EReturnOrderStatus.Nrcy)
            {
                return "Nrcy";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(string strType)
        {
            return GetText(GetEnumType(strType));
        }

        public static string GetText(EReturnOrderStatus type)
        {
            if (type == EReturnOrderStatus.Received)
            {
                return "已收到";
            }
            else if (type == EReturnOrderStatus.Nrcy)
            {
                return "未收到";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EReturnOrderStatus GetEnumType(string typeStr)
        {
            EReturnOrderStatus retval = EReturnOrderStatus.Nrcy;

            if (Equals(EReturnOrderStatus.Received, typeStr))
            {
                retval = EReturnOrderStatus.Received;
            }
            else if (Equals(EReturnOrderStatus.Nrcy, typeStr))
            {
                retval = EReturnOrderStatus.Nrcy;
            }
            return retval;
        }

        public static bool Equals(EReturnOrderStatus type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EReturnOrderStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EReturnOrderStatus type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EReturnOrderStatus.Received, false));
                listControl.Items.Add(GetListItem(EReturnOrderStatus.Nrcy, false));
            }
        }
    }
}
