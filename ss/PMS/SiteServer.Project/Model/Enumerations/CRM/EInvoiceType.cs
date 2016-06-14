using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.Project.Model
{
    public enum EInvoiceType
	{
        SiteYun,           //SiteYun发票
        SiteServer,        //SiteServer发票
	}

    public class EInvoiceTypeUtils
	{
		public static string GetValue(EInvoiceType type)
		{
            if (type == EInvoiceType.SiteYun)
			{
                return "SiteYun";
			}
            else if (type == EInvoiceType.SiteServer)
            {
                return "SiteServer";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EInvoiceType type)
		{
            if (type == EInvoiceType.SiteYun)
			{
                return "SiteYun发票";
			}
            else if (type == EInvoiceType.SiteServer)
            {
                return "SiteServer发票";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EInvoiceType GetEnumType(string typeStr)
		{
            EInvoiceType retval = EInvoiceType.SiteYun;

            if (Equals(EInvoiceType.SiteYun, typeStr))
			{
                retval = EInvoiceType.SiteYun;
			}
            else if (Equals(EInvoiceType.SiteServer, typeStr))
            {
                retval = EInvoiceType.SiteServer;
            }
			return retval;
		}

		public static bool Equals(EInvoiceType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EInvoiceType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EInvoiceType type, bool selected)
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
                listControl.Items.Add(GetListItem(EInvoiceType.SiteYun, false));
                listControl.Items.Add(GetListItem(EInvoiceType.SiteServer, false));
            }
        }
	}
}
