using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;
using System;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.B2C.Model;
using System.Text;

namespace SiteServer.STL.Parser.StlEntity
{
    public class StlB2CEntities
    {
        private StlB2CEntities()
        {
        }

        public const string EntityName = "B2C";                  //商品图片实体

        //cart
        public static string PurchaseNumAll = "purchaseNumAll";
        public static string PriceTotalAll = "priceTotalAll";
        public static string PriceReturnAll = "priceReturnAll";
        public static string PriceShipmentAll = "priceShipmentAll";
        public static string PriceActualAll = "priceActualAll";
        public static string ClickMinusCart = "clickMinusCart";
        public static string ClickPlusCart = "clickPlusCart";
        public static string ClickUpdateCart = "clickUpdateCart";
        public static string ClickDeleteCart = "clickDeleteCart";
        public static string ClickSubmitCart = "clickSubmitCart";
        public static string Cart_PriceTotal = "cart.priceTotal";
        //contentTemplate
        public static string PriceMarket = "priceMarket";
        public static string PriceSale = "priceSale";
        public static string PriceSaved = "priceSaved";
        public static string Stock = "stock";
        public static string PurchaseNum = "PurchaseNum";
        public static string ClickSelectSpec = "clickSelectSpec";
        public static string ClickBuy = "clickBuy";
        public static string ClickAddToCart = "clickAddToCart";
        public static string ClickAddToCartForFilter = "clickAddToCartForFilter";
        public static string ClickAddToFollow = "clickAddToFollow";
        //order
        public static string ClickSubmitOrder = "clickSubmitOrder";
        public static string ClickSelectConsignee = "clickSelectConsignee";
        public static string ClickChangeConsignee = "clickChangeConsignee";
        public static string ClickEditConsignee = "clickEditConsignee";
        public static string ClickRemoveConsignee = "clickRemoveConsignee";
        public static string ClickAddConsignee = "clickAddConsignee";
        public static string ClickSaveConsignee = "clickSaveConsignee";
        public static string ClickSelectPaymentShipment = "clickSelectPaymentShipment";
        public static string ClickChangePayment = "clickChangePayment";
        public static string ClickChangeShipment = "clickChangeShipment";
        public static string ClickSavePaymentShipment = "clickSavePaymentShipment";
        public static string ClickSelectInvoice = "clickSelectInvoice";
        public static string ClickChangeInvoice = "clickChangeInvoice";
        public static string ClickEditInvoice = "clickEditInvoice";
        public static string ClickRemoveInvoice = "clickRemoveInvoice";
        public static string ClickAddInvoice = "clickAddInvoice";
        public static string ClickSaveInvoice = "clickSaveInvoice";
        public static string ClickVatInvoice = "clickVatInvoice";
        public static string ClickNotVatInvoice = "clickNotVatInvoice";
        public static string ClickCompanyInvoice = "clickCompanyInvoice";
        public static string ClickNotCompanyInvoice = "clickNotCompanyInvoice";

        public static string Consignee_ID = "consignee.id";
        public static string Consignee_Title = "consignee.title";
        public static string Consignee_Summary = "consignee.summary";

        public static string Consignee_Consignee = "consignee.consignee";
        public static string Consignee_Address = "consignee.address";
        public static string Consignee_Mobile = "consignee.mobile";
        public static string Consignee_Tel = "consignee.tel";
        public static string Consignee_Email = "consignee.email";

        public static string PaymentName = "paymentName";
        public static string ShipmentName = "shipmentName";

        public static string Payment_ID = "payment.id";
        public static string Payment_PaymentName = "payment.paymentName";
        public static string Payment_Description = "payment.description";
        public static string Shipment_ID = "shipment.id";
        public static string Shipment_ShipmentName = "shipment.shipmentName";
        public static string Shipment_Description = "shipment.description";

