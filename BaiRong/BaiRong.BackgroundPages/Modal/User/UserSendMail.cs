using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;

using BaiRong.Controls;


using System.Collections.Generic;

namespace BaiRong.BackgroundPages.Modal
{
    public class UserSendMail : BackgroundBasePage
    {
        protected TextBox MailUserNames;
        protected TextBox MailTitle;
        protected BREditor Body;

        private string from;

        public const string From_Emails = "Emails";
        public const string From_User = "User";

        public static string GetOpenWindowString()
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("From", From_User);
            return PageUtilityPF.GetOpenWindowStringWithCheckBoxValue("发送邮件", "modal_userSendMail.aspx", arguments, "UserIDCollection", "请选择接收邮件的会员！");
        }

        public static string GetOpenWindowStringToEmails(string email)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("From", From_Emails);
            arguments.Add("Emails", email);
            return PageUtilityPF.GetOpenWindowString("发送邮件", "modal_userSendMail.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.from = base.GetQueryString("From");

            if (!IsPostBack)
            {
                ArrayList mailUserNameArrayList = new ArrayList();
                if (this.from == From_Emails)
                {
                    string emails = base.GetQueryString("Emails");
                    mailUserNameArrayList = TranslateUtils.StringCollectionToArrayList(emails);
                }
                else if (this.from == From_User)
                {
                    List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                    foreach (int userID in userIDList)
                    {
                        UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                        if (userInfo != null && !string.IsNullOrEmpty(userInfo.Email))
                        {
                            if (!mailUserNameArrayList.Contains(userInfo.Email))
                            {
                                mailUserNameArrayList.Add(userInfo.Email);
                            }
                        }
                    }
                }

                this.MailUserNames.Text = TranslateUtils.ObjectCollectionToString(mailUserNameArrayList, ",");
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {

                //ISmtpMail smtpMail = MailUtils.GetInstance();
                //string[] usernames = this.MailUserNames.Text.Split(new char[] { ',' });
                //smtpMail.AddRecipient(usernames);

                //smtpMail.MailDomainPort = UserConfigManager.Additional.MailDomainPort;
                //smtpMail.IsHtml = true;
                //smtpMail.Subject = this.MailTitle.Text;

                //smtpMail.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + this.Body.Text + "</pre>";
                //smtpMail.MailDomain = UserConfigManager.Additional.MailDomain;
                //smtpMail.MailServerUserName = UserConfigManager.Additional.MailServerUserName;
                //smtpMail.MailServerPassword = UserConfigManager.Additional.MailServerPassword;

                ////开始发送
                //string errorMessage = string.Empty;
                //bool isSuccess = smtpMail.Send(out errorMessage);
                string errorMessage = string.Empty;
                bool isSuccess = UserMailManager.SendMail(this.MailUserNames.Text, this.MailTitle.Text, this.Body.Text, out errorMessage);
                if (isSuccess)
                {
                    base.SuccessMessage("邮件发送成功！");
                }
                else
                {
                    base.FailMessage("邮件发送失败：" + errorMessage);
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "邮件发送失败：" + ex.Message);
            }

            //JsUtils.OpenWindow.CloseModalPage(Page);
        }
    }
}
