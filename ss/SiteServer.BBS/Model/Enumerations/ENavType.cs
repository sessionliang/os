using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
    public enum ENavType
    {
        Header,                 //主导航
        Secondary,              //下级导航
        Footer,                 //页尾导航
    }

    public class ENavTypeUtils
    {
        public static string GetValue(ENavType type)
        {
            if (type == ENavType.Header)
            {
                return "Header";
            }
            else if (type == ENavType.Secondary)
            {
                return "Secondary";
            }
            else if (type == ENavType.Footer)
            {
                return "Footer";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ENavType type)
        {
            if (type == ENavType.Header)
            {
                return "主导航";
            }
            else if (type == ENavType.Secondary)
            {
                return "下级导航";
            }
            else if (type == ENavType.Footer)
            {
                return "页尾导航";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ENavType GetEnumType(string typeStr)
        {
            ENavType retval = ENavType.Header;

            if (Equals(ENavType.Header, typeStr))
            {
                retval = ENavType.Header;
            }
            else if (Equals(ENavType.Secondary, typeStr))
            {
                retval = ENavType.Secondary;
            }
            else if (Equals(ENavType.Footer, typeStr))
            {
                retval = ENavType.Footer;
            }

            return retval;
        }

        public static bool Equals(ENavType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ENavType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ENavType type, bool selected)
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
                listControl.Items.Add(GetListItem(ENavType.Header, false));
                listControl.Items.Add(GetListItem(ENavType.Secondary, false));
                listControl.Items.Add(GetListItem(ENavType.Footer, false));
            }
        }
    }
}
