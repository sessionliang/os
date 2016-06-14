using BaiRong.Core;
using BaiRong.Model;
using SiteServer.API.Model;
using SiteServer.API.Model.B2C;
using SiteServer.B2C.Core;
using SiteServer.B2C.Core.Alipay;
using SiteServer.B2C.Core.Union;
using SiteServer.B2C.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;


namespace SiteServer.API.Controllers.B2C
{
    public class OrderController : ApiController
    {
        public const string UserOrderItemCommentImageFileName = "OrderItemComment";

        [HttpGet]
        [ActionName("GetOrderParameter")]
        public IHttpActionResult GetOrderParameter()
        {
            if (!RequestUtils.IsAnonymous)
            {
                string userName = RequestUtils.Current.UserName;

                ConsigneeInfo consignee = null;
                PaymentInfo payment = null;
                ShipmentInfo shipment = null;
                InvoiceInfo invoice = null;
                List<ConsigneeInfo> consignees = new List<ConsigneeInfo>();
                List<PaymentInfo> payments = new List<PaymentInfo>();
                List<ShipmentInfo> shipments = new List<ShipmentInfo>();
                List<InvoiceInfo> invoices = new List<InvoiceInfo>();

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                if (publishmentSystemInfo != null)
                {
                    OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetLatestOrderInfo(publishmentSystemInfo.PublishmentSystemID, userName);

                    consignees = DataProviderB2C.ConsigneeDAO.GetConsigneeInfoList(publishmentSystemInfo.GroupSN, userName);
                    foreach (ConsigneeInfo c in consignees)
                    {
                        if (orderInfo != null && orderInfo.ConsigneeID == c.ID)
                        {
                            consignee = c;
                        }
                    }
                    if (consignee == null && consignees.Count > 0)
                    {
                        consignee = consignees[0];
                    }

                    List<PaymentInfo> paymentInfoList = DataProviderB2C.PaymentDAO.GetPaymentInfoList(publishmentSystemInfo.PublishmentSystemID);
                    foreach (PaymentInfo paymentInfo in paymentInfoList)
                    {
                        if (paymentInfo.IsEnabled)
                        {
                            payments.Add(paymentInfo);
                            if (orderInfo != null && orderInfo.PaymentID == paymentInfo.ID)
                            {
                                payment = paymentInfo;
                            }
                        }
                    }
                    if (payment == null && payments.Count > 0)
                    {
                        payment = payments[0];
                    }

                    List<ShipmentInfo> shipmentInfoList = DataProviderB2C.ShipmentDAO.GetShipmentInfoList(publishmentSystemInfo.PublishmentSystemID);
                    foreach (ShipmentInfo shipmentInfo in shipmentInfoList)
                    {
                        if (shipmentInfo.IsEnabled)
                        {
                            shipments.Add(shipmentInfo);
                            if (orderInfo != null && orderInfo.ShipmentID == shipmentInfo.ID)
                            {
                                shipment = shipmentInfo;
                            }
                        }
                    }
                    if (shipment == null && shipments.Count > 0)
                    {
                        shipment = shipments[0];
                    }

                    invoices = DataProviderB2C.InvoiceDAO.GetInvoiceInfoList(publishmentSystemInfo.GroupSN, userName);
                    foreach (InvoiceInfo i in invoices)
                    {
                        if (orderInfo != null && orderInfo.InvoiceID == i.ID)
                        {
                            invoice = i;
                        }
                    }
                    if (invoice == null && invoices.Count > 0)
                    {
                        invoice = invoices[0];
                    }
                }

                var orderParameter = new OrderParameter { IsAnonymous = RequestUtils.IsAnonymous, Consignee = consignee, Payment = payment, Shipment = shipment, Invoice = invoice, Consignees = consignees, Payments = payments, Shipments = shipments, Invoices = invoices };
                return Ok(orderParameter);
            }
            else
            {
                var orderParameter = new OrderParameter { IsAnonymous = RequestUtils.IsAnonymous };
                return Ok(orderParameter);
            }
        }

        #region Consignee

