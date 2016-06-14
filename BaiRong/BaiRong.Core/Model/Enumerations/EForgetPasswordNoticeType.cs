using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BaiRong.Model
{
    public enum EForgetPasswordNoticeType
    {
        None, //不发送
        Email,  //邮件
        Message, //站内信
    }

    public class EForgetPasswordNoticeTypeUtils
    {
        public static string GetValue(EForgetPasswordNoticeType type)
        {
            if (type == EForgetPasswordNoticeType.None)
            {
                return "None";
            }
            else if (type == EForgetPasswordNoticeType.Email)
            {
                return "Email";
            }
            else if (type == EForgetPasswordNoticeType.Message)
            {
                return "Message";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EForgetPasswordNoticeType type)
        {
            if (type == EForgetPasswordNoticeType.None)
            {
                return "不发送";
            }
            else if (type == EForgetPasswordNoticeType.Email)
            {
                return "发送Email";
            }
            else if (type == EForgetPasswordNoticeType.Message)
            {
                return "发送站内信";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EForgetPasswordNoticeType GetEnumType(string typeStr)
        {
            EForgetPasswordNoticeType retval = EForgetPasswordNoticeType.None;

            if (Equals(EForgetPasswordNoticeType.None, typeStr))
            {
                retval = EForgetPasswordNoticeType.None;
            }
            else if (Equals(EForgetPasswordNoticeType.Email, typeStr))
            {
                retval = EForgetPasswordNoticeType.Email;
            }
            else if (Equals(EForgetPasswordNoticeType.Message, typeStr))
            {
                retval = EForgetPasswordNoticeType.Message;
            }

            return retval;
        }

        public static bool Equals(EForgetPasswordNoticeType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EForgetPasswordNoticeType type)
        {
            return Equals(type, typeStr);
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EForgetPasswordNoticeType.None, false));
                listControl.Items.Add(GetListItem(EForgetPasswordNoticeType.Email, false));
                listControl.Items.Add(GetListItem(EForgetPasswordNoticeType.Message, false));
            }
        }

        public static ListItem GetListItem(EForgetPasswordNoticeType type, bool seleted)
        {
            ListItem item = new ListItem(GetText(type),GetValue(type));
            if (seleted)
            {
                item.Selected = true;
            }
            return item;
        }
    }
}
