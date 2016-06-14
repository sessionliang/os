using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EGovPublicIdentifierType
	{
        Department,
        Channel,
        Attribute,
        Sequence
	}

    public class EGovPublicIdentifierTypeUtils
	{
		public static string GetValue(EGovPublicIdentifierType type)
		{
            if (type == EGovPublicIdentifierType.Department)
			{
                return "Department";
			}
            else if (type == EGovPublicIdentifierType.Channel)
			{
                return "Channel";
            }
            else if (type == EGovPublicIdentifierType.Attribute)
            {
                return "Attribute";
            }
            else if (type == EGovPublicIdentifierType.Sequence)
            {
                return "Sequence";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EGovPublicIdentifierType type)
		{
            if (type == EGovPublicIdentifierType.Department)
			{
                return "�����������";
			}
            else if (type == EGovPublicIdentifierType.Channel)
			{
                return "����������";
            }
            else if (type == EGovPublicIdentifierType.Attribute)
            {
                return "�ֶ�ֵ";
            }
            else if (type == EGovPublicIdentifierType.Sequence)
            {
                return "˳���";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EGovPublicIdentifierType GetEnumType(string typeStr)
		{
            EGovPublicIdentifierType retval = EGovPublicIdentifierType.Department;

            if (Equals(EGovPublicIdentifierType.Department, typeStr))
			{
                retval = EGovPublicIdentifierType.Department;
			}
            else if (Equals(EGovPublicIdentifierType.Channel, typeStr))
			{
                retval = EGovPublicIdentifierType.Channel;
            }
            else if (Equals(EGovPublicIdentifierType.Attribute, typeStr))
            {
                retval = EGovPublicIdentifierType.Attribute;
            }
            else if (Equals(EGovPublicIdentifierType.Sequence, typeStr))
            {
                retval = EGovPublicIdentifierType.Sequence;
            }
			return retval;
		}

		public static bool Equals(EGovPublicIdentifierType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EGovPublicIdentifierType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EGovPublicIdentifierType type, bool selected)
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
                listControl.Items.Add(GetListItem(EGovPublicIdentifierType.Department, false));
                listControl.Items.Add(GetListItem(EGovPublicIdentifierType.Channel, false));
                listControl.Items.Add(GetListItem(EGovPublicIdentifierType.Attribute, false));
                listControl.Items.Add(GetListItem(EGovPublicIdentifierType.Sequence, false));
            }
        }
	}
}
