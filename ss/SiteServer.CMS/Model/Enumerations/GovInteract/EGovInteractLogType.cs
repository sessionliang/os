using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EGovInteractLogType
	{
        New,                //������
        Accept,             //����
        Deny,               //�ܾ�
        SwitchTo,           //ת��
        Translate,          //ת��
        Comment,            //��ʾ
        Redo,               //Ҫ�󷵹�
        Reply,              //����
        Check,              //���
	}

    public class EGovInteractLogTypeUtils
	{
		public static string GetValue(EGovInteractLogType type)
		{
            if (type == EGovInteractLogType.New)
			{
                return "New";
            }
            else if (type == EGovInteractLogType.Accept)
            {
                return "Accept";
            }
            else if (type == EGovInteractLogType.Deny)
			{
                return "Deny";
            }
            else if (type == EGovInteractLogType.SwitchTo)
            {
                return "SwitchTo";
            }
            else if (type == EGovInteractLogType.Translate)
            {
                return "Translate";
            }
            else if (type == EGovInteractLogType.Comment)
            {
                return "Comment";
            }
            else if (type == EGovInteractLogType.Redo)
            {
                return "Redo";
            }
            else if (type == EGovInteractLogType.Reply)
            {
                return "Reply";
            }
            else if (type == EGovInteractLogType.Check)
            {
                return "Check";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EGovInteractLogType type)
		{
            if (type == EGovInteractLogType.New)
            {
                return "ǰ̨ ���� �ύ���";
            }
            else if (type == EGovInteractLogType.Accept)
            {
                return "������";
            }
            else if (type == EGovInteractLogType.Deny)
            {
                return "�ܾ����";
            }
            else if (type == EGovInteractLogType.SwitchTo)
            {
                return "ת����";
            }
            else if (type == EGovInteractLogType.Translate)
            {
                return "ת�ư��";
            }
            else if (type == EGovInteractLogType.Comment)
            {
                return "��ʾ���";
            }
            else if (type == EGovInteractLogType.Redo)
            {
                return "Ҫ�󷵹�";
            }
            else if (type == EGovInteractLogType.Reply)
            {
                return "�ظ����";
            }
            else if (type == EGovInteractLogType.Check)
            {
                return "���ͨ�����";
            }
            else
            {
                throw new Exception();
            }
		}

		public static EGovInteractLogType GetEnumType(string typeStr)
		{
            EGovInteractLogType retval = EGovInteractLogType.New;

            if (Equals(EGovInteractLogType.New, typeStr))
			{
                retval = EGovInteractLogType.New;
            }
            else if (Equals(EGovInteractLogType.Accept, typeStr))
            {
                retval = EGovInteractLogType.Accept;
            }
            else if (Equals(EGovInteractLogType.Deny, typeStr))
			{
                retval = EGovInteractLogType.Deny;
            }
            else if (Equals(EGovInteractLogType.SwitchTo, typeStr))
            {
                retval = EGovInteractLogType.SwitchTo;
            }
            else if (Equals(EGovInteractLogType.Translate, typeStr))
            {
                retval = EGovInteractLogType.Translate;
            }
            else if (Equals(EGovInteractLogType.Comment, typeStr))
            {
                retval = EGovInteractLogType.Comment;
            }
            else if (Equals(EGovInteractLogType.Redo, typeStr))
            {
                retval = EGovInteractLogType.Redo;
            }
            else if (Equals(EGovInteractLogType.Reply, typeStr))
            {
                retval = EGovInteractLogType.Reply;
            }
            else if (Equals(EGovInteractLogType.Check, typeStr))
            {
                retval = EGovInteractLogType.Check;
            }
			return retval;
		}

		public static bool Equals(EGovInteractLogType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EGovInteractLogType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EGovInteractLogType type, bool selected)
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
                listControl.Items.Add(GetListItem(EGovInteractLogType.New, false));
                listControl.Items.Add(GetListItem(EGovInteractLogType.Accept, false));
                listControl.Items.Add(GetListItem(EGovInteractLogType.Deny, false));
                listControl.Items.Add(GetListItem(EGovInteractLogType.SwitchTo, false));
                listControl.Items.Add(GetListItem(EGovInteractLogType.Translate, false));
                listControl.Items.Add(GetListItem(EGovInteractLogType.Comment, false));
                listControl.Items.Add(GetListItem(EGovInteractLogType.Redo, false));
                listControl.Items.Add(GetListItem(EGovInteractLogType.Reply, false));
                listControl.Items.Add(GetListItem(EGovInteractLogType.Check, false));
            }
        }
	}
}
