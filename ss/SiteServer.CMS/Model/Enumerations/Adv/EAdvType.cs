using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EAdvType
    {
        JsCode,         //JS����
        HtmlCode,       //HTML����
        Text,           //����
        Image,          //ͼƬ
        Flash           //Flash
    }

    public class EAdvTypeUtils
    {
        public static string GetValue(EAdvType type)
        {
            if (type == EAdvType.JsCode)
            {
                return "JsCode";
            }
            else if (type == EAdvType.HtmlCode)
            {
                return "HtmlCode";
            }
            else if (type == EAdvType.Text)
            {
                return "Text";
            }
            else if (type == EAdvType.Image)
            {
                return "Image";
            }
            else if (type == EAdvType.Flash)
            {
                return "Flash";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EAdvType type)
        {
            if (type == EAdvType.JsCode)
            {
                return "JS����";
            } if (type == EAdvType.HtmlCode)
            {
                return "HTML����";
            }
            else if (type == EAdvType.Text)
            {
                return "����";
            }
            else if (type == EAdvType.Image)
            {
                return "ͼƬ";
            }
            else if (type == EAdvType.Flash)
            {
                return "Flash";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EAdvType GetEnumType(string typeStr)
        {
            EAdvType retval = EAdvType.JsCode;

            if (Equals(EAdvType.JsCode, typeStr))
            {
                retval = EAdvType.JsCode;
            }
            else if (Equals(EAdvType.HtmlCode, typeStr))
            {
                retval = EAdvType.HtmlCode;
            }
            else if (Equals(EAdvType.Text, typeStr))
            {
                retval = EAdvType.Text;
            }
            else if (Equals(EAdvType.Image, typeStr))
            {
                retval = EAdvType.Image;
            }
            else if (Equals(EAdvType.Flash, typeStr))
            {
                retval = EAdvType.Flash;
            }

            return retval;
        }

        public static bool Equals(EAdvType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAdvType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAdvType type, bool selected)
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
                listControl.Items.Add(GetListItem(EAdvType.HtmlCode, false));
                listControl.Items.Add(GetListItem(EAdvType.JsCode, false));
                listControl.Items.Add(GetListItem(EAdvType.Text, false));
                listControl.Items.Add(GetListItem(EAdvType.Image, false));
                listControl.Items.Add(GetListItem(EAdvType.Flash, false));
            }
        }
    }
}
