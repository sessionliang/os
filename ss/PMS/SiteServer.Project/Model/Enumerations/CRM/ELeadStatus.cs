using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.Project.Model
{
    public enum ELeadStatus
	{
        New,                    //��������δ����
        Contacting,             //�Ѵ����Ӵ���
        Invalid,                //��Ч����
        Failure,                //����
        Success                 //�ɵ�
	}

    public class ELeadStatusUtils
	{
		public static string GetValue(ELeadStatus type)
		{
            if (type == ELeadStatus.New)
			{
                return "New";
			}
            else if (type == ELeadStatus.Contacting)
            {
                return "Contacting";
            }
            else if (type == ELeadStatus.Invalid)
            {
                return "Invalid";
            }
            else if (type == ELeadStatus.Failure)
            {
                return "Failure";
            }
            else if (type == ELeadStatus.Success)
            {
                return "Success";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ELeadStatus type)
		{
            if (type == ELeadStatus.New)
			{
                return "��������δ����";
			}
            else if (type == ELeadStatus.Contacting)
            {
                return "�Ѵ����Ӵ���";
            }
            else if (type == ELeadStatus.Invalid)
            {
                return "��Ч����";
            }
            else if (type == ELeadStatus.Failure)
            {
                return "����";
            }
            else if (type == ELeadStatus.Success)
            {
                return "�ɵ�";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ELeadStatus GetEnumType(string typeStr)
		{
            ELeadStatus retval = ELeadStatus.New;

            if (Equals(ELeadStatus.New, typeStr))
			{
                retval = ELeadStatus.New;
			}
            else if (Equals(ELeadStatus.Contacting, typeStr))
            {
                retval = ELeadStatus.Contacting;
            }
            else if (Equals(ELeadStatus.Invalid, typeStr))
            {
                retval = ELeadStatus.Invalid;
            }
            else if (Equals(ELeadStatus.Failure, typeStr))
            {
                retval = ELeadStatus.Failure;
            }
            else if (Equals(ELeadStatus.Success, typeStr))
            {
                retval = ELeadStatus.Success;
            }
			return retval;
		}

		public static bool Equals(ELeadStatus type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ELeadStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ELeadStatus type, bool selected)
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
                listControl.Items.Add(GetListItem(ELeadStatus.New, false));
                listControl.Items.Add(GetListItem(ELeadStatus.Contacting, false));
                listControl.Items.Add(GetListItem(ELeadStatus.Invalid, false));
                listControl.Items.Add(GetListItem(ELeadStatus.Failure, false));
                listControl.Items.Add(GetListItem(ELeadStatus.Success, false));
            }
        }
	}
}
