using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EPhotoListType
    {
        Large,			    //��ͼ���
        Thumbnail,			//Сͼ���
    }

    public class EPhotoListTypeUtils
    {
        public static string GetValue(EPhotoListType type)
        {
            if (type == EPhotoListType.Large)
            {
                return "Large";
            }
            else if (type == EPhotoListType.Thumbnail)
            {
                return "Thumbnail";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EPhotoListType type)
        {
            if (type == EPhotoListType.Large)
            {
                return "��ͼ���";
            }
            else if (type == EPhotoListType.Thumbnail)
            {
                return "Сͼ���";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EPhotoListType GetEnumType(string typeStr)
        {
            EPhotoListType retval = EPhotoListType.Large;

            if (Equals(EPhotoListType.Large, typeStr))
            {
                retval = EPhotoListType.Large;
            }
            else if (Equals(EPhotoListType.Thumbnail, typeStr))
            {
                retval = EPhotoListType.Thumbnail;
            }

            return retval;
        }

        public static bool Equals(EPhotoListType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EPhotoListType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EPhotoListType type, bool selected)
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
                listControl.Items.Add(GetListItem(EPhotoListType.Large, false));
                listControl.Items.Add(GetListItem(EPhotoListType.Thumbnail, false));
            }
        }

    }
}
