using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using System.Web.UI.WebControls;

namespace SiteServer.B2C.Model
{
    public enum EOrderStatus
    {
        Handling,       //处理中
        Refunded,       //已退货
        Canceled,       //已作废
        Completed,      //已完成
    }

    public class EOrderStatusUtils
    {
        public static string GetValue(EOrderStatus type)
		{
            if (type == EOrderStatus.Handling)
			{
                return "Handling";
			}
            else if (type == EOrderStatus.Refunded)
			{
                return "Refunded";
            }
            else if (type == EOrderStatus.Canceled)
            {
                return "Canceled";
            }
            else if (type == EOrderStatus.Completed)
            {
                return "Completed";
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

        public static string GetText(EOrderStatus type)
        {
            if (type == EOrderStatus.Handling)
            {
                return "处理中";
            }
            else if (type == EOrderStatus.Refunded)
            {
                return "已退货";
            }
            else if (type == EOrderStatus.Canceled)
            {
                return "已作废";
            }
            else if (type == EOrderStatus.Completed)
            {
                return "已完成";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EOrderStatus GetEnumType(string typeStr)
        {
            EOrderStatus retval = EOrderStatus.Handling;

            if (Equals(EOrderStatus.Handling, typeStr))
            {
                retval = EOrderStatus.Handling;
            }
            else if (Equals(EOrderStatus.Refunded, typeStr))
            {
                retval = EOrderStatus.Refunded;
            }
            else if (Equals(EOrderStatus.Canceled, typeStr))
            {
                retval = EOrderStatus.Canceled;
            }
            else if (Equals(EOrderStatus.Completed, typeStr))
            {
                retval = EOrderStatus.Completed;
            }
            return retval;
        }

        public static bool Equals(EOrderStatus type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EOrderStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EOrderStatus type, bool selected)
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
                listControl.Items.Add(GetListItem(EOrderStatus.Handling, false));
                listControl.Items.Add(GetListItem(EOrderStatus.Refunded, false));
                listControl.Items.Add(GetListItem(EOrderStatus.Canceled, false));
                listControl.Items.Add(GetListItem(EOrderStatus.Completed, false));
            }
        }
    }

    

}
