using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System;
using BaiRong.Core.Net;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;

namespace BaiRong.Core
{
    public class UserMailManager
    {
        private UserMailManager()
        {
        }

        public static bool SendMail(string emailList, string mailTitle, string mailBody, out string errorMessage)
        {
            try
            {
                ISmtpMail smtpMail = MailUtils.GetInstance();
                string[] usernames = emailList.Split(new char[] { ',', ';' });
                smtpMail.AddRecipient(usernames);

                smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                smtpMail.IsHtml = true;
                smtpMail.Subject = mailTitle;

                smtpMail.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + mailBody + "</pre>";
                smtpMail.MailDomain = ConfigManager.Additional.MailDomain;
                smtpMail.MailFromName = ConfigManager.Additional.MailFromName;
                smtpMail.MailServerUserName = ConfigManager.Additional.MailServerUserName;
                smtpMail.MailServerPassword = ConfigManager.Additional.MailServerPassword;
                smtpMail.EnabledSsl = ConfigManager.Additional.EnableSsl;

                return smtpMail.Send(out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
