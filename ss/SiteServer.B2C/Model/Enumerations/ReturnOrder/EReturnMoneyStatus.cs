using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.B2C.Model
{
    /// <summary>
    /// 退换货申请状态
    /// </summary>
    public enum EReturnMoneyStatus
    {
        Refund,       //处理中
        UnRefund       //已退货
    }

    public class EReturnMoneyStatusUtils
    {
        public static string GetValue(EReturnMoneyStatus type)
        {
            if (type == EReturnMoneyStatus.Refund)
            {
                return "Refund";
            }
            else if (type == EReturnMoneyStatus.UnRefund)
            {
                return "UnRefund";
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

        public static string GetText(EReturnMoneyStatus type)
        {
            if (type == EReturnMoneyStatus.Refund)
            {
                return "已退款";
            }
            else if (type == EReturnMoneyStatus.UnRefund)
            {
                return "未退款";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EReturnMoneyStatus GetEnumType(string typeStr)
        {
            EReturnMoneyStatus retval = EReturnMoneyStatus.UnRefund;

            if (Equals(EReturnMoneyStatus.Refund, typeStr))
            {
                retval = EReturnMoneyStatus.Refund;
            }
            else if (Equals(EReturnMoneyStatus.UnRefund, typeStr))
            {
                retval = EReturnMoneyStatus.UnRefund;
            }
            return retval;
        }

        public static bool Equals(EReturnMoneyStatus type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EReturnMoneyStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EReturnMoneyStatus type, bool selected)
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
                listControl.Items.Add(GetListItem(EReturnMoneyStatus.Refund, false));
                listControl.Items.Add(GetListItem(EReturnMoneyStatus.UnRefund, false));
            }
        }
    }
}
