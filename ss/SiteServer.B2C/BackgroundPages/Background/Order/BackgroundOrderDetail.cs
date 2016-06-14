using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundOrderDetail : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public Literal ltlOrderSN;

        public Literal ltlOrderStatus;
        public Literal ltlPaymentStatus;
        public Literal ltlShipmentStatus;

        public Literal ltlPriceTotal;
        public Literal ltlPriceReturn;
        public Literal ltlPriceShipment;
        public Literal ltlPriceActual;

        public Repeater rptOrderItems;
        public Literal ltlAttibutesOrder;
        public Literal ltlAttibutesUser;
        public Literal ltlAttibutesConsignee;
        public PlaceHolder phInvoice;
        public Literal ltlAttibutesInvoice;
        public Literal ltlConsigneeCopy;

        protected OrderInfo orderInfo;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("OrderID");

            this.orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(base.GetIntQueryString("OrderID"));
            if (this.orderInfo == null)
            {
                base.Response.End();
                return;
            }

            if (!IsPostBack)
            {
                this.ltlPageTitle.Text = this.orderInfo.OrderSN;
                this.ltlOrderSN.Text = this.orderInfo.OrderSN;

                this.ltlOrderStatus.Text = EOrderStatusUtils.GetText(this.orderInfo.OrderStatus);
                EPaymentStatus paymentStatus = EPaymentStatusUtils.GetEnumType(this.orderInfo.PaymentStatus);
                this.ltlPaymentStatus.Text = EPaymentStatusUtils.GetText(paymentStatus);
                if (paymentStatus == EPaymentStatus.Unpaid)
                {
                    this.ltlPaymentStatus.Text = "<span style='color:red'>未支付</span>";
                }
                else if (paymentStatus == EPaymentStatus.Paid)
                {
                    this.ltlPaymentStatus.Text = "<span style='color:green'>已支付</span>";
                }
                this.ltlShipmentStatus.Text = EShipmentStatusUtils.GetText(this.orderInfo.ShipmentStatus);

                this.ltlPriceTotal.Text = this.orderInfo.PriceTotal.ToString("c");
                this.ltlPriceReturn.Text = this.orderInfo.PriceReturn.ToString("c");
                this.ltlPriceShipment.Text = this.orderInfo.PriceShipment.ToString("c");
                this.ltlPriceActual.Text = this.orderInfo.PriceActual.ToString("c");

                this.rptOrderItems.DataSource = DataProviderB2C.OrderItemDAO.GetItemInfoList(this.orderInfo.ID);
                this.rptOrderItems.ItemDataBound += new RepeaterItemEventHandler(rptOrderItems_ItemDataBound);
                this.rptOrderItems.DataBind();

                this.ltlAttibutesOrder.Text = this.GetAttributesOrderHtml();
                this.ltlAttibutesUser.Text = this.GetAttributesUserHtml();
                string consigneeSummary = string.Empty;
                this.ltlAttibutesConsignee.Text = this.GetAttributesConsigneeHtml(this.orderInfo.ConsigneeID, out consigneeSummary);

                if (this.orderInfo.InvoiceID > 0)
                {
                    this.phInvoice.Visible = true;
                    this.ltlAttibutesInvoice.Text = this.GetAttributesInvoiceHtml(this.orderInfo.InvoiceID);
                }
                else
                {
                    this.phInvoice.Visible = false;
                }

                this.ltlConsigneeCopy.Text = string.Format(@"<a class=""btn"" href=""javascript:;"" id=""order_consignee_copy"" info=""{0}"">复制收货人信息</a>", consigneeSummary);
            }
        }

        private string GetAttributesOrderHtml()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            attributes.Add("订单状态", OrderManager.GetOrderStatus(this.orderInfo));
            attributes.Add("下单时间", DateUtils.ParseThisMoment(orderInfo.TimeOrder));
            PaymentInfo paymentInfo = DataProviderB2C.PaymentDAO.GetPaymentInfo(this.orderInfo.PaymentID);
            attributes.Add("支付方式", paymentInfo.PaymentName);
            ShipmentInfo shipmentInfo = DataProviderB2C.ShipmentDAO.GetShipmentInfo(this.orderInfo.ShipmentID);
            if (shipmentInfo != null)
            {
                attributes.Add("配送方式", shipmentInfo.ShipmentName);
            }
            attributes.Add("是否开票", StringUtils.GetBoolText(this.orderInfo.InvoiceID > 0));

            StringBuilder builder = new StringBuilder();
            foreach (string key in attributes.Keys)
            {
                builder.AppendFormat(@"
<tr>
    <td class=""attributeName"">{0}：</td>
    <td>{1}</td>
</tr>
", key, attributes[key]);
            }
            return builder.ToString();
        }

        private string GetAttributesUserHtml()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(this.orderInfo.UserName))
            {
                attributes.Add("账号", "匿名用户");
            }
            else
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(base.PublishmentSystemInfo.GroupSN, this.orderInfo.UserName);

                if (userInfo != null)
                {
                    attributes.Add("账号", userInfo.UserName);
                    attributes.Add("姓名", userInfo.DisplayName);
                    attributes.Add("注册时间", DateUtils.ParseThisMoment(userInfo.CreateDate));
                    attributes.Add("注册 IP", userInfo.CreateIPAddress);
                    attributes.Add("最后活动时间", DateUtils.ParseThisMoment(userInfo.LastActivityDate));
                    attributes.Add("邮箱", userInfo.Email);
                    attributes.Add("手机", userInfo.Mobile);
                }
            }

            StringBuilder builder = new StringBuilder();
            foreach (string key in attributes.Keys)
            {
                builder.AppendFormat(@"
<tr>
    <td class=""attributeName"">{0}：</td>
    <td>{1}</td>
</tr>
", key, attributes[key]);
            }
            return builder.ToString();
        }

        private string GetAttributesConsigneeHtml(int consigneeID, out string consigneeSummary)
        {
            consigneeSummary = string.Empty;
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(consigneeID);

            if (consigneeInfo != null)
            {
                attributes.Add("收货人", consigneeInfo.Consignee);
                attributes.Add("所在地区", OrderManager.GetLocation(consigneeInfo));
                attributes.Add("详细地址", consigneeInfo.Address);
                attributes.Add("邮编", consigneeInfo.Zipcode);
                attributes.Add("手机", consigneeInfo.Mobile);
                attributes.Add("固定电话", consigneeInfo.Tel);
                attributes.Add("邮箱", consigneeInfo.Email);
            }

            StringBuilder builder = new StringBuilder();
            foreach (string key in attributes.Keys)
            {
                string value = attributes[key];
                builder.AppendFormat(@"
<tr>
    <td class=""attributeName"">{0}：</td>
    <td>{1}</td>
</tr>
", key, value);
                if (!string.IsNullOrEmpty(value))
                {
                    consigneeSummary += "," + value;
                }
            }
            if (consigneeSummary.Length > 0)
            {
                consigneeSummary = consigneeSummary.Substring(1);
            }
            return builder.ToString();
        }

        private string GetAttributesInvoiceHtml(int invoiceID)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            InvoiceInfo invoiceInfo = DataProviderB2C.InvoiceDAO.GetInvoiceInfo(invoiceID);

            if (invoiceInfo != null)
            {
                attributes.Add("发票类型", invoiceInfo.IsVat ? "增值税发票" : "普通发票");

                if (!invoiceInfo.IsVat)
                {
                    attributes.Add("发票抬头", invoiceInfo.IsCompany ? invoiceInfo.CompanyName : "个人");
                }
                else
                {
                    attributes.Add("单位名称", invoiceInfo.VatCompanyName);
                    attributes.Add("纳税人识别号", invoiceInfo.VatCode);
                    attributes.Add("注册地址", invoiceInfo.VatAddress);
                    attributes.Add("注册电话", invoiceInfo.VatPhone);
                    attributes.Add("开户银行", invoiceInfo.VatBankName);
                    attributes.Add("银行帐户", invoiceInfo.VatBankAccount);
                }

                attributes.Add("收票人姓名", invoiceInfo.ConsigneeName);
                attributes.Add("收票人手机号", invoiceInfo.ConsigneeMobile);
                attributes.Add("送票地址", invoiceInfo.ConsigneeAddress);
            }

            StringBuilder builder = new StringBuilder();
            foreach (string key in attributes.Keys)
            {
                string value = attributes[key];
                builder.AppendFormat(@"
<tr>
    <td class=""attributeName"">{0}：</td>
    <td>{1}</td>
</tr>
", key, value);
            }

            return builder.ToString();
        }

        private void rptOrderItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlGoodsSN = e.Item.FindControl("ltlGoodsSN") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlSpec = e.Item.FindControl("ltlSpec") as Literal;
                Literal ltlPrice = e.Item.FindControl("ltlPrice") as Literal;
                Literal ltlPurchaseNum = e.Item.FindControl("ltlPurchaseNum") as Literal;
                Literal ltlIsShipment = e.Item.FindControl("ltlIsShipment") as Literal;

                OrderItemInfo itemInfo = e.Item.DataItem as OrderItemInfo;

                ltlItemIndex.Text = Convert.ToString(e.Item.ItemIndex + 1);
                ltlGoodsSN.Text = itemInfo.GoodsSN;
                ltlTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtility.GetContentUrl(base.PublishmentSystemInfo, NodeManager.GetNodeInfo(base.PublishmentSystemID, itemInfo.ChannelID), itemInfo.ContentID, base.PublishmentSystemInfo.Additional.VisualType), itemInfo.Title);
                GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(base.PublishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);
                GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(itemInfo.GoodsID, contentInfo);
                ltlSpec.Text = SpecManager.GetSpecValues(base.PublishmentSystemInfo, goodsInfo);
                ltlPrice.Text = TranslateUtils.ToCurrency(itemInfo.PriceSale);
                ltlPurchaseNum.Text = itemInfo.PurchaseNum.ToString();
                ltlIsShipment.Text = StringUtils.GetBoolText(itemInfo.IsShipment);
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int orderID)
        {
            return PageUtils.GetB2CUrl(string.Format(@"background_orderDetail.aspx?PublishmentSystemID={0}&OrderID={1}", publishmentSystemID, orderID));
        }
    }
}
