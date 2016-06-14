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
	public class BackgroundMailSubscribeFormat : BackgroundBasePage
	{
        public TextBox MailSubscribeTitle;
        public TextBox MailSubscribeContent;

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Mail, "邮件格式设置", AppManager.CMS.Permission.WebSite.Mail);

                this.MailSubscribeTitle.Text = base.PublishmentSystemInfo.Additional.MailSubscribeTitle;
                this.MailSubscribeContent.Text = base.PublishmentSystemInfo.Additional.MailSubscribeContent;
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.MailSubscribeTitle = this.MailSubscribeTitle.Text;
                base.PublishmentSystemInfo.Additional.MailSubscribeContent = this.MailSubscribeContent.Text;
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改邮件订阅格式");

                    base.SuccessMessage("邮件订阅格式修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "邮件订阅格式修改失败！");
				}
			}
		}
	}
}
