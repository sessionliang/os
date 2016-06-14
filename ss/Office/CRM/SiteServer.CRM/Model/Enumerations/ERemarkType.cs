using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum ERemarkType
	{
        Accept,             //����
        SwitchTo,           //ת��
        Comment,            //��ע
        Redo                //Ҫ�󷵹�
	}

    public class ERemarkTypeUtils
	{
		public static string GetValue(ERemarkType type)
		{
            if (type == ERemarkType.Accept)
			{
                return "Accept";
			}
            else if (type == ERemarkType.SwitchTo)
            {
                return "SwitchTo";
            }
            else if (type == ERemarkType.Comment)
            {
                return "Comment";
            }
            else if (type == ERemarkType.Redo)
            {
                return "Redo";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ERemarkType type)
		{
            if (type == ERemarkType.Accept)
			{
                return "����";
			}
            else if (type == ERemarkType.SwitchTo)
            {
                return "ת��";
            }
            else if (type == ERemarkType.Comment)
            {
                return "��ע";
            }
            else if (type == ERemarkType.Redo)
            {
                return "Ҫ�󷵹�";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ERemarkType GetEnumType(string typeStr)
		{
            ERemarkType retval = ERemarkType.Accept;

            if (Equals(ERemarkType.Accept, typeStr))
			{
                retval = ERemarkType.Accept;
			}
            else if (Equals(ERemarkType.SwitchTo, typeStr))
            {
                retval = ERemarkType.SwitchTo;
            }
            else if (Equals(ERemarkType.Comment, typeStr))
            {
                retval = ERemarkType.Comment;
            }
            else if (Equals(ERemarkType.Redo, typeStr))
            {
                retval = ERemarkType.Redo;
            }
			return retval;
		}

		public static bool Equals(ERemarkType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ERemarkType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ERemarkType type, bool selected)
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
                listControl.Items.Add(GetListItem(ERemarkType.Accept, false));
                listControl.Items.Add(GetListItem(ERemarkType.SwitchTo, false));
                listControl.Items.Add(GetListItem(ERemarkType.Comment, false));
                listControl.Items.Add(GetListItem(ERemarkType.Redo, false));
            }
        }
	}
}
