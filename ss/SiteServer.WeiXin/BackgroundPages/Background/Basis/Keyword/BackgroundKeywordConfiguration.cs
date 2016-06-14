using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Web.UI.HtmlControls;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Model;
using SiteServer.WeiXin.Core;


namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundKeywordConfiguration : BackgroundBasePageWX
	{
        public CheckBox cbIsWelcome;
        public TextBox tbWelcomeKeyword;
        public Button btnWelcomeKeywordSelect;

        public CheckBox cbIsDefaultReply;
        public TextBox tbDefaultReplyKeyword;
        public Button btnDefaultReplyKeywordSelect;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_keywordConfiguration.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Accounts, AppManager.WeiXin.LeftMenu.Function.ID_SetReply, string.Empty, AppManager.WeiXin.Permission.WebSite.SetReply);

                AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                this.cbIsWelcome.Checked = accountInfo.IsWelcome;
                this.tbWelcomeKeyword.Text = accountInfo.WelcomeKeyword;
                this.btnWelcomeKeywordSelect.Attributes.Add("onclick", Modal.KeywordSelect.GetOpenWindowString(base.PublishmentSystemID, "selectWelcomeKeyword"));

                this.cbIsDefaultReply.Checked = accountInfo.IsDefaultReply;
                this.tbDefaultReplyKeyword.Text = accountInfo.DefaultReplyKeyword;
                this.btnDefaultReplyKeywordSelect.Attributes.Add("onclick", Modal.KeywordSelect.GetOpenWindowString(base.PublishmentSystemID, "selectDefaultReplyKeyword"));
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				try
				{
                    AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                    accountInfo.IsWelcome = this.cbIsWelcome.Checked;
                    accountInfo.WelcomeKeyword = this.tbWelcomeKeyword.Text;
                    if (string.IsNullOrEmpty(this.tbWelcomeKeyword.Text))
                    {
                        accountInfo.IsWelcome = false;
                    }
                    accountInfo.IsDefaultReply = this.cbIsDefaultReply.Checked;
                    accountInfo.DefaultReplyKeyword = this.tbDefaultReplyKeyword.Text;
                    if (string.IsNullOrEmpty(this.tbDefaultReplyKeyword.Text))
                    {
                        accountInfo.IsDefaultReply = false;
                    }

                    if (!string.IsNullOrEmpty(accountInfo.WelcomeKeyword) && !DataProviderWX.KeywordMatchDAO.IsExists(base.PublishmentSystemID, accountInfo.WelcomeKeyword))
                    {
                        base.FailMessage(string.Format("保存失败，关键词“{0}”不存在，请先在关键词回复中添加", accountInfo.WelcomeKeyword));
                        return;
                    }
                    if (!string.IsNullOrEmpty(accountInfo.DefaultReplyKeyword) && !DataProviderWX.KeywordMatchDAO.IsExists(base.PublishmentSystemID, accountInfo.DefaultReplyKeyword))
                    {
                        base.FailMessage(string.Format("保存失败，关键词“{0}”不存在，请先在关键词回复中添加", accountInfo.DefaultReplyKeyword));
                        return;
                    }

                    DataProviderWX.AccountDAO.Update(accountInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改智能回复设置");
                    base.SuccessMessage("智能回复设置配置成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "智能回复设置配置失败！");
				}
			}
		}
	}
}