        public static string Invoice_Summary = "invoice.summary";
        public static string Invoice_ItemSummary = "invoice.itemSummary";
        public static string Invoice_ID = "invoice.id";
        public static string Invoice_VatCompanyName = "invoice.vatCompanyName";
        public static string Invoice_VatCode = "invoice.vatCode";
        public static string Invoice_VatAddress = "invoice.vatAddress";
        public static string Invoice_VatPhone = "invoice.vatPhone";
        public static string Invoice_VatBankName = "invoice.vatBankName";
        public static string Invoice_VatBankAccount = "invoice.vatBankAccount";
        public static string Invoice_CompanyName = "invoice.companyName";
        public static string Invoice_ConsigneeName = "invoice.consigneeName";
        public static string Invoice_ConsigneeMobile = "invoice.consigneeMobile";
        public static string Invoice_ConsigneeAddress = "invoice.consigneeAddress";

        //order success
        public static string ClickSubmitPayment = "clickSubmitPayment";
        public static string Order_OrderSN = "order.orderSN";
        public static string Order_PriceActual = "order.priceActual";

        //order list
        public static string ClickSubmitOrderPayment = "clickSubmitOrderPayment";
        public static string ClickRemoveOrder = "clickRemoveOrder";

        //filter
        public static string ClickFilter = "clickFilter";
        public static string ClickOrderDefault = "clickOrderDefault";
        public static string ClickOrderSales = "clickOrderSales";
        public static string ClickOrderPriceSale = "clickOrderPriceSale";
        public static string ClickOrderComments = "clickOrderComments";
        public static string ClickOrderAddDate = "clickOrderAddDate";
        public static string FilterID = "filterID";
        public static string FItemID = "fItemID";
        public static string FilterItemID = "filterItemID";
        public static string FilterName = "filterName";
        public static string FilterCrumbs = "filterCrumbs";
        public static string ClickFilterPreviousPage = "clickfilterPreviousPage";
        public static string ClickFilterNextPage = "clickFilterNextPage";
        public static string ClickFilterPageNum = "clickFilterPageNum";
        public static string FilterKeywords = "filterKeywords";

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(PurchaseNumAll, "购物车数量");
                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            try
            {
                string entityName = StlParserUtility.GetNameFromEntity(stlEntity);

                string type = entityName.Substring(5, entityName.Length - 6);

                if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PurchaseNumAll))
                {
                    parsedContent = "<%=getAllPurchaseNum()%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PriceTotalAll))
                {
                    parsedContent = "<%=amount.priceTotal%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PriceReturnAll))
                {
                    parsedContent = "<%=amount.priceReturn%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PriceShipmentAll))
                {
                    parsedContent = "<%=amount.priceShipment%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PriceActualAll))
                {
                    parsedContent = "<%=amount.priceActual%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickMinusCart))
                {
                    parsedContent = "b2cController.updateCart(<%=i%>, 1, 'minus')";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickPlusCart))
                {
                    parsedContent = "b2cController.updateCart(<%=i%>, 1, 'plus')";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickUpdateCart))
                {
                    parsedContent = "b2cController.updateCart(<%=i%>, $(this).val())";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickDeleteCart))
                {
                    parsedContent = "b2cController.deleteCart(<%=i%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSubmitCart))
                {
                    parsedContent = "b2cController.submitCarts();";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Cart_PriceTotal))
                {
                    parsedContent = "<%=cart.price * cart.purchaseNum%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PriceMarket))
                {
                    parsedContent = string.Format(@"<span id=""priceMarket"">{0}</span>", contextInfo.ContentInfo.GetExtendedAttribute(GoodsContentAttribute.PriceMarket));
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PriceSale))
                {
                    parsedContent = string.Format(@"<span id=""priceSale"">{0}</span>", contextInfo.ContentInfo.GetExtendedAttribute(GoodsContentAttribute.PriceSale));
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PriceSaved))
                {
                    parsedContent = string.Format(@"<span id=""priceSaved"">{0}</span>", Convert.ToString(TranslateUtils.ToDecimal(contextInfo.ContentInfo.GetExtendedAttribute(GoodsContentAttribute.PriceMarket)) - TranslateUtils.ToDecimal(contextInfo.ContentInfo.GetExtendedAttribute(GoodsContentAttribute.PriceSale))));
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Stock))
                {
                    parsedContent = string.Format(@"<span id=""stock"">{0}</span>", contextInfo.ContentInfo.GetExtendedAttribute(GoodsContentAttribute.Stock));
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PurchaseNum))
                {
                    parsedContent = string.Format(@"<input type=""text"" id=""purchaseNum"" class=""number"" value=""{0}"" >", 1);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSelectSpec))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_SPEC);
                    parsedContent = "specController.selectItemID(this)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickBuy))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_SPEC);
                    parsedContent = "specController.buy()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickAddToCart))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_SPEC);
                    parsedContent = "specController.addToCart()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickAddToCartForFilter))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_SPEC);
                    parsedContent = "<%if(content.id&&content.firstGoodID){%>specController.addToCart(<%=content.id%>,<%=content.firstGoodID%>)<%}else{%>specController.addToCart()<%}%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickAddToFollow))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_SPEC);
                    parsedContent = "<%if(content.id&&content.firstGoodID){%>specController.addToFollow(<%=content.id%>)<%}else{%>specController.addToFollow()<%}%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSubmitOrder))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    string currentUrl = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
                    parsedContent = string.Format("orderController.submitOrder('success.html?returnUrl={0}')", currentUrl);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSelectConsignee))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.selectConsignee(true)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickChangeConsignee))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.changeConsignee(<%=item.id%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickAddConsignee))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.addConsignee()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickEditConsignee))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.editConsignee(<%=item.id%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickRemoveConsignee))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.removeConsignee(this, <%=item.id%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSaveConsignee))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.saveConsignee()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSelectPaymentShipment))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.selectPaymentShipment(true)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickChangePayment))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.changePayment(<%=i%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickChangeShipment))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.changeShipment(<%=i%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSavePaymentShipment))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.savePaymentShipment()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSelectInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.selectInvoice(true)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickChangeInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.changeInvoice(<%=item.id%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickEditInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.editInvoice(<%=item.id%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickRemoveInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.removeInvoice(this, <%=item.id%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickAddInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.addInvoice()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSaveInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.saveInvoice()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickVatInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.isVatInvoice(true)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickNotVatInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.isVatInvoice(false)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickCompanyInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.isCompanyInvoice(true)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickNotCompanyInvoice))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER);
                    parsedContent = "orderController.isCompanyInvoice(false)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Consignee_ID))
                {
                    parsedContent = "<%=item.id%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Consignee_Title))
                {
                    parsedContent = "<%=item.consignee%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Consignee_Summary))
                {
                    parsedContent = "<%==getConsigneeName(item)%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Consignee_Consignee))
                {
                    parsedContent = "<%=consignee.consignee%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Consignee_Address))
                {
                    parsedContent = "<%=consignee.address%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Consignee_Mobile))
                {
                    parsedContent = "<%=consignee.mobile%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Consignee_Tel))
                {
                    parsedContent = "<%=consignee.tel%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Consignee_Email))
                {
                    parsedContent = "<%=consignee.email%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.PaymentName))
                {
                    parsedContent = "<%==payment.paymentName%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ShipmentName))
                {
                    parsedContent = "<%==shipment.shipmentName%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Payment_ID))
                {
                    parsedContent = "<%=item.id%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Payment_PaymentName))
                {
                    parsedContent = "<%=item.paymentName%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Payment_Description))
                {
                    parsedContent = "<%==item.description%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Shipment_ID))
                {
                    parsedContent = "<%=item.id%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Shipment_ShipmentName))
                {
                    parsedContent = "<%=item.shipmentName%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Shipment_Description))
                {
                    parsedContent = "<%==item.description%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_ID))
                {
                    parsedContent = "<%=item.id%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_Summary))
                {
                    parsedContent = "<%=getInvoiceName()%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_ItemSummary))
                {
                    parsedContent = "<%=getInvoiceName(item)%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_VatCompanyName))
                {
                    parsedContent = "<%=invoice.vatCompanyName%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_VatCode))
                {
                    parsedContent = "<%=invoice.vatCode%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_VatAddress))
                {
                    parsedContent = "<%=invoice.vatAddress%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_VatPhone))
                {
                    parsedContent = "<%=invoice.vatPhone%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_VatBankName))
                {
                    parsedContent = "<%=invoice.vatBankName%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_VatBankAccount))
                {
                    parsedContent = "<%=invoice.vatBankAccount%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_CompanyName))
                {
                    parsedContent = "<%=invoice.companyName%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_ConsigneeName))
                {
                    parsedContent = "<%=invoice.consigneeName%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_ConsigneeMobile))
                {
                    parsedContent = "<%=invoice.consigneeMobile%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Invoice_ConsigneeAddress))
                {
                    parsedContent = "<%=invoice.consigneeAddress%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSubmitPayment))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_SUCCESS);
                    parsedContent = "orderSuccessController.submitPayment()";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickSubmitOrderPayment))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_LIST);
                    parsedContent = "orderListController.submitOrderPayment(<%=order.id%>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickRemoveOrder))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_ORDER_LIST);
                    parsedContent = "if (confirm('此操作将删除订单，确认吗？')){{orderListController.removeOrder(<%=order.id%>)}}";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Order_OrderSN))
                {
                    parsedContent = "<%=order.orderSN%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.Order_PriceActual))
                {
                    parsedContent = "<%=order.priceActual%>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickFilter))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    int filterID = TranslateUtils.EvalInt(contextInfo.ItemContainer.FilterItem.DataItem, "filterID");
                    int itemID = TranslateUtils.EvalInt(contextInfo.ItemContainer.FilterItem.DataItem, "itemID");
                    parsedContent = string.Format("filterController.redirect({0}, {1});", filterID, itemID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickOrderDefault))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "filterController.redirect(0, 0, 'default');";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickOrderSales))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "filterController.redirect(0, 0, 'Sales');";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickOrderPriceSale))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "filterController.redirect(0, 0, 'PriceSale');";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickOrderComments))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "filterController.redirect(0, 0, 'Comments');";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickOrderAddDate))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "filterController.redirect(0, 0, 'AddDate');";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.FilterKeywords))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    int uniqueID = pageInfo.UniqueID;
                    parsedContent = "<input type='text' class='scmr_int' placeholder='再结果中搜索' id='filterKeywords_" + uniqueID + "'/><input type='button' class='scmr_btn1' value='确定' onclick=\"filterController.redirect(0, 0, '', 1, $('#filterKeywords_" + uniqueID + "').val());\"/>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.FilterItemID))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    int filterID = TranslateUtils.EvalInt(contextInfo.ItemContainer.FilterItem.DataItem, "filterID");
                    int itemID = TranslateUtils.EvalInt(contextInfo.ItemContainer.FilterItem.DataItem, "itemID");
                    parsedContent = string.Format("{0}_{1}", filterID, itemID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.FilterID))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    int filterID = TranslateUtils.EvalInt(contextInfo.ItemContainer.FilterItem.DataItem, "filterID");
                    parsedContent = string.Format("Filter_{0}", filterID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.FItemID))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    int itemID = TranslateUtils.EvalInt(contextInfo.ItemContainer.FilterItem.DataItem, "itemID");
                    parsedContent = string.Format("FItem_{0}", itemID);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.FilterName))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    string filterName = TranslateUtils.EvalString(contextInfo.ItemContainer.FilterItem.DataItem, "filterName");
                    parsedContent = string.Format("{0}", filterName);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.FilterCrumbs))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "<div id='filterCrumbs'></div>";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickFilterPreviousPage))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "filterController.redirect(0, 0, '', <%= pageItem.previousPageIndex %>);";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickFilterNextPage))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "filterController.redirect(0, 0, '', <%= pageItem.nextPageIndex %>)";
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlB2CEntities.ClickFilterPageNum))
                {
                    pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.C_B2C_FILTER);
                    parsedContent = "filterController.redirect(0, 0, '', <%= pageNum %>)";
                }
                else
                {
                    parsedContent = string.Format("<%={0}%>", type);
                }
            }
            catch { }

            return parsedContent;
        }
    }
}
