using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
	public enum EPaymentAlipay
	{
        Direct,
        Escow,
        DualFun
	}

    public class EPaymentAlipayUtils
	{
		public static string GetValue(EPaymentAlipay type)
		{
            if (type == EPaymentAlipay.Direct)
			{
                return "Direct";
            }
            else if (type == EPaymentAlipay.Escow)
			{
                return "Escow";
            }
            else if (type == EPaymentAlipay.DualFun)
            {
                return "DualFun";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EPaymentAlipay type)
		{
            if (type == EPaymentAlipay.Direct)
			{
                return "使用即时到帐交易接口";
            }
            else if (type == EPaymentAlipay.Escow)
			{
				return "使用担保交易接口";
            }
            else if (type == EPaymentAlipay.DualFun)
            {
                return "使用标准双接口";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EPaymentAlipay GetEnumType(string typeStr)
		{
            EPaymentAlipay retval = EPaymentAlipay.Direct;

            if (Equals(EPaymentAlipay.Direct, typeStr))
			{
                retval = EPaymentAlipay.Direct;
            }
            else if (Equals(EPaymentAlipay.Escow, typeStr))
			{
                retval = EPaymentAlipay.Escow;
            }
            else if (Equals(EPaymentAlipay.DualFun, typeStr))
            {
                retval = EPaymentAlipay.DualFun;
            }
			return retval;
		}

		public static bool Equals(EPaymentAlipay type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EPaymentAlipay type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EPaymentAlipay type, bool selected)
		{
			ListItem item = new ListItem(GetText(type), GetValue(type));
			if (selected)
			{
				item.Selected = true;
			}
			return item;
		}

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EPaymentAlipay.Direct, false));
                listControl.Items.Add(GetListItem(EPaymentAlipay.Escow, false));
                listControl.Items.Add(GetListItem(EPaymentAlipay.DualFun, false));
            }
        }

	}
}
