using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BaiRong.Model
{
    public enum EUserNoticeTemplateType
    {
        Message, //站内信
        Email, //邮箱
        Phone, //短信
    }

    public class EUserNoticeTemplateTypeUtils
    {
        public static string GetValue(EUserNoticeTemplateType type)
        {
            if (type == EUserNoticeTemplateType.Message)
            {
                return "Message";
            }
            else if (type == EUserNoticeTemplateType.Email)
            {
                return "Email";
            }
            else if (type == EUserNoticeTemplateType.Phone)
            {
                return "Phone";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserNoticeTemplateType type)
        {
            if (type == EUserNoticeTemplateType.Message)
            {
                return "站内信";
            }
            else if (type == EUserNoticeTemplateType.Email)
            {
                return "邮箱";
            }
            else if (type == EUserNoticeTemplateType.Phone)
            {
                return "短信";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserNoticeTemplateType GetEnumType(string typeStr)
        {
            if (Equals(typeStr, EUserNoticeTemplateType.Email))
                return EUserNoticeTemplateType.Email;
            else if (Equals(typeStr, EUserNoticeTemplateType.Message))
                return EUserNoticeTemplateType.Message;
            else if (Equals(typeStr, EUserNoticeTemplateType.Phone))
                return EUserNoticeTemplateType.Phone;
            else
                throw new Exception();
        }

        public static bool Equals(string typeStr, EUserNoticeTemplateType type)
        {
            if (string.IsNullOrEmpty(typeStr))
                return false;
            if (string.Equals(typeStr.ToLower(), GetValue(type).ToLower()))
                return true;
            return false;
        }

        public static bool Equals(EUserNoticeType type, string typeStr)
        {
            return Equals(typeStr, type);
        }

        public static List<string> GetAllVaules()
        {
            List<string> retval = new List<string>();
            retval.Add(GetValue(EUserNoticeTemplateType.Email));
            retval.Add(GetValue(EUserNoticeTemplateType.Message));
            retval.Add(GetValue(EUserNoticeTemplateType.Phone));
            return retval;
        }

        public static void AddListItemsToInstall(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(new ListItem(EUserNoticeTemplateTypeUtils.GetText(EUserNoticeTemplateType.Email), EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Email)));
                listControl.Items.Add(new ListItem(EUserNoticeTemplateTypeUtils.GetText(EUserNoticeTemplateType.Message), EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Message)));
                listControl.Items.Add(new ListItem(EUserNoticeTemplateTypeUtils.GetText(EUserNoticeTemplateType.Phone), EUserNoticeTemplateTypeUtils.GetValue(EUserNoticeTemplateType.Phone)));

            }
        }
    }
}
