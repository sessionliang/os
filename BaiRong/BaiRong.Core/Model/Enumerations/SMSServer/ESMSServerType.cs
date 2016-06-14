using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace BaiRong.Model
{
    public enum ESMSServerType
    {
        YunPian,
        WeiMi,
    }

    public class ESMSServerTypeUtils
    {
        public static string GetValue(ESMSServerType type)
        {
            if (type == ESMSServerType.YunPian)
            {
                return "YunPian";
            }
            else if (type == ESMSServerType.WeiMi)
            {
                return "WeiMi";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ESMSServerType type)
        {
            if (type == ESMSServerType.YunPian)
            {
                return "云片短信";
            }
            else if (type == ESMSServerType.WeiMi)
            {
                return "微米短信";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetDescription(ESMSServerType type)
        {
            if (type == ESMSServerType.YunPian)
            {
                return "云片短信";
            }
            else if (type == ESMSServerType.WeiMi)
            {
                return "微米短信";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ESMSServerType GetEnumType(string typeStr)
        {
            ESMSServerType retval = ESMSServerType.YunPian;
            if (ESMSServerTypeUtils.Equals(typeStr, ESMSServerType.WeiMi))
                retval = ESMSServerType.WeiMi;
            return retval;
        }

        public static bool Equals(ESMSServerType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ESMSServerType type)
        {
            return Equals(type, typeStr);
        }

        public static List<ESMSServerType> GetESMSServerTypeList()
        {
            List<ESMSServerType> list = new List<ESMSServerType>();
            list.Add(ESMSServerType.YunPian);
            //list.Add(ESMSServerType.WeiMi);
            return list;
        }

        public static ListItem GetListItem(ESMSServerType type, bool selected)
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
                listControl.Items.Add(GetListItem(ESMSServerType.YunPian, false));
                //listControl.Items.Add(GetListItem(ESMSServerType.WeiMi, false));
            }
        }

        public static NameValueCollection GetParamCollection(ESMSServerType smsServerType)
        {
            return new NameValueCollection();
        }

        public static SMSServerInfoExtend GetAdditional(ESMSServerType smsServerType)
        {
            SMSServerInfoExtend additional = new SMSServerInfoExtend();

            if (ESMSServerTypeUtils.Equals(smsServerType, ESMSServerType.YunPian))
            {
                //发送设置
                additional.SendUrl = "http://yunpian.com/v1/sms/send.json";
                additional.SendParams = "apikey={@apikey}&mobile={@" + SMSServerManager.MOBILE + "}&text={@" + SMSServerManager.TEXT + "}";
                NameValueCollection SendSettings = new NameValueCollection();
                SendSettings.Add("SendRequestType", "Post");
                SendSettings.Add("SendParamsType", "String");
                SendSettings.Add("ReturnContentType", "Json");
                SendSettings.Add("OKFlag", "Code");
                SendSettings.Add("OKValue", "0");
                SendSettings.Add("MsgFlag", "msg");
                additional.SendSettings = SendSettings;

                //查询剩余短信条数
                additional.LastSmsSearchUrl = "http://yunpian.com/v1/user/get.json";
                additional.LastSmsSearchParams = "apikey={@apikey}";
                NameValueCollection LastSmsSearchSettings = new NameValueCollection();
                LastSmsSearchSettings.Add("SendRequestType", "Post");
                LastSmsSearchSettings.Add("SendParamsType", "String");
                LastSmsSearchSettings.Add("ReturnContentType", "Json");
                LastSmsSearchSettings.Add("OKFlag", "Code");
                LastSmsSearchSettings.Add("OKValue", "0");
                LastSmsSearchSettings.Add("RetrunValueKey", "balance");
                LastSmsSearchSettings.Add("MsgFlag", "msg");
                additional.LastSmsSearchSettings = LastSmsSearchSettings;

                //添加短信
                additional.InsertTemplateUrl = "http://yunpian.com/v1/tpl/add.json";
                additional.InsertTemplateParams = "apikey={@apikey}&tpl_content={@" + SMSServerManager.TEMPLATE_TEXT + "}";
                NameValueCollection InsertTemplateSettings = new NameValueCollection();
                InsertTemplateSettings.Add("SendRequestType", "Post");
                InsertTemplateSettings.Add("SendParamsType", "String");
                InsertTemplateSettings.Add("ReturnContentType", "Json");
                InsertTemplateSettings.Add("OKFlag", "Code");
                InsertTemplateSettings.Add("OKValue", "0");
                InsertTemplateSettings.Add("RetrunValueKey", "tpl_id");
                InsertTemplateSettings.Add("MsgFlag", "msg");
                additional.InsertTemplateSettings = InsertTemplateSettings;

                //修改短信
                additional.UpdateTemplateUrl = "http://yunpian.com/v1/tpl/update.json";
                additional.UpdateTemplateParams = "apikey={@apikey}&tpl_content={@" + SMSServerManager.TEMPLATE_TEXT + "}&tpl_id={@" + SMSServerManager.TEMPLATE_ID + "}";
                NameValueCollection UpdateTemplateSettings = new NameValueCollection();
                UpdateTemplateSettings.Add("SendRequestType", "Post");
                UpdateTemplateSettings.Add("SendParamsType", "String");
                UpdateTemplateSettings.Add("ReturnContentType", "Json");
                UpdateTemplateSettings.Add("OKFlag", "Code");
                UpdateTemplateSettings.Add("OKValue", "0");
                UpdateTemplateSettings.Add("RetrunValueKey", "tpl_id");
                UpdateTemplateSettings.Add("MsgFlag", "msg");
                additional.UpdateTemplateSettings = UpdateTemplateSettings;

                //删除短信
                additional.DeleteTemplateUrl = "http://yunpian.com/v1/tpl/del.json";
                additional.DeleteTemplateParams = "apikey={@apikey}&tpl_id={@" + SMSServerManager.TEMPLATE_ID + "}";
                NameValueCollection DeleteTemplateSettings = new NameValueCollection();
                DeleteTemplateSettings.Add("SendRequestType", "Post");
                DeleteTemplateSettings.Add("SendParamsType", "String");
                DeleteTemplateSettings.Add("ReturnContentType", "Json");
                DeleteTemplateSettings.Add("OKFlag", "Code");
                DeleteTemplateSettings.Add("OKValue", "0");
                DeleteTemplateSettings.Add("RetrunValueKey", "");
                DeleteTemplateSettings.Add("MsgFlag", "msg");
                additional.DeleteTemplateSettings = DeleteTemplateSettings;

                //查询短信
                additional.SearchTemplateUrl = "http://yunpian.com/v1/tpl/get.json";
                additional.SearchTemplateParams = "apikey={@apikey}&tpl_id={@" + SMSServerManager.TEMPLATE_ID + "}";
                NameValueCollection SearchTemplateSettings = new NameValueCollection();
                SearchTemplateSettings.Add("SendRequestType", "Post");
                SearchTemplateSettings.Add("SendParamsType", "String");
                SearchTemplateSettings.Add("ReturnContentType", "Json");
                SearchTemplateSettings.Add("OKFlag", "Code");
                SearchTemplateSettings.Add("OKValue", "0");
                SearchTemplateSettings.Add("RetrunValueKey", "tpl_content");
                SearchTemplateSettings.Add("MsgFlag", "msg");
                additional.SearchTemplateSettings = SearchTemplateSettings;
            }
            else if (ESMSServerTypeUtils.Equals(smsServerType, ESMSServerType.WeiMi))
            {
                //发送设置
                additional.SendUrl = "http://api.weimi.cc/2/sms/send.html";
                additional.SendParams = "uid={@uid}&pas={@pas}&mob={@" + SMSServerManager.MOBILE + "}&con={@" + SMSServerManager.TEXT + "}&isAd=false&type=json";
                NameValueCollection SendSettings = new NameValueCollection();
                SendSettings.Add("SendRequestType", "Post");
                SendSettings.Add("SendParamsType", "String");
                SendSettings.Add("ReturnContentType", "Json");
                SendSettings.Add("OKFlag", "code");
                SendSettings.Add("OKValue", "0");
                SendSettings.Add("MsgFlag", "msg");
                additional.SendSettings = SendSettings;

                //查询剩余短信条数
                additional.LastSmsSearchUrl = "http://api.weimi.cc/2/account/balance.html";
                additional.LastSmsSearchParams = "uid={@uid}&pas={@pas}&type=json";
                NameValueCollection LastSmsSearchSettings = new NameValueCollection();
                LastSmsSearchSettings.Add("SendRequestType", "Post");
                LastSmsSearchSettings.Add("SendParamsType", "String");
                LastSmsSearchSettings.Add("ReturnContentType", "Json");
                LastSmsSearchSettings.Add("OKFlag", "Code");
                LastSmsSearchSettings.Add("OKValue", "0");
                LastSmsSearchSettings.Add("RetrunValueKey", "sms-left");
                LastSmsSearchSettings.Add("MsgFlag", "msg");
                additional.LastSmsSearchSettings = LastSmsSearchSettings;

            }
            return additional;

        }

        public static string GetTemplate(string templateText, ESMSServerType smsServerType)
        {
            if (ESMSServerTypeUtils.Equals(smsServerType, ESMSServerType.YunPian))
            {
                //簽名
                if (templateText.IndexOf("【") != 0)
                {
                    templateText = "【云片网】" + templateText;
                }

                templateText = templateText.Replace("[", "#");
                templateText = templateText.Replace("]", "#");
            }
            else if (ESMSServerTypeUtils.Equals(smsServerType, ESMSServerType.WeiMi))
            {
                //簽名
                if (templateText.IndexOf("【") != 0)
                {
                    templateText = "【微米】" + templateText;
                }
            }
            return templateText;
        }
    }
}
