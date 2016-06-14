using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Web.UI.HtmlControls;

using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
	public class BackgroundConfigurationShipment : BackgroundBasePage
	{
        public TextBox tbShipmentPrice;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            base.BreadCrumb(AppManager.B2C.LeftMenu.ID_ConfigrationB2C, "运费设置", AppManager.B2C.Permission.WebSite.Configration);

			if (!IsPostBack)
			{
                this.tbShipmentPrice.Text = base.PublishmentSystemInfo.Additional.ShipmentPrice.ToString();
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.ShipmentPrice = TranslateUtils.ToDecimal(this.tbShipmentPrice.Text);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改运费设置");

                    base.SuccessMessage("运费设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "运费设置修改失败！");
				}
			}
		}
	}
}