        [HttpGet]
        [ActionName("AddConsignee")]
        public IHttpActionResult AddConsignee()
        {
            ConsigneeInfo item = new ConsigneeInfo(HttpContext.Current.Request.QueryString);

            Parameter parameter = new Parameter { IsSuccess = false };

            if (!RequestUtils.IsAnonymous && RequestUtils.PublishmentSystemInfo != null)
            {
                try
                {
                    item.GroupSN = RequestUtils.PublishmentSystemInfo.GroupSN;
                    item.UserName = RequestUtils.CurrentUserName;
                    int consigneeID = DataProviderB2C.ConsigneeDAO.Insert(item);
                    item.ID = consigneeID;

                    return Ok(new { IsSuccess = true, Id = item.ID });
                }
                catch (Exception ex)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
                }
            }
            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("UpdateConsignee")]
        public IHttpActionResult UpdateConsignee()
        {
            ConsigneeInfo item = new ConsigneeInfo(HttpContext.Current.Request.QueryString);

            Parameter parameter = new Parameter { IsSuccess = false };

            if (!RequestUtils.IsAnonymous && RequestUtils.PublishmentSystemInfo != null)
            {
                try
                {
                    item.IsDefault = true;
                    DataProviderB2C.ConsigneeDAO.Update(item);

                    parameter = new Parameter { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
                }
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("DeleteConsignee")]
        public IHttpActionResult DeleteConsignee(int id)
        {
            Parameter parameter = new Parameter { IsSuccess = false };

            try
            {
                DataProviderB2C.ConsigneeDAO.Delete(id);
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        #endregion

        #region Invoice

        [HttpGet]
        [ActionName("AddInvoice")]
        public IHttpActionResult AddInvoice()
        {
            InvoiceInfo item = new InvoiceInfo(HttpContext.Current.Request.QueryString);

            Parameter parameter = new Parameter { IsSuccess = false };

            if (!RequestUtils.IsAnonymous)
            {
                try
                {
                    item.GroupSN = RequestUtils.PublishmentSystemInfo != null ? RequestUtils.PublishmentSystemInfo.GroupSN : "";
                    item.UserName = RequestUtils.CurrentUserName;
                    int invoiceID = DataProviderB2C.InvoiceDAO.Insert(item);
                    item.ID = invoiceID;

                    return Ok(new { IsSuccess = true, Id = item.ID });
                }
                catch (Exception ex)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
                }
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("UpdateInvoice")]
        public IHttpActionResult UpdateInvoice()
        {
            InvoiceInfo item = new InvoiceInfo(HttpContext.Current.Request.QueryString);

            Parameter parameter = new Parameter { IsSuccess = false };

            try
            {
                item.IsDefault = true;
                DataProviderB2C.InvoiceDAO.Update(item);

                parameter = new Parameter { IsSuccess = true };
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("DeleteInvoice")]
        public IHttpActionResult DeleteInvoice(int id)
        {
            Parameter parameter = new Parameter { IsSuccess = false };

            try
            {
                DataProviderB2C.InvoiceDAO.Delete(id);
                parameter = new Parameter { IsSuccess = true };
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        #endregion

        [HttpGet]
        [ActionName("SubmitOrder_bak")]
        public IHttpActionResult SubmitOrder_bak()
        {
            int consigneeID = RequestUtils.GetIntQueryString("consigneeID");
            int paymentID = RequestUtils.GetIntQueryString("paymentID");
            int invoiceID = RequestUtils.GetIntQueryString("invoiceID");

            Parameter parameter = new Parameter { IsSuccess = false };

            if (!RequestUtils.IsAnonymous && RequestUtils.PublishmentSystemInfo != null)
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;

                try
                {
                    string orderSN = OrderManager.GetOrderSN();
                    string userName = RequestUtils.CurrentUserName;
                    string ipAddress = PageUtils.GetIPAddress();
                    string orderStatus = EOrderStatusUtils.GetValue(EOrderStatus.Handling);
                    string paymentStatus = EPaymentStatusUtils.GetValue(EPaymentStatus.Unpaid);
                    string shipmentStatus = EShipmentStatusUtils.GetValue(EShipmentStatus.UnShipment);
                    DateTime timeOrder = DateTime.Now;
                    DateTime timePayment = DateUtils.SqlMinValue;
                    DateTime timeShipment = DateUtils.SqlMinValue;
                    decimal checkTotalPrice = 0;

                    if (consigneeID > 0)
                    {
                        ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(consigneeID);
                        consigneeInfo.IsOrder = true;
                        consigneeID = DataProviderB2C.ConsigneeDAO.Insert(consigneeInfo);
                    }

                    int shipmentID = 0;

                    if (invoiceID > 0)
                    {
                        InvoiceInfo invoiceInfo = DataProviderB2C.InvoiceDAO.GetInvoiceInfo(invoiceID);
                        invoiceInfo.IsOrder = true;
                        invoiceID = DataProviderB2C.InvoiceDAO.Insert(invoiceInfo);
                    }

                    List<CartInfo> cartInfoList = DataProviderB2C.CartDAO.GetCartInfoList(publishmentSystemInfo.PublishmentSystemID, PageUtils.SessionID, RequestUtils.CurrentUserName);
                    AmountInfo amountInfo = PriceManager.GetAmountInfoByCarts(publishmentSystemInfo, cartInfoList);
                    string summary = string.Empty;

                    OrderInfo orderInfo = new OrderInfo { PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID, OrderSN = orderSN, UserName = userName, IPAddress = ipAddress, OrderStatus = orderStatus, PaymentStatus = paymentStatus, ShipmentStatus = shipmentStatus, TimeOrder = timeOrder, TimePayment = timePayment, TimeShipment = timeShipment, ConsigneeID = consigneeID, PaymentID = paymentID, ShipmentID = shipmentID, InvoiceID = invoiceID, PriceTotal = amountInfo.PriceTotal, PriceShipment = amountInfo.PriceShipment, PriceReturn = amountInfo.PriceReturn, PriceActual = amountInfo.PriceActual, Summary = summary };

                    int orderID = DataProviderB2C.OrderDAO.Insert(orderInfo);

                    List<OrderItemInfo> itemInfoList = new List<OrderItemInfo>();
                    List<int> cartIDList = new List<int>();
                    foreach (CartInfo cartInfo in cartInfoList)
                    {
                        cartIDList.Add(cartInfo.CartID);

                        GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, cartInfo.ChannelID, cartInfo.ContentID);
                        GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(cartInfo.GoodsID, contentInfo);
                        if (contentInfo != null && goodsInfo != null)
                        {
                            decimal priceSale = PriceManager.GetPrice(contentInfo, goodsInfo);

                            OrderItemInfo itemInfo = new OrderItemInfo { OrderID = orderID, ChannelID = cartInfo.ChannelID, ContentID = cartInfo.ContentID, GoodsID = cartInfo.GoodsID, GoodsSN = goodsInfo.GoodsSN, Title = contentInfo.Title, ThumbUrl = contentInfo.ThumbUrl, PriceSale = priceSale, PurchaseNum = cartInfo.PurchaseNum, IsShipment = false };
                            itemInfoList.Add(itemInfo);

                            //��Ʒ���͹����Ϊ-1ʱ����������Ʒ����
                            if (contentInfo.Stock != -1 && goodsInfo.Stock != -1)
                            {
                                if (goodsInfo.Stock < cartInfo.PurchaseNum || goodsInfo.Stock == 0)
                                    throw new Exception("�Բ��𣬿���������㣬��Ʒ��" + contentInfo.Title + "�����ţ�" + goodsInfo.GoodsSN + "����ʣ������Ϊ��" + goodsInfo.Stock + "��");
                                if (contentInfo.Stock < cartInfo.PurchaseNum || contentInfo.Stock == 0)
                                    throw new Exception("�Բ��𣬿���������㣬��Ʒ��" + contentInfo.Title + "��ʣ������Ϊ��" + contentInfo.Stock + "��");
                            }
                            checkTotalPrice += priceSale * cartInfo.PurchaseNum;
                        }
                    }
                    DataProviderB2C.OrderItemDAO.Insert(itemInfoList);

                    DataProviderB2C.CartDAO.Delete(cartIDList);

                    //��ⶩ��������ܼۺͶ����ܼ��Ƿ�һ��
                    if (orderInfo.PriceTotal != checkTotalPrice)
                    {
                        DataProviderB2C.OrderDAO.Delete(orderInfo.ID);
                        foreach (OrderItemInfo orderItem in itemInfoList)
                        {
                            DataProviderB2C.OrderItemDAO.Delete(orderItem.ID);
                        }

                        parameter = new Parameter { IsSuccess = false, ErrorMessage = "�µ�ʧ�ܣ��������ܼ۳������⣬�������Ա��ϵ��" };
                    }

                    LogUtils.AddUserLog(userName, "�µ�", "�����ܼۣ�" + orderInfo.PriceTotal + " , ���������ܼۣ�" + checkTotalPrice);

                    parameter = new Parameter { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
                }
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("SubmitOrder")]
        public IHttpActionResult SubmitOrder()
        {
            int consigneeID = RequestUtils.GetIntQueryString("consigneeID");
            int paymentID = RequestUtils.GetIntQueryString("paymentID");
            int invoiceID = RequestUtils.GetIntQueryString("invoiceID");

            Parameter parameter = new Parameter { IsSuccess = false };

            if (!RequestUtils.IsAnonymous && RequestUtils.PublishmentSystemInfo != null)
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;

                try
                {
                    string errorMessage = string.Empty;

                    if (consigneeID > 0)
                    {
                        ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(consigneeID);
                        consigneeInfo.IsOrder = true;
                        consigneeID = DataProviderB2C.ConsigneeDAO.Insert(consigneeInfo);
                    }

                    int shipmentID = 0;

                    if (invoiceID > 0)
                    {
                        InvoiceInfo invoiceInfo = DataProviderB2C.InvoiceDAO.GetInvoiceInfo(invoiceID);
                        invoiceInfo.IsOrder = true;
                        invoiceID = DataProviderB2C.InvoiceDAO.Insert(invoiceInfo);
                    }

                    List<CartInfo> cartInfoList = DataProviderB2C.CartDAO.GetCartInfoList(publishmentSystemInfo.PublishmentSystemID, PageUtils.SessionID, RequestUtils.CurrentUserName);

                    bool flag = DataProviderB2C.OrderDAO.SubmitOrderWithTrans(publishmentSystemInfo, consigneeID, shipmentID, invoiceID, paymentID, cartInfoList, out errorMessage);

                    parameter = new Parameter { IsSuccess = flag, ErrorMessage = errorMessage };
                }
                catch (Exception ex)
                {
                    parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
                }
            }

            return Ok(parameter);
        }

        [HttpGet]
        [ActionName("GetLatestOrder")]
        public IHttpActionResult GetLatestOrder()
        {
            OrderSuccessParameter parameter = new OrderSuccessParameter { IsSuccess = false };

            OrderInfo orderInfo = null;

            bool isPC = RequestUtils.GetBoolQueryString("isPC");

            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            if (publishmentSystemInfo != null)
            {
                if (!RequestUtils.IsAnonymous)
                {
                    orderInfo = DataProviderB2C.OrderDAO.GetLatestOrderInfo(publishmentSystemInfo.PublishmentSystemID, RequestUtils.CurrentUserName);
                }

                if (orderInfo != null && EOrderStatusUtils.Equals(orderInfo.OrderStatus, EOrderStatus.Handling) && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                {
                    PaymentInfo paymentInfo = PaymentManager.GetPaymentInfo(orderInfo.PaymentID);
                    string clickString = string.Empty;
                    string log = string.Empty;
                    string paymentForm = "";
                    if (isPC)
                    {
                        paymentForm = PaymentManager.GetPaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                    }
                    else
                    {
                        // ���ǿ  �ֻ�֧�� �ȴ��ӿ����� begin

                        paymentForm = PaymentManager.GetMobilePaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);

                        // ���ǿ  �ֻ�֧�� �ȴ��ӿ����� end
                    }

                    parameter = new OrderSuccessParameter { IsSuccess = true, PaymentName = paymentInfo.PaymentName, Order = orderInfo, PaymentForm = paymentForm, ClickString = clickString };

                    orderInfo.Extended = log;
                    DataProviderB2C.OrderDAO.Update(orderInfo);
                }
            }

            return Ok(parameter);
        }

        [HttpPost]
        [ActionName("AlipayNotify")]
        public HttpResponseMessage AlipayNotify(int Id)
        {
            try
            {
                //�̻�������
                string out_trade_no = HttpContext.Current.Request.Form["out_trade_no"];

                OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(out_trade_no);
                SortedDictionary<string, string> sPara = PaymentManager.GetAlipayNotifyRequestPost();

                if (sPara.Count > 0)//�ж��Ƿ��д����ز���
                {
                    Notify aliNotify = new Notify(Id, orderInfo.ID);
                    StringBuilder log = new StringBuilder(orderInfo.Extended);

                    bool verifyResult = aliNotify.Verify(sPara, HttpContext.Current.Request.Form["notify_id"], HttpContext.Current.Request.Form["sign"], log);

                    if (verifyResult)//��֤�ɹ�
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //������������̻���ҵ���߼��������


                        //�������������ҵ���߼�����д�������´�������ο�������
                        //��ȡ֧������֪ͨ���ز������ɲο������ĵ��з������첽֪ͨ�����б�

                        //֧�������׺�

                        string trade_no = HttpContext.Current.Request.Form["trade_no"];

                        //����״̬
                        string trade_status = HttpContext.Current.Request.Form["trade_status"];
                        //�ֻ�����״̬
                        string trade_result = HttpContext.Current.Request.QueryString["result"];


                        if (trade_status == "TRADE_FINISHED"
                            || trade_status == "TRADE_SUCCESS"
                            || trade_status == "WAIT_SELLER_SEND_GOODS"
                            || trade_result == "success")
                        {

                            //DataProviderB2C.OrderDAO.Payment(orderID);
                            if (orderInfo != null && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                            {
                                orderInfo.PaymentStatus = EPaymentStatusUtils.GetValue(EPaymentStatus.Paid);
                                orderInfo.TimePayment = DateTime.Now;
                                orderInfo.Extended = trade_no;//֧�������׺�
                                DataProviderB2C.OrderDAO.Update(orderInfo);
                            }
                        }


                        //if (HttpContext.Current.Request.Form["trade_status"] == "TRADE_FINISHED")
                        //{
                        //    //�жϸñʶ����Ƿ����̻���վ���Ѿ���������
                        //    //���û�������������ݶ����ţ�out_trade_no�����̻���վ�Ķ���ϵͳ�в鵽�ñʶ�������ϸ����ִ���̻���ҵ�����
                        //    //���������������ִ���̻���ҵ�����

                        //    //ע�⣺
                        //    //���ֽ���״ֻ̬����������³���
                        //    //1����ͨ����ͨ��ʱ���ˣ���Ҹ���ɹ���
                        //    //2����ͨ�˸߼���ʱ���ˣ��Ӹñʽ��׳ɹ�ʱ�����𣬹���ǩԼʱ�Ŀ��˿�ʱ�ޣ��磺���������ڿ��˿һ�����ڿ��˿�ȣ���
                        //}
                        //else if (HttpContext.Current.Request.Form["trade_status"] == "TRADE_SUCCESS")
                        //{
                        //    //�жϸñʶ����Ƿ����̻���վ���Ѿ���������
                        //    //���û�������������ݶ����ţ�out_trade_no�����̻���վ�Ķ���ϵͳ�в鵽�ñʶ�������ϸ����ִ���̻���ҵ�����
                        //    //���������������ִ���̻���ҵ�����

                        //    //ע�⣺
                        //    //���ֽ���״ֻ̬��һ������³��֡�����ͨ�˸߼���ʱ���ˣ���Ҹ���ɹ���
                        //}
                        //else
                        //{
                        //}

                        //�������������ҵ���߼�����д�������ϴ�������ο�������

                        return new HttpResponseMessage() { Content = new StringContent("success") };

                        //Response.Write("success");  //�벻Ҫ�޸Ļ�ɾ��

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    else//��֤ʧ��
                    {

                        return new HttpResponseMessage() { Content = new StringContent("fail") };
                        //Response.Write("fail");
                    }
                }
                else
                {

                    return new HttpResponseMessage() { Content = new StringContent("��֪ͨ����") };
                    //Response.Write("��֪ͨ����");
                }
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return new HttpResponseMessage() { Content = new StringContent("fail") };
            }


        }

        [HttpPost]
        [ActionName("UnionNotify")]
        public HttpResponseMessage UnionNotify(int Id)
        {
            try
            {
                // ʹ��Dictionary�������
                Dictionary<string, string> resData = new Dictionary<string, string>();

                NameValueCollection coll = HttpContext.Current.Request.Form;

                string[] requestItem = coll.AllKeys;

                for (int i = 0; i < requestItem.Length; i++)
                {
                    resData.Add(requestItem[i], coll[requestItem[i]]);
                }

                string respcode = resData["respCode"];//��Ӧ��
                string queryId = resData["queryId"];//���ײ�ѯ��ˮ��
                string orderId = resData["orderId"];//�̻�������
                OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderId);

                if (HttpContext.Current.Request.HttpMethod == "POST")
                {
                    SDKConfig unionConfig = new SDKConfig(Id, orderInfo.ID);
                    CertUtil.sdkConfig = SDKUtil.sdkConfig = SecurityUtil.sdkConfig = unionConfig;

                    // ���ر����в�����UPOG,��ʾServer����ȷ���ս�������,����Ҫ��֤Server�˷��ر��ĵ�ǩ��
                    if (SDKUtil.Validate(resData, Encoding.UTF8))
                    {
                        //Response.Write("�̻�����֤���ر���ǩ���ɹ�\n");

                        //�̻��˸��ݷ��ر������ݴ����Լ���ҵ���߼� 

                        if (orderInfo != null && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                        {
                            orderInfo.PaymentStatus = EPaymentStatusUtils.GetValue(EPaymentStatus.Paid);
                            orderInfo.TimePayment = DateTime.Now;
                            orderInfo.Extended = queryId;//���ײ�ѯ��ˮ��
                            DataProviderB2C.OrderDAO.Update(orderInfo);
                        }
                    }
                    else
                    {
                        return new HttpResponseMessage() { Content = new StringContent("fail") };
                    }

                }
                return new HttpResponseMessage() { Content = new StringContent("success") };
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return new HttpResponseMessage() { Content = new StringContent("fail") };
            }


        }

        [HttpGet]
        [ActionName("GetReturnValue")]
        public IHttpActionResult GetReturnValue()
        {
            OrderReturnParameter parameter = new OrderReturnParameter { IsSuccess = false };
            OrderInfo orderInfo = new OrderInfo();

            #region ֧����������Ϣ
            //http://www.siteyun.com/utils/return.html?buyer_email=13911897774&buyer_id=2088012113156761&exterface=create_direct_pay_by_user&is_success=T&notify_id=RqPnCoPT3K9%252Fvwbh3I75J3cxssNTRr6fPtDws1ngegEai4D8jTh4FfittRcWT6Z%252BUXTx&notify_time=2014-01-03+14%3A39%3A04&notify_type=trade_status_sync&out_trade_no=FFC6381CCBE2C834&payment_type=1&seller_email=corp%40siteserver.cn&seller_id=2088701507833361&subject=SiteYun%E4%BA%91%E5%BB%BA%E7%AB%99%E5%95%86%E5%9F%8E%E8%AE%A2%E5%8D%95&total_fee=0.14&trade_no=2014010343335976&trade_status=TRADE_SUCCESS&sign=4d7aff82a5a33657dab035107ed7fed1&sign_type=MD5
            //֧����������Ϣ
            string out_trade_no = HttpContext.Current.Request.QueryString["out_trade_no"];
            //֧�������׺�
            string trade_no = HttpContext.Current.Request.QueryString["trade_no"];
            //����״̬
            string trade_status = HttpContext.Current.Request.QueryString["trade_status"];
            //�ֻ�����״̬
            string trade_result = HttpContext.Current.Request.QueryString["result"];

            if (trade_status == "TRADE_FINISHED"
                || trade_status == "TRADE_SUCCESS"
                || trade_status == "WAIT_SELLER_SEND_GOODS"
                || trade_result == "success")
            {
                //�жϸñʶ����Ƿ����̻���վ���Ѿ���������
                //���û�������������ݶ����ţ�out_trade_no�����̻���վ�Ķ���ϵͳ�в鵽�ñʶ�������ϸ����ִ���̻���ҵ�����
                //���������������ִ���̻���ҵ�����

                orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(out_trade_no);
                if (orderInfo != null && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                {
                    orderInfo.Extended = trade_no;
                    orderInfo.PaymentStatus = EPaymentStatus.Paid.ToString();
                    orderInfo.TimePayment = DateTime.Now;
                    DataProviderB2C.OrderDAO.Update(orderInfo);
                    //DataProviderB2C.OrderDAO.Payment(orderInfo.ID);
                }
            }

            #endregion

            #region ��������
            string orderId = HttpContext.Current.Request.QueryString["orderId"];
            if (!string.IsNullOrEmpty(orderId))
                orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderId);
            #endregion

            parameter = new OrderReturnParameter { IsSuccess = true, Order = orderInfo };

            return Ok(parameter);
        }

        [HttpPost]
        [ActionName("ReturnTransit")]
        public HttpResponseMessage ReturnTransit()
        {

            OrderReturnParameter parameter = new OrderReturnParameter { IsSuccess = false };
            OrderInfo orderInfo = new OrderInfo();

            #region ����������Ϣ
            Dictionary<string, string> resData = new Dictionary<string, string>();

            NameValueCollection coll = HttpContext.Current.Request.Form;

            string[] requestItem = coll.AllKeys;

            for (int i = 0; i < requestItem.Length; i++)
            {
                resData.Add(requestItem[i], coll[requestItem[i]]);
            }
            string respcode = resData["respCode"];//��Ӧ��
            string queryId = resData["queryId"];//���ײ�ѯ��ˮ��
            string orderId = resData["orderId"];//�̻�������

            if (respcode == "00")
            {
                //�жϸñʶ����Ƿ����̻���վ���Ѿ���������
                //���û�������������ݶ����ţ�orderId�����̻���վ�Ķ���ϵͳ�в鵽�ñʶ�������ϸ����ִ���̻���ҵ�����
                //���������������ִ���̻���ҵ�����

                orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderId);
                if (orderInfo != null && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                {
                    orderInfo.Extended = queryId;
                    orderInfo.TimePayment = DateTime.Now;
                    orderInfo.PaymentStatus = EPaymentStatus.Paid.ToString();
                    DataProviderB2C.OrderDAO.Update(orderInfo);
                    //DataProviderB2C.OrderDAO.Payment(orderInfo.ID);
                }

            }
            #endregion

            //ȡ������ĵ�ַ
            string unionReturnUrl = CacheUtils.Get("UnionReturnUrl_" + orderInfo.OrderSN).ToString();
            CacheUtils.Remove("UnionReturnUrl_" + orderInfo.OrderSN);
            HttpContext.Current.Response.Redirect(unionReturnUrl);
            return new HttpResponseMessage() { Content = new StringContent("success") };
        }


        #region Home

        [HttpGet]
        [ActionName("GetOrderList")]
        public IHttpActionResult GetOrderList()
        {
            List<OrderListParameter> parameterList = new List<OrderListParameter>();

            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            if (publishmentSystemInfo != null)
            {
                bool isCompleted = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isCompleted"]);
                bool isPayment = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isPayment"]);
                bool isAll = false;
                bool isPC = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isPC"], true);
                if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["isCompleted"]) && string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["isCompleted"]))
                {
                    isAll = true;
                }

                List<OrderInfo> orderInfoList = new List<OrderInfo>();

                if (isAll)
                {
                    orderInfoList = DataProviderB2C.OrderDAO.GetOrderInfoList(RequestUtils.PublishmentSystemID, RequestUtils.CurrentUserName);
                }
                else
                {
                    if (isCompleted)
                    {
                        orderInfoList = DataProviderB2C.OrderDAO.GetOrderInfoList(RequestUtils.PublishmentSystemID, RequestUtils.CurrentUserName, isCompleted);
                    }
                    else
                    {
                        orderInfoList = DataProviderB2C.OrderDAO.GetOrderInfoList(RequestUtils.PublishmentSystemID, RequestUtils.CurrentUserName, isCompleted, isPayment);
                    }
                }

                foreach (OrderInfo orderInfo in orderInfoList)
                {
                    bool isPaymentClick = false;
                    string paymentForm = string.Empty;
                    string clickString = string.Empty;

                    if (EOrderStatusUtils.Equals(orderInfo.OrderStatus, EOrderStatus.Handling) && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                    {
                        PaymentInfo paymentInfo = PaymentManager.GetPaymentInfo(orderInfo.PaymentID);
                        if (paymentInfo != null)
                        {
                            string log = string.Empty;
                            if (isPC)
                                paymentForm = PaymentManager.GetPaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                            else
                                paymentForm = PaymentManager.GetMobilePaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                            if (!string.IsNullOrEmpty(paymentForm))
                            {
                                isPaymentClick = true;
                            }
                            orderInfo.Extended = log;
                            DataProviderB2C.OrderDAO.Update(orderInfo);
                        }
                    }

                    orderInfo.OrderStatus = EOrderStatusUtils.GetText(orderInfo.OrderStatus);
                    orderInfo.PaymentStatus = EPaymentStatusUtils.GetText(orderInfo.PaymentStatus);
                    orderInfo.ShipmentStatus = EShipmentStatusUtils.GetText(orderInfo.ShipmentStatus);

                    List<OrderItemInfo> itemInfoList = DataProviderB2C.OrderItemDAO.GetItemInfoList(RequestUtils.CurrentUserName, orderInfo.ID);
                    List<OrderItemParameter> itemParameterList = new List<OrderItemParameter>();

                    foreach (OrderItemInfo itemInfo in itemInfoList)
                    {
                        GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);

                        GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(itemInfo.GoodsID, contentInfo);
                        string navigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, itemInfo.ChannelID), itemInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
                        string thumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.ThumbUrl));
                        string spec = SpecManager.GetSpecValues(publishmentSystemInfo, goodsInfo);
                        spec = spec.Replace("/api", string.Empty).Replace("</div><div>", "<br />").Replace("<div>", string.Empty).Replace("</div>", string.Empty);

                        OrderItemParameter itemParameter = new OrderItemParameter { GoodsSN = itemInfo.GoodsSN, Title = itemInfo.Title, ThumbUrl = thumbUrl, PriceSale = itemInfo.PriceSale, PurchaseNum = itemInfo.PurchaseNum, NavigationUrl = navigationUrl, Spec = spec };
                        itemParameterList.Add(itemParameter);
                    }

                    OrderListParameter parameter = new OrderListParameter { OrderInfo = orderInfo, Items = itemParameterList, IsPaymentClick = isPaymentClick, PaymentForm = paymentForm, ClickString = clickString };

                    parameterList.Add(parameter);
                }
            }

            return Ok(parameterList);
        }

        [HttpGet]
        [ActionName("GetAllOrderList")]
        public IHttpActionResult GetAllOrderList()
        {
            #region ��ҳ����
            int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
            int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
            string isCompleted = HttpContext.Current.Request.QueryString["isCompleted"];
            string isPayment = HttpContext.Current.Request.QueryString["isPayment"];
            int total = DataProviderB2C.OrderDAO.GetCountByUser(RequestUtils.CurrentUserName, isCompleted, isPayment);
            string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);
            #endregion
            List<OrderListParameter> parameterList = new List<OrderListParameter>();
            ArrayList publishmentSystemInfoArray = PublishmentSystemManager.GetPublishmentSystemIDArrayList(BaiRong.Model.EPublishmentSystemType.B2C);

            bool isAll = false;
            bool isPC = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isPC"], true);
            int orderTime = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["orderTime"], 0);
            string keywords = PageUtils.FilterSql(HttpContext.Current.Request.QueryString["keywords"]);
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["isCompleted"]) && string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["isPayment"]))
            {
                isAll = true;
            }
            List<OrderInfo> orderInfoList = new List<OrderInfo>();

            orderInfoList = DataProviderB2C.OrderDAO.GetOrderInfoList(RequestUtils.CurrentUserName, isCompleted, isPayment, pageIndex, prePageNum, orderTime, keywords);

            #region ��ȡÿһ��b2c����Ʒ
            foreach (OrderInfo orderInfo in orderInfoList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(orderInfo.PublishmentSystemID);
                if (publishmentSystemInfo != null)
                {
                    try
                    {
                        bool isPaymentClick = false;
                        string paymentForm = string.Empty;
                        string clickString = string.Empty;

                        if (EOrderStatusUtils.Equals(orderInfo.OrderStatus, EOrderStatus.Handling) && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                        {
                            PaymentInfo paymentInfo = PaymentManager.GetPaymentInfo(orderInfo.PaymentID);
                            if (paymentInfo != null)
                            {
                                string log = string.Empty;
                                if (isPC)
                                    paymentForm = PaymentManager.GetPaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                                else
                                    paymentForm = PaymentManager.GetMobilePaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                                if (!string.IsNullOrEmpty(paymentForm))
                                {
                                    isPaymentClick = true;
                                }
                                orderInfo.Extended = log;
                                DataProviderB2C.OrderDAO.Update(orderInfo);
                            }
                        }

                        orderInfo.OrderStatus = EOrderStatusUtils.GetText(orderInfo.OrderStatus);
                        orderInfo.PaymentStatus = EPaymentStatusUtils.GetText(orderInfo.PaymentStatus);
                        orderInfo.ShipmentStatus = EShipmentStatusUtils.GetText(orderInfo.ShipmentStatus);

                        List<OrderItemInfo> itemInfoList = DataProviderB2C.OrderItemDAO.GetItemInfoList(orderInfo.ID);
                        List<OrderItemParameter> itemParameterList = new List<OrderItemParameter>();

                        foreach (OrderItemInfo itemInfo in itemInfoList)
                        {
                            GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);
                            GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(itemInfo.GoodsID, contentInfo);
                            string navigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, itemInfo.ChannelID), itemInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
                            string thumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.ThumbUrl));
                            string spec = SpecManager.GetSpecValues(publishmentSystemInfo, goodsInfo);
                            spec = spec.Replace("/api", string.Empty).Replace("</div><div>", "<br />").Replace("<div>", string.Empty).Replace("</div>", string.Empty);

                            OrderItemParameter itemParameter = new OrderItemParameter { OrderItemID = itemInfo.ID, OrderID = itemInfo.OrderID, GoodsSN = itemInfo.GoodsSN, Title = itemInfo.Title, ThumbUrl = thumbUrl, PriceSale = itemInfo.PriceSale, PurchaseNum = itemInfo.PurchaseNum, NavigationUrl = navigationUrl, Spec = spec };

                            itemParameter.IsApplyReturn = DataProviderB2C.OrderItemReturnDAO.GetCount(string.Format(" OrderItemID = {0} ", itemInfo.ID)) > 0;

                            itemParameterList.Add(itemParameter);


                        }
                        PublishmentSystemParameter publishmentSystemParamter = new PublishmentSystemParameter()
                        {
                            PublishmentSystemID = orderInfo.PublishmentSystemID,
                            PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName,
                            PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl
                        };
                        OrderListParameter parameter = new OrderListParameter { PublishmentSystemInfo = publishmentSystemParamter, OrderInfo = orderInfo, Items = itemParameterList, IsPaymentClick = isPaymentClick, PaymentForm = paymentForm, ClickString = clickString };

                        parameterList.Add(parameter);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                #endregion
            }
            var orderInfoListParms = new { PageJson = pageJson, OrderInfoList = parameterList };
            return Ok(orderInfoListParms);
        }

        [HttpGet]
        [ActionName("GetOrder")]
        public IHttpActionResult GetOrder(int id)
        {
            OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(id);
            orderInfo.OrderStatus = EOrderStatusUtils.GetText(EOrderStatusUtils.GetEnumType(orderInfo.OrderStatus));
            orderInfo.PaymentStatus = EPaymentStatusUtils.GetText(EPaymentStatusUtils.GetEnumType(orderInfo.PaymentStatus));
            orderInfo.ShipmentStatus = EShipmentStatusUtils.GetText(EShipmentStatusUtils.GetEnumType(orderInfo.ShipmentStatus));

            List<OrderItemInfo> itemInfoList = DataProviderB2C.OrderItemDAO.GetItemInfoList(RequestUtils.CurrentUserName, id);

            ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(orderInfo.ConsigneeID);
            InvoiceInfo invoiceInfo = DataProviderB2C.InvoiceDAO.GetInvoiceInfo(orderInfo.InvoiceID);

            OrderResponse orderResponse = new OrderResponse { Order = orderInfo, ItemList = itemInfoList, Consignee = consigneeInfo, Invoice = invoiceInfo };
            return Ok(orderResponse);
        }

        [HttpGet]
        [ActionName("DeleteOrder")]
        public IHttpActionResult DeleteOrder(int id)
        {
            Parameter parameter = new Parameter { IsSuccess = false };

            try
            {
                DataProviderB2C.OrderDAO.Delete(id);
            }
            catch (Exception ex)
            {
                parameter = new Parameter { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return Ok(parameter);
        }

        /// <summary>
        /// ��ȡ�û�ĳ�����Ķ������飨��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOrderItemList")]
        public IHttpActionResult GetOrderItemList()
        {
            int orderID = RequestUtils.GetIntQueryString("orderID");
            bool isPC = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isPC"], true);
            int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
            int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
            int total = DataProviderB2C.OrderItemDAO.GetCountByUser(RequestUtils.CurrentUserName, orderID, string.Empty);
            string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;


            OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderID);
            List<OrderListParameter> orderList = new List<OrderListParameter>();
            Hashtable ht = new Hashtable();


            List<OrderItemInfo> itemInfoList = DataProviderB2C.OrderItemDAO.GetItemInfoList(RequestUtils.CurrentUserName, orderID, pageIndex, prePageNum);
            List<OrderItemParameter> itemParameterList = new List<OrderItemParameter>();
            List<OrderItemCommentInfo> itemCommentInfoList = new List<OrderItemCommentInfo>();
            foreach (OrderItemInfo itemInfo in itemInfoList)
            {
                try
                {
                    List<OrderItemCommentParameter> itemCommentParameterList = new List<OrderItemCommentParameter>();
                    if (RequestUtils.PublishmentSystemInfo == null)
                    {
                        orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(itemInfo.OrderID);
                        publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(orderInfo.PublishmentSystemID);
                    }
                    if (publishmentSystemInfo == null)
                        continue;
                    GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);
                    GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(itemInfo.GoodsID, contentInfo);
                    string navigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, itemInfo.ChannelID), itemInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
                    string thumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.ThumbUrl));
                    string spec = SpecManager.GetSpecValues(publishmentSystemInfo, goodsInfo);
                    spec = spec.Replace("/api", string.Empty).Replace("</div><div>", "<br />").Replace("<div>", string.Empty).Replace("</div>", string.Empty);

                    itemCommentInfoList = DataProviderB2C.OrderItemCommentDAO.GetItemCommentInfoList(itemInfo.ID);
                    foreach (OrderItemCommentInfo itemCommentInfo in itemCommentInfoList)
                    {
                        OrderItemCommentParameter itemCommentParameter = new OrderItemCommentParameter { AddDate = itemCommentInfo.AddDate, Comment = itemCommentInfo.Comment, AddUser = itemCommentInfo.AddUser, GoodCount = itemCommentInfo.GoodCount, IsAnonymous = itemCommentInfo.IsAnonymous, OrderItemID = itemCommentInfo.OrderItemID, OrderUrl = itemCommentInfo.OrderUrl, Star = itemCommentInfo.Star, Tags = itemCommentInfo.Tags };
                        if (!string.IsNullOrEmpty(itemCommentInfo.ImageUrl))
                        {
                            string[] images = itemCommentInfo.ImageUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string image in images)
                            {
                                itemCommentParameter.ImageUrl += PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, image)) + ",";
                            }
                            itemCommentParameter.ImageUrl = itemCommentParameter.ImageUrl.TrimEnd(',');
                        }
                        else
                        {
                            itemCommentParameter.ImageUrl = "";
                        }

                        itemCommentParameterList.Add(itemCommentParameter);
                    }

                    GoodsContentInfo goodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);
                    OrderItemParameter itemParameter = new OrderItemParameter { OrderItemID = itemInfo.ID, GoodsSN = itemInfo.GoodsSN, Title = itemInfo.Title, ThumbUrl = thumbUrl, PriceSale = itemInfo.PriceSale, PurchaseNum = itemInfo.PurchaseNum, NavigationUrl = navigationUrl, Spec = spec, OrderItemCommentList = itemCommentParameterList };
                    itemParameter.DefaultTags = TranslateUtils.StringCollectionToStringList(goodsContentInfo.Tags, ' ');
                    itemParameterList.Add(itemParameter);

                }
                catch (Exception)
                {
                    continue;
                }
            }

            ht.Add(orderInfo.OrderSN.ToLower(), itemParameterList);

            bool isPaymentClick = false;
            string paymentForm = string.Empty;
            string clickString = string.Empty;
            if (EOrderStatusUtils.Equals(orderInfo.OrderStatus, EOrderStatus.Handling) && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
            {
                PaymentInfo paymentInfo = PaymentManager.GetPaymentInfo(orderInfo.PaymentID);
                if (paymentInfo != null)
                {
                    string log = string.Empty;
                    if (isPC)
                        paymentForm = PaymentManager.GetPaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                    else
                        paymentForm = PaymentManager.GetMobilePaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                    if (!string.IsNullOrEmpty(paymentForm))
                    {
                        isPaymentClick = true;
                    }
                    orderInfo.Extended = log;
                    DataProviderB2C.OrderDAO.Update(orderInfo);
                }
            }
            PublishmentSystemParameter publishmentSystemParamter = new PublishmentSystemParameter()
            {
                PublishmentSystemID = orderInfo.PublishmentSystemID,
                PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName,
                PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl
            };

            OrderListParameter parameter = new OrderListParameter { PublishmentSystemInfo = publishmentSystemParamter, OrderInfo = orderInfo, IsPaymentClick = isPaymentClick, PaymentForm = paymentForm, ClickString = clickString };
            orderList.Add(parameter);

            var orderParms = new { orderList = orderList, orderItemList = ht, pageJson = pageJson };
            return Ok(orderParms);

        }

        /// <summary>
        /// ��ȡ�û����еĶ�������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAllOrderItemList")]
        public IHttpActionResult GetAllOrderItemList()
        {
            bool isPC = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isPC"], true);
            int orderTime = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["orderTime"], 0);
            string keywords = PageUtils.FilterSql(HttpContext.Current.Request.QueryString["keywords"]);
            int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
            int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
            string isCompleted = PageUtils.FilterSql(HttpContext.Current.Request.QueryString["isCompleted"]);
            string isPayment = PageUtils.FilterSql(HttpContext.Current.Request.QueryString["isPayment"]);
            int total = DataProviderB2C.OrderDAO.GetCountByUser(RequestUtils.CurrentUserName, isCompleted, isPayment);
            string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);

            List<OrderInfo> orderList = DataProviderB2C.OrderDAO.GetOrderInfoList(RequestUtils.CurrentUserName, isCompleted, isPayment, pageIndex, prePageNum, orderTime, keywords);
            List<OrderListParameter> orderListResult = new List<OrderListParameter>();
            Hashtable ht = new Hashtable();
            foreach (OrderInfo orderInfo in orderList)
            {
                List<OrderItemInfo> itemInfoList = DataProviderB2C.OrderItemDAO.GetItemInfoList(orderInfo.ID);
                List<OrderItemParameter> itemParameterList = new List<OrderItemParameter>();
                List<OrderItemCommentInfo> itemCommentInfoList = new List<OrderItemCommentInfo>();

                if (orderInfo == null)
                    continue;
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(orderInfo.PublishmentSystemID);
                if (publishmentSystemInfo == null)
                    continue;
                foreach (OrderItemInfo itemInfo in itemInfoList)
                {

                    try
                    {
                        List<OrderItemCommentParameter> itemCommentParameterList = new List<OrderItemCommentParameter>();
                        GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);
                        GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(itemInfo.GoodsID, contentInfo);
                        string navigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, itemInfo.ChannelID), itemInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
                        string thumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.ThumbUrl));
                        string spec = SpecManager.GetSpecValues(publishmentSystemInfo, goodsInfo);
                        spec = spec.Replace("/api", string.Empty).Replace("</div><div>", "<br />").Replace("<div>", string.Empty).Replace("</div>", string.Empty);

                        itemCommentInfoList = DataProviderB2C.OrderItemCommentDAO.GetItemCommentInfoList(itemInfo.ID);
                        foreach (OrderItemCommentInfo itemCommentInfo in itemCommentInfoList)
                        {
                            OrderItemCommentParameter itemCommentParameter = new OrderItemCommentParameter { AddDate = itemCommentInfo.AddDate, Comment = itemCommentInfo.Comment, AddUser = itemCommentInfo.AddUser, GoodCount = itemCommentInfo.GoodCount, IsAnonymous = itemCommentInfo.IsAnonymous, OrderItemID = itemCommentInfo.OrderItemID, OrderUrl = itemCommentInfo.OrderUrl, Star = itemCommentInfo.Star, Tags = itemCommentInfo.Tags };
                            if (!string.IsNullOrEmpty(itemCommentInfo.ImageUrl))
                            {
                                string[] images = itemCommentInfo.ImageUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string image in images)
                                {
                                    itemCommentParameter.ImageUrl += PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, image)) + ",";
                                }
                                itemCommentParameter.ImageUrl = itemCommentParameter.ImageUrl.TrimEnd(',');
                            }
                            else
                            {
                                itemCommentParameter.ImageUrl = "";
                            }

                            itemCommentParameterList.Add(itemCommentParameter);
                        }

                        GoodsContentInfo goodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);
                        OrderItemParameter itemParameter = new OrderItemParameter { OrderItemID = itemInfo.ID, GoodsSN = itemInfo.GoodsSN, Title = itemInfo.Title, ThumbUrl = thumbUrl, PriceSale = itemInfo.PriceSale, PurchaseNum = itemInfo.PurchaseNum, NavigationUrl = navigationUrl, Spec = spec, OrderItemCommentList = itemCommentParameterList };
                        itemParameter.DefaultTags = TranslateUtils.StringCollectionToStringList(goodsContentInfo.Tags, ' ');
                        itemParameterList.Add(itemParameter);

                        
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                ht.Add(orderInfo.OrderSN.ToLower(), itemParameterList);
                bool isPaymentClick = false;
                string paymentForm = string.Empty;
                string clickString = string.Empty;
                if (EOrderStatusUtils.Equals(orderInfo.OrderStatus, EOrderStatus.Handling) && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                {
                    PaymentInfo paymentInfo = PaymentManager.GetPaymentInfo(orderInfo.PaymentID);
                    if (paymentInfo != null)
                    {
                        string log = string.Empty;
                        if (isPC)
                            paymentForm = PaymentManager.GetPaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                        else
                            paymentForm = PaymentManager.GetMobilePaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                        if (!string.IsNullOrEmpty(paymentForm))
                        {
                            isPaymentClick = true;
                        }
                        orderInfo.Extended = log;
                        DataProviderB2C.OrderDAO.Update(orderInfo);
                    }
                }
                PublishmentSystemParameter publishmentSystemParamter = new PublishmentSystemParameter()
                {
                    PublishmentSystemID = orderInfo.PublishmentSystemID,
                    PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName,
                    PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl
                };
                OrderListParameter parameter = new OrderListParameter { PublishmentSystemInfo = publishmentSystemParamter, OrderInfo = orderInfo, IsPaymentClick = isPaymentClick, PaymentForm = paymentForm, ClickString = clickString };
                orderListResult.Add(parameter);
            }


            var orderParms = new { orderList = orderListResult, orderItemList = ht, pageJson = pageJson };
            return Ok(orderParms);

        }

        [HttpGet]
        [ActionName("SaveOrderItemComment")]
        public IHttpActionResult SaveOrderItemComment()
        {
            int orderID = RequestUtils.GetIntQueryString("orderID");
            int orderItemID = RequestUtils.GetIntQueryString("orderItemID");
            int orderItemCommentID = RequestUtils.GetIntQueryString("orderItemCommentID");
            string goodsSN = RequestUtils.GetQueryString("goodsSN");
            int star = RequestUtils.GetIntQueryString("star");
            string tags = RequestUtils.GetQueryString("tags");
            string comment = RequestUtils.GetQueryString("comment");
            bool isAnonymous = RequestUtils.GetBoolQueryString("isAnonymous");
            string imageUrl = RequestUtils.GetQueryString("imageUrl");

            OrderItemCommentInfo itemCommentInfo = new OrderItemCommentInfo();
            itemCommentInfo.ID = orderItemCommentID;
            itemCommentInfo.AddDate = DateTime.Now;
            itemCommentInfo.AddUser = RequestUtils.CurrentUserName;
            itemCommentInfo.Comment = comment;
            itemCommentInfo.GoodCount = 0;
            itemCommentInfo.IsAnonymous = isAnonymous;
            itemCommentInfo.OrderItemID = orderItemID;
            itemCommentInfo.OrderUrl = string.Empty;
            itemCommentInfo.Star = star;
            itemCommentInfo.Tags = tags;

            //����ͼƬ
            if (!string.IsNullOrEmpty(imageUrl))
            {
                StringBuilder sbImageUrl = new StringBuilder();
                ArrayList arrayList = TranslateUtils.StringCollectionToArrayList(imageUrl);
                foreach (string image in arrayList)
                {
                    string[] iamgeArr = image.Split(new string[] { RequestUtils.PublishmentSystemInfo.PublishmentSystemDir }, StringSplitOptions.RemoveEmptyEntries);
                    if (iamgeArr.Length < 1)
                        continue;
                    string virtualImage = iamgeArr[1];
                    virtualImage = PageUtility.GetPublishmentSystemUrlByPhysicalPath(RequestUtils.PublishmentSystemInfo, virtualImage);
                    virtualImage = PageUtility.GetVirtualUrl(RequestUtils.PublishmentSystemInfo, virtualImage);
                    sbImageUrl.Append(virtualImage + ",");
                }
                if (sbImageUrl.Length > 0)
                    sbImageUrl.Length--;
                itemCommentInfo.ImageUrl = sbImageUrl.ToString();
            }

            try
            {
                if (itemCommentInfo.ID == 0)
                    DataProviderB2C.OrderItemCommentDAO.Insert(itemCommentInfo);
            }
            catch (Exception)
            {

                return Ok(new { isSuccess = false });
            }


            return Ok(new { isSuccess = true });
        }

        [HttpGet]
        [ActionName("GetLatestOrderInAll")]
        public IHttpActionResult GetLatestOrderInAll()
        {
            bool isPC = RequestUtils.GetBoolQueryString("isPC");

            OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetLatestOrderInfo(RequestUtils.CurrentUserName);
            List<OrderListParameter> parameterList = new List<OrderListParameter>();

            if (orderInfo != null)
            {

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(orderInfo.PublishmentSystemID);
                if (publishmentSystemInfo != null)
                {

                    PaymentInfo paymentInfo = PaymentManager.GetPaymentInfo(orderInfo.PaymentID);
                    string clickString = string.Empty;
                    string log = string.Empty;
                    string paymentForm = "";
                    bool isPaymentClick = false;
                    if (isPC)
                    {
                        paymentForm = PaymentManager.GetPaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                    }
                    else
                    {
                        // ���ǿ  �ֻ�֧�� �ȴ��ӿ����� begin

                        paymentForm = PaymentManager.GetMobilePaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);

                        // ���ǿ  �ֻ�֧�� �ȴ��ӿ����� end
                    }

                    orderInfo.Extended = log;
                    DataProviderB2C.OrderDAO.Update(orderInfo);

                    PublishmentSystemParameter publishmentSystemParamter = new PublishmentSystemParameter()
                    {
                        PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID,
                        PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName,
                        PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl
                    };

                    List<OrderItemInfo> itemInfoList = DataProviderB2C.OrderItemDAO.GetItemInfoList(RequestUtils.CurrentUserName, orderInfo.ID);
                    List<OrderItemParameter> itemParameterList = new List<OrderItemParameter>();

                    foreach (OrderItemInfo itemInfo in itemInfoList)
                    {
                        GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);
                        GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(itemInfo.GoodsID, contentInfo);
                        string navigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, itemInfo.ChannelID), itemInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
                        string thumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.ThumbUrl));
                        string spec = SpecManager.GetSpecValues(publishmentSystemInfo, goodsInfo);
                        spec = spec.Replace("/api", string.Empty).Replace("</div><div>", "<br />").Replace("<div>", string.Empty).Replace("</div>", string.Empty);

                        OrderItemParameter itemParameter = new OrderItemParameter { OrderItemID = itemInfo.ID, GoodsSN = itemInfo.GoodsSN, Title = itemInfo.Title, ThumbUrl = thumbUrl, PriceSale = itemInfo.PriceSale, PurchaseNum = itemInfo.PurchaseNum, NavigationUrl = navigationUrl, Spec = spec };
                        itemParameterList.Add(itemParameter);
                    }

                    if (!string.IsNullOrEmpty(paymentForm))
                    {
                        isPaymentClick = true;
                    }
                    orderInfo.OrderStatus = EOrderStatusUtils.GetText(orderInfo.OrderStatus);
                    orderInfo.PaymentStatus = EPaymentStatusUtils.GetText(orderInfo.PaymentStatus);
                    orderInfo.ShipmentStatus = EShipmentStatusUtils.GetText(orderInfo.ShipmentStatus);
                    OrderListParameter parameter = new OrderListParameter { PublishmentSystemInfo = publishmentSystemParamter, OrderInfo = orderInfo, Items = itemParameterList, IsPaymentClick = isPaymentClick, PaymentForm = paymentForm, ClickString = clickString };
                    parameterList.Add(parameter);
                }
            }
            var orderInfoListParms = new { OrderInfoList = parameterList };
            return Ok(orderInfoListParms);
        }

        [HttpGet]
        [ActionName("GetOrderStatistic")]
        public IHttpActionResult GetOrderStatistic()
        {
            try
            {
                int noPay = 0;
                int noCompleted = 0;
                int comment = 0;
                int total = 0;
                int commentTotal = 0;
                bool isSuccess = DataProviderB2C.OrderDAO.GetOrderStatistic(RequestUtils.CurrentUserName, out noPay, out noCompleted, out comment, out total, out commentTotal);
                var parameter = new { isSuccess = true, NoPay = noPay, NoCompleted = noCompleted, NoComment = commentTotal - comment, Total = total };
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var errorParam = new { isSuccess = false, errorMessage = ex.Message };
                return Ok(errorParam);
            }

        }


        [HttpGet]
        [ActionName("GetOrderItem")]
        public IHttpActionResult GetOrderItem()
        {
            try
            {
                int orderItemId = RequestUtils.GetIntQueryString("orderItemId");
                int publishmentSystemID = RequestUtils.GetIntQueryString("publishmentSystemID");
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (publishmentSystemInfo == null)
                {
                    return Ok(new { isSuccess = false, errorMessage = "publishmentSystemInfo is null" });
                }
                PublishmentSystemParameter publishmentSystemParameter = new PublishmentSystemParameter()
                {
                    PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID,
                    PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName,
                    PublishmentSystemUrl = PageUtils.AddProtocolToUrl(publishmentSystemInfo.PublishmentSystemUrl)
                };

                OrderItemInfo orderItemInfo = DataProviderB2C.OrderItemDAO.GetItemInfo(orderItemId);
                if (orderItemInfo == null)
                {
                    return Ok(new { isSuccess = false, errorMessage = "orderItemInfo is null" });
                }
                OrderItemParameter orderItemParamInfo = new OrderItemParameter(orderItemInfo)
                {
                    OrderItemID = orderItemInfo.ID,
                    NavigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, orderItemInfo.ChannelID), orderItemInfo.ContentID, publishmentSystemInfo.Additional.VisualType)
                };

                OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderItemInfo.OrderID);
                if (orderInfo == null)
                {
                    return Ok(new { isSuccess = false, errorMessage = "orderInfo is null" });
                }
                ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(orderInfo.ConsigneeID);
                OrderParameter orderParameter = new OrderParameter()
                {
                    Consignee = consigneeInfo
                };
                orderItemParamInfo.NavigationUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, orderItemParamInfo.NavigationUrl));
                orderItemParamInfo.ThumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, orderItemParamInfo.ThumbUrl));

                return Ok(new { isSuccess = true, publishmentSystemInfo = publishmentSystemParameter, orderItemInfo = orderItemParamInfo, orderInfo = orderParameter });
            }
            catch (Exception ex)
            {
                return Ok(new { isSuccess = false, errorMessage = ex.Message });
            }
        }

        [HttpGet]
        [ActionName("GetAllOrderListWithQiao")]
        public IHttpActionResult GetAllOrderListWithQiao()
        {

            #region ��ҳ����
            int pageIndex = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("pageIndex"));
            int prePageNum = TranslateUtils.ToInt(RequestUtils.GetQueryStringNoSqlAndXss("prePageNum"));
            string isCompleted = HttpContext.Current.Request.QueryString["isCompleted"];
            string isPayment = HttpContext.Current.Request.QueryString["isPayment"];
            int total = DataProviderB2C.OrderDAO.GetCountByUser(RequestUtils.CurrentUserName, isCompleted, isPayment);
            string pageJson = PageDataUtils.ParsePageJson(pageIndex, prePageNum, total);
            #endregion
            ArrayList parameterList = new ArrayList();
            ArrayList publishmentSystemInfoArray = PublishmentSystemManager.GetPublishmentSystemIDArrayList(BaiRong.Model.EPublishmentSystemType.B2C);

            bool isAll = false;
            bool isPC = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isPC"], true);
            int orderTime = TranslateUtils.ToInt(HttpContext.Current.Request.QueryString["orderTime"], 0);
            string keywords = PageUtils.FilterSql(HttpContext.Current.Request.QueryString["keywords"]);
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["isCompleted"]) && string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["isPayment"]))
            {
                isAll = true;
            }
            List<OrderInfo> orderInfoList = new List<OrderInfo>();

            orderInfoList = DataProviderB2C.OrderDAO.GetOrderInfoList(RequestUtils.CurrentUserName, isCompleted, isPayment, pageIndex, prePageNum, orderTime, keywords);

            #region ��ȡÿһ��b2c����Ʒ
            foreach (OrderInfo orderInfo in orderInfoList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(orderInfo.PublishmentSystemID);
                if (publishmentSystemInfo != null)
                {
                    try
                    {
                        bool isPaymentClick = false;
                        string paymentForm = string.Empty;
                        string clickString = string.Empty;

                        if (EOrderStatusUtils.Equals(orderInfo.OrderStatus, EOrderStatus.Handling) && EPaymentStatusUtils.Equals(orderInfo.PaymentStatus, EPaymentStatus.Unpaid))
                        {
                            PaymentInfo paymentInfo = PaymentManager.GetPaymentInfo(orderInfo.PaymentID);
                            if (paymentInfo != null)
                            {
                                string log = string.Empty;
                                if (isPC)
                                    paymentForm = PaymentManager.GetPaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                                else
                                    paymentForm = PaymentManager.GetMobilePaymentForm(paymentInfo, publishmentSystemInfo, orderInfo, out clickString, out log);
                                if (!string.IsNullOrEmpty(paymentForm))
                                {
                                    isPaymentClick = true;
                                }
                                orderInfo.Extended = log;
                                DataProviderB2C.OrderDAO.Update(orderInfo);
                            }
                        }

                        orderInfo.OrderStatus = EOrderStatusUtils.GetText(orderInfo.OrderStatus);
                        orderInfo.PaymentStatus = EPaymentStatusUtils.GetText(orderInfo.PaymentStatus);
                        orderInfo.ShipmentStatus = EShipmentStatusUtils.GetText(orderInfo.ShipmentStatus);

                        List<OrderItemInfo> itemInfoList = DataProviderB2C.OrderItemDAO.GetItemInfoList(orderInfo.ID);
                        List<OrderItemParameter> itemParameterList = new List<OrderItemParameter>();

                        foreach (OrderItemInfo itemInfo in itemInfoList)
                        {
                            GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, itemInfo.ChannelID, itemInfo.ContentID);
                            GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(itemInfo.GoodsID, contentInfo);
                            string navigationUrl = PageUtility.GetContentUrl(publishmentSystemInfo, NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, itemInfo.ChannelID), itemInfo.ContentID, publishmentSystemInfo.Additional.VisualType);
                            string thumbUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.ThumbUrl));
                            string spec = SpecManager.GetSpecValues(publishmentSystemInfo, goodsInfo);
                            spec = spec.Replace("/api", string.Empty).Replace("</div><div>", "<br />").Replace("<div>", string.Empty).Replace("</div>", string.Empty);

                            OrderItemParameter itemParameter = new OrderItemParameter { OrderItemID = itemInfo.ID, OrderID = itemInfo.OrderID, GoodsSN = itemInfo.GoodsSN, Title = itemInfo.Title, ThumbUrl = thumbUrl, PriceSale = itemInfo.PriceSale, PurchaseNum = itemInfo.PurchaseNum, NavigationUrl = navigationUrl, Spec = spec };

                            itemParameter.IsApplyReturn = DataProviderB2C.OrderItemReturnDAO.GetCount(string.Format(" OrderItemID = {0} ", itemInfo.ID)) > 0;

                            itemParameterList.Add(itemParameter);


                        }
                        PublishmentSystemParameter publishmentSystemParamter = new PublishmentSystemParameter()
                        {
                            PublishmentSystemID = orderInfo.PublishmentSystemID,
                            PublishmentSystemName = publishmentSystemInfo.PublishmentSystemName,
                            PublishmentSystemUrl = publishmentSystemInfo.PublishmentSystemUrl
                        };
                        var parameter = new { PublishmentSystemInfo = publishmentSystemParamter, OrderInfo = orderInfo, Items = itemParameterList, IsPaymentClick = isPaymentClick, PaymentForm = paymentForm, ClickString = clickString, qiaoUrl = publishmentSystemInfo.Additional.Attributes["qiaourl"] };

                        parameterList.Add(parameter);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                #endregion
            }
            var orderInfoListParms = new { PageJson = pageJson, OrderInfoList = parameterList };
            return Ok(orderInfoListParms);

        }

        /// <summary>
        /// �ϴ�������Ƭ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UploadCommentImage")]
        public IHttpActionResult UploadCommentImage()
        {
            try
            {
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    UserInfo user = RequestUtils.Current;
                    int orderItemID = RequestUtils.GetIntQueryString("orderItemID");
                    OrderItemInfo orderItemInfo = DataProviderB2C.OrderItemDAO.GetItemInfo(orderItemID);


                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    string fileName = PathUtils.GetFileName(file.FileName);
                    string fileExtend = PathUtils.GetExtension(fileName).Trim('.');
                    EImageType imageType = EImageTypeUtils.GetEnumType(fileExtend);
                    //������ļ���
                    string localFileName = PathUtility.GetUploadFileName(RequestUtils.PublishmentSystemInfo, fileName);
                    //������ļ�����
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(RequestUtils.PublishmentSystemInfo, fileExtend);
                    string localFilePath = PathUtils.Combine(localDirectoryPath, UserOrderItemCommentImageFileName, orderItemID.ToString(), localFileName);

                    localFilePath = APIPageUtils.ParseUrl(localFilePath);

                    if (!PathUtility.IsImageExtenstionAllowed(RequestUtils.PublishmentSystemInfo, fileExtend))
                    {
                        var errorParm = new { isSuccess = false, errorMessage = "�ϴ�ʧ�ܣ��ϴ�ͼƬ��ʽ����ȷ��" };
                        return Ok(errorParm);
                    }
                    if (!PathUtility.IsImageSizeAllowed(RequestUtils.PublishmentSystemInfo, file.ContentLength))
                    {
                        var errorParm = new { isSuccess = false, errorMessage = "�ϴ�ʧ�ܣ��ϴ�ͼƬ�����涨�ļ���С��" };
                        return Ok(errorParm);
                    }

                    bool isImage = EFileSystemTypeUtils.IsImage(fileExtend);

                    if (isImage)
                    {
                        //�����˻�ͼƬ
                        DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(localFilePath));
                        file.SaveAs(localFilePath);

                        string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(RequestUtils.PublishmentSystemInfo, localFilePath);

                        return Ok(new { isSuccess = true, imageUrl = PageUtils.AddProtocolToUrl(imageUrl), OrderItemID = orderItemID });
                    }

                }
                return Ok(new { isSuccess = false });
            }
            catch (Exception ex)
            {
                var errorParm = new { isSuccess = false, errorMessage = ex.Message };
                return Ok(errorParm);
            }
        }

        #endregion
    }
}
