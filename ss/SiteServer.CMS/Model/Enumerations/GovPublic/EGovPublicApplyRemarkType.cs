using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EGovPublicApplyRemarkType
	{
        Accept,             //����
        SwitchTo,           //ת��
        Comment,            //��ʾ
        Redo                //Ҫ�󷵹�
	}

    public class EGovPublicApplyRemarkTypeUtils
	{
		public static string GetValue(EGovPublicApplyRemarkType type)
		{
            if (type == EGovPublicApplyRemarkType.Accept)
			{
                return "Accept";
			}
            else if (type == EGovPublicApplyRemarkType.SwitchTo)
            {
                return "SwitchTo";
            }
            else if (type == EGovPublicApplyRemarkType.Comment)
            {
                return "Comment";
            }
            else if (type == EGovPublicApplyRemarkType.Redo)
            {
                return "Redo";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EGovPublicApplyRemarkType type)
		{
            if (type == EGovPublicApplyRemarkType.Accept)
			{
                return "����";
			}
            else if (type == EGovPublicApplyRemarkType.SwitchTo)
            {
                return "ת��";
            }
            else if (type == EGovPublicApplyRemarkType.Comment)
            {
                return "��ʾ";
            }
            else if (type == EGovPublicApplyRemarkType.Redo)
            {
                return "Ҫ�󷵹�";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EGovPublicApplyRemarkType GetEnumType(string typeStr)
		{
            EGovPublicApplyRemarkType retval = EGovPublicApplyRemarkType.Accept;

            if (Equals(EGovPublicApplyRemarkType.Accept, typeStr))
			{
                retval = EGovPublicApplyRemarkType.Accept;
			}
            else if (Equals(EGovPublicApplyRemarkType.SwitchTo, typeStr))
            {
                retval = EGovPublicApplyRemarkType.SwitchTo;
            }
            else if (Equals(EGovPublicApplyRemarkType.Comment, typeStr))
            {
                retval = EGovPublicApplyRemarkType.Comment;
            }
            else if (Equals(EGovPublicApplyRemarkType.Redo, typeStr))
            {
                retval = EGovPublicApplyRemarkType.Redo;
            }
			return retval;
		}

		public static bool Equals(EGovPublicApplyRemarkType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EGovPublicApplyRemarkType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EGovPublicApplyRemarkType type, bool selected)
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
                listControl.Items.Add(GetListItem(EGovPublicApplyRemarkType.Accept, false));
                listControl.Items.Add(GetListItem(EGovPublicApplyRemarkType.SwitchTo, false));
                listControl.Items.Add(GetListItem(EGovPublicApplyRemarkType.Comment, false));
                listControl.Items.Add(GetListItem(EGovPublicApplyRemarkType.Redo, false));
            }
        }
	}
}
