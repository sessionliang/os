using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EUserMessageType
    {
        New,
        Private,
        System,
        SystemAnnouncement
    }

    public class EUserMessageTypeUtils
    {
        public static string GetValue(EUserMessageType type)
        {
            if (type == EUserMessageType.New)
            {
                return "New";
            }
            else if (type == EUserMessageType.Private)
            {
                return "Private";
            }
            else if (type == EUserMessageType.System)
            {
                return "System";
            }
            else if (type == EUserMessageType.SystemAnnouncement)
            {
                return "SystemAnnouncement";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserMessageType type)
        {
            if (type == EUserMessageType.New)
            {
                return "未读消息";
            }
            else if (type == EUserMessageType.Private)
            {
                return "私人消息";
            }
            else if (type == EUserMessageType.System)
            {
                return "系统消息";
            }
            else if (type == EUserMessageType.SystemAnnouncement)
            {
                return "系统公告";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserMessageType GetEnumType(string typeStr)
        {
            EUserMessageType retval = EUserMessageType.Private;

            if (Equals(EUserMessageType.New, typeStr))
            {
                retval = EUserMessageType.New;
            }
            else if (Equals(EUserMessageType.Private, typeStr))
            {
                retval = EUserMessageType.Private;
            }
            else if (Equals(EUserMessageType.System, typeStr))
            {
                retval = EUserMessageType.System;
            }
            else if (Equals(EUserMessageType.SystemAnnouncement, typeStr))
            {
                retval = EUserMessageType.SystemAnnouncement;
            }
            return retval;
        }

        public static bool Equals(EUserMessageType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EUserMessageType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EUserMessageType type, bool selected)
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
                listControl.Items.Add(GetListItem(EUserMessageType.New, false));
                listControl.Items.Add(GetListItem(EUserMessageType.Private, false));
                listControl.Items.Add(GetListItem(EUserMessageType.System, false));
                listControl.Items.Add(GetListItem(EUserMessageType.SystemAnnouncement, false));
            }
        }
    }
}
