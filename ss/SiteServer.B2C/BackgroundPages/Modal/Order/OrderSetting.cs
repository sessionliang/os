using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using System.Text;
using SiteServer.CMS.BackgroundPages;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using System.Collections.Generic;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class OrderSetting : BackgroundBasePage
    {
        public DropDownList ddlOrderStatus;
        public DropDownList ddlPaymentStatus;
        public DropDownList ddlShipmentStatus;

        private ArrayList idArrayList;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public static string GetShowPopWinString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("设置属性", "modal_orderSetting.aspx", arguments, "IDCollection", "请选择需要设置的订单！", 500, 350);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.idArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("IDCollection"));

            if (!IsPostBack)
            {
                ListItem listItem = new ListItem("<<保持不变>>", string.Empty);
                this.ddlOrderStatus.Items.Add(listItem);
                EOrderStatusUtils.AddListItems(this.ddlOrderStatus);

                listItem = new ListItem("<<保持不变>>", string.Empty);
                this.ddlPaymentStatus.Items.Add(listItem);
                EPaymentStatusUtils.AddListItems(this.ddlPaymentStatus);

                listItem = new ListItem("<<保持不变>>", string.Empty);
                this.ddlShipmentStatus.Items.Add(listItem);
                EShipmentStatusUtils.AddListItems(this.ddlShipmentStatus);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                foreach (int orderID in this.idArrayList)
                {
                    OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderID);

                    if (!string.IsNullOrEmpty(this.ddlOrderStatus.SelectedValue))
                    {
                        orderInfo.OrderStatus = this.ddlOrderStatus.SelectedValue;
                    }

                    if (!string.IsNullOrEmpty(this.ddlPaymentStatus.SelectedValue))
                    {
                        orderInfo.PaymentStatus = this.ddlPaymentStatus.SelectedValue;
                    }

                    if (!string.IsNullOrEmpty(this.ddlShipmentStatus.SelectedValue))
                    {
                        orderInfo.ShipmentStatus = this.ddlShipmentStatus.SelectedValue;
                        if (EShipmentStatusUtils.Equals(this.ddlShipmentStatus.SelectedValue, EShipmentStatus.Shipment))
                        {
                            //发货，减少库存
                            List<OrderItemInfo> orderItemList = DataProviderB2C.OrderItemDAO.GetItemInfoList(orderInfo.ID);
                            foreach (OrderItemInfo orderItem in orderItemList)
                            {
                                GoodsContentInfo goodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(base.PublishmentSystemInfo, orderItem.ChannelID, orderItem.ContentID);
                                GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(orderItem.GoodsID, goodsContentInfo);
                                if (goodsContentInfo.Stock != -1 || goodsInfo.Stock != -1)//库存为-1的时候，不限制数量，同时也不处理数量
                                {
                                    if (goodsContentInfo.Stock > orderItem.PurchaseNum && goodsInfo.Stock > orderItem.PurchaseNum)
                                    {
                                        goodsContentInfo.Stock = goodsContentInfo.Stock - orderItem.PurchaseNum;
                                        goodsInfo.Stock = goodsInfo.Stock - orderItem.PurchaseNum;
                                        DataProviderB2C.GoodsDAO.Update(goodsInfo);
                                        DataProviderB2C.GoodsContentDAO.UpdateGoodContentCount(base.PublishmentSystemInfo.AuxiliaryTableForGoods, orderItem.ContentID, goodsContentInfo.Stock);
                                    }
                                    else
                                    {
                                        base.FailMessage("商品库存数量不足！");
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    DataProviderB2C.OrderDAO.Update(orderInfo);
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
                isChanged = false;
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }

    }
}
