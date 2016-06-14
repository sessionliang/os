using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EGovPublicApplyState
	{
        New,                //������
        Denied,             //�ܾ�����
        Accepted,           //������
        Redo,               //Ҫ�󷵹�
        Replied,            //�Ѱ���
        Checked,            //�����
	}

    public class EGovPublicApplyStateUtils
	{
		public static string GetValue(EGovPublicApplyState type)
		{
            if (type == EGovPublicApplyState.New)
			{
                return "New";
			}
            else if (type == EGovPublicApplyState.Denied)
			{
                return "Denied";
            }
            else if (type == EGovPublicApplyState.Accepted)
            {
                return "Accepted";
            }
            else if (type == EGovPublicApplyState.Redo)
            {
                return "Redo";
            }
            else if (type == EGovPublicApplyState.Replied)
            {
                return "Replied";
            }
            else if (type == EGovPublicApplyState.Checked)
            {
                return "Checked";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EGovPublicApplyState type)
		{
            if (type == EGovPublicApplyState.New)
			{
                return "������";
			}
            else if (type == EGovPublicApplyState.Denied)
			{
                return "�ܾ�����";
            }
            else if (type == EGovPublicApplyState.Accepted)
            {
                return "������";
            }
            else if (type == EGovPublicApplyState.Redo)
            {
                return "Ҫ�󷵹�";
            }
            else if (type == EGovPublicApplyState.Replied)
            {
                return "�Ѱ���";
            }
            else if (type == EGovPublicApplyState.Checked)
            {
                return "�������";
            }
			else
			{
				throw new Exception();
			}
		}

        public static string GetFrontText(EGovPublicApplyState type)
        {
            if (type == EGovPublicApplyState.Denied)
            {
                return "�ܾ�����";
            }
            else if (type == EGovPublicApplyState.Checked)
            {
                return "�������";
            }
            else
            {
                return "���������";
            }
        }

		public static EGovPublicApplyState GetEnumType(string typeStr)
		{
            EGovPublicApplyState retval = EGovPublicApplyState.New;

            if (Equals(EGovPublicApplyState.New, typeStr))
			{
                retval = EGovPublicApplyState.New;
			}
            else if (Equals(EGovPublicApplyState.Denied, typeStr))
			{
                retval = EGovPublicApplyState.Denied;
            }
            else if (Equals(EGovPublicApplyState.Accepted, typeStr))
            {
                retval = EGovPublicApplyState.Accepted;
            }
            else if (Equals(EGovPublicApplyState.Redo, typeStr))
            {
                retval = EGovPublicApplyState.Redo;
            }
            else if (Equals(EGovPublicApplyState.Replied, typeStr))
            {
                retval = EGovPublicApplyState.Replied;
            }
            else if (Equals(EGovPublicApplyState.Checked, typeStr))
            {
                retval = EGovPublicApplyState.Checked;
            }
			return retval;
		}

		public static bool Equals(EGovPublicApplyState type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EGovPublicApplyState type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EGovPublicApplyState type, bool selected)
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
                listControl.Items.Add(GetListItem(EGovPublicApplyState.New, false));
                listControl.Items.Add(GetListItem(EGovPublicApplyState.Denied, false));
                listControl.Items.Add(GetListItem(EGovPublicApplyState.Accepted, false));
                listControl.Items.Add(GetListItem(EGovPublicApplyState.Redo, false));
                listControl.Items.Add(GetListItem(EGovPublicApplyState.Replied, false));
                listControl.Items.Add(GetListItem(EGovPublicApplyState.Checked, false));
            }
        }
	}
}
