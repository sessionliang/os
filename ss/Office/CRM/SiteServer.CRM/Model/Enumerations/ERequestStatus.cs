using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum ERequestStatus
	{
        Processing,         //处理中
        Closed,             //已关闭
	}

    public class ERequestStatusUtils
	{
		public static string GetValue(ERequestStatus type)
		{
            if (type == ERequestStatus.Processing)
			{
                return "Processing";
			}
            else if (type == ERequestStatus.Closed)
            {
                return "Closed";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ERequestStatus type)
		{
            if (type == ERequestStatus.Processing)
			{
                return "处理中";
			}
            else if (type == ERequestStatus.Closed)
            {
                return "已关闭";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ERequestStatus GetEnumType(string typeStr)
		{
            ERequestStatus retval = ERequestStatus.Processing;

            if (Equals(ERequestStatus.Processing, typeStr))
			{
                retval = ERequestStatus.Processing;
			}
            else if (Equals(ERequestStatus.Closed, typeStr))
            {
                retval = ERequestStatus.Closed;
            }
			return retval;
		}

		public static bool Equals(ERequestStatus type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ERequestStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ERequestStatus type, bool selected)
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
                listControl.Items.Add(GetListItem(ERequestStatus.Processing, false));
                listControl.Items.Add(GetListItem(ERequestStatus.Closed, false));
            }
        }
	}
}
