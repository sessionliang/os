using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Web.UI.HtmlControls;


namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovInteractConfigurationAlert : BackgroundGovInteractBasePage
	{
        public TextBox tbGovInteractApplyDateLimit;
        public RadioButtonList rblGovInteractApplyAlertDateIsAfter;
        public TextBox tbGovInteractApplyAlertDate;
        public TextBox tbGovInteractApplyYellowAlertDate;
        public TextBox tbGovInteractApplyRedAlertDate;
        public RadioButtonList rblGovInteractApplyIsDeleteAllowed;

		public void Page_Load(object sender, EventArgs E)
		{
            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, AppManager.CMS.LeftMenu.GovInteract.ID_GovInteractConfiguration, "办件预警设置", AppManager.CMS.Permission.WebSite.GovInteractConfiguration);

                this.tbGovInteractApplyDateLimit.Text = base.PublishmentSystemInfo.Additional.GovInteractApplyDateLimit.ToString();
                this.rblGovInteractApplyAlertDateIsAfter.SelectedValue = (base.PublishmentSystemInfo.Additional.GovInteractApplyAlertDate > 0).ToString();
                int alertDate = base.PublishmentSystemInfo.Additional.GovInteractApplyAlertDate;
                if (alertDate < 0) alertDate = -alertDate;
                this.tbGovInteractApplyAlertDate.Text = alertDate.ToString();
                this.tbGovInteractApplyYellowAlertDate.Text = base.PublishmentSystemInfo.Additional.GovInteractApplyYellowAlertDate.ToString();
                this.tbGovInteractApplyRedAlertDate.Text = base.PublishmentSystemInfo.Additional.GovInteractApplyRedAlertDate.ToString();
                this.rblGovInteractApplyIsDeleteAllowed.SelectedValue = base.PublishmentSystemInfo.Additional.GovInteractApplyIsDeleteAllowed.ToString();
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.GovInteractApplyDateLimit = TranslateUtils.ToInt(this.tbGovInteractApplyDateLimit.Text);
                int alertDate = TranslateUtils.ToInt(this.tbGovInteractApplyAlertDate.Text);
                if (!TranslateUtils.ToBool(this.rblGovInteractApplyAlertDateIsAfter.SelectedValue))
                {
                    alertDate = -alertDate;
                }
                base.PublishmentSystemInfo.Additional.GovInteractApplyAlertDate = alertDate;
                base.PublishmentSystemInfo.Additional.GovInteractApplyYellowAlertDate = TranslateUtils.ToInt(this.tbGovInteractApplyYellowAlertDate.Text);
                base.PublishmentSystemInfo.Additional.GovInteractApplyRedAlertDate = TranslateUtils.ToInt(this.tbGovInteractApplyRedAlertDate.Text);
                base.PublishmentSystemInfo.Additional.GovInteractApplyIsDeleteAllowed = TranslateUtils.ToBool(this.rblGovInteractApplyIsDeleteAllowed.SelectedValue);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改互动交流设置");

                    base.SuccessMessage("互动交流设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "互动交流设置修改失败！");
				}
			}
		}
	}
}
