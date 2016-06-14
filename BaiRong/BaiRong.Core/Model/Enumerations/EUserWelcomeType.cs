using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EUserWelcomeType
    {
        None,
        Email,
        Message
    }

    public class EUserWelcomeTypeUtils
    {
        public static string GetValue(EUserWelcomeType type)
        {
            if (type == EUserWelcomeType.None)
            {
                return "None";
            }
            else if (type == EUserWelcomeType.Email)
            {
                return "Email";
            }
            else if (type == EUserWelcomeType.Message)
            {
                return "Message";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserWelcomeType type)
        {
            if (type == EUserWelcomeType.None)
            {
                return "不发送";
            }
            else if (type == EUserWelcomeType.Email)
            {
                return "发送Email";
            }
            else if (type == EUserWelcomeType.Message)
            {
                return "发送站内信";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserWelcomeType GetEnumType(string typeStr)
        {
            EUserWelcomeType retval = EUserWelcomeType.None;

            if (Equals(EUserWelcomeType.None, typeStr))
            {
                retval = EUserWelcomeType.None;
            }
            else if (Equals(EUserWelcomeType.Email, typeStr))
            {
                retval = EUserWelcomeType.Email;
            }
            else if (Equals(EUserWelcomeType.Message, typeStr))
            {
                retval = EUserWelcomeType.Message;
            }

            return retval;
        }

        public static bool Equals(EUserWelcomeType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EUserWelcomeType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EUserWelcomeType type, bool selected)
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
                listControl.Items.Add(GetListItem(EUserWelcomeType.None, false));
                listControl.Items.Add(GetListItem(EUserWelcomeType.Email, false));
                listControl.Items.Add(GetListItem(EUserWelcomeType.Message, false));
            }
        }
    }
}
