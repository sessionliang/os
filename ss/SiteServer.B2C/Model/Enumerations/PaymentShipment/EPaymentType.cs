using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
	public enum EPaymentType
	{
        Alipay,
        //Tenpay,
        WeiXinPay,
        Unionpay,
        COD
	}

    public class EPaymentTypeUtils
	{
		public static string GetValue(EPaymentType type)
		{
            if (type == EPaymentType.Alipay)
			{
                return "Alipay";
            }
            //else if (type == EPaymentType.Tenpay)
            //{
            //    return "Tenpay";
            //}
            else if (type == EPaymentType.WeiXinPay)
            {
                return "WeiXinPay";
            }
            else if (type == EPaymentType.Unionpay)
            {
                return "Unionpay";
            }
            else if (type == EPaymentType.COD)
            {
                return "COD";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EPaymentType type)
		{
            if (type == EPaymentType.Alipay)
			{
                return "支付宝";
            }
            //else if (type == EPaymentType.Tenpay)
            //{
            //    return "财付通";
            //}
            else if (type == EPaymentType.WeiXinPay)
            {
                return "微信支付";
            }
            else if (type == EPaymentType.Unionpay)
            {
                return "银联支付";
            }
            else if (type == EPaymentType.COD)
            {
                return "货到付款";
            }
			else
			{
				throw new Exception();
			}
		}

        public static string GetDescription(EPaymentType type)
        {
            if (type == EPaymentType.Alipay)
            {
                return "支付宝，全球领先的独立第三方支付平台。";
            }
            //else if (type == EPaymentType.Tenpay)
            //{
            //    return "财付通，在线支付专家，带给您安全快捷的网上付款体验。";
            //}
            else if (type == EPaymentType.WeiXinPay)
            {
                return "微信支付旨在为广大微信用户及商户提供更优质的支付服务，微信的支付和安全系统由腾讯财付通提供支持。";
            }
            else if (type == EPaymentType.Unionpay)
            {
                return "银联在线支付";
            }
            else if (type == EPaymentType.COD)
            {
                return "送货上门后再收款。";
            }
            else
            {
                throw new Exception();
            }
        }

		public static EPaymentType GetEnumType(string typeStr)
		{
            EPaymentType retval = EPaymentType.Alipay;

            if (Equals(EPaymentType.Alipay, typeStr))
			{
                retval = EPaymentType.Alipay;
            }
            //else if (Equals(EPaymentType.Tenpay, typeStr))
            //{
            //    retval = EPaymentType.Tenpay;
            //}
            else if (Equals(EPaymentType.WeiXinPay, typeStr))
            {
                retval = EPaymentType.WeiXinPay;
            }
            else if (Equals(EPaymentType.Unionpay, typeStr))
            {
                retval = EPaymentType.Unionpay;
            }
            else if (Equals(EPaymentType.COD, typeStr))
            {
                retval = EPaymentType.COD;
            }
			return retval;
		}

		public static bool Equals(EPaymentType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EPaymentType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EPaymentType type, bool selected)
		{
			ListItem item = new ListItem(GetText(type), GetValue(type));
			if (selected)
			{
				item.Selected = true;
			}
			return item;
		}

		public static List<EPaymentType> GetEPaymentList()
		{
            List<EPaymentType> list = new List<EPaymentType>();
            list.Add(EPaymentType.Alipay);
            //list.Add(EPaymentType.Tenpay);
            //list.Add(EPaymentType.WeiXinPay);
            list.Add(EPaymentType.Unionpay);
            list.Add(EPaymentType.COD);
            return list;
		}

	}
}
