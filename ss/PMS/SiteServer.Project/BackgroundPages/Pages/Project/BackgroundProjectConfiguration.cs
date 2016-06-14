using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Collections;
using System.Web.UI.HtmlControls;


namespace SiteServer.Project.BackgroundPages
{
	public class BackgroundProjectConfiguration : BackgroundBasePage
	{
        public TextBox tbProjectType;

        public TextBox tbApplyDateLimit;
        public RadioButtonList rblApplyAlertDateIsAfter;
        public TextBox tbApplyAlertDate;
        public TextBox tbApplyYellowAlertDate;
        public TextBox tbApplyRedAlertDate;
        public RadioButtonList rblApplyIsDeleteAllowed;

		public void Page_Load(object sender, EventArgs E)
		{
			if (!IsPostBack)
			{
                this.tbProjectType.Text = TranslateUtils.ObjectCollectionToString(ConfigurationManager.Additional.ProjectTypeCollection);

                this.tbApplyDateLimit.Text = ConfigurationManager.Additional.ProjectApplyDateLimit.ToString();
                this.rblApplyAlertDateIsAfter.SelectedValue = (ConfigurationManager.Additional.ProjectApplyAlertDate > 0).ToString();
                int alertDate = ConfigurationManager.Additional.ProjectApplyAlertDate;
                if (alertDate < 0) alertDate = -alertDate;
                this.tbApplyAlertDate.Text = alertDate.ToString();
                this.tbApplyYellowAlertDate.Text = ConfigurationManager.Additional.ProjectApplyYellowAlertDate.ToString();
                this.tbApplyRedAlertDate.Text = ConfigurationManager.Additional.ProjectApplyRedAlertDate.ToString();
                this.rblApplyIsDeleteAllowed.SelectedValue = ConfigurationManager.Additional.ProjectApplyIsDeleteAllowed.ToString();
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                ConfigurationManager.Additional.ProjectTypeCollection = TranslateUtils.StringCollectionToStringList(this.tbProjectType.Text);

                ConfigurationManager.Additional.ProjectApplyDateLimit = TranslateUtils.ToInt(this.tbApplyDateLimit.Text);
                int alertDate = TranslateUtils.ToInt(this.tbApplyAlertDate.Text);
                if (!TranslateUtils.ToBool(this.rblApplyAlertDateIsAfter.SelectedValue))
                {
                    alertDate = -alertDate;
                }
                ConfigurationManager.Additional.ProjectApplyAlertDate = alertDate;
                ConfigurationManager.Additional.ProjectApplyYellowAlertDate = TranslateUtils.ToInt(this.tbApplyYellowAlertDate.Text);
                ConfigurationManager.Additional.ProjectApplyRedAlertDate = TranslateUtils.ToInt(this.tbApplyRedAlertDate.Text);
                ConfigurationManager.Additional.ProjectApplyIsDeleteAllowed = TranslateUtils.ToBool(this.rblApplyIsDeleteAllowed.SelectedValue);
				
				try
				{
                    DataProvider.ConfigurationDAO.Update(ConfigurationManager.Instance);

                    base.SuccessMessage("项目设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "项目设置修改失败！");
				}
			}
		}
	}
}
