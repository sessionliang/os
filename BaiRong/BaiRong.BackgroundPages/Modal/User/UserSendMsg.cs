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
    public class UserSendMsg : BackgroundBasePage
    {
        protected TextBox MailUserNames;
        protected TextBox MailTitle;
        protected BREditor Body;

        private string from;

        public const string From_Users = "Users";
        public const string From_User = "User";

        public static string GetOpenWindowString()
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("From", From_User);
            return PageUtilityPF.GetOpenWindowStringWithCheckBoxValue("����վ����", "modal_userSendMsg.aspx", arguments, "UserIDCollection", "��ѡ�����վ���ŵĻ�Ա��");
        }

        public static string GetOpenWindowStringToUsers(string email)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("From", From_Users);
            arguments.Add("Users", email);
            return PageUtilityPF.GetOpenWindowString("����վ����", "modal_userSendMsg.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.from = base.GetQueryString("From");

            if (!IsPostBack)
            {
                ArrayList mailUserNameArrayList = new ArrayList();
                if (this.from == From_Users)
                {
                    string emails = base.GetQueryString("Users");
                    mailUserNameArrayList = TranslateUtils.StringCollectionToArrayList(emails);
                }
                else if (this.from == From_User)
                {
                    List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                    foreach (int userID in userIDList)
                    {
                        UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                        if (userInfo != null && !string.IsNullOrEmpty(userInfo.UserName))
                        {
                            if (!mailUserNameArrayList.Contains(userInfo.UserName))
                            {
                                mailUserNameArrayList.Add(userInfo.UserName);
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

                ////��ʼ����
                //string errorMessage = string.Empty;
                //bool isSuccess = smtpMail.Send(out errorMessage);
                UserMessageManager.SendSystemMessage(TranslateUtils.StringCollectionToArrayList(this.MailUserNames.Text), this.MailTitle.Text, this.Body.Text);
                base.SuccessMessage("վ���ŷ��ͳɹ���");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "վ���ŷ���ʧ�ܣ�" + ex.Message);
            }

            //JsUtils.OpenWindow.CloseModalPage(Page);
        }
    }
}
