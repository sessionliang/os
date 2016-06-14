using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;


using BaiRong.Controls;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundShipmentAdd : BackgroundBasePage
	{
        public Literal ltlPageTitle;

        public TextBox tbShipmentName;
        public RadioButtonList rblShipmentPeriod;
        public BREditor breDescription;

        private int shipmentID;

        public static string GetRedirectUrl(int publishmentSystemID, int shipmentID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_shipmentAdd.aspx?publishmentSystemID={0}&shipmentID={1}", publishmentSystemID, shipmentID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.shipmentID = base.GetIntQueryString("shipmentID");

			if (!IsPostBack)
			{
                string pageTitle = this.shipmentID == 0 ? "添加配送方式" : "修改配送方式";
                base.BreadCrumbConsole(AppManager.B2C.LeftMenu.ID_PaymentShipment, pageTitle, string.Empty);
                this.ltlPageTitle.Text = pageTitle;

                EShipmentPeriodUtils.AddListItems(this.rblShipmentPeriod);
                ControlUtils.SelectListItems(this.rblShipmentPeriod, EShipmentPeriodUtils.GetValue(EShipmentPeriod.All));

                if (this.shipmentID > 0)
                {
                    ShipmentInfo shipmentInfo = DataProviderB2C.ShipmentDAO.GetShipmentInfo(this.shipmentID);
                    if (shipmentInfo != null)
                    {
                        this.tbShipmentName.Text = shipmentInfo.ShipmentName;
                        ControlUtils.SelectListItems(this.rblShipmentPeriod, EShipmentPeriodUtils.GetValue(shipmentInfo.ShipmentPeriod));
                        this.breDescription.Text = shipmentInfo.Description;
                    }
                }
			}
		}

        public override void Submit_OnClick(object sender, System.EventArgs e)
		{
            try
            {
                if (this.shipmentID > 0)
                {
                    ShipmentInfo shipmentInfo = DataProviderB2C.ShipmentDAO.GetShipmentInfo(this.shipmentID);

                    shipmentInfo.ShipmentName = this.tbShipmentName.Text;
                    shipmentInfo.ShipmentPeriod = EShipmentPeriodUtils.GetEnumType(this.rblShipmentPeriod.SelectedValue);
                    shipmentInfo.Description = this.breDescription.Text;

                    DataProviderB2C.ShipmentDAO.Update(shipmentInfo);
                    base.SuccessMessage("修改配送方式成功！");
                }
                else
                {
                    ShipmentInfo shipmentInfo = new ShipmentInfo(this.shipmentID, base.PublishmentSystemID, this.tbShipmentName.Text, EShipmentPeriodUtils.GetEnumType(this.rblShipmentPeriod.SelectedValue), true, 0, this.breDescription.Text);

                    DataProviderB2C.ShipmentDAO.Insert(shipmentInfo);
                    base.SuccessMessage("添加配送方式成功！");
                }

                base.AddWaitAndRedirectScript(BackgroundShipment.GetRedirectUrl(base.PublishmentSystemID));
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
		}
	}
}
