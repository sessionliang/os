using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserConfigMail : BackgroundBasePage
    {
        public TextBox MailDomain;
        public TextBox MailDomainPort;
        public TextBox MailServerUserName;
        public TextBox MailServerPassword;
        public TextBox MailFromName;
        public RadioButtonList MailIsEnabled;
        public DropDownList EnableSsl;

        public TextBox TestMail;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserCenterSetting, "�û��ʼ�����", AppManager.User.Permission.Usercenter_Setting);

                EBooleanUtils.AddListItems(this.MailIsEnabled, "��", "��");
                ControlUtils.SelectListItems(this.MailIsEnabled, ConfigManager.Additional.MailIsEnabled.ToString());

                EBooleanUtils.AddListItems(this.EnableSsl, "��", "��");
                ControlUtils.SelectListItems(this.EnableSsl, ConfigManager.Additional.EnableSsl.ToString());

                this.MailDomain.Text = ConfigManager.Additional.MailDomain;
                this.MailDomainPort.Text = ConfigManager.Additional.MailDomainPort.ToString();
                this.MailFromName.Text = ConfigManager.Additional.MailFromName;
                this.MailServerUserName.Text = ConfigManager.Additional.MailServerUserName;
                this.MailServerPassword.Text = ConfigManager.Additional.MailServerPassword;
                this.MailIsEnabled.SelectedValue = ConfigManager.Additional.MailIsEnabled.ToString();
                this.MailServerPassword.Attributes.Add("value", ConfigManager.Additional.MailServerPassword);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ConfigManager.Additional.MailDomain = this.MailDomain.Text;
                ConfigManager.Additional.MailDomainPort = int.Parse(this.MailDomainPort.Text);
                ConfigManager.Additional.MailFromName = this.MailFromName.Text;
                ConfigManager.Additional.MailServerUserName = this.MailServerUserName.Text;
                ConfigManager.Additional.MailServerPassword = this.MailServerPassword.Text;
                ConfigManager.Additional.MailIsEnabled = TranslateUtils.ToBool(this.MailIsEnabled.SelectedValue);
                ConfigManager.Additional.EnableSsl = TranslateUtils.ToBool(this.EnableSsl.SelectedValue);

                try
                {
                    BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

                    string errorMessage = string.Empty;
                    if (ConfigManager.Additional.MailIsEnabled && !UserMailManager.SendMail(this.MailServerUserName.Text, "�ʼ����Ͳ���", "�ʼ����Ͳ��Գɹ���", out errorMessage))
                    {
                        ConfigManager.Additional.MailIsEnabled = false;
                        BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                        base.FailMessage("�ʼ����������ô��󣬼�⵽�޷������ʼ������������ã�");
                    }
                    else
                    {
                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�޸��û��ʼ�����");
                        base.SuccessMessage("�û��ʼ������޸ĳɹ���");
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "�û��ʼ������޸�ʧ�ܣ�");
                }
            }
        }

        public void Send_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (string.IsNullOrEmpty(this.TestMail.Text))
                {
                    base.FailMessage("������д��������ĵ�ַ��");
                    return;
                }

                string errorMessage;
                bool isSuccess = UserMailManager.SendMail(this.TestMail.Text, "�ʼ����Ͳ���", "�ʼ����Ͳ��Գɹ���", out errorMessage);

                if (isSuccess)
                {
                    base.SuccessMessage("�ʼ����ͳɹ���");
                }
                else
                {
                    base.FailMessage("�ʼ�����ʧ�ܣ�" + errorMessage);
                }
            }
        }
    }
}
