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
	public class BackgroundPaymentConfiguration : BackgroundBasePage
	{
        public TextBox tbPaymentType;
        public TextBox tbCashBackType;

		public void Page_Load(object sender, EventArgs E)
		{
			if (!IsPostBack)
			{
                this.tbPaymentType.Text = TranslateUtils.ObjectCollectionToString(ConfigurationManager.Additional.PaymentTypeCollection);
                this.tbCashBackType.Text = TranslateUtils.ObjectCollectionToString(ConfigurationManager.Additional.PaymentCashBackTypeCollection);
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{				
				try
				{
                    ConfigurationManager.Additional.PaymentTypeCollection = TranslateUtils.StringCollectionToStringList(this.tbPaymentType.Text);
                    ConfigurationManager.Additional.PaymentCashBackTypeCollection = TranslateUtils.StringCollectionToStringList(this.tbCashBackType.Text);

                    DataProvider.ConfigurationDAO.Update(ConfigurationManager.Instance);

                    base.SuccessMessage("回款设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "回款设置修改失败！");
				}
			}
		}
	}
}
