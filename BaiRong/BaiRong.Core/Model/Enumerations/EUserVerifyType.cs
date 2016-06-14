using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EUserVerifyType
    {
        None,
        Email,
        Manually
    }

    public class EUserVerifyTypeUtils
    {
        public static string GetValue(EUserVerifyType type)
        {
            if (type == EUserVerifyType.None)
            {
                return "None";
            }
            else if (type == EUserVerifyType.Email)
            {
                return "Email";
            }
            else if (type == EUserVerifyType.Manually)
            {
                return "Manually";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserVerifyType type)
        {
            if (type == EUserVerifyType.None)
            {
                return "无验证";
            }
            else if (type == EUserVerifyType.Email)
            {
                return "Email验证";
            }
            else if (type == EUserVerifyType.Manually)
            {
                return "人工审核";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserVerifyType GetEnumType(string typeStr)
        {
            EUserVerifyType retval = EUserVerifyType.None;

            if (Equals(EUserVerifyType.None, typeStr))
            {
                retval = EUserVerifyType.None;
            }
            else if (Equals(EUserVerifyType.Email, typeStr))
            {
                retval = EUserVerifyType.Email;
            }
            else if (Equals(EUserVerifyType.Manually, typeStr))
            {
                retval = EUserVerifyType.Manually;
            }

            return retval;
        }

        public static bool Equals(EUserVerifyType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EUserVerifyType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EUserVerifyType type, bool selected)
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
                listControl.Items.Add(GetListItem(EUserVerifyType.None, false));
                listControl.Items.Add(GetListItem(EUserVerifyType.Email, false));
                listControl.Items.Add(GetListItem(EUserVerifyType.Manually, false));
            }
        }
    }
}
