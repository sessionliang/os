using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
    public enum EPaymentStatus
    {
        Unpaid,  //未支付
        Paid, //已支付
        Refund  //全额退款
    }
    public class EPaymentStatusUtils
    {
        public static string GetValue(EPaymentStatus type)
        {
            if (type == EPaymentStatus.Unpaid)
            {
                return "Unpaid";
            }
            else if (type == EPaymentStatus.Paid)
            {
                return "Paid";
            }
            else if (type == EPaymentStatus.Refund)
            {
                return "Refund";
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

        public static string GetText(EPaymentStatus type)
        {
            if (type == EPaymentStatus.Unpaid)
            {
                return "未支付";
            }
            else if (type == EPaymentStatus.Paid)
            {
                return "已支付";
            }
            else if (type == EPaymentStatus.Refund)
            {
                return "全额退款";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EPaymentStatus GetEnumType(string typeStr)
        {
            EPaymentStatus retval = EPaymentStatus.Unpaid;

            if (Equals(EPaymentStatus.Unpaid, typeStr))
            {
                retval = EPaymentStatus.Unpaid;
            }
            else if (Equals(EPaymentStatus.Paid, typeStr))
            {
                retval = EPaymentStatus.Paid;
            }
            else if (Equals(EPaymentStatus.Refund, typeStr))
            {
                retval = EPaymentStatus.Refund;
            }
            return retval;
        }

        public static bool Equals(EPaymentStatus type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EPaymentStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EPaymentStatus type, bool selected)
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
                listControl.Items.Add(GetListItem(EPaymentStatus.Unpaid, false));
                listControl.Items.Add(GetListItem(EPaymentStatus.Paid, false));
                listControl.Items.Add(GetListItem(EPaymentStatus.Refund, false));
            }
        }
    }
}
