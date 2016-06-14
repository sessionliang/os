using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundShipment : BackgroundBasePage
    {
        public Repeater rptContents;

        private List<ShipmentInfo> shipmentInfoList;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_shipment.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("isDelete") != null && base.GetQueryString("shipmentID") != null)
            {
                int shipmentID = base.GetIntQueryString("shipmentID");
                if (shipmentID > 0)
                {
                    DataProviderB2C.ShipmentDAO.Delete(shipmentID);
                    base.SuccessMessage("配送方式删除成功");
                }
            }
            else if (base.GetQueryString("isEnable") != null && base.GetQueryString("shipmentID") != null)
            {
                int shipmentID = base.GetIntQueryString("shipmentID");
                if (shipmentID > 0)
                {
                    ShipmentInfo shipmentInfo = DataProviderB2C.ShipmentDAO.GetShipmentInfo(shipmentID);
                    if (shipmentInfo != null)
                    {
                        string action = shipmentInfo.IsEnabled ? "禁用" : "启用";
                        shipmentInfo.IsEnabled = !shipmentInfo.IsEnabled;
                        DataProviderB2C.ShipmentDAO.Update(shipmentInfo);
                        base.SuccessMessage(string.Format("成功{0}配送方式", action));
                    }
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.B2C.LeftMenu.ID_PaymentShipment, "配送方式", string.Empty);

                this.shipmentInfoList = DataProviderB2C.ShipmentDAO.GetShipmentInfoList(base.PublishmentSystemID);

                this.rptContents.DataSource = this.shipmentInfoList;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
        }

        private void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ShipmentInfo shipmentInfo = e.Item.DataItem as ShipmentInfo;

                Literal ltlShipmentName = e.Item.FindControl("ltlShipmentName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlShipmentPeriod = e.Item.FindControl("ltlShipmentPeriod") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlConfigUrl = e.Item.FindControl("ltlConfigUrl") as Literal;
                Literal ltlIsEnabledUrl = e.Item.FindControl("ltlIsEnabledUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlShipmentName.Text = shipmentInfo.ShipmentName;
                ltlDescription.Text = StringUtils.MaxLengthText(shipmentInfo.Description, 200);
                ltlShipmentPeriod.Text = EShipmentPeriodUtils.GetText(shipmentInfo.ShipmentPeriod);
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(shipmentInfo.IsEnabled);

                string urlConfig = BackgroundShipmentAdd.GetRedirectUrl(base.PublishmentSystemID, shipmentInfo.ID);
                ltlConfigUrl.Text = string.Format(@"<a href=""{0}"">修改</a>", urlConfig);

                string action = shipmentInfo.IsEnabled ? "禁用" : "启用";

                string urlIsEnabled = BackgroundShipment.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&isEnable=True&shipmentID={0}", shipmentInfo.ID);
                ltlIsEnabledUrl.Text = string.Format(@"<a href=""{0}"">{1}</a>", urlIsEnabled, action);

                string urlDelete = BackgroundShipment.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&isDelete=True&shipmentID={0}", shipmentInfo.ID);
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onclick=""javascript:return confirm('此操作将删除选定配送方式，确认吗？');"">删除</a>", urlDelete);
            }
        }
    }
}
