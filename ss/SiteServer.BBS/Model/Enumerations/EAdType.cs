using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
    public enum EAdType
    {
        JsCode,         //JS´úÂë
        HtmlCode,       //HTML´úÂë
        Text,           //ÎÄ×Ö
        Image,          //Í¼Æ¬
        Flash           //Flash
    }

    public class EAdTypeUtils
    {
        public static string GetValue(EAdType type)
        {
            if (type == EAdType.JsCode)
            {
                return "JsCode";
            }
            else if (type == EAdType.HtmlCode)
            {
                return "HtmlCode";
            }
            else if (type == EAdType.Text)
            {
                return "Text";
            }
            else if (type == EAdType.Image)
            {
                return "Image";
            }
            else if (type == EAdType.Flash)
            {
                return "Flash";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EAdType type)
        {
            if (type == EAdType.JsCode)
            {
                return "JS´úÂë";
            } if (type == EAdType.HtmlCode)
            {
                return "HTML´úÂë";
            }
            else if (type == EAdType.Text)
            {
                return "ÎÄ×Ö";
            }
            else if (type == EAdType.Image)
            {
                return "Í¼Æ¬";
            }
            else if (type == EAdType.Flash)
            {
                return "Flash";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EAdType GetEnumType(string typeStr)
        {
            EAdType retval = EAdType.JsCode;

            if (Equals(EAdType.JsCode, typeStr))
            {
                retval = EAdType.JsCode;
            }
            else if (Equals(EAdType.HtmlCode, typeStr))
            {
                retval = EAdType.HtmlCode;
            }
            else if (Equals(EAdType.Text, typeStr))
            {
                retval = EAdType.Text;
            }
            else if (Equals(EAdType.Image, typeStr))
            {
                retval = EAdType.Image;
            }
            else if (Equals(EAdType.Flash, typeStr))
            {
                retval = EAdType.Flash;
            }

            return retval;
        }

        public static bool Equals(EAdType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAdType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAdType type, bool selected)
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
                listControl.Items.Add(GetListItem(EAdType.HtmlCode, false));
                listControl.Items.Add(GetListItem(EAdType.JsCode, false));
                listControl.Items.Add(GetListItem(EAdType.Text, false));
                listControl.Items.Add(GetListItem(EAdType.Image, false));
                listControl.Items.Add(GetListItem(EAdType.Flash, false));
            }
        }
    }
}
