using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
    public enum EPeriodType
    {
        None,               //����
        Once,               //һ��
        Everyday,           //ÿ��
        Hour,               //���Сʱ
        Minute,             //�������
    }

    public class EPeriodTypeUtils
    {
        public static string GetValue(EPeriodType type)
        {
            if (type == EPeriodType.None)
            {
                return "None";
            }
            else if (type == EPeriodType.Once)
            {
                return "Once";
            }
            else if (type == EPeriodType.Everyday)
            {
                return "Everyday";
            }
            else if (type == EPeriodType.Hour)
            {
                return "Hour";
            }
            else if (type == EPeriodType.Minute)
            {
                return "Minute";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EPeriodType type)
        {
            if (type == EPeriodType.None)
            {
                return "����";
            }
            else if (type == EPeriodType.Once)
            {
                return "һ��";
            }
            else if (type == EPeriodType.Everyday)
            {
                return "ÿ��";
            }
            else if (type == EPeriodType.Hour)
            {
                return "���Сʱ";
            }
            else if (type == EPeriodType.Minute)
            {
                return "�������";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EPeriodType GetEnumType(string typeStr)
        {
            EPeriodType retval = EPeriodType.None;

            if (Equals(EPeriodType.None, typeStr))
            {
                retval = EPeriodType.None;
            }
            else if (Equals(EPeriodType.Once, typeStr))
            {
                retval = EPeriodType.Once;
            }
            else if (Equals(EPeriodType.Everyday, typeStr))
            {
                retval = EPeriodType.Everyday;
            }
            else if (Equals(EPeriodType.Hour, typeStr))
            {
                retval = EPeriodType.Hour;
            }
            else if (Equals(EPeriodType.Minute, typeStr))
            {
                retval = EPeriodType.Minute;
            }

            return retval;
        }

        public static bool Equals(EPeriodType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EPeriodType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EPeriodType type, bool selected)
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
                listControl.Items.Add(GetListItem(EPeriodType.None, false));
                listControl.Items.Add(GetListItem(EPeriodType.Once, false));
                listControl.Items.Add(GetListItem(EPeriodType.Everyday, false));
                listControl.Items.Add(GetListItem(EPeriodType.Hour, false));
                listControl.Items.Add(GetListItem(EPeriodType.Minute, false));
            }
        }
    }
}
