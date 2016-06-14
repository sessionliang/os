using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EGovInteractRemarkType
	{
        Accept,             //����
        SwitchTo,           //ת��
        Translate,          //ת��
        Comment,            //��ʾ
        Redo                //Ҫ�󷵹�
	}

    public class EGovInteractRemarkTypeUtils
	{
		public static string GetValue(EGovInteractRemarkType type)
		{
            if (type == EGovInteractRemarkType.Accept)
			{
                return "Accept";
			}
            else if (type == EGovInteractRemarkType.SwitchTo)
            {
                return "SwitchTo";
            }
            else if (type == EGovInteractRemarkType.Translate)
            {
                return "Translate";
            }
            else if (type == EGovInteractRemarkType.Comment)
            {
                return "Comment";
            }
            else if (type == EGovInteractRemarkType.Redo)
            {
                return "Redo";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EGovInteractRemarkType type)
		{
            if (type == EGovInteractRemarkType.Accept)
			{
                return "����";
			}
            else if (type == EGovInteractRemarkType.SwitchTo)
            {
                return "ת��";
            }
            else if (type == EGovInteractRemarkType.Translate)
            {
                return "ת��";
            }
            else if (type == EGovInteractRemarkType.Comment)
            {
                return "��ʾ";
            }
            else if (type == EGovInteractRemarkType.Redo)
            {
                return "Ҫ�󷵹�";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EGovInteractRemarkType GetEnumType(string typeStr)
		{
            EGovInteractRemarkType retval = EGovInteractRemarkType.Accept;

            if (Equals(EGovInteractRemarkType.Accept, typeStr))
			{
                retval = EGovInteractRemarkType.Accept;
			}
            else if (Equals(EGovInteractRemarkType.SwitchTo, typeStr))
            {
                retval = EGovInteractRemarkType.SwitchTo;
            }
            else if (Equals(EGovInteractRemarkType.Translate, typeStr))
            {
                retval = EGovInteractRemarkType.Translate;
            }
            else if (Equals(EGovInteractRemarkType.Comment, typeStr))
            {
                retval = EGovInteractRemarkType.Comment;
            }
            else if (Equals(EGovInteractRemarkType.Redo, typeStr))
            {
                retval = EGovInteractRemarkType.Redo;
            }
			return retval;
		}

		public static bool Equals(EGovInteractRemarkType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EGovInteractRemarkType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EGovInteractRemarkType type, bool selected)
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
                listControl.Items.Add(GetListItem(EGovInteractRemarkType.Accept, false));
                listControl.Items.Add(GetListItem(EGovInteractRemarkType.SwitchTo, false));
                listControl.Items.Add(GetListItem(EGovInteractRemarkType.Translate, false));
                listControl.Items.Add(GetListItem(EGovInteractRemarkType.Comment, false));
                listControl.Items.Add(GetListItem(EGovInteractRemarkType.Redo, false));
            }
        }
	}
}
