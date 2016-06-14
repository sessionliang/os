using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Web.UI.HtmlControls;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundMailSendFormat : BackgroundBasePage
	{
        public TextBox MailSendTitle;
        public TextBox MailSendContent;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Mail, "邮件格式设置", AppManager.CMS.Permission.WebSite.Mail);

                this.MailSendTitle.Text = base.PublishmentSystemInfo.Additional.MailSendTitle;
                this.MailSendContent.Text = base.PublishmentSystemInfo.Additional.MailSendContent;
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.MailSendTitle = this.MailSendTitle.Text;
                base.PublishmentSystemInfo.Additional.MailSendContent = this.MailSendContent.Text;
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改推荐好友邮件格式");

                    base.SuccessMessage("推荐好友邮件格式修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "推荐好友邮件格式修改失败！");
				}
			}
		}
	}
}
