using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.Project.Model
{
    public enum ELimitType
	{
        Normal,                 //����
        Alert,                  //Ԥ��
        Yellow,                 //����
        Red,                    //����
        Green                   //�������
	}

    public class ELimitTypeUtils
	{
		public static string GetValue(ELimitType type)
		{
            if (type == ELimitType.Normal)
			{
                return "Normal";
			}
            else if (type == ELimitType.Alert)
			{
                return "Alert";
            }
            else if (type == ELimitType.Yellow)
            {
                return "Yellow";
            }
            else if (type == ELimitType.Red)
            {
                return "Red";
            }
            else if (type == ELimitType.Green)
            {
                return "Green";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ELimitType type)
		{
            if (type == ELimitType.Normal)
			{
                return "δ����";
			}
            else if (type == ELimitType.Alert)
			{
                return "Ԥ��";
            }
            else if (type == ELimitType.Yellow)
            {
                return "����";
            }
            else if (type == ELimitType.Red)
            {
                return "����";
            }
            else if (type == ELimitType.Green)
            {
                return "�������";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ELimitType GetEnumType(string typeStr)
		{
            ELimitType retval = ELimitType.Normal;

            if (Equals(ELimitType.Normal, typeStr))
			{
                retval = ELimitType.Normal;
			}
            else if (Equals(ELimitType.Alert, typeStr))
			{
                retval = ELimitType.Alert;
            }
            else if (Equals(ELimitType.Yellow, typeStr))
            {
                retval = ELimitType.Yellow;
            }
            else if (Equals(ELimitType.Red, typeStr))
            {
                retval = ELimitType.Red;
            }
            else if (Equals(ELimitType.Green, typeStr))
            {
                retval = ELimitType.Green;
            }
			return retval;
		}

		public static bool Equals(ELimitType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ELimitType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ELimitType type, bool selected)
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
                listControl.Items.Add(GetListItem(ELimitType.Normal, false));
                listControl.Items.Add(GetListItem(ELimitType.Alert, false));
                listControl.Items.Add(GetListItem(ELimitType.Yellow, false));
                listControl.Items.Add(GetListItem(ELimitType.Red, false));
                listControl.Items.Add(GetListItem(ELimitType.Green, false));
            }
        }
	}
}
