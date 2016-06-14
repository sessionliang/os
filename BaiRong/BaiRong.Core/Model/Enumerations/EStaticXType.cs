using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{

    public enum EStatictisXType
    {
        Hour,          //Сʱ
        Day,            //��
        Month,       //��
        Year,           //��
    }

    public class EStatictisXTypeUtils
    {
        public static string GetValue(EStatictisXType type)
        {
            if (type == EStatictisXType.Hour)
            {
                return "Hour";
            }
            else if (type == EStatictisXType.Day)
            {
                return "Day";
            }
            else if (type == EStatictisXType.Month)
            {
                return "Month";
            }
            else if (type == EStatictisXType.Year)
            {
                return "Year";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EStatictisXType type)
        {
            if (type == EStatictisXType.Hour)
            {
                return "ʱ";
            }
            else if (type == EStatictisXType.Day)
            {
                return "��";
            }
            else if (type == EStatictisXType.Month)
            {
                return "��";
            }
            else if (type == EStatictisXType.Year)
            {
                return "��";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EStatictisXType GetEnumType(string typeStr)
        {
            EStatictisXType retval = EStatictisXType.Day;

            if (Equals(EStatictisXType.Hour, typeStr))
            {
                retval = EStatictisXType.Hour;
            }
            else if (Equals(EStatictisXType.Day, typeStr))
            {
                retval = EStatictisXType.Day;
            }
            else if (Equals(EStatictisXType.Month, typeStr))
            {
                retval = EStatictisXType.Month;
            }
            else if (Equals(EStatictisXType.Year, typeStr))
            {
                retval = EStatictisXType.Year;
            }

            return retval;
        }

        public static bool Equals(EStatictisXType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EStatictisXType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EStatictisXType type, bool selected)
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
                listControl.Items.Add(GetListItem(EStatictisXType.Day, false));
                listControl.Items.Add(GetListItem(EStatictisXType.Month, false));
                listControl.Items.Add(GetListItem(EStatictisXType.Year, false));
            }
        }

    }
}
