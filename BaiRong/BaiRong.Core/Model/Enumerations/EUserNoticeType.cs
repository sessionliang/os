using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BaiRong.Model
{
    public enum EUserNoticeType
    {
        ClientLeaveMessageToProduct, //买家对商品留言
        ClientLeaveMessageToBussiness, //买家对商铺留言

        RemindBusinessToShipments, //提醒卖家发货

        FindPasswordAfter,  //找回密码之后通知
        WelcomeAfterRegiste, //注册之后发送欢迎信息
        ValidateForRegiste,  //注册验证信息
        BindEmail,//绑定邮箱
        FindPassword,//找回密码的验证信息发送方式
        BindPhone,//绑定手机校验码
    }

    public class EUserNoticeTypeUtils
    {
        public static string GetValue(EUserNoticeType type)
        {
            if (type == EUserNoticeType.ClientLeaveMessageToProduct)
            {
                return "ClientLeaveMessageToProduct";
            }
            else if (type == EUserNoticeType.ClientLeaveMessageToBussiness)
            {
                return "ClientLeaveMessageToBussiness";
            }
            else if (type == EUserNoticeType.RemindBusinessToShipments)
            {
                return "RemindBusinessToShipments";
            }
            else if (type == EUserNoticeType.FindPasswordAfter)
            {
                return "FindPasswordAfter";
            }
            else if (type == EUserNoticeType.WelcomeAfterRegiste)
            {
                return "WelcomeAfterRegiste";
            }
            else if (type == EUserNoticeType.ValidateForRegiste)
            {
                return "ValidateForRegiste";
            }
            else if (type == EUserNoticeType.BindEmail)
            {
                return "BindEmail";
            }
            else if (type == EUserNoticeType.FindPassword)
            {
                return "FindPassword";
            }
            else if (type == EUserNoticeType.BindPhone)
            {
                return "BindPhone";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserNoticeType type)
        {
            if (type == EUserNoticeType.ClientLeaveMessageToProduct)
            {
                return "买家对商品留言时，请通知我";
            }
            else if (type == EUserNoticeType.ClientLeaveMessageToBussiness)
            {
                return "买家对商铺留言时，请通知我";
            }
            else if (type == EUserNoticeType.RemindBusinessToShipments)
            {
                return "提醒卖家发货";
            }
            else if (type == EUserNoticeType.FindPasswordAfter)
            {
                return "找回密码之后，请通知我";
            }
            else if (type == EUserNoticeType.WelcomeAfterRegiste)
            {
                return "注册成功之后，发送欢迎信息";
            }
            else if (type == EUserNoticeType.ValidateForRegiste)
            {
                return "关闭注册人工审核之后，注册时发送验证信息";
            }
            else if (type == EUserNoticeType.BindEmail)
            {
                return "绑定用户邮箱";
            }
            else if (type == EUserNoticeType.FindPassword)
            {
                return "找回密码的验证信息发送方式";
            }
            else if (type == EUserNoticeType.BindPhone)
            {
                return "绑定用户手机";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserNoticeType GetEnumType(string typeStr)
        {
            if (Equals(typeStr, EUserNoticeType.ClientLeaveMessageToBussiness))
                return EUserNoticeType.ClientLeaveMessageToBussiness;
            else if (Equals(typeStr, EUserNoticeType.ClientLeaveMessageToProduct))
                return EUserNoticeType.ClientLeaveMessageToProduct;
            else if (Equals(typeStr, EUserNoticeType.RemindBusinessToShipments))
                return EUserNoticeType.RemindBusinessToShipments;
            else if (Equals(typeStr, EUserNoticeType.FindPasswordAfter))
                return EUserNoticeType.FindPasswordAfter;
            else if (Equals(typeStr, EUserNoticeType.WelcomeAfterRegiste))
                return EUserNoticeType.WelcomeAfterRegiste;
            else if (Equals(typeStr, EUserNoticeType.ValidateForRegiste))
                return EUserNoticeType.ValidateForRegiste;
            else if (Equals(typeStr, EUserNoticeType.BindEmail))
                return EUserNoticeType.BindEmail;
            else if (Equals(typeStr, EUserNoticeType.FindPassword))
                return EUserNoticeType.FindPassword;
            else if (Equals(typeStr, EUserNoticeType.BindPhone))
                return EUserNoticeType.BindPhone;
            else
                throw new Exception();
        }

        public static bool Equals(string typeStr, EUserNoticeType type)
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
            //retval.Add(GetValue(EUserNoticeType.ClientLeaveMessageToBussiness));
            //retval.Add(GetValue(EUserNoticeType.ClientLeaveMessageToProduct));
            //retval.Add(GetValue(EUserNoticeType.RemindBusinessToShipments));
            retval.Add(GetValue(EUserNoticeType.FindPasswordAfter));
            retval.Add(GetValue(EUserNoticeType.WelcomeAfterRegiste));
            retval.Add(GetValue(EUserNoticeType.ValidateForRegiste));
            retval.Add(GetValue(EUserNoticeType.BindEmail));
            retval.Add(GetValue(EUserNoticeType.FindPassword));
            retval.Add(GetValue(EUserNoticeType.BindPhone));
            return retval;
        }

        public static void AddListItemsToInstall(ListControl listControl)
        {
            if (listControl != null)
            {
                //listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.ClientLeaveMessageToBussiness), EUserNoticeTypeUtils.GetValue(EUserNoticeType.ClientLeaveMessageToBussiness)));
                //listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.ClientLeaveMessageToProduct), EUserNoticeTypeUtils.GetValue(EUserNoticeType.ClientLeaveMessageToProduct)));
                listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.FindPasswordAfter), EUserNoticeTypeUtils.GetValue(EUserNoticeType.FindPasswordAfter)));
                //listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.RemindBusinessToShipments), EUserNoticeTypeUtils.GetValue(EUserNoticeType.RemindBusinessToShipments)));
                listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.ValidateForRegiste), EUserNoticeTypeUtils.GetValue(EUserNoticeType.ValidateForRegiste)));
                //listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.WelcomeAfterRegiste), EUserNoticeTypeUtils.GetValue(EUserNoticeType.WelcomeAfterRegiste)));
                listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.BindEmail), EUserNoticeTypeUtils.GetValue(EUserNoticeType.BindEmail)));
                listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.FindPassword), EUserNoticeTypeUtils.GetValue(EUserNoticeType.FindPassword)));
                listControl.Items.Add(new ListItem(EUserNoticeTypeUtils.GetText(EUserNoticeType.FindPassword), EUserNoticeTypeUtils.GetValue(EUserNoticeType.BindPhone)));

            }
        }
    }
}
