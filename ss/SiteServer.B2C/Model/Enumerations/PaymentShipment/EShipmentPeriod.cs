using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.B2C.Model
{
    public enum EShipmentPeriod
    {
        Workday,                    //只工作日送货
        Holiday,                    //只双休日、假日送货
        All                         //工作日、双休日与假日均可送货
    }
    public class EShipmentPeriodUtils
    {
        public static string GetValue(EShipmentPeriod type)
        {
            if (type == EShipmentPeriod.Workday)
            {
                return "Workday";
            }
            else if (type == EShipmentPeriod.Holiday)
            {
                return "Holiday";
            }
            else if (type == EShipmentPeriod.All)
            {
                return "All";
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

        public static string GetText(EShipmentPeriod type)
        {
            if (type == EShipmentPeriod.Workday)
            {
                return "只工作日送货";
            }
            else if (type == EShipmentPeriod.Holiday)
            {
                return "只双休日、假日送货";
            }
            else if (type == EShipmentPeriod.All)
            {
                return "工作日、双休日与假日均可送货";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EShipmentPeriod GetEnumType(string typeStr)
        {
            EShipmentPeriod retval = EShipmentPeriod.Workday;

            if (Equals(EShipmentPeriod.Workday, typeStr))
            {
                retval = EShipmentPeriod.Workday;
            }
            else if (Equals(EShipmentPeriod.Holiday, typeStr))
            {
                retval = EShipmentPeriod.Holiday;
            }
            else if (Equals(EShipmentPeriod.All, typeStr))
            {
                retval = EShipmentPeriod.All;
            }
            return retval;
        }

        public static bool Equals(EShipmentPeriod type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EShipmentPeriod type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EShipmentPeriod type, bool selected)
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
                listControl.Items.Add(GetListItem(EShipmentPeriod.Workday, false));
                listControl.Items.Add(GetListItem(EShipmentPeriod.Holiday, false));
                listControl.Items.Add(GetListItem(EShipmentPeriod.All, false));
            }
        }
    }
}
