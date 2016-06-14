using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;

using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using System.Text;
using System;
using System.Collections.Generic;
using SiteServer.B2C.Core.Alipay;
using System.Collections.Specialized;
using System.Web;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.B2C.Core.Union;

namespace SiteServer.B2C.Core
{
    public class PaymentManager
    {
        public static string GetPaymentName(int paymentID)
        {
            return DataProviderB2C.PaymentDAO.GetPaymentInfo(paymentID).PaymentName;
        }

        public static PaymentInfo GetPaymentInfo(int paymentID)
        {
            return DataProviderB2C.PaymentDAO.GetPaymentInfo(paymentID);
        }

        private static string GetNotifyUrl(PublishmentSystemInfo publishmentSystemInfo, PaymentInfo paymentInfo, OrderInfo orderInfo)
        {
            string notifyUrl = string.Empty;

            if (paymentInfo.PaymentType == EPaymentType.Alipay)
            {
                notifyUrl = string.Format("/api/order/alipaynotify/{0}", publishmentSystemInfo.PublishmentSystemID);
            }
            else if (paymentInfo.PaymentType == EPaymentType.Unionpay)
            {
                notifyUrl = string.Format("/api/order/unionnotify/{0}", publishmentSystemInfo.PublishmentSystemID);
            }
            notifyUrl = PageUtils.AddProtocolToUrl(PageUtils.Combine(HttpContext.Current.Request.Url.Authority, notifyUrl));

            return notifyUrl;
        }

        private static string GetReturnUrl(PublishmentSystemInfo publishmentSystemInfo, PaymentInfo paymentInfo, OrderInfo orderInfo)
        {
            string returnUrl = string.Empty;
            if (paymentInfo.PaymentType == EPaymentType.Alipay)
            {
                returnUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, "@/utils/return.html");
                if (!PageUtils.IsProtocolUrl(returnUrl))
                {
                    returnUrl = PageUtils.AddProtocolToUrl(PageUtils.Combine(HttpContext.Current.Request.Url.Authority, returnUrl));
                }

            }
            else if (paymentInfo.PaymentType == EPaymentType.Unionpay)
            {
                //保存起来，支付成功之后，跳转使用
                returnUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, "@/utils/return.html?orderId=" + orderInfo.OrderSN);
                if (!PageUtils.IsProtocolUrl(returnUrl))
                {
                    returnUrl = PageUtils.AddProtocolToUrl(PageUtils.Combine(HttpContext.Current.Request.Url.Authority, returnUrl));
                }
                CacheUtils.Max("UnionReturnUrl_" + orderInfo.OrderSN, returnUrl);
                returnUrl = string.Format("/api/order/returnTransit");
            }
            if (!PageUtils.IsProtocolUrl(returnUrl))
            {
                returnUrl = PageUtils.AddProtocolToUrl(PageUtils.Combine(HttpContext.Current.Request.Url.Authority, returnUrl));
            }

