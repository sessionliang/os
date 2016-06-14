using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundOrderListAll : BackgroundBasePage
    {
        public HyperLink hlSetting;

        public DropDownList ddlPublishmentSystem;

        public DropDownList ddlOrderStatus;
        public DropDownList ddlPaymentStatus;
        public DropDownList ddlShipmentStatus;
        public TextBox tbOrderSN;
        public TextBox tbKeyword;

        public Repeater rptContents;
        public SqlPager spContents;

        private Hashtable typeHashtable = new Hashtable();

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("Delete")))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(base.GetQueryString("IDCollection"));
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderB2C.OrderDAO.Delete(list);
                        base.SuccessMessage("订单删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "订单删除失败！");
                    }
                }
            }
            if (!string.IsNullOrEmpty(base.GetQueryString("UpdateState")))
            {
                int orderID = base.GetIntQueryString("OrderID");
                if (orderID > 0)
                {
                    try
                    {
                        OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderID);
                        orderInfo.OrderStatus = EOrderStatusUtils.GetValue(EOrderStatus.Canceled);
                        DataProviderB2C.OrderDAO.Update(orderInfo);
                        base.SuccessMessage("订单删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "订单删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProviderB2C.OrderDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_SiteSettings, "订单列表", AppManager.Platform.Permission.Platform_SiteSettings);

                ListItem listItem = new ListItem("全部", string.Empty);
                this.ddlOrderStatus.Items.Add(listItem);
                EOrderStatusUtils.AddListItems(this.ddlOrderStatus);
                if (!string.IsNullOrEmpty(base.GetQueryString("orderStatus")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlOrderStatus, base.GetQueryString("orderStatus"));
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlPaymentStatus.Items.Add(listItem);
                EPaymentStatusUtils.AddListItems(this.ddlPaymentStatus);
                if (!string.IsNullOrEmpty(base.GetQueryString("paymentStatus")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlPaymentStatus, base.GetQueryString("paymentStatus"));
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlShipmentStatus.Items.Add(listItem);
                EShipmentStatusUtils.AddListItems(this.ddlShipmentStatus);
                if (!string.IsNullOrEmpty(base.GetQueryString("shipmentStatus")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlShipmentStatus, base.GetQueryString("shipmentStatus"));
                }


                #region 绑定站点信息
                //绑定站点信息
                ArrayList publishmentSystemIDArray = new ArrayList();
                publishmentSystemIDArray.AddRange(PublishmentSystemManager.GetPublishmentSystemIDArrayList(EPublishmentSystemType.B2C));
                publishmentSystemIDArray.AddRange(PublishmentSystemManager.GetPublishmentSystemIDArrayList(EPublishmentSystemType.WeixinB2C));
                listItem = new ListItem("全部", string.Empty);
                this.ddlPublishmentSystem.Items.Add(listItem);
                PublishmentSystemInfo publishmentSystemInfo = null;
                foreach (int publishmentSystemID in publishmentSystemIDArray)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    if (publishmentSystemInfo != null)
                    {
                        listItem = new ListItem(publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString());
                        this.ddlPublishmentSystem.Items.Add(listItem);
                    }
                }
                if (!string.IsNullOrEmpty(base.GetQueryString("publishmentSystemID")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlPublishmentSystem, base.GetQueryString("publishmentSystemID"));
                }
                #endregion

                this.tbOrderSN.Text = base.GetQueryString("orderSN");
                this.tbKeyword.Text = base.GetQueryString("keyword");

                this.spContents.DataBind();

                if (this.hlSetting != null)
                {
                    this.hlSetting.Attributes.Add("onclick", Modal.OrderSetting.GetShowPopWinString(base.PublishmentSystemID));
                }
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderInfo orderInfo = new OrderInfo(e.Item.DataItem);

                Literal ltlTr = e.Item.FindControl("ltlTr") as Literal;
                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlOrderSN = e.Item.FindControl("ltlOrderSN") as Literal;
                Literal ltlPriceActual = e.Item.FindControl("ltlPriceActual") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlOrderStatus = e.Item.FindControl("ltlOrderStatus") as Literal;
                Literal ltlPaymentStatus = e.Item.FindControl("ltlPaymentStatus") as Literal;
                Literal ltlShipmentStatus = e.Item.FindControl("ltlShipmentStatus") as Literal;
                Literal ltlTimeOrder = e.Item.FindControl("ltlTimeOrder") as Literal;
                Literal ltlTimePayment = e.Item.FindControl("ltlTimePayment") as Literal;
                Literal ltlBtnOperate = e.Item.FindControl("ltlBtnOperate") as Literal;


                ltlTr.Text = @"<tr>";
                if (!EOrderStatusUtils.Equals(orderInfo.OrderStatus, EOrderStatus.Handling))
                {
                    ltlTr.Text = @"<tr class=""success"">";
                }

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlOrderSN.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", BackgroundOrderDetail.GetRedirectUrl(base.PublishmentSystemID, orderInfo.ID), orderInfo.OrderSN);

                ltlPriceActual.Text = orderInfo.PriceActual.ToString("c");
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(orderInfo.PublishmentSystemID);
                string groupSN = publishmentSystemInfo != null ? publishmentSystemInfo.GroupSN : string.Empty;
                ltlUserName.Text = OrderManager.GetOrderUser(groupSN, orderInfo.UserName);
                ltlOrderStatus.Text = EOrderStatusUtils.GetText(orderInfo.OrderStatus);

                EPaymentStatus paymentStatus = EPaymentStatusUtils.GetEnumType(orderInfo.PaymentStatus);
                ltlPaymentStatus.Text = EPaymentStatusUtils.GetText(paymentStatus);
                if (paymentStatus == EPaymentStatus.Unpaid)
                {
                    ltlPaymentStatus.Text = "<span style='color:red'>未支付</span>";
                }
                else if (paymentStatus == EPaymentStatus.Paid)
                {
                    ltlPaymentStatus.Text = "<span style='color:green'>已支付</span>";
                    ltlTimePayment.Text = DateUtils.ParseThisMoment(orderInfo.TimePayment);
                }

                ltlShipmentStatus.Text = EShipmentStatusUtils.GetText(orderInfo.ShipmentStatus);

                ltlTimeOrder.Text = DateUtils.ParseThisMoment(orderInfo.TimeOrder);

                string urlDelete = PageUtils.AddQueryString(BackgroundOrderListAll.GetRedirectUrl(base.PublishmentSystemID, orderInfo.ID), "UpdateState", "True");

                ltlBtnOperate.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">删除订单</a>", JsUtils.GetRedirectStringWithConfirm(urlDelete, "此操作将删除所选订单信息，确认吗？"));
            }
        }

        protected string GetSelectString()
        {
            if (!string.IsNullOrEmpty(base.GetQueryString("orderStatus")) || !string.IsNullOrEmpty(base.GetQueryString("paymentStatus")) || !string.IsNullOrEmpty(base.GetQueryString("shipmentStatus")) || !string.IsNullOrEmpty(base.GetQueryString("typeID")) || !string.IsNullOrEmpty(base.GetQueryString("addUserName")) || !string.IsNullOrEmpty(base.GetQueryString("userName")) || !string.IsNullOrEmpty(base.GetQueryString("keyword")))
            {
                return DataProviderB2C.OrderDAO.GetSelectString(base.PublishmentSystemID, base.GetQueryString("orderStatus"), base.GetQueryString("paymentStatus"), base.GetQueryString("shipmentStatus"), base.GetQueryString("orderSN"), base.GetQueryString("keyword"));
            }
            else
            {
                return DataProviderB2C.OrderDAO.GetSelectString(base.PublishmentSystemID);
            }
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = BackgroundOrderListAll.GetRedirectUrl(TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue), this.ddlOrderStatus.SelectedValue, this.ddlPaymentStatus.SelectedValue, this.ddlShipmentStatus.SelectedValue, this.tbOrderSN.Text, this.tbKeyword.Text, TranslateUtils.ToInt(base.GetQueryString("page"), 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_orderListAll.aspx?PublishmentSystemID={0}", publishmentSystemID));
        }

        public static string GetRedirectUrl(int publishmentSystemID, int orderID)
        {
            return PageUtils.GetB2CUrl(string.Format("background_orderListAll.aspx?PublishmentSystemID={0}&OrderID={1}", publishmentSystemID, orderID));
        }

        private static string GetRedirectUrl(int publishmentSystemID, string orderStatus, string paymentStatus, string shipmentStatus, string orderSN, string keyword, int page)
        {
            return PageUtils.GetB2CUrl(string.Format("background_orderListAll.aspx?PublishmentSystemID={0}&orderStatus={1}&paymentStatus={2}&shipmentStatus={3}&orderSN={4}&keyword={5}&page={6}", publishmentSystemID, orderStatus, paymentStatus, shipmentStatus, orderSN, keyword, page));
        }
    }
}
