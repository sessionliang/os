using System;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace SiteServer.CMS.Model
{
    public interface ITagStyleMailSMSBaseInfo
    {
        bool IsMail { get; set; }

        ETriState MailReceiver { get; set; }

        string MailTo { get; set; }

        string MailFiledName { get; set; }

        string MailTitle { get; set; }

        bool IsMailTemplate { get; set; }

        string MailContent { get; set; }

        bool IsSMS { get; set; }

        ETriState SMSReceiver { get; set; }

        string SMSTo { get; set; }

        string SMSFiledName { get; set; }

        bool IsSMSTemplate { get; set; }

        string SMSContent { get; set; }
    }
}