            return returnUrl;
        }

        public static string GetPaymentForm(PaymentInfo paymentInfo, PublishmentSystemInfo publishmentSystemInfo, OrderInfo orderInfo, out string clickString, out string log)
        {
            string html = string.Empty;
            clickString = string.Empty;
            log = string.Empty;

            if (paymentInfo.PaymentType == EPaymentType.Alipay)
            {
                string notify_url = PaymentManager.GetNotifyUrl(publishmentSystemInfo, paymentInfo, orderInfo);
                string return_url = PaymentManager.GetReturnUrl(publishmentSystemInfo, paymentInfo, orderInfo);

                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();

                Config config = new Config(publishmentSystemInfo.PublishmentSystemID, orderInfo.ID);

                if (config.AlipayType == EPaymentAlipay.Direct)
                {
                    sParaTemp.Add("partner", config.Partner);
                    sParaTemp.Add("_input_charset", config.Input_charset);
                    sParaTemp.Add("service", config.Service);
                    sParaTemp.Add("payment_type", config.Payment_type);
                    sParaTemp.Add("notify_url", notify_url);
                    sParaTemp.Add("return_url", return_url);
                    sParaTemp.Add("seller_email", config.Seller_email);
                    sParaTemp.Add("out_trade_no", orderInfo.OrderSN);
                    sParaTemp.Add("subject", publishmentSystemInfo.PublishmentSystemName + "订单");
                    sParaTemp.Add("total_fee", orderInfo.PriceActual.ToString());
                    sParaTemp.Add("body", string.Empty);
                    sParaTemp.Add("show_url", string.Empty);
                    sParaTemp.Add("anti_phishing_key", string.Empty);
                    sParaTemp.Add("exter_invoke_ip", PageUtils.GetIPAddress());
                }

                else if (config.AlipayType == EPaymentAlipay.DualFun)
                {
                    ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(orderInfo.ConsigneeID);
                    if (consigneeInfo == null)
                    {
                        consigneeInfo = new ConsigneeInfo();
                    }

                    sParaTemp.Add("partner", config.Partner);
                    sParaTemp.Add("_input_charset", config.Input_charset);
                    sParaTemp.Add("service", config.Service);
                    sParaTemp.Add("payment_type", config.Payment_type);
                    sParaTemp.Add("notify_url", notify_url);
                    sParaTemp.Add("return_url", return_url);
                    sParaTemp.Add("seller_email", config.Seller_email);
                    sParaTemp.Add("out_trade_no", orderInfo.OrderSN);
                    sParaTemp.Add("subject", publishmentSystemInfo.PublishmentSystemName + "订单");
                    sParaTemp.Add("price", orderInfo.PriceActual.ToString());
                    sParaTemp.Add("quantity", config.Quantity);
                    sParaTemp.Add("logistics_fee", config.Logistics_fee);
                    sParaTemp.Add("logistics_type", config.Logistics_type);
                    sParaTemp.Add("logistics_payment", config.Logistics_payment);
                    sParaTemp.Add("body", string.Empty);
                    sParaTemp.Add("show_url", string.Empty);
                    sParaTemp.Add("receive_name", consigneeInfo.Consignee);
                    sParaTemp.Add("receive_address", consigneeInfo.Address);
                    sParaTemp.Add("receive_zip", consigneeInfo.Zipcode);
                    sParaTemp.Add("receive_phone", consigneeInfo.Tel);
                    sParaTemp.Add("receive_mobile", consigneeInfo.Mobile);
                }

                else if (config.AlipayType == EPaymentAlipay.Escow)
                {
                    ConsigneeInfo consigneeInfo = DataProviderB2C.ConsigneeDAO.GetConsigneeInfo(orderInfo.ConsigneeID);
                    if (consigneeInfo == null)
                    {
                        consigneeInfo = new ConsigneeInfo();
                    }

                    sParaTemp.Add("partner", config.Partner);
                    sParaTemp.Add("_input_charset", config.Input_charset.ToLower());
                    sParaTemp.Add("service", "create_partner_trade_by_buyer");
                    sParaTemp.Add("payment_type", config.Payment_type);
                    sParaTemp.Add("notify_url", notify_url);
                    sParaTemp.Add("return_url", return_url);
                    sParaTemp.Add("seller_email", config.Seller_email);
                    sParaTemp.Add("out_trade_no", orderInfo.OrderSN);
                    sParaTemp.Add("subject", publishmentSystemInfo.PublishmentSystemName + "订单");
                    sParaTemp.Add("price", orderInfo.PriceActual.ToString());
                    sParaTemp.Add("quantity", config.Quantity);
                    sParaTemp.Add("logistics_fee", config.Logistics_fee);
                    sParaTemp.Add("logistics_type", config.Logistics_type);
                    sParaTemp.Add("logistics_payment", config.Logistics_payment);
                    sParaTemp.Add("body", string.Empty);
                    sParaTemp.Add("show_url", string.Empty);
                    sParaTemp.Add("receive_name", consigneeInfo.Consignee);
                    sParaTemp.Add("receive_address", consigneeInfo.Address);
                    sParaTemp.Add("receive_zip", consigneeInfo.Zipcode);
                    sParaTemp.Add("receive_phone", consigneeInfo.Tel);
                    sParaTemp.Add("receive_mobile", consigneeInfo.Mobile);
                }

                StringBuilder logBuilder = new StringBuilder();
                //建立请求

                Submit submit = new Submit(publishmentSystemInfo.PublishmentSystemID, orderInfo.ID);

                html = submit.BuildRequest(sParaTemp, "get", "确认", logBuilder);

                log = logBuilder.ToString();

                clickString = submit.BuildRequestBtnClickString();
            }
            else if (paymentInfo.PaymentType == EPaymentType.Unionpay)
            {
                //银联支付
                string return_url = PaymentManager.GetReturnUrl(publishmentSystemInfo, paymentInfo, orderInfo);
                string notiny_url = PaymentManager.GetNotifyUrl(publishmentSystemInfo, paymentInfo, orderInfo);
                Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
                SDKConfig unionConfig = new SDKConfig(publishmentSystemInfo.PublishmentSystemID, orderInfo.ID);

                sParaTemp.Add("version", unionConfig.Version);
                sParaTemp.Add("encoding", unionConfig.Input_Encoding);
                sParaTemp.Add("certId", CertUtil.GetSignCertId());
                sParaTemp.Add("txnType", unionConfig.Txn_Type);
                sParaTemp.Add("txnSubType", unionConfig.Txn_SubType);
                sParaTemp.Add("bizType", unionConfig.Biz_Type);
                sParaTemp.Add("frontUrl", return_url);
                sParaTemp.Add("backUrl", notiny_url);
                sParaTemp.Add("signMethod", unionConfig.Sign_Method);
                sParaTemp.Add("channelType", "07");//渠道类型，07-PC，08-手机
                sParaTemp.Add("accessType", unionConfig.Access_Type);
                sParaTemp.Add("merId", unionConfig.Mer_ID);
                sParaTemp.Add("orderId", orderInfo.OrderSN);
                sParaTemp.Add("txnTime", DateTime.Now.ToString("yyyyMMddHHmmss"));
                sParaTemp.Add("txnAmt", ((int)(orderInfo.PriceActual * 100)).ToString());//单位：分
                sParaTemp.Add("currencyCode", unionConfig.Currency_Code);
                sParaTemp.Add("reqReserved", publishmentSystemInfo.PublishmentSystemName + "订单");

                SDKUtil.Sign(sParaTemp, Encoding.UTF8);

                html = SDKUtil.CreateAutoSubmitForm(orderInfo.ID, SDKConfig.FrontTransUrl, sParaTemp, Encoding.UTF8);
                clickString = SDKUtil.CreateAutoSubmitFormBtnClickString(orderInfo.ID);

            }
            else if (paymentInfo.PaymentType == EPaymentType.WeiXinPay)
            {
                //微信支付
                //NativePay nativePay = new NativePay();
                //string url = nativePay.GetPayUrl(orderInfo.OrderSN);
                //html = string.Format("<img src='{0}'/>", url);
            }

            return html;
        }

        public static SortedDictionary<string, string> GetAlipayNotifyRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        public static SortedDictionary<string, string> GetAlipayReturnRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        public static PaymentInfo GetPaymentInfo(int publishmentSystemID, EPaymentType paymentType)
        {
            List<PaymentInfo> paymentInfoList = DataProviderB2C.PaymentDAO.GetPaymentInfoList(publishmentSystemID);
            foreach (PaymentInfo paymentInfo in paymentInfoList)
            {
                if (paymentInfo.IsEnabled && paymentInfo.PaymentType == paymentType)
                {
                    return paymentInfo;
                }
            }
            return null;
        }

        public static string GetMobilePaymentForm(PaymentInfo paymentInfo, PublishmentSystemInfo publishmentSystemInfo, OrderInfo orderInfo, out string clickString, out string log)
        {
            string html = string.Empty;
            clickString = string.Empty;
            log = string.Empty;
            if (paymentInfo.PaymentType == EPaymentType.Alipay)
            {
                string GATEWAY_NEW = "http://wappaygw.alipay.com/service/rest.htm?";


                AlipayMobile.Config config = new AlipayMobile.Config(publishmentSystemInfo.PublishmentSystemID, orderInfo.ID);
                AlipayMobile.Submit submit = new AlipayMobile.Submit(publishmentSystemInfo.PublishmentSystemID, orderInfo.ID);


                //返回格式
                string format = "xml";
                //必填，不需要修改

                //返回格式
                string v = "2.0";
                //必填，不需要修改

                //请求号
                string req_id = DateTime.Now.ToString("yyyyMMddHHmmss");
                //必填，须保证每次请求都是唯一

                //req_data详细信息

                //服务器异步通知页面路径

                string notify_url = PaymentManager.GetNotifyUrl(publishmentSystemInfo, paymentInfo, orderInfo);

                //需http://格式的完整路径，不允许加?id=123这类自定义参数

                //页面跳转同步通知页面路径
                string call_back_url = PaymentManager.GetReturnUrl(publishmentSystemInfo, paymentInfo, orderInfo);
                //需http://格式的完整路径，不允许加?id=123这类自定义参数

                //卖家支付宝帐户
                string seller_email = config.Seller_email;
                //必填

                //商户订单号
                string out_trade_no = orderInfo.OrderSN.Trim();
                //商户网站订单系统中唯一订单号，必填

                //订单名称
                string subject = publishmentSystemInfo.PublishmentSystemName + orderInfo.OrderSN + "订单";
                //必填

                //付款金额
                string total_fee = orderInfo.PriceActual.ToString();
                //string total_fee = "0.01";
                //必填

                //请求业务参数详细
                string req_dataToken = "<direct_trade_create_req><notify_url>" + notify_url + "</notify_url><call_back_url>" + call_back_url + "</call_back_url><seller_account_name>" + seller_email + "</seller_account_name><out_trade_no>" + out_trade_no + "</out_trade_no><subject>" + subject + "</subject><total_fee>" + total_fee + "</total_fee></direct_trade_create_req>";
                //必填

                //把请求参数打包成数组
                Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
                sParaTempToken.Add("partner", config.Partner);
                sParaTempToken.Add("_input_charset", config.Input_charset.ToLower());
                sParaTempToken.Add("sec_id", config.Sign_type.ToUpper());
                sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
                sParaTempToken.Add("format", format);
                sParaTempToken.Add("v", v);
                sParaTempToken.Add("req_id", req_id);
                sParaTempToken.Add("req_data", req_dataToken);

                //建立请求
                string sHtmlTextToken = submit.BuildRequest(GATEWAY_NEW, sParaTempToken);
                //URLDECODE返回的信息
                Encoding code = Encoding.GetEncoding(config.Input_charset);
                sHtmlTextToken = HttpUtility.UrlDecode(sHtmlTextToken, code);

                //解析远程模拟提交后返回的信息
                Dictionary<string, string> dicHtmlTextToken = submit.ParseResponse(sHtmlTextToken);

                //获取token
                string request_token = dicHtmlTextToken["request_token"];

                ////////////////////////////////////////////根据授权码token调用交易接口alipay.wap.auth.authAndExecute////////////////////////////////////////////
                //业务详细
                string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
                //必填

                //把请求参数打包成数组
                Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
                sParaTemp.Add("partner", config.Partner);
                sParaTemp.Add("_input_charset", config.Input_charset.ToLower());
                sParaTemp.Add("sec_id", config.Sign_type.ToUpper());
                sParaTemp.Add("service", "alipay.wap.auth.authAndExecute");
                sParaTemp.Add("format", format);
                sParaTemp.Add("v", v);
                sParaTemp.Add("req_data", req_data);

                StringBuilder logBuilder = new StringBuilder();

                html = submit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");

                log = logBuilder.ToString();

                clickString = submit.BuildRequestBtnClickString();
            }
            else if (paymentInfo.PaymentType == EPaymentType.Unionpay)
            {
                //银联支付
                string return_url = PaymentManager.GetReturnUrl(publishmentSystemInfo, paymentInfo, orderInfo);
                string notiny_url = PaymentManager.GetNotifyUrl(publishmentSystemInfo, paymentInfo, orderInfo);
                Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
                SDKConfig unionConfig = new SDKConfig(publishmentSystemInfo.PublishmentSystemID, orderInfo.ID);


                sParaTemp.Add("version", unionConfig.Version);
                sParaTemp.Add("encoding", unionConfig.Input_Encoding);
                sParaTemp.Add("certId", CertUtil.GetSignCertId());
                sParaTemp.Add("txnType", unionConfig.Txn_Type);
                sParaTemp.Add("txnSubType", unionConfig.Txn_SubType);
                sParaTemp.Add("bizType", unionConfig.Biz_Type);
                sParaTemp.Add("frontUrl", return_url);
                sParaTemp.Add("backUrl", notiny_url);
                sParaTemp.Add("signMethod", unionConfig.Sign_Method);
                sParaTemp.Add("channelType", "08");//渠道类型，07-PC，08-手机
                sParaTemp.Add("accessType", unionConfig.Access_Type);
                sParaTemp.Add("merId", unionConfig.Mer_ID);
                sParaTemp.Add("orderId", orderInfo.OrderSN);
                sParaTemp.Add("txnTime", DateTime.Now.ToString("yyyyMMddHHmmss"));
                sParaTemp.Add("txnAmt", ((int)(orderInfo.PriceActual * 100)).ToString());//单位：分
                sParaTemp.Add("currencyCode", unionConfig.Currency_Code);
                sParaTemp.Add("reqReserved", publishmentSystemInfo.PublishmentSystemName + "订单");



                SDKUtil.Sign(sParaTemp, Encoding.UTF8);

                html = SDKUtil.CreateAutoSubmitForm(orderInfo.ID, SDKConfig.FrontTransUrl, sParaTemp, Encoding.UTF8);
                clickString = SDKUtil.CreateAutoSubmitFormBtnClickString(orderInfo.ID);

            }
            else if (paymentInfo.PaymentType == EPaymentType.WeiXinPay)
            {
                //微信支付
                string wxEditAddrParam = string.Empty;
                JsApiPay jsApiPay = new JsApiPay(HttpContext.Current);
                try
                {
                    //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                    jsApiPay.GetOpenidAndAccessToken();

                    //获取收货地址js函数入口参数
                    wxEditAddrParam = jsApiPay.GetEditAddressParameters();
                    DbCacheManager.Insert("PaymentManager.WeiXin.openid" + UserManager.Current.UserName, jsApiPay.openid);
                    clickString = "";
                }
                catch (Exception ex)
                {
                    clickString = "";
                }
            }

            return html;
        }
    }
}
