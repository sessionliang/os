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
    public class BackgroundGovPublicApplyConfiguration : BackgroundGovPublicBasePage
	{
        public TextBox tbGovPublicApplyDateLimit;
        public RadioButtonList rblGovPublicApplyAlertDateIsAfter;
        public TextBox tbGovPublicApplyAlertDate;
        public TextBox tbGovPublicApplyYellowAlertDate;
        public TextBox tbGovPublicApplyRedAlertDate;
        public RadioButtonList rblGovPublicApplyIsDeleteAllowed;

		public void Page_Load(object sender, EventArgs E)
		{
            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicApplyConfiguration, "依申请公开设置", AppManager.CMS.Permission.WebSite.GovPublicApplyConfiguration);

                this.tbGovPublicApplyDateLimit.Text = base.PublishmentSystemInfo.Additional.GovPublicApplyDateLimit.ToString();
                this.rblGovPublicApplyAlertDateIsAfter.SelectedValue = (base.PublishmentSystemInfo.Additional.GovPublicApplyAlertDate > 0).ToString();
                int alertDate = base.PublishmentSystemInfo.Additional.GovPublicApplyAlertDate;
                if (alertDate < 0) alertDate = -alertDate;
                this.tbGovPublicApplyAlertDate.Text = alertDate.ToString();
                this.tbGovPublicApplyYellowAlertDate.Text = base.PublishmentSystemInfo.Additional.GovPublicApplyYellowAlertDate.ToString();
                this.tbGovPublicApplyRedAlertDate.Text = base.PublishmentSystemInfo.Additional.GovPublicApplyRedAlertDate.ToString();
                this.rblGovPublicApplyIsDeleteAllowed.SelectedValue = base.PublishmentSystemInfo.Additional.GovPublicApplyIsDeleteAllowed.ToString();
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.GovPublicApplyDateLimit = TranslateUtils.ToInt(this.tbGovPublicApplyDateLimit.Text);
                int alertDate = TranslateUtils.ToInt(this.tbGovPublicApplyAlertDate.Text);
                if (!TranslateUtils.ToBool(this.rblGovPublicApplyAlertDateIsAfter.SelectedValue))
                {
                    alertDate = -alertDate;
                }
                base.PublishmentSystemInfo.Additional.GovPublicApplyAlertDate = alertDate;
                base.PublishmentSystemInfo.Additional.GovPublicApplyYellowAlertDate = TranslateUtils.ToInt(this.tbGovPublicApplyYellowAlertDate.Text);
                base.PublishmentSystemInfo.Additional.GovPublicApplyRedAlertDate = TranslateUtils.ToInt(this.tbGovPublicApplyRedAlertDate.Text);
                base.PublishmentSystemInfo.Additional.GovPublicApplyIsDeleteAllowed = TranslateUtils.ToBool(this.rblGovPublicApplyIsDeleteAllowed.SelectedValue);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改依申请公开设置");

                    base.SuccessMessage("依申请公开设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "依申请公开设置修改失败！");
				}
			}
		}
	}
}
