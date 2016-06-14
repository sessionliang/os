using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum EApplyState
	{
        New,                //�°��
        Denied,             //�ܾ�����
        Accepted,           //������
        Redo,               //Ҫ�󷵹�
        Replied,            //�Ѱ���
        Checked,            //�����
	}

    public class EApplyStateUtils
	{
		public static string GetValue(EApplyState type)
		{
            if (type == EApplyState.New)
			{
                return "New";
			}
            else if (type == EApplyState.Denied)
			{
                return "Denied";
            }
            else if (type == EApplyState.Accepted)
            {
                return "Accepted";
            }
            else if (type == EApplyState.Redo)
            {
                return "Redo";
            }
            else if (type == EApplyState.Replied)
            {
                return "Replied";
            }
            else if (type == EApplyState.Checked)
            {
                return "Checked";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EApplyState type)
		{
            if (type == EApplyState.New)
			{
                return "�°��";
			}
            else if (type == EApplyState.Denied)
			{
                return "�ܾ�����";
            }
            else if (type == EApplyState.Accepted)
            {
                return "������";
            }
            else if (type == EApplyState.Redo)
            {
                return "Ҫ�󷵹�";
            }
            else if (type == EApplyState.Replied)
            {
                return "�Ѱ���";
            }
            else if (type == EApplyState.Checked)
            {
                return "�������";
            }
			else
			{
				throw new Exception();
			}
		}

        public static string GetFrontText(EApplyState type)
        {
            if (type == EApplyState.Denied)
            {
                return "�ܾ�����";
            }
            else if (type == EApplyState.Checked)
            {
                return "�������";
            }
            else
            {
                return "������";
            }
        }

		public static EApplyState GetEnumType(string typeStr)
		{
            EApplyState retval = EApplyState.New;

            if (Equals(EApplyState.New, typeStr))
			{
                retval = EApplyState.New;
			}
            else if (Equals(EApplyState.Denied, typeStr))
			{
                retval = EApplyState.Denied;
            }
            else if (Equals(EApplyState.Accepted, typeStr))
            {
                retval = EApplyState.Accepted;
            }
            else if (Equals(EApplyState.Redo, typeStr))
            {
                retval = EApplyState.Redo;
            }
            else if (Equals(EApplyState.Replied, typeStr))
            {
                retval = EApplyState.Replied;
            }
            else if (Equals(EApplyState.Checked, typeStr))
            {
                retval = EApplyState.Checked;
            }
			return retval;
		}

		public static bool Equals(EApplyState type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EApplyState type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EApplyState type, bool selected)
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
                listControl.Items.Add(GetListItem(EApplyState.New, false));
                listControl.Items.Add(GetListItem(EApplyState.Denied, false));
                listControl.Items.Add(GetListItem(EApplyState.Accepted, false));
                listControl.Items.Add(GetListItem(EApplyState.Redo, false));
                listControl.Items.Add(GetListItem(EApplyState.Replied, false));
                listControl.Items.Add(GetListItem(EApplyState.Checked, false));
            }
        }

        public static void AddListItemsToWork(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EApplyState.New, false));
                listControl.Items.Add(GetListItem(EApplyState.Accepted, false));
                listControl.Items.Add(GetListItem(EApplyState.Redo, false));
                listControl.Items.Add(GetListItem(EApplyState.Replied, false));
            }
        }
	}
}
