using System;
using System.Web;
using System.Web.UI;
using BaiRong.Core;

namespace BaiRong.Core
{
	public class MessageUtils
	{
        private MessageUtils()
		{
		}

        public static void SaveMessage(Message.EMessageType messageType, string message)
        {
            CookieUtils.SetCookie(Message.GetCookieName(messageType), message, DateTime.MaxValue);
        }

        private static string DecodeMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                //message = StringUtils.HtmlDecode(message);
                message = message.Replace("''", "\"");
            }
            return message;
        }

        public static string GetMessageHtml(Message.EMessageType messageType, string message, Control control)
        {
            string messageHtml = string.Empty;
            message = MessageUtils.DecodeMessage(message);
            if (messageType == Message.EMessageType.Success)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    messageHtml = string.Format(@"<DIV class=""msg_succeed"">{0}</DIV>", message);
                }
            }
            else if (messageType == Message.EMessageType.Error)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    messageHtml = string.Format(@"<DIV class=""msg_failed"">{0}</DIV>", message);
                }
            }
            else if (messageType == Message.EMessageType.Info)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    messageHtml = string.Format(@"<DIV class=""msg_info"">{0}</DIV>", message);
                }
            }
            return messageHtml;
        }

        public static string GetMessageHtml(Control control)
        {
            Message.EMessageType messageType = Message.EMessageType.None;
            string message = string.Empty;
            if (CookieUtils.IsExists(Message.GetCookieName(Message.EMessageType.Success)))
            {
                messageType = Message.EMessageType.Success;
                message = CookieUtils.GetCookie(Message.GetCookieName(Message.EMessageType.Success));
                CookieUtils.Erase(Message.GetCookieName(Message.EMessageType.Success));
            }
            else if (CookieUtils.IsExists(Message.GetCookieName(Message.EMessageType.Error)))
            {
                messageType = Message.EMessageType.Error;
                message = CookieUtils.GetCookie(Message.GetCookieName(Message.EMessageType.Error));
                CookieUtils.Erase(Message.GetCookieName(Message.EMessageType.Error));
            }
            else if (CookieUtils.IsExists(Message.GetCookieName(Message.EMessageType.Info)))
            {
                messageType = Message.EMessageType.Info;
                message = CookieUtils.GetCookie(Message.GetCookieName(Message.EMessageType.Info));
                CookieUtils.Erase(Message.GetCookieName(Message.EMessageType.Info));
            }
            return GetMessageHtml(messageType, message, control);
        }

        public static string GetAlertHtml(Message.EMessageType messageType, string message, Control control)
        {
            string messageHtml = string.Empty;
            message = MessageUtils.DecodeMessage(message);
            if (messageType == Message.EMessageType.Success)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    messageHtml = string.Format(@"
<div class=""alert alert-success"">
    <button type=""button"" class=""close"" data-dismiss=""alert"">&times;</button>
  <strong>�ɹ�!</strong>&nbsp;&nbsp; {0}</div>", message);
                }
            }
            else if (messageType == Message.EMessageType.Error)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    messageHtml = string.Format(@"
<div class=""alert alert-error"">
    <button type=""button"" class=""close"" data-dismiss=""alert"">&times;</button>
  <strong>����!</strong>&nbsp;&nbsp; {0}</div>", message);
                }
            }
            else if (messageType == Message.EMessageType.Info)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    messageHtml = string.Format(@"
<div class=""alert alert-info"">
    <button type=""button"" class=""close"" data-dismiss=""alert"">&times;</button>
  <strong>��ʾ!</strong>&nbsp;&nbsp; {0}</div>", message);
                }
            }
            return messageHtml;
        }

        public static string GetAlertHtml(Control control, string text)
        {
            Message.EMessageType messageType = Message.EMessageType.None;
            string message = string.Empty;
            if (CookieUtils.IsExists(Message.GetCookieName(Message.EMessageType.Success)))
            {
                messageType = Message.EMessageType.Success;
                message = CookieUtils.GetCookie(Message.GetCookieName(Message.EMessageType.Success));
                CookieUtils.Erase(Message.GetCookieName(Message.EMessageType.Success));
            }
            else if (CookieUtils.IsExists(Message.GetCookieName(Message.EMessageType.Error)))
            {
                messageType = Message.EMessageType.Error;
                message = CookieUtils.GetCookie(Message.GetCookieName(Message.EMessageType.Error));
                CookieUtils.Erase(Message.GetCookieName(Message.EMessageType.Error));
            }
            else if (CookieUtils.IsExists(Message.GetCookieName(Message.EMessageType.Info)))
            {
                messageType = Message.EMessageType.Info;
                message = CookieUtils.GetCookie(Message.GetCookieName(Message.EMessageType.Info));
                CookieUtils.Erase(Message.GetCookieName(Message.EMessageType.Info));
            }
            else if (!string.IsNullOrEmpty(text))
            {
                messageType = Message.EMessageType.Info;
                message = text;
            }
            return GetAlertHtml(messageType, message, control);
        }

        #region Message
        public class Message
        {
            private const string cookieName = "BaiRong_Message";
            public static string GetCookieName(EMessageType messageType)
            {
                return string.Format("{0}_{1}", cookieName, EMessageTypeUtils.GetValue(messageType));
            }

            public enum EMessageType
            {
                Success,
                Error,
                Info,
                None
            }

            public class EMessageTypeUtils
            {
                public static string GetValue(EMessageType type)
                {
                    if (type == EMessageType.Success)
                    {
                        return "Success";
                    }
                    else if (type == EMessageType.Error)
                    {
                        return "Error";
                    }
                    else if (type == EMessageType.Info)
                    {
                        return "Info";
                    }
                    else if (type == EMessageType.None)
                    {
                        return "None";
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                public static EMessageType GetEnumType(string typeStr)
                {
                    EMessageType retval = EMessageType.None;

                    if (Equals(EMessageType.Success, typeStr))
                    {
                        retval = EMessageType.Success;
                    }
                    else if (Equals(EMessageType.Error, typeStr))
                    {
                        retval = EMessageType.Error;
                    }
                    else if (Equals(EMessageType.Info, typeStr))
                    {
                        retval = EMessageType.Info;
                    }

                    return retval;
                }
            }
        }
        #endregion

        #region Constants

        public const string InsertSuccess = "��ӳɹ���";
        public const string UpdateSuccess = "���³ɹ���";
        public const string DeleteSuccess = "ɾ���ɹ���";
        public const string CheckSuccess = "��˳ɹ���";
        public const string InsertFail = "���ʧ�ܣ�";
        public const string UpdateFail = "����ʧ�ܣ�";
        public const string DeleteFail = "ɾ��ʧ�ܣ�";
        public const string CheckFail = "���ʧ�ܣ�";

        public const string AccountLocked = "��¼ʧ�ܣ������ʻ��Ѿ���������";
        public const string AccountUnchecked = "��¼ʧ�ܣ������ʻ���δ����ˣ�";
        public const string AccountError = "��¼ʧ�ܣ������ԣ�";

        //public const string CheckDenied = "��˲�ͨ��";
        //public const string Unchecked = "δ���";
        //public const string CheckedLevel1 = "һ�����ͨ��";
        //public const string CheckedLevel2 = "�������ͨ��";
        //public const string CheckedLevel3 = "�������ͨ��";
        //public const string CheckedLevel4 = "�ļ����ͨ��";
        //public const string CheckedLevel5 = "�弶���ͨ��";


        public const string PageErrorParameterIsNotCorrect = "��ҳ��Ҫ��ȷ�Ĳ���������룡";

        public const string PermissionNotVisible = "�Բ�����û��Ȩ�������ҳ!";

        #endregion
    }
}
