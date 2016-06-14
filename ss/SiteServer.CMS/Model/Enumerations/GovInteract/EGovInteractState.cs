using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EGovInteractState
	{
        New,                //�°��
        Denied,             //�ܾ�����
        Accepted,           //������
        Redo,               //Ҫ�󷵹�
        Replied,            //�Ѱ���
        Checked,            //�����
	}

    public class EGovInteractStateUtils
	{
		public static string GetValue(EGovInteractState type)
		{
            if (type == EGovInteractState.New)
			{
                return "New";
			}
            else if (type == EGovInteractState.Denied)
			{
                return "Denied";
            }
            else if (type == EGovInteractState.Accepted)
            {
                return "Accepted";
            }
            else if (type == EGovInteractState.Redo)
            {
                return "Redo";
            }
            else if (type == EGovInteractState.Replied)
            {
                return "Replied";
            }
            else if (type == EGovInteractState.Checked)
            {
                return "Checked";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EGovInteractState type)
		{
            if (type == EGovInteractState.New)
			{
                return "�°��";
			}
            else if (type == EGovInteractState.Denied)
			{
                return "�ܾ�����";
            }
            else if (type == EGovInteractState.Accepted)
            {
                return "������";
            }
            else if (type == EGovInteractState.Redo)
            {
                return "Ҫ�󷵹�";
            }
            else if (type == EGovInteractState.Replied)
            {
                return "�Ѱ���";
            }
            else if (type == EGovInteractState.Checked)
            {
                return "�������";
            }
			else
			{
				throw new Exception();
			}
		}

        public static string GetFrontText(EGovInteractState type)
        {
            if (type == EGovInteractState.Denied)
            {
                return "�ܾ�����";
            }
            else if (type == EGovInteractState.Checked)
            {
                return "�������";
            }
            else
            {
                return "������";
            }
        }

		public static EGovInteractState GetEnumType(string typeStr)
		{
            EGovInteractState retval = EGovInteractState.New;

            if (Equals(EGovInteractState.New, typeStr))
			{
                retval = EGovInteractState.New;
			}
            else if (Equals(EGovInteractState.Denied, typeStr))
			{
                retval = EGovInteractState.Denied;
            }
            else if (Equals(EGovInteractState.Accepted, typeStr))
            {
                retval = EGovInteractState.Accepted;
            }
            else if (Equals(EGovInteractState.Redo, typeStr))
            {
                retval = EGovInteractState.Redo;
            }
            else if (Equals(EGovInteractState.Replied, typeStr))
            {
                retval = EGovInteractState.Replied;
            }
            else if (Equals(EGovInteractState.Checked, typeStr))
            {
                retval = EGovInteractState.Checked;
            }
			return retval;
		}

		public static bool Equals(EGovInteractState type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EGovInteractState type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EGovInteractState type, bool selected)
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
                listControl.Items.Add(GetListItem(EGovInteractState.New, false));
                listControl.Items.Add(GetListItem(EGovInteractState.Denied, false));
                listControl.Items.Add(GetListItem(EGovInteractState.Accepted, false));
                listControl.Items.Add(GetListItem(EGovInteractState.Redo, false));
                listControl.Items.Add(GetListItem(EGovInteractState.Replied, false));
                listControl.Items.Add(GetListItem(EGovInteractState.Checked, false));
            }
        }
	}
}
