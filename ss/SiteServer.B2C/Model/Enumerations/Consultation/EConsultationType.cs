using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
    public enum EConsultationType
    {
        Goods,              //商品咨询
        StockShipment,        //库存配送
        Payment,             //支付问题
        Invoice,            //发票保修
        Promotion,          //促销及赠品
    }

    public class EConsultationTypeUtils
    {
        public static string GetValue(EConsultationType type)
        {
            if (type == EConsultationType.Goods)
            {
                return "Goods";
            }
            else if (type == EConsultationType.StockShipment)
            {
                return "StockShipment";
            }
            else if (type == EConsultationType.Payment)
            {
                return "Payment";
            }
            else if (type == EConsultationType.Invoice)
            {
                return "Invoice";
            }
            else if (type == EConsultationType.Promotion)
            {
                return "Promotion";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(string strType)
        {
            return GetText(GetEnumType(strType));
        }

        public static string GetText(EConsultationType type)
        {
            if (type == EConsultationType.Goods)
            {
                return "商品咨询";
            }
            else if (type == EConsultationType.StockShipment)
            {
                return "库存配送";
            }
            else if (type == EConsultationType.Payment)
            {
                return "支付问题";
            }
            else if (type == EConsultationType.Invoice)
            {
                return "发票保修";
            }
            else if (type == EConsultationType.Promotion)
            {
                return "促销及赠品";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EConsultationType GetEnumType(string typeStr)
        {
            EConsultationType retval = EConsultationType.Goods;

            if (Equals(EConsultationType.StockShipment,typeStr))
            {
                return EConsultationType.StockShipment;
            }
            else if (Equals(EConsultationType.Payment, typeStr))
            {
                return EConsultationType.Payment;
            }
            else if (Equals(EConsultationType.Invoice, typeStr))
            {
                return EConsultationType.Invoice;
            }
            else if (Equals(EConsultationType.Promotion, typeStr))
            {
                return EConsultationType.Promotion;
            }
            return retval;
        }

        public static bool Equals(EConsultationType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EConsultationType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EConsultationType type, bool selected)
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
                listControl.Items.Add(GetListItem(EConsultationType.Goods, false));
                listControl.Items.Add(GetListItem(EConsultationType.StockShipment, false));
                listControl.Items.Add(GetListItem(EConsultationType.Payment, false));
                listControl.Items.Add(GetListItem(EConsultationType.Invoice, false));
                listControl.Items.Add(GetListItem(EConsultationType.Promotion, false));
            }
        }
    }
}
