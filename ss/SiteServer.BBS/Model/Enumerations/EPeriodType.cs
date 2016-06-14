using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
    public enum EPeriodType
    {
        None,               //不限
        Once,               //一次
        Everyday,           //每天
        Hour,               //间隔小时
        Minute,             //间隔分钟
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
                return "不限";
            }
            else if (type == EPeriodType.Once)
            {
                return "一次";
            }
            else if (type == EPeriodType.Everyday)
            {
                return "每天";
            }
            else if (type == EPeriodType.Hour)
            {
                return "间隔小时";
            }
            else if (type == EPeriodType.Minute)
            {
                return "间隔分钟";
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
