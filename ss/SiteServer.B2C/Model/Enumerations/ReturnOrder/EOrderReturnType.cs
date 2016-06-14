using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.B2C.Model.Enumerations.ReturnOrder
{
    /// <summary>
    /// 退换货申请状态
    /// </summary>
    public enum EOrderReturnType
    {
        Return, //退货
        Exchange, //换货
        Repair  // 维修
    }

    public class EOrderReturnTypeUtils
    {
        public static string GetValue(EOrderReturnType type)
        {
            if (type == EOrderReturnType.Return)
            {
                return "Return";
            }
            else if (type == EOrderReturnType.Exchange)
            {
                return "Exchange";
            }
            else if (type == EOrderReturnType.Repair)
            {
                return "Repair";
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

        public static string GetText(EOrderReturnType type)
        {
            if (type == EOrderReturnType.Return)
            {
                return "退货";
            }
            else if (type == EOrderReturnType.Exchange)
            {
                return "换货";
            }
            else if (type == EOrderReturnType.Repair)
            {
                return "维修";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EOrderReturnType GetEnumType(string typeStr)
        {
            EOrderReturnType retval = EOrderReturnType.Exchange;

            if (Equals(EOrderReturnType.Return, typeStr))
            {
                retval = EOrderReturnType.Return;
            }
            else if (Equals(EOrderReturnType.Exchange, typeStr))
            {
                retval = EOrderReturnType.Exchange;
            }
            else if (Equals(EOrderReturnType.Repair, typeStr))
            {
                retval = EOrderReturnType.Repair;
            }
            return retval;
        }

        public static bool Equals(EOrderReturnType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EOrderStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EOrderReturnType type, bool selected)
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
                listControl.Items.Add(GetListItem(EOrderReturnType.Return, false));
                listControl.Items.Add(GetListItem(EOrderReturnType.Exchange, false));
            }
        }
    }
}
