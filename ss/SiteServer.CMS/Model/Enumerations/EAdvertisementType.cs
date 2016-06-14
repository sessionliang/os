using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{

    public enum EAdvertisementType
    {
        FloatImage,				//Ư�����
        ScreenDown,             //ȫ������
        OpenWindow,             //��������
    }

    public class EAdvertisementTypeUtils
    {
        public static string GetValue(EAdvertisementType type)
        {
            if (type == EAdvertisementType.FloatImage)
            {
                return "FloatImage";
            }
            else if (type == EAdvertisementType.ScreenDown)
            {
                return "ScreenDown";
            }
            else if (type == EAdvertisementType.OpenWindow)
            {
                return "OpenWindow";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EAdvertisementType type)
        {
            if (type == EAdvertisementType.FloatImage)
            {
                return "Ư�����";
            }
            else if (type == EAdvertisementType.ScreenDown)
            {
                return "ȫ������";
            }
            else if (type == EAdvertisementType.OpenWindow)
            {
                return "��������";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EAdvertisementType GetEnumType(string typeStr)
        {
            EAdvertisementType retval = EAdvertisementType.FloatImage;

            if (Equals(EAdvertisementType.FloatImage, typeStr))
            {
                retval = EAdvertisementType.FloatImage;
            }
            else if (Equals(EAdvertisementType.ScreenDown, typeStr))
            {
                retval = EAdvertisementType.ScreenDown;
            }
            else if (Equals(EAdvertisementType.OpenWindow, typeStr))
            {
                retval = EAdvertisementType.OpenWindow;
            }

            return retval;
        }

        public static bool Equals(EAdvertisementType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAdvertisementType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAdvertisementType type, bool selected)
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
                listControl.Items.Add(GetListItem(EAdvertisementType.FloatImage, false));
                listControl.Items.Add(GetListItem(EAdvertisementType.ScreenDown, false));
                listControl.Items.Add(GetListItem(EAdvertisementType.OpenWindow, false));
            }
        }
    }
}
