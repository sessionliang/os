using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
   public enum EShipmentStatus
    {
       UnShipment,   //未发货
       Shipment,     //已发货
       Returned      //已退货
    }

   public class EShipmentStatusUtils
   {
       public static string GetValue(EShipmentStatus type)
       {
           if (type == EShipmentStatus.UnShipment)
           {
               return "UnShipment";
           }
           else if (type == EShipmentStatus.Shipment)
           {
               return "Shipment";
           }
           else if (type == EShipmentStatus.Returned)
           {
               return "Returned";
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

       public static string GetText(EShipmentStatus type)
       {
           if (type == EShipmentStatus.UnShipment)
           {
               return "未发货";
           }
           else if (type == EShipmentStatus.Shipment)
           {
               return "已发货";
           }
           else if (type == EShipmentStatus.Returned)
           {
               return "已退货";
           }
           else
           {
               throw new Exception();
           }
       }

       public static EShipmentStatus GetEnumType(string typeStr)
       {
           EShipmentStatus retval = EShipmentStatus.UnShipment;

           if (Equals(EShipmentStatus.UnShipment, typeStr))
           {
               retval = EShipmentStatus.UnShipment;
           }
           else if (Equals(EShipmentStatus.Shipment, typeStr))
           {
               retval = EShipmentStatus.Shipment;
           }
           else if (Equals(EShipmentStatus.Returned, typeStr))
           {
               retval = EShipmentStatus.Returned;
           }
           return retval;
       }

       public static bool Equals(EShipmentStatus type, string typeStr)
       {
           if (string.IsNullOrEmpty(typeStr)) return false;
           if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
           {
               return true;
           }
           return false;
       }

       public static bool Equals(string typeStr, EShipmentStatus type)
       {
           return Equals(type, typeStr);
       }

       public static ListItem GetListItem(EShipmentStatus type, bool selected)
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
               listControl.Items.Add(GetListItem(EShipmentStatus.UnShipment, false));
               listControl.Items.Add(GetListItem(EShipmentStatus.Shipment, false));
               listControl.Items.Add(GetListItem(EShipmentStatus.Returned, false));
           }
       }
   }
}
