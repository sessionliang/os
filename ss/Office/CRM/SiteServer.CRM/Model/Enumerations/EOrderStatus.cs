using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum EOrderStatus
	{
        New,                //新订单
        Messaged,           //已通知
        Formed,             //表单已确认
        Opened,             //已开通
        Completed           //已结项
	}

    public class EOrderStatusUtils
	{
		public static string GetValue(EOrderStatus type)
		{
            if (type == EOrderStatus.New)
			{
                return "New";
			}
            else if (type == EOrderStatus.Messaged)
            {
                return "Messaged";
            }
            else if (type == EOrderStatus.Formed)
            {
                return "Formed";
            }
            else if (type == EOrderStatus.Opened)
            {
                return "Opened";
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

		public static string GetText(EOrderStatus type)
		{
            if (type == EOrderStatus.New)
			{
                return "新订单";
			}
            else if (type == EOrderStatus.Messaged)
            {
                return "已通知";
            }
            else if (type == EOrderStatus.Formed)
            {
                return "表单已确认";
            }
            else if (type == EOrderStatus.Opened)
            {
                return "已开通";
            }
            else if (type == EOrderStatus.Completed)
            {
                return "已结项";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EOrderStatus GetEnumType(string typeStr)
		{
            EOrderStatus retval = EOrderStatus.New;

            if (Equals(EOrderStatus.New, typeStr))
			{
                retval = EOrderStatus.New;
			}
            else if (Equals(EOrderStatus.Messaged, typeStr))
            {
                retval = EOrderStatus.Messaged;
            }
            else if (Equals(EOrderStatus.Formed, typeStr))
            {
                retval = EOrderStatus.Formed;
            }
            else if (Equals(EOrderStatus.Opened, typeStr))
            {
                retval = EOrderStatus.Opened;
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

        public static void AddListItems(EOrderType orderType, ListControl listControl)
        {
            if (listControl != null)
            {
                if (orderType == EOrderType.Aliyun_Moban)
                {
                    listControl.Items.Add(GetListItem(EOrderStatus.New, false));
                    listControl.Items.Add(GetListItem(EOrderStatus.Messaged, false));
                    listControl.Items.Add(GetListItem(EOrderStatus.Formed, false));
                    listControl.Items.Add(GetListItem(EOrderStatus.Opened, false));
                    listControl.Items.Add(GetListItem(EOrderStatus.Completed, false));
                }
                else if (orderType == EOrderType.Taobao_Service)
                {
                    listControl.Items.Add(GetListItem(EOrderStatus.New, false));
                    listControl.Items.Add(GetListItem(EOrderStatus.Opened, false));
                    listControl.Items.Add(GetListItem(EOrderStatus.Completed, false));
                }
                else if (orderType == EOrderType.Aliyun_Software)
                {
                    listControl.Items.Add(GetListItem(EOrderStatus.New, false));
                    listControl.Items.Add(GetListItem(EOrderStatus.Messaged, false));
                    listControl.Items.Add(GetListItem(EOrderStatus.Completed, false));
                }               
            }
        }

        public static string GetClass(EOrderStatus status)
        {
            string clazz = string.Empty;

            if (status == EOrderStatus.New)
            {
                clazz = "btn-danger";
            }
            else if (status == EOrderStatus.Messaged)
            {
                clazz = "btn-warning";
            }
            else if (status == EOrderStatus.Formed)
            {
                clazz = "btn-danger";
            }
            else if (status == EOrderStatus.Opened)
            {
                clazz = "btn-info";
            }
            else if (status == EOrderStatus.Completed)
            {
                clazz = "btn-success";
            }
            return clazz;
        }
	}
}
