using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationMail : BackgroundBasePage
	{
        public TextBox MailDomain;
        public TextBox MailDomainPort;
        public TextBox MailFromName;
        public TextBox MailServerUserName;
        public TextBox MailServerPassword;

        public TextBox TestMail;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "邮件发送设置", AppManager.CMS.Permission.WebSite.Configration);

                this.MailDomain.Text = base.PublishmentSystemInfo.Additional.MailDomain;
                this.MailDomainPort.Text = base.PublishmentSystemInfo.Additional.MailDomainPort.ToString();
                this.MailFromName.Text = base.PublishmentSystemInfo.Additional.MailFromName;
                this.MailServerUserName.Text = base.PublishmentSystemInfo.Additional.MailServerUserName;
                this.MailServerPassword.Text = base.PublishmentSystemInfo.Additional.MailServerPassword;
                this.MailServerPassword.Attributes.Add("value", base.PublishmentSystemInfo.Additional.MailServerPassword);
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.MailDomain = this.MailDomain.Text;
                base.PublishmentSystemInfo.Additional.MailDomainPort = int.Parse(this.MailDomainPort.Text);
                base.PublishmentSystemInfo.Additional.MailFromName = this.MailFromName.Text;
                base.PublishmentSystemInfo.Additional.MailServerUserName = this.MailServerUserName.Text;
                base.PublishmentSystemInfo.Additional.MailServerPassword = this.MailServerPassword.Text;

                try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改邮件发送设置");

                    base.SuccessMessage("邮件设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "邮件设置修改失败！");
				}
			}
		}

        public void Send_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (string.IsNullOrEmpty(this.TestMail.Text))
                {
                    base.FailMessage("必须填写发送邮箱的地址！");
                    return;
                }

                string errorMessage;
                bool isSuccess = MessageManager.SendMailByPublishmentSystemID(base.PublishmentSystemID, this.TestMail.Text, "邮件发送测试", "邮件发送测试成功！", out errorMessage);

                if (isSuccess)
                {
                    base.SuccessMessage("邮件发送成功！");
                }
                else
                {
                    base.FailMessage("邮件发送失败：" + errorMessage);
                }
            }
        }
	}
}
