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
    public class UserSendSMS : BackgroundBasePage
    {
        protected TextBox MailUserNames;
        protected BREditor Body;

        private string from;

        public const string From_Mobiles = "Mobiles";
        public const string From_User = "User";

        public static string GetOpenWindowString()
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("From", From_User);
            return PageUtilityPF.GetOpenWindowStringWithCheckBoxValue("���Ͷ���", "modal_userSendSMS.aspx", arguments, "UserIDCollection", "��ѡ����ն��ŵĻ�Ա��");
        }

        public static string GetOpenWindowStringToMobiles(string email)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("From", From_Mobiles);
            arguments.Add("Mobiles", email);
            return PageUtilityPF.GetOpenWindowString("���Ͷ���", "modal_userSendSMS.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.from = base.GetQueryString("From");

            if (!IsPostBack)
            {
                ArrayList mailUserNameArrayList = new ArrayList();
                if (this.from == From_Mobiles)
                {
                    string emails = base.GetQueryString("Mobiles");
                    mailUserNameArrayList = TranslateUtils.StringCollectionToArrayList(emails);
                }
                else if (this.from == From_User)
                {
                    List<int> userIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserIDCollection"));
                    foreach (int userID in userIDList)
                    {
                        UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                        if (userInfo != null && !string.IsNullOrEmpty(userInfo.Mobile))
                        {
                            if (!mailUserNameArrayList.Contains(userInfo.Mobile))
                            {
                                mailUserNameArrayList.Add(userInfo.Mobile);
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

                if (!SMSServerManager.IsEnabled)
                {
                    base.FailMessage("�������úö��ŷ����̲�����");
                    return;
                }

                string errorMessage = string.Empty;

                bool isSuccess = SMSServerManager.Send(this.MailUserNames.Text, this.Body.Text, out errorMessage);
                if (isSuccess)
                {
                    base.SuccessMessage("���ŷ��ͳɹ���");
                }
                else
                {
                    base.FailMessage("���ŷ���ʧ�ܣ�" + errorMessage);
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "���ŷ���ʧ�ܣ�" + ex.Message);
            }

            //JsUtils.OpenWindow.CloseModalPage(Page);
        }
    }
}
