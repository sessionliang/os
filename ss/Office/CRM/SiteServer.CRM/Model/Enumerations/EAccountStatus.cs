using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum EAccountStatus
	{
        Contracted,             //ǩԼ�ͻ�
        Invalid,                //��Ч�ͻ�
	}

    public class EAccountStatusUtils
	{
		public static string GetValue(EAccountStatus type)
		{
            if (type == EAccountStatus.Contracted)
			{
                return "Contracted";
			}
            else if (type == EAccountStatus.Invalid)
            {
                return "Invalid";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EAccountStatus type)
		{
            if (type == EAccountStatus.Contracted)
			{
                return "ǩԼ�ͻ�";
			}
            else if (type == EAccountStatus.Invalid)
            {
                return "��Ч�ͻ�";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EAccountStatus GetEnumType(string typeStr)
		{
            EAccountStatus retval = EAccountStatus.Contracted;

            if (Equals(EAccountStatus.Contracted, typeStr))
			{
                retval = EAccountStatus.Contracted;
			}
            else if (Equals(EAccountStatus.Invalid, typeStr))
            {
                retval = EAccountStatus.Invalid;
            }
			return retval;
		}

		public static bool Equals(EAccountStatus type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EAccountStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAccountStatus type, bool selected)
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
                listControl.Items.Add(GetListItem(EAccountStatus.Contracted, false));
                listControl.Items.Add(GetListItem(EAccountStatus.Invalid, false));
            }
        }
	}
}
