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
                return "֧����";
            }
            //else if (type == EPaymentType.Tenpay)
            //{
            //    return "�Ƹ�ͨ";
            //}
            else if (type == EPaymentType.WeiXinPay)
            {
                return "΢��֧��";
            }
            else if (type == EPaymentType.Unionpay)
            {
                return "����֧��";
            }
            else if (type == EPaymentType.COD)
            {
                return "��������";
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
                return "֧������ȫ�����ȵĶ���������֧��ƽ̨��";
            }
            //else if (type == EPaymentType.Tenpay)
            //{
            //    return "�Ƹ�ͨ������֧��ר�ң���������ȫ��ݵ����ϸ������顣";
            //}
            else if (type == EPaymentType.WeiXinPay)
            {
                return "΢��֧��ּ��Ϊ���΢���û����̻��ṩ�����ʵ�֧������΢�ŵ�֧���Ͱ�ȫϵͳ����Ѷ�Ƹ�ͨ�ṩ֧�֡�";
            }
            else if (type == EPaymentType.Unionpay)
            {
                return "��������֧��";
            }
            else if (type == EPaymentType.COD)
            {
                return "�ͻ����ź����տ";
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
